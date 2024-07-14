using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

public enum ChickenEnemyType { Base,Flying,Boomer}
public enum ChickenNavType { Linear,Random, Loop }
public class ChickenEnermyMannager : Singleton<ChickenEnermyMannager>
{
    public GameObject[] chickenEnemyPrefab;
    public Transform[] chickenSpawnPoints;

    public List<spawnnedChickenEnemiesData> spawnnedChickenEnemies;

    public bool destoyingEnemies = false;

    [SerializeField] float enermySpawnDelay = 1.0f;
    [SerializeField] int spawnAmout = 6;

    [SerializeField] List<spawnnedChickenEnemiesData> _chickensToRemove = null;

    Coroutine spawnRoutine;
    float pressedCooldown = 0.5f;


    [Serializable]
    public class spawnnedChickenEnemiesData
    {
        public GameObject spawnnedChickenGO;
        public ChickenEnermy ChickenEnemyScript;
        public ValidNavPoints targetLocation;
        public ChickenNavType chickenNavType;
        public float chickenSpeed;
        public float chickenRotateSpeed;
        public float alignmentFactor;
        public ValidNavPoints startNavPoint;
        public List<ValidNavPoints> pastNavPoints;
    }

    private void Start()
    {
        spawnRoutine = StartCoroutine(SpawnChickenEnemies(enermySpawnDelay, spawnAmout));
    }

    private void Update()
    {
        if (pressedCooldown > 0)
        {
            pressedCooldown -= Time.deltaTime;
        }
        else
        {
            pressedCooldown = 0;
        }
    }

    public void RandomizeChickenEnemies(InputAction.CallbackContext _context)
    {
        if (_context.ReadValue<float>() == 1 && pressedCooldown == 0)
        {
            pressedCooldown = 0.5f;
            print("Pressed");
            _CNM.StopMovement();
            KillAllChickenEnemies();

            if (spawnRoutine != null)
            {
                StopCoroutine("SpawnChickenEnemies");
            }
            StartCoroutine(SpawnChickenEnemies(enermySpawnDelay, spawnAmout));
            _CNM.StartMovement();
        }
        else if (_context.ReadValue<float>() == 0)
        {
            
        }
    }

    /// <summary>
    /// Spawns In Chickens With a Delay and a Spawn Amount
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="spawnAmount"></param>
    /// <returns></returns>
    private IEnumerator SpawnChickenEnemies(float seconds, int spawnAmount)
    {
        //print("SpawnChickenEnemies Called");

        for (int i = 0; i < spawnAmount; i++)
        {
            if (i >= chickenSpawnPoints.Length)
            {
                Debug.LogError("Not enough spawn points defined.");
                yield break;
            }
            if (chickenSpawnPoints[i] == null)
            {
                Debug.LogError("Spawn point is null at index: " + i);
                yield break;
            }

            SpawnChickenEnemy(chickenSpawnPoints[i], chickenSpawnPoints[i], i);
            yield return new WaitForSeconds(seconds);
        }
        StopCoroutine("SpawnChickenEnemies");
    }

    private void SpawnChickenEnemy(Transform _spawnPos, Transform _spawnRot, int _spawnPointIndex)
    {
        //print($"SpawnChickenEnemy Called Spawn pos = {_spawnPos.transform.position} spawn rot = {_spawnRot.transform.rotation} and index = {_spawnPointIndex}");


        GameObject enemyPrefab = chickenEnemyPrefab[UnityEngine.Random.Range(0, chickenEnemyPrefab.Length)];

        GameObject enemySpawn = Instantiate(enemyPrefab, _spawnPos.position, _spawnRot.rotation);

        spawnnedChickenEnemiesData enemySpawnData = new spawnnedChickenEnemiesData();
        enemySpawnData.spawnnedChickenGO = enemySpawn;

        ValidNavPoints closestNavPoint = FindClosestNavPoint(enemySpawn.transform, _CNM.NavPoints);
        enemySpawnData.targetLocation = closestNavPoint;


        ChickenEnermy chickenEnemyComponent = enemySpawn.GetComponent<ChickenEnermy>();

        enemySpawnData.ChickenEnemyScript = chickenEnemyComponent;
        enemySpawnData.chickenSpeed = chickenEnemyComponent.chickenEnemySO.chickenEnemySpeed;
        enemySpawnData.chickenRotateSpeed = chickenEnemyComponent.chickenEnemySO.chickenEnemyRotateSpeed;

        chickenEnemyComponent.setup();
        spawnnedChickenEnemies.Add(enemySpawnData);

        _UIM.UpdateEnemies(GetEnemyCount(), spawnAmout);

    }

