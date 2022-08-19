using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [Header("Road")]
    [SerializeField] Vector3 roadSpawnPos;

    [Header("City")]
    [SerializeField] Vector3 citySpawnPos;

    [Header("Obstacles")]
    [SerializeField] Vector3 obstaclesSpawnPos;

    [Header("Power Ups")]
    [SerializeField] Vector3 powerUpMinSpawnPos = new(-4, 0.5f, 20);
    [SerializeField] Vector3 powerUpMaxSpawnPos = new(4, 3, 150);

    [Header("Enemies")]
    [SerializeField] Vector3 enemyLeftSpawnPos;
    [SerializeField] Vector3 enemyRightSpawnPos;

    [Header("Arrows")]
    [SerializeField] int amountOfPickUpArrowsToSpawn = 5;
    [SerializeField] Vector3 pickUpArrowsMinSpawnPos;
    [SerializeField] Vector3 pickUpArrowsMaxSpawnPos;
    [SerializeField] Vector3 shootingArrrowOffset;

    float zOffset = 167;

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

    GameObject pooledShootingArrow;
    List<GameObject> pooledPickApArrows = new List<GameObject>();
    List<GameObject> previousPickUpArrows = new List<GameObject>();
    GameObject pooledPickUpArrow;

    private void OnEnable()
    {
        EventManager.Instance.onIncreasedDifficulty += SpawnNextEnemy;
        EventManager.Instance.onSpawnTrigger += SpawnNextMapObstaclesAndPowerUps;
        EventManager.Instance.onSpawnTrigger += SpawnNextPickUpArrows;
        EventManager.Instance.onRemoveTrigger += DeactivatePreviousMapAndObstacles;
        EventManager.Instance.onRemoveTrigger += DeactivatePreviousPickUpArrows;
        EventManager.Instance.onSpawnShootingArrow = SpawnNextShootingArrow;
        EventManager.Instance.onArrowHit += DeactivateObject;
    }

    void Start()
    {
        GetPooledRoad(Vector3.zero);
        GetPooledCity(Vector3.zero);
        GetPooledObstacles(Vector3.zero);
        GetPooledPowerUp(GetRandomPosition(powerUpMinSpawnPos, powerUpMaxSpawnPos));
        SpawnNextPickUpArrows();
    }

    void SpawnNextEnemy()
    {
        GetPooledEnemy(GetRandomPosition(enemyLeftSpawnPos, enemyRightSpawnPos));
    }

    void SpawnNextMapObstaclesAndPowerUps()
    {
        GetPooledRoad(roadSpawnPos);
        GetPooledCity(citySpawnPos);
        GetPooledObstacles(obstaclesSpawnPos);
        GetPooledPowerUp(GetRandomPosition(powerUpMinSpawnPos, powerUpMaxSpawnPos));
    }

    void SpawnNextPickUpArrows()
    {
        previousPickUpArrows.AddRange(pooledPickApArrows);
        pooledPickApArrows.Clear();
        for (int i = 0; i < amountOfPickUpArrowsToSpawn; i++)
        {
            GetPooledPickUpArrow(GetRandomPosition(pickUpArrowsMinSpawnPos,pickUpArrowsMaxSpawnPos));
        }
    }

    void SpawnNextShootingArrow(Vector3 pos)
    {
        GetPooledShootingArrow(pos);
        EventManager.Instance.onShoot?.Invoke(pooledShootingArrow);
    }

    void GetPooledRoad(Vector3 newPos)
    {
        GetPooledObject(ObjectType.Road, ref pooledRoad, ref previousRoad, newPos);
    }

    void GetPooledCity(Vector3 newPos)
    {
        GetPooledObject(ObjectType.City, ref pooledCity, ref previousCity, newPos);
    }

    void GetPooledObstacles(Vector3 newPos)
    {
        GetPooledObject(ObjectType.Obstacles, ref pooledObstacles, ref previousObstacles, newPos);
    }

    void GetPooledPowerUp(Vector3 newPos)
    {
        GetPooledObject(ObjectType.PowerUp, ref pooledPowerUp, ref previousPowerUp, newPos);
    }

    void GetPooledEnemy(Vector3 newPos)
    {
        GetPooledObject(ObjectType.Enemy, ref pooledEnemy, ref previousEnemy, newPos);
    }

    void GetPooledPickUpArrow(Vector3 newPos)
    {
        pooledPickApArrows.Add(pooledPickUpArrow);
        pooledPickUpArrow = ObjectPooler.Instance.GetPooledObject(ObjectType.PickUpArrow);
        pooledPickUpArrow.SetActive(true);
        if (previousPickUpArrows.Count > 0) 
        {
            pooledPickUpArrow.transform.position = new Vector3(newPos.x, newPos.y, newPos.z + zOffset);
        } else
        {
            pooledPickUpArrow.transform.position = newPos;
        }
    }

    void GetPooledShootingArrow(Vector3 newPos)
    {
        GetPooledObject(ObjectType.ShootingArrow, ref pooledShootingArrow, newPos);
        pooledShootingArrow.transform.position = newPos + shootingArrrowOffset;
    }

    void GetPooledObject(ObjectType objType, ref GameObject pooledObject, ref GameObject previousObject, Vector3 newPos)
    {
        AssignPreviousObject(ref pooledObject, ref previousObject);
        pooledObject = ObjectPooler.Instance.GetPooledObject(objType);
        pooledObject.SetActive(true);
        PositionPooledObject(objType, pooledObject, previousObject, newPos);
    }

    void GetPooledObject(ObjectType objType, ref GameObject pooledObject, Vector3 newPos)
    {
        pooledObject = ObjectPooler.Instance.GetPooledObject(objType);
        pooledObject.SetActive(true);
        PositionPooledObject(objType, pooledObject, null, newPos);
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
                pooledObj.transform.position = new Vector3(newPos.x, newPos.y, zOffset + newPos.z);
            }
            else
            {
                pooledObj.transform.position += newPos;
            }
        }
        else if (objType == ObjectType.Enemy || objType == ObjectType.ShootingArrow)
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

    void DeactivatePreviousMapAndObstacles()
    {
        DeactivateObject(previousRoad);
        DeactivateObject(previousCity);
        DeactivateObject(previousObstacles);
        DeactivateObject(previousPowerUp);
    }

    void DeactivatePreviousPickUpArrows()
    {
        foreach(GameObject obj in previousPickUpArrows)
        {
            DeactivateObject(obj);
        }
        previousPickUpArrows.Clear();
    }

    void DeactivateObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
}
