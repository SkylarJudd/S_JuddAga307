using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ChickenEnemyType { Base,Flying,Boomer}
public enum ChickenNavType { Linear,Random, Loop }
public class ChickenEnermyMannager : Singleton<ChickenEnermyMannager>
{
    public GameObject[] chickenEnemyPrefab;
    public Transform[] chickenSpawnPoints;

    public List<spawnnedChickenEnemiesData> spawnnedChickenEnemies;

    [SerializeField] float enermySpawnDelay = 1.0f;
    [SerializeField] int spawnAmout = 6;

    public class spawnnedChickenEnemiesData
    {
        public GameObject spawnnedChickenGO;
        public Transform targetLocation;
        public ChickenNavType chickenNavType;
        public float chickenSpeed;
        public float chickenRotateSpeed;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies(enermySpawnDelay, spawnAmout));
    }

    /// <summary>
    /// Spawns In Chickens With a Delay and a Spawn Amount
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="spawnAmount"></param>
    /// <returns></returns>
    private IEnumerator SpawnEnemies(float seconds, int spawnAmount)
    {

        for (int i = 0; i < spawnAmount; i++)
        {
            SpawnChickenEnermy(chickenSpawnPoints[i], chickenSpawnPoints[i], i);
            yield return new WaitForSeconds(seconds);
        }

    }

    private void SpawnChickenEnermy(Transform _spawnPos, Transform _spawnRot, int _spawnPointIndex)
    {
        GameObject enemySpawn = Instantiate(chickenEnemyPrefab[Random.Range(0, chickenEnemyPrefab.Length)], _spawnPos.position, _spawnRot.rotation);

        spawnnedChickenEnemiesData enemySpawnData = new spawnnedChickenEnemiesData();
        enemySpawnData.spawnnedChickenGO = enemySpawn;

        Transform closestNavPoint = getClosestEnermy(enemySpawn.transform, _CNM.NavPoints);
        enemySpawnData.targetLocation = closestNavPoint;

        spawnnedChickenEnemies.Add(enemySpawnData);
        enemySpawn.GetComponent<ChickenEnermy>().setup(chickenSpawnPoints[_spawnPointIndex]);

        print("Enemy Count is " + GetEnemyCount());
    }

    public void SpawnChickenEnermy(Transform _spawnPos, Transform _spawnRot, int _spawnPointIndex, int _chickenIndex)
    {

        GameObject enemySpawn = Instantiate(chickenEnemyPrefab[_chickenIndex], _spawnPos.position, _spawnRot.rotation);

        spawnnedChickenEnemiesData enemySpawnData = new spawnnedChickenEnemiesData();
        enemySpawnData.spawnnedChickenGO = enemySpawn;

        Transform closestNavPoint = getClosestEnermy(enemySpawn.transform, _CNM.NavPoints);
        enemySpawnData.targetLocation = closestNavPoint;

        spawnnedChickenEnemies.Add(enemySpawnData);
        enemySpawn.GetComponent<ChickenEnermy>().setup(chickenSpawnPoints[_spawnPointIndex]);

        print("Enemy Count is " + GetEnemyCount());
    }

    public void SpawnChickenEnermy()
    {
        Transform rndTrans = GetRandomSpawnPos();
        GameObject rndPrefab = GetRandomChickenEnermy();

        GameObject enemySpawn = Instantiate(rndPrefab, rndTrans.position, rndTrans.rotation);

        spawnnedChickenEnemiesData enemySpawnData = new spawnnedChickenEnemiesData();
        enemySpawnData.spawnnedChickenGO = enemySpawn;

        Transform closestNavPoint = getClosestEnermy(enemySpawn.transform, _CNM.NavPoints);
        enemySpawnData.targetLocation = closestNavPoint;

        spawnnedChickenEnemies.Add(enemySpawnData);
        enemySpawn.GetComponent<ChickenEnermy>().setup(rndTrans);

        print("Enemy Count is " + GetEnemyCount());
    }

    private int GetEnemyCount()
    {
        return spawnnedChickenEnemies.Count;
    }

    public Transform GetRandomSpawnPos()
    {
        return chickenSpawnPoints[Random.Range(0, chickenSpawnPoints.Length)];
    }

    public GameObject GetRandomChickenEnermy()
    {
        return chickenEnemyPrefab[Random.Range(0, chickenEnemyPrefab.Length)];
    }

    
}
