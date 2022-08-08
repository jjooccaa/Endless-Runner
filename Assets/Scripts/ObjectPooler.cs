using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    [Header("Roads")]
    [SerializeField] GameObject[] roadsToPool;
    [SerializeField] List<GameObject> pooledRoads;

    [Header("Cities")]
    [SerializeField] GameObject[] citiesToPool;
    [SerializeField] List<GameObject> pooledCities;

    [Header("Obstacles")]
    [SerializeField] GameObject[] obstaclesToPool;
    [SerializeField] List<GameObject> pooledObstacles;

    private void Awake()
    {
        SharedInstance = this;

        pooledRoads = new List<GameObject>();
        PoolObjects(roadsToPool, pooledRoads);

        pooledCities = new List<GameObject>();
        PoolObjects(citiesToPool, pooledCities);

        pooledObstacles = new List<GameObject>();
        PoolObjects(obstaclesToPool, pooledObstacles);
    }

    void PoolObjects(GameObject[] objectsToPool, List<GameObject> pooledObjects)
    {
        //Loop through list of pooled objects, deactivating them and adding them to the list
        for (int i = 0; i < objectsToPool.Length; i++)
        {
            GameObject obj = Instantiate(objectsToPool[i],objectsToPool[i].transform.position,objectsToPool[i].transform.rotation);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }
    }

    public GameObject GetPooledRoad()
    {
        int randomIndex;

        while (true)
        {
            randomIndex = Random.Range(0, pooledRoads.Count);

            if (!pooledRoads[randomIndex].activeInHierarchy)
            {

                break;
            }
        }

        return pooledRoads[randomIndex];
    }

    public GameObject GetPooledCity()
    {
        int randomIndex;

        while (true)
        {
            randomIndex = Random.Range(0, pooledCities.Count);

            if (!pooledCities[randomIndex].activeInHierarchy)
            {

                break;
            }
        }

        return pooledCities[randomIndex];
    }

    public GameObject GetPooledObstacles()
    {
        int randomIndex;

        while (true)
        {
            randomIndex = Random.Range(0, pooledObstacles.Count);

            if (!pooledObstacles[randomIndex].activeInHierarchy)
            {

                break;
            }

        }

        return pooledObstacles[randomIndex];
    }
}
