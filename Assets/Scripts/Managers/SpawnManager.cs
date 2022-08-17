using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] Vector3 roadSpawnPos;
    [SerializeField] Vector3 citySpawnPos;
    [SerializeField] Vector3 obstaclesSpawnPos;
    [SerializeField] Vector3 powerUpMinSpawnPos = new(-4, 0.5f, 20);
    [SerializeField] Vector3 powerUpMaxSpawnPos = new(4, 3, 150);
    [SerializeField] Vector3 enemyLeftSpawnPos;
    [SerializeField] Vector3 enemyRightSpawnPos;

    float zLength = 227;

    GameObject pooledRoad;
    GameObject previousRoad;

    GameObject pooledCity;
    GameObject previousCity;

    GameObject pooledObstacles;
    GameObject previousObstacles;

    GameObject pooledPowerUp;
    GameObject previousPowerUp;

    GameObject pooledEnemy;
    GameObject previousEnemy;

    private void OnEnable()
    {
        EventManager.Instance.onIncreasedDifficulty += SpawnNextEnemy;
    }

    void Start()
    {
        GetPooledRoad(Vector3.zero);
        GetPooledCity(Vector3.zero);
        GetPooledObstacles(Vector3.zero);
        GetPooledPowerUp(GetRandomPosition(powerUpMinSpawnPos, powerUpMaxSpawnPos));
    }

    public void SpawnNextEnemy()
    {
        GetPooledEnemy(GetRandomPosition(enemyLeftSpawnPos,enemyRightSpawnPos));
    }

    public void SpawnNextMapObstaclesAndPowerUps()
    {
        GetPooledRoad(roadSpawnPos);
        GetPooledCity(citySpawnPos);
        GetPooledObstacles(obstaclesSpawnPos);
        GetPooledPowerUp(GetRandomPosition(powerUpMinSpawnPos, powerUpMaxSpawnPos));
    }

    void GetPooledRoad(Vector3 newPos)
    {
        PoolObject(ObjectType.Road, ref pooledRoad, ref previousRoad, newPos);
    }

    void GetPooledCity(Vector3 newPos)
    {
        PoolObject(ObjectType.City, ref pooledCity, ref previousCity, newPos);
    }

    void GetPooledObstacles(Vector3 newPos)
    {
        PoolObject(ObjectType.Obstacles, ref pooledObstacles, ref previousObstacles, newPos);
    }

    void GetPooledPowerUp(Vector3 newPos)
    {
        PoolObject(ObjectType.PowerUp, ref pooledPowerUp, ref previousPowerUp, newPos);
    }

    void GetPooledEnemy(Vector3 newPos)
    {
        PoolObject(ObjectType.Enemy, ref pooledEnemy, ref previousEnemy, newPos);
    }

    void PoolObject(ObjectType objType, ref GameObject pooledObject, ref GameObject previousObject, Vector3 newPos)
    {
        AssignPreviousObject(ref pooledObject, ref previousObject);
        pooledObject = ObjectPooler.Instance.GetPooledObject(objType);
        pooledObject.SetActive(true);
        PositionPooledObject(objType, pooledObject, previousObject, newPos);
    }

    void AssignPreviousObject(ref GameObject pooledObj, ref GameObject previousObj)
    {
        if (pooledObj != null)
        {
            previousObj = pooledObj;
        }
    }

    void PositionPooledObject(ObjectType objType, GameObject pooledObj, GameObject previousObj, Vector3 newPos)
    {
        if (objType == ObjectType.PowerUp)
        {
            if (previousObj != null)
            {
                pooledObj.transform.position = new Vector3(newPos.x, newPos.y, zLength + newPos.z);
            }
            else
            {
                pooledObj.transform.position += newPos;
            }
        }
        else if (objType == ObjectType.Enemy)
        {
            pooledObj.transform.position = newPos;
        }
        else
        {
            // If we have previous object, add newPos vector to that object's position and if we don't, add it to current object position
            pooledObj.transform.position = (previousObj != null) ? previousObj.transform.position + newPos
                : pooledObj.transform.position + newPos;
        }
    }

    Vector3 GetRandomPosition(Vector3 minSpawnPos, Vector3 maxSpawnPos)
    {
        float xPos = Random.Range(minSpawnPos.x, maxSpawnPos.x);
        float yPos = Random.Range(minSpawnPos.y, maxSpawnPos.y);
        float zPos = Random.Range(minSpawnPos.z, maxSpawnPos.z);

        return new Vector3(xPos, yPos, zPos);
    }

    public void DeactivatePreviousMapAndObstacles()
    {
        DeactivateObject(previousRoad);
        DeactivateObject(previousCity);
        DeactivateObject(previousObstacles);
        DeactivateObject(previousPowerUp);
    }

    void DeactivateObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
}