    //public void SpawnChickenEnemySpacific(Transform _spawnPos, Transform _spawnRot, int _spawnPointIndex, int _chickenIndex)
    //{

    //    GameObject enemySpawn = Instantiate(chickenEnemyPrefab[_chickenIndex], _spawnPos.position, _spawnRot.rotation);

    //    spawnnedChickenEnemiesData enemySpawnData = new spawnnedChickenEnemiesData();
    //    enemySpawnData.spawnnedChickenGO = enemySpawn;

    //    ValidNavPoints closestNavPoint = FindClosestNavPoint(enemySpawn.transform, _CNM.NavPoints);
    //    enemySpawnData.targetLocation = closestNavPoint;

    //    spawnnedChickenEnemies.Add(enemySpawnData);
    //    enemySpawn.GetComponent<ChickenEnermy>().setup();

    //    print("Enemy Count is " + GetEnemyCount());
    //}

    //public void SpawnChickenEnemyRandom()
    //{
    //    Transform rndTrans = GetRandomSpawnPos();
    //    GameObject rndPrefab = GetRandomChickenEnermy();

    //    GameObject enemySpawn = Instantiate(rndPrefab, rndTrans.position, rndTrans.rotation);

    //    spawnnedChickenEnemiesData enemySpawnData = new spawnnedChickenEnemiesData();
    //    enemySpawnData.spawnnedChickenGO = enemySpawn;

    //    ValidNavPoints closestNavPoint = FindClosestNavPoint(enemySpawn.transform, _CNM.NavPoints);
    //    enemySpawnData.targetLocation = closestNavPoint;

    //    spawnnedChickenEnemies.Add(enemySpawnData);
    //    enemySpawn.GetComponent<ChickenEnermy>().setup();

    //    print("Enemy Count is " + GetEnemyCount());
    //}

    private int GetEnemyCount() => spawnnedChickenEnemies.Count;


    public Transform GetRandomSpawnPos() => chickenSpawnPoints[UnityEngine.Random.Range(0, chickenSpawnPoints.Length)];


    public GameObject GetRandomChickenEnermy() => chickenEnemyPrefab[UnityEngine.Random.Range(0, chickenEnemyPrefab.Length)];


    private void GameEvents_OnChickenEnermyDie(GameObject _deadChickenGO)
    {
        print("Event Succsful");
        spawnnedChickenEnemiesData _spawnnedChickenEnemiesData = null;

        foreach (spawnnedChickenEnemiesData _spanwnChickEnemyData in spawnnedChickenEnemies)
        {
            if (_spanwnChickEnemyData.spawnnedChickenGO == _deadChickenGO)
            {
                _spawnnedChickenEnemiesData = _spanwnChickEnemyData;
                break;
            }
        }

        if (_spawnnedChickenEnemiesData != null)
        {
            Destroy(_spawnnedChickenEnemiesData.spawnnedChickenGO);
            spawnnedChickenEnemies.Remove(_spawnnedChickenEnemiesData);
        }
        _UIM.UpdateEnemies(GetEnemyCount() , spawnAmout);

    }

    public void KillAllChickenEnemies()
    {
        // Make a copy of the list to avoid modification during iteration
        List<spawnnedChickenEnemiesData> _chickensToRemove = new List<spawnnedChickenEnemiesData>(spawnnedChickenEnemies);

        print($"Chickens to remove count = {_chickensToRemove.Count}");

        foreach (spawnnedChickenEnemiesData _chickenEnemiesData in _chickensToRemove)
        {
            // Debugging statement to see which chicken is being processed
            Debug.Log($"Processing chicken: {_chickenEnemiesData.spawnnedChickenGO.name}");

            // Ensure the object is not null before calling the method
            if (_chickenEnemiesData != null && _chickenEnemiesData.ChickenEnemyScript != null)
            {
                _chickenEnemiesData.ChickenEnemyScript.EnemyDie();
            }
            else
            {
                Debug.LogWarning("Chicken enemy script is null or chicken enemy data is null.");
            }
        }

        // After processing, clear the original list if needed
        spawnnedChickenEnemies.Clear();
    }

    private void OnEnable()
    {
        GameEvents.OnChickenEnemyDie += GameEvents_OnChickenEnermyDie;
    }



    private void OnDisable()
    {
        GameEvents.OnChickenEnemyDie -= GameEvents_OnChickenEnermyDie;
    }
}
