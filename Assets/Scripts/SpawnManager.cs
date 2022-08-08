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
        // Get new pooled road, make it active and position it based on last Z position
        pooledRoad = ObjectPooler.SharedInstance.GetPooledRoad();
        pooledRoad.SetActive(true);
        pooledRoad.transform.position = (previousRoad != null) ? NewZPosition(previousRoad.transform.position, zOffset)
            : NewZPosition(pooledRoad.transform.position, zOffset);
    }

    void PoolCity(float zOffset)
    {
        if (pooledCity != null)
        {
            previousCity = pooledCity;
        }

        pooledCity = ObjectPooler.SharedInstance.GetPooledCity();
        pooledCity.SetActive(true);
        pooledCity.transform.position = (previousCity != null) ? NewZPosition(previousCity.transform.position, zOffset)
            : NewZPosition(pooledCity.transform.position, zOffset);
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
        pooledObstacles.transform.position = (previousObstacles != null) ? NewZPosition(previousObstacles.transform.position, zOffset)
            : NewZPosition(pooledObstacles.transform.position, zOffset);
    }    
    
    void DeactivateObject(GameObject obj)
    {
        if(obj != null)
        {
            obj.SetActive(false);
        }
    }

    Vector3 NewZPosition(Vector3 oldPosition, float zOffset)
    {

        return oldPosition + new Vector3(0, 0, zOffset);
    }
}
