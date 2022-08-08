using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] float zOffset = 230;
    
    GameObject pooledRoad;
    GameObject previousRoad;

    GameObject pooledCity;
    GameObject previousCity;

    GameObject pooledObstacles;
    GameObject previousObstacles;

    // Start is called before the first frame update
    void Start()
    {
        // Pool road, city and obstacles with zero Z offset
        PoolRoad(0);
        PoolCity(0);
        PoolObstacles(0);
    }

    public void SpawnTriggerActivated()
    {
        PoolRoad(zOffset);
        PoolCity(zOffset);
        PoolObstacles(zOffset);
    }

    public void RemoveTriggerActivated()
    {
        DeactivateObject(previousRoad);
        DeactivateObject(previousCity);
        DeactivateObject(previousObstacles);
    }

    void PoolRoad(float zOffset)
    {
        if (pooledRoad != null)
        {
            previousRoad = pooledRoad;
        }

        pooledRoad = ObjectPooler.SharedInstance.GetPooledRoad();
        pooledRoad.SetActive(true);
        Vector3 newPosition = new Vector3(0, 0, zOffset);
        pooledRoad.transform.position = (previousRoad != null) ? previousRoad.transform.position + newPosition
            : pooledRoad.transform.position + newPosition;
    }

    void PoolCity(float zOffset)
    {
        if (pooledCity != null)
        {
            previousCity = pooledCity;
        }

        pooledCity = ObjectPooler.SharedInstance.GetPooledCity();
        pooledCity.SetActive(true);
        Vector3 newPosition = new Vector3(0, 0, zOffset);
        pooledCity.transform.position = (previousCity != null) ? previousCity.transform.position + newPosition
            : pooledCity.transform.position + newPosition;
    }

    void PoolObstacles(float zOffset)
    {
        if (pooledObstacles != null)
        {
            previousObstacles = pooledObstacles;
            pooledObstacles = null;
        }
        
        pooledObstacles = ObjectPooler.SharedInstance.GetPooledObstacles();
        pooledObstacles.SetActive(true);
        Vector3 newPosition = new Vector3(0, 0, zOffset);
        pooledObstacles.transform.position = (previousObstacles != null) ? previousObstacles.transform.position  + newPosition
            : pooledObstacles.transform.position + newPosition;
    }    
    
    void DeactivateObject(GameObject obj)
    {
        if(obj != null)
        {
            obj.SetActive(false);
        }
    }
}
