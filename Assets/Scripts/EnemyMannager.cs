
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyType
{
    OneHanded,
    TwoHanded,
    Archer,
}

public enum PotrolType
{
    Linear,
    Random, 
    Loop,
}

public class EnemyMannager : Singleton<EnemyMannager>
{
    public GameObject[] enemyPrefab;
    public Transform[] spawnPoints;

    public List<GameObject> spawnnedEnemies;

    [SerializeField] static float enermySpawnDelay = 1.0f;
    [SerializeField] static int spawnAmout = 6;

    public string killsCondition = "archer";



    #region Start
    private void Start()
    {
        //SpawnEnemiesAtOnePoint();
        //SpawnSnemiesAtAllPoints();
        //SpawnEnemiesWithForLoop();
        StartCoroutine(SpawnEnimiesWithEnumerator(enermySpawnDelay, spawnAmout));

    }
    #endregion

    #region Update
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K) && GetEnemyCount() != 0) { KillEnemy(spawnnedEnemies[0]); }

        if (Input.GetKeyUp(KeyCode.L) && GetEnemyCount() != 0) { KillSpacificEnemy(killsCondition); }

        if (Input.GetKeyDown(KeyCode.P)) { KillAllEnemies(); }
    }
    #endregion
    #region OutDatedFunctions
    //private void SpawnEnemiesAtOnePoint()
    //{
    //    int randomSpawn = Random.Range(0, spawnPoints.Length);
    //    SpawnEnermy(spawnPoints[randomSpawn], spawnPoints[randomSpawn]);
    //}

    //private void SpawnEnemiesAtAllPoints()
    //{
    //    foreach (Transform _SP in spawnPoints)
    //    {
    //        SpawnEnermy(_SP, _SP);
    //    }
    //}
    #endregion

    private void SpawnEnemiesWithForLoop()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnEnermy(spawnPoints[i], spawnPoints[i], i);
        }
    }

    /// <summary>
    /// Spawns In Enemies With A delay Until we reach our spawn amout
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="spawnAmount"></param>
    private IEnumerator SpawnEnimiesWithEnumerator(float seconds, int spawnAmount)
    {

        for (int i = 0; i < spawnAmount; i++)
        {
            SpawnEnermy(spawnPoints[i], spawnPoints[i], i);
            yield return new WaitForSeconds(seconds);
        }

    }

    /// <summary>
    /// This spawns a random enemy from  enemyPrefab array, using two inputs for the position and rotaion
    /// </summary>
    /// <param name="_spawnPos"></param>
    /// <param name="_spawnRot"></param>
    private void SpawnEnermy(Transform _spawnPos, Transform _spawnRot, int i)
    {
        GameObject enemySpawn = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], _spawnPos.position, _spawnRot.rotation);
        spawnnedEnemies.Add(enemySpawn);
        enemySpawn.GetComponent<Enemy>().setup(spawnPoints[i]);



        print("Enemy Count is " + GetEnemyCount());
    }

    /// <summary>
    /// gets the amount of enemies in the scene
    /// </summary>
    /// <returns> amount of enemies </returns>
    private int GetEnemyCount()
    {
        return spawnnedEnemies.Count;
    }

    /// <summary>
    /// kills an inputed Enemy
    /// </summary>
    /// <param name="_enemy"></param>
    public void KillEnemy(GameObject _enemy)
    {
        if (GetEnemyCount() == 0)
            return;

        Destroy(_enemy);
        spawnnedEnemies.Remove(_enemy);


    }
    /// <summary>
    /// Kills a ramdom Enemy
    /// </summary>
    private void KillRandomEnemy()
    {
        if (GetEnemyCount() == 0)
            return;

        KillEnemy(spawnnedEnemies[Random.Range(0, GetEnemyCount())]);
    }

    /// <summary>
    /// Kills An Enemy By name
    /// </summary>
    /// <param name="_condition"></param>

    private void KillSpacificEnemy(string _condition)
    {
        for (int i = 0; i < spawnnedEnemies.Count; i++)
        {
            if (spawnnedEnemies[i].name.Contains(_condition))
            {
                KillEnemy(spawnnedEnemies[i]);
            }
        }
    }

    /// <summary>
    /// kills all enemies in the spawnned Enemies list
    /// </summary>

    public void KillAllEnemies()
    {
        if (GetEnemyCount() == 0)
            return;

        int totalEnemies = GetEnemyCount();

        for (int i = spawnnedEnemies.Count - 1; i >= 0; i--)
        {
            KillEnemy(spawnnedEnemies[i]);
        }
    }
    /// <summary>
    /// Gets a random Spawn Point from the enemyMannager
    /// </summary>
    public Transform GetRandomSpawnPos()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    /// <summary>
    /// gets a Specific spawn point based on the pased in int
    /// </summary>
    /// <param name="_spawnPoint">The Posiion of the spawn point in the array</param>
    public Transform GetSpawnPoint(int _spawnPoint)
    {
        return spawnPoints[_spawnPoint];
    }

    /// <summary>
    /// gets the total Number of our spawn Points
    /// </summary>
    public int GetSpawnPointsCount() =>  spawnPoints.Length;

    private void GameEvents_OnEnermyDie(GameObject obj)
    {
        KillEnemy(obj);
    }

    private void OnEnable()
    {
        GameEvents.OnEnermyDie += GameEvents_OnEnermyDie;
    }

   

    private void OnDisable()
    {
        GameEvents.OnEnermyDie -= GameEvents_OnEnermyDie;
    }
}
