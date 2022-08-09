using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    [Header("Roads")]
    [SerializeField] int numberOfRoadsToPool;
    [SerializeField] GameObject[] roadPrefabs;
    [SerializeField] List<GameObject> pooledRoads;

    [Header("Cities")]
    [SerializeField] int numberOfCitiesToPool;
    [SerializeField] GameObject[] cityPrefabs;
    [SerializeField] List<GameObject> pooledCities;

    [Header("Obstacles")]
    [SerializeField] int numberOfObstaclesToPool;
    [SerializeField] GameObject[] obstaclesPrefabs;
    [SerializeField] List<GameObject> pooledObstacles;

    private void Awake()
    {
        SharedInstance = this;

        pooledRoads = new List<GameObject>();
        PoolObjects(roadPrefabs, pooledRoads, numberOfRoadsToPool);

        pooledCities = new List<GameObject>();
        PoolObjects(cityPrefabs, pooledCities, numberOfCitiesToPool);

        pooledObstacles = new List<GameObject>();
        PoolObjects(obstaclesPrefabs, pooledObstacles, numberOfObstaclesToPool);
    }

    void PoolObjects(GameObject[] objectPrefabs, List<GameObject> pooledObjects, int numberOfObjectsToPool)
    {
        //Loop through list of pooled objects, deactivating them and adding them to the list
        int indexOfPrefabs = 0;
        for (int i = 0; i < numberOfObjectsToPool; i++, indexOfPrefabs++)
        {
            if(indexOfPrefabs + 1 > objectPrefabs.Length) // if we reach end of Objects Prefabs, reset index
            {
                indexOfPrefabs = 0;
            }
            GameObject obj = Instantiate(objectPrefabs[indexOfPrefabs], objectPrefabs[indexOfPrefabs].transform.position, objectPrefabs[indexOfPrefabs].transform.rotation);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager 
        }
    }

    public GameObject GetPooledRoad()
    {

        return GetRandomPoolOjbect(pooledRoads);
    }

    public GameObject GetPooledCity()
    {

       return GetRandomPoolOjbect(pooledCities);
    }

    public GameObject GetPooledObstacles()
    {

        return GetRandomPoolOjbect(pooledObstacles);
    }

    GameObject GetRandomPoolOjbect(List<GameObject> pooledObjects)
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
        for(int i = 0; i < objectsList.Count; i++)
        {
            if(!objectsList[i].activeInHierarchy)
            {

                return true;
            }
        }

        return false;
    }
}
