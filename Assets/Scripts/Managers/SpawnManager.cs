using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : Singleton<SpawnManager>
{
    [Header("Road")]
    [SerializeField] Vector3 roadSpawnPos;
    GameObject pooledRoad;
    GameObject previousRoad;

    [Header("City")]
    [SerializeField] Vector3 citySpawnPos;
    GameObject pooledCity;
    GameObject previousCity;

    [Header("Obstacles")]
    [SerializeField] Vector3 obstaclesSpawnPos;
    GameObject pooledObstacles;
    GameObject previousObstacles;

    [Header("Power Ups")]
    [SerializeField] Vector3 powerUpMinSpawnPos;
    [SerializeField] Vector3 powerUpMaxSpawnPos;
    GameObject pooledPowerUp;
    GameObject previousPowerUp;

    [Header("Enemies")]
    [SerializeField] Vector3 enemyLeftSpawnPos;
    [SerializeField] Vector3 enemyRightSpawnPos;
    GameObject pooledEnemy;
    GameObject previousEnemy;

    [Header("Coins")]
    [SerializeField] int amountOfCoinsToSpawn = 3;
    [SerializeField] Vector3 coinsMinSpawnPos;
    [SerializeField] Vector3 coinsMaxSpawnPos;
    GameObject pooledCoin;
    List<GameObject> pooledCoins = new List<GameObject>();
    List<GameObject> previousCoins = new List<GameObject>();

    [Header("Arrows")]
    [SerializeField] int amountOfPickUpArrowsToSpawn = 5;
    [SerializeField] Vector3 pickUpArrowsMinSpawnPos;
    [SerializeField] Vector3 pickUpArrowsMaxSpawnPos;
    GameObject pooledPickUpArrow;
    List<GameObject> pooledPickApArrows = new List<GameObject>();
    List<GameObject> previousPickUpArrows = new List<GameObject>();

    [SerializeField] Vector3 shootingArrrowOffset;
    GameObject pooledShootingArrow;

    float zOffset = 167;

    private void OnEnable()
    {
        EventManager.Instance.onIncreasedDifficulty += SpawnNextEnemy;
        EventManager.Instance.onSpawnTrigger += SpawnNextObjects;
        EventManager.Instance.onRemoveTrigger += DeactivatePreviousObjects;
        EventManager.Instance.onSpawnShootingArrow = SpawnNextShootingArrow;
        EventManager.Instance.onArrowHit += DeactivateObject;
    }

    void Start()
    { 
        if (SceneManager.GetSceneByName(SceneName.ENDLESS_RUNNER_GAME).isLoaded)
        {
            GetPooledRoad(Vector3.zero);
            GetPooledCity(Vector3.zero);
            GetPooledObstacles(Vector3.zero);
            SpawnNextPowerUp();
            SpawnNextCoins();
            SpawnNextPickUpArrows();
        }
    }

    void SpawnNextObjects()
    {
        GetPooledRoad(roadSpawnPos);
        GetPooledCity(citySpawnPos);
        GetPooledObstacles(obstaclesSpawnPos);
        SpawnNextPowerUp();
        SpawnNextCoins();
        SpawnNextPickUpArrows();
    }

    void SpawnNextPowerUp()
    {
        if (!ChallengeMode.IsEasyModeActive && !ChallengeMode.IsMediumModeActive)
        {
            GetPooledPowerUp(GetRandomPosition(powerUpMinSpawnPos, powerUpMaxSpawnPos));
        }
    }

    void SpawnNextEnemy()
    {
        if (SceneManager.GetSceneByName(SceneName.ENDLESS_RUNNER_GAME).isLoaded)
        {
            GetPooledEnemy(GetRandomPosition(enemyLeftSpawnPos, enemyRightSpawnPos));
        }
    }

    void SpawnNextCoins()
    {
        previousCoins.AddRange(pooledCoins);
        pooledCoins.Clear();
        for (int i = 0; i < amountOfCoinsToSpawn; i++)
        {
            GetPooledCoin(GetRandomPosition(coinsMinSpawnPos, coinsMaxSpawnPos));
        }
    }

    void SpawnNextPickUpArrows()
    {
        if (!ChallengeMode.IsMediumModeActive)
        {
            previousPickUpArrows.AddRange(pooledPickApArrows);
            pooledPickApArrows.Clear();
            for (int i = 0; i < amountOfPickUpArrowsToSpawn; i++)
            {
                GetPooledPickUpArrow(GetRandomPosition(pickUpArrowsMinSpawnPos, pickUpArrowsMaxSpawnPos));
            }
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

    void GetPooledCoin(Vector3 newPos) 
    {
        GetPooledObject(ObjectType.Coin, ref pooledCoin, previousCoins, newPos);
        pooledCoins.Add(pooledCoin);
    }

    void GetPooledPickUpArrow(Vector3 newPos)
    {
        GetPooledObject(ObjectType.PickUpArrow, ref pooledPickUpArrow, previousPickUpArrows, newPos);
        pooledPickApArrows.Add(pooledPickUpArrow);
    }

    void GetPooledShootingArrow(Vector3 newPos)
    {
        GetPooledObject(ObjectType.ShootingArrow, ref pooledShootingArrow, newPos);
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

    void GetPooledObject(ObjectType objType, ref GameObject pooledObject, List<GameObject> previousObjects, Vector3 newPos)
    {
        pooledObject = ObjectPooler.Instance.GetPooledObject(objType);
        pooledObject.SetActive(true);
        PositionPooledObjectInList(previousObjects, pooledObject, newPos);
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
        else if (objType == ObjectType.Enemy)
        {
            pooledObj.transform.position = newPos;
        }
        else if (objType == ObjectType.ShootingArrow)
        {
            pooledObj.transform.position = newPos + shootingArrrowOffset; 
        }
        else
        {
            // If we have previous object, add newPos vector to that object's position and if we don't, add it to current object position
            pooledObj.transform.position = (previousObj != null) ? previousObj.transform.position + newPos
                : pooledObj.transform.position + newPos;
        }
    }

    void PositionPooledObjectInList(List<GameObject> previousObjects, GameObject pooledObj, Vector3 newPos)
    {
        if (previousObjects.Count > 0)
        {
            pooledObj.transform.position = new Vector3(newPos.x, newPos.y, newPos.z + zOffset);
        }
        else
        {
            pooledObj.transform.position = newPos;
        }
    }

    Vector3 GetRandomPosition(Vector3 minSpawnPos, Vector3 maxSpawnPos)
    {
        float xPos = Random.Range(minSpawnPos.x, maxSpawnPos.x);
        float yPos = Random.Range(minSpawnPos.y, maxSpawnPos.y);
        float zPos = Random.Range(minSpawnPos.z, maxSpawnPos.z);

        return new Vector3(xPos, yPos, zPos);
    }

    void DeactivatePreviousObjects()
    {
        DeactivateObject(previousRoad);
        DeactivateObject(previousCity);
        DeactivateObject(previousObstacles);
        DeactivateObject(previousPowerUp);
        DeactivateObjects(previousCoins);
        DeactivateObjects(previousPickUpArrows);
    }

    void DeactivateObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }

    void DeactivateObjects(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            DeactivateObject(obj);
        }
        objects.Clear();
    }
}
