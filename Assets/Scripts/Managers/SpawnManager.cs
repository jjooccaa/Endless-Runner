using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] Vector3 roadSpawnPos;
    [SerializeField] Vector3 citySpawnPos;
    [SerializeField] Vector3 obstaclesSpawnPos;

    float zLength = 227;

    float powerUXMinSpawnPos = -4; //FIXME Mozda je za buducnost bolje da to budu prazni objekti kojima ces uzimati vector, samo iz razloga ako hoces da promenis poziciju mozda je lakse da samo pomeris taj obj i ona automatski iscita vrednost
    float powerUpXMaxSpawnPos = 4;
    float powerUpYMinSpawnPos = 0.5f;
    float powerUpYMaxSpawnPos = 3;
    float powerUpZMinSpawnPos = 20;
    float powerUpZMaxSpawnPos = 150;

    GameObject pooledRoad;
    GameObject previousRoad;

    GameObject pooledCity;
    GameObject previousCity;

    GameObject pooledObstacles;
    GameObject previousObstacles;

    GameObject pooledPowerUp;
    GameObject previousPowerUp;

    // Start is called before the first frame update
    void Start()
    {
        PoolRoad(Vector3.zero);
        PoolCity(Vector3.zero);
        PoolObstacles(Vector3.zero);
        PoolPowerUp(GetRandomPosition(powerUXMinSpawnPos, powerUpXMaxSpawnPos, powerUpYMinSpawnPos, powerUpYMaxSpawnPos, powerUpZMinSpawnPos, powerUpZMaxSpawnPos));
    }

    public void SpawnNextMapObstaclesAndPowerUps()
    {
        PoolRoad(roadSpawnPos);
        PoolCity(citySpawnPos);
        PoolObstacles(obstaclesSpawnPos);
        PoolPowerUp(GetRandomPosition(powerUXMinSpawnPos, powerUpXMaxSpawnPos, powerUpYMinSpawnPos, powerUpYMaxSpawnPos, powerUpZMinSpawnPos, powerUpZMaxSpawnPos));
    }

    void PoolRoad(Vector3 newPos)
    {
        PoolObject(ObjectType.Road, ref pooledRoad, ref previousRoad, newPos);
    }

    void PoolCity(Vector3 newPos)
    {
        PoolObject(ObjectType.City, ref pooledCity, ref previousCity, newPos);
    }

    void PoolObstacles(Vector3 newPos)
    {
        PoolObject(ObjectType.Obstacles, ref pooledObstacles, ref previousObstacles, newPos);
    }

    void PoolPowerUp(Vector3 newPos)
    {
        PoolObject(ObjectType.PowerUp, ref pooledPowerUp, ref previousPowerUp, newPos);
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
        else
        {
            // If we have previous object, add newPos vector to that object's position and if we don't, add it to current object position
            pooledObj.transform.position = (previousObj != null) ? previousObj.transform.position + newPos
                : pooledObj.transform.position + newPos;
        }
    }

    Vector3 GetRandomPosition(float minXPos, float maxXPos, float minYPos, float maxYpos, float minZPos, float maxZPos)
    {
        float xPos = Random.Range(minXPos, maxXPos);
        float yPos = Random.Range(minYPos, maxYpos);
        float zPos = Random.Range(minZPos, maxZPos);

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
