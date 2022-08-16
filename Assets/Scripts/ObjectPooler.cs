using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    None = 0,
    Road = 1,
    City = 2,
    Obstacles = 3,
    PowerUp = 4,
    Enemy = 5
}
public class ObjectPooler : Singleton<ObjectPooler>
{
    [Header("Roads")]
    [SerializeField] int numberOfRoadsToPool;
    [SerializeField] GameObject[] roadPrefabs;
    [SerializeField] List<GameObject> pooledRoads = new List<GameObject>();

    [Header("Cities")]
    [SerializeField] int numberOfCitiesToPool;
    [SerializeField] GameObject[] cityPrefabs;
    [SerializeField] List<GameObject> pooledCities = new List<GameObject>();

    [Header("Obstacles")]
    [SerializeField] int numberOfObstaclesToPool;
    [SerializeField] GameObject[] obstaclesPrefabs;
    [SerializeField] List<GameObject> pooledObstacles = new List<GameObject>();

    [Header("Power Ups")]
    [SerializeField] int numberOfPowerUpsToPool;
    [SerializeField] GameObject[] powerUpPrefabs;
    [SerializeField] List<GameObject> pooledPowerUps = new List<GameObject>();

    [Header("Enemies")]
    [SerializeField] int numberOfEnemiesToPool;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] List<GameObject> pooledEnemies = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();

        PoolObjects(roadPrefabs, pooledRoads, numberOfRoadsToPool);
        PoolObjects(cityPrefabs, pooledCities, numberOfCitiesToPool);
        PoolObjects(obstaclesPrefabs, pooledObstacles, numberOfObstaclesToPool);
        PoolObjects(powerUpPrefabs, pooledPowerUps, numberOfPowerUpsToPool);
        PoolObjects(enemyPrefabs, pooledEnemies, numberOfEnemiesToPool);
    }

    void PoolObjects(GameObject[] objectPrefabs, List<GameObject> pooledObjects, int numberOfObjectsToPool)
    {
        //Loop through list of pooled objects, deactivating them and adding them to the list
        for (int i = 0; i < numberOfObjectsToPool; i++)
        {
            GameObject obj;
            if (i < objectPrefabs.Length)
            {
                obj = Instantiate(objectPrefabs[i], objectPrefabs[i].transform.position, objectPrefabs[i].transform.rotation);
            }
            else
            {
                int objectindex = Random.Range(0, objectPrefabs.Length);
                obj = Instantiate(objectPrefabs[objectindex], objectPrefabs[objectindex].transform.position, objectPrefabs[objectindex].transform.rotation);
            }

            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager 
        }
    }

    public GameObject GetPooledObject(ObjectType objType)
    {
        switch (objType)
        {
            case ObjectType.Road:
                return GetRandomPoolObject(pooledRoads);

            case ObjectType.City:
                return GetRandomPoolObject(pooledCities);

            case ObjectType.Obstacles:
                return GetRandomPoolObject(pooledObstacles);

            case ObjectType.PowerUp:
                return GetRandomPoolObject(pooledPowerUps);

            case ObjectType.Enemy:
                return GetRandomPoolObject(pooledEnemies);

            case ObjectType.None:
                break;
        }

        return null;
    }

    GameObject GetRandomPoolObject(List<GameObject> pooledObjects)
    {
        while (CheckForInactiveObjects(pooledObjects))
        {
            int randomIndex = Random.Range(0, pooledObjects.Count);

            if (!pooledObjects[randomIndex].activeInHierarchy)
            {

                return pooledObjects[randomIndex];
            }
        }

        return null;
    }

    bool CheckForInactiveObjects(List<GameObject> objectsList)
    {
        foreach (GameObject obj in objectsList)
        {
            if (!obj.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }
}
