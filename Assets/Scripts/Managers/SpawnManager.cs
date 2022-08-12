using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] float zOffset = 227;
    
    GameObject pooledRoad;
    GameObject previousRoad;

    GameObject pooledCity;
    GameObject previousCity;

    GameObject pooledObstacles;
    GameObject previousObstacles;

    // Start is called before the first frame update
    void Start()
    {
        PoolRoad(0);
        PoolCity(0);
        PoolObstacles(0);
    }

    public void SpawnNextMapAndObstacles()
    {
        PoolRoad(zOffset);
        PoolCity(zOffset);
        PoolObstacles(zOffset);
    }

    void PoolRoad(float zOffset) 
    {
        PoolObject(ObjectType.Road, ref pooledRoad, ref previousRoad, zOffset);
    }

    void PoolCity(float zOffset) 
    {
        PoolObject(ObjectType.City, ref pooledCity, ref previousCity, zOffset);
    }

    void PoolObstacles(float zOffset)
    {
        PoolObject(ObjectType.Obstacles, ref pooledObstacles, ref previousObstacles, zOffset);
    }

    void PoolObject(ObjectType objType, ref GameObject pooledObject, ref GameObject previousObject, float zOffset)
    {
        AssignPreviousObject(ref pooledObject, ref previousObject);
        pooledObject = ObjectPooler.Instance.GetPooledObject(objType);
        ActivateAndPositionPooledObject(pooledObject, previousObject, zOffset);
    }

    void AssignPreviousObject(ref GameObject pooledObj, ref GameObject previousObj)
    {
        if (pooledObj != null)
        {
            previousObj = pooledObj;
        }
    }

    void ActivateAndPositionPooledObject(GameObject pooledObj, GameObject previousObj, float zOffset)
    {
        pooledObj.SetActive(true);
        MoveToNewZPosition(pooledObj, previousObj, zOffset);
    }

    void MoveToNewZPosition(GameObject pooledObj, GameObject previousPooledObj, float zOffset)
    {
        Vector3 newZPos = new(0, 0, zOffset);

        // If we have previous object, add z offset to that object's position and if we don't, add it to current object position
        pooledObj.transform.position = (previousPooledObj != null) ? previousPooledObj.transform.position + newZPos
            : pooledObj.transform.position + newZPos;
    }

    public void DeactivatePreviousMapAndObstacles()
    {
        DeactivateObject(previousRoad);
        DeactivateObject(previousCity);
        DeactivateObject(previousObstacles);
    }

    void DeactivateObject(GameObject obj)
    {
        if(obj != null)
        {
            obj.SetActive(false);
        }
    }
}
