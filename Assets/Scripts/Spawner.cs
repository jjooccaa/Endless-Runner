using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Roads")]
    [SerializeField] GameObject[] roadPrefabs;
    [SerializeField] List<GameObject> roadsList;

    [Header("Cities")]
    [SerializeField] GameObject[] cityPrefabs;
    [SerializeField] List<GameObject> citiesList;

    [Header("Obstacles")]
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] List<GameObject> obstaclesList;

    [SerializeField] float zOffset;

    int spawnedRoadsCounter;
    int spawnedCitiesCounter;
    int spawnedObstaclesCounter;

    int delayDestroying = 4;

    public void SpawnRoad()
    {
        if (roadsList.Count > 0)
        {
            float newZPos = roadsList[roadsList.Count - 1].transform.position.z + zOffset;
            Vector3 spawnPoint = new Vector3(roadsList[roadsList.Count - 1].transform.position.x, roadsList[roadsList.Count - 1].transform.position.y, newZPos);

            GameObject tempRoad = roadPrefabs[GetRandomIndex(roadPrefabs.Length)];
            GameObject newRoad = Instantiate(tempRoad, spawnPoint, tempRoad.transform.rotation);
            roadsList.Add(newRoad);
        } else
        {
            GameObject tempRoad = roadPrefabs[GetRandomIndex(roadPrefabs.Length)];
            GameObject newRoad = Instantiate(tempRoad, tempRoad.transform.position, tempRoad.transform.rotation);
            roadsList.Add(newRoad);
        }
        
        spawnedRoadsCounter++;
    }

    public void RemoveRoad()
    {
        int index = 0;
        if (roadsList.Count > 2)
        {
            index = spawnedRoadsCounter - 2;
        }

        // Destroy road game object after some time
        Destroy(roadsList[index].gameObject, delayDestroying);
    }

    public void SpawnCity()
    {
        if (roadsList.Count > 0)
        {
            float newZPos = citiesList[citiesList.Count - 1].transform.position.z + zOffset;
            Vector3 spawnPoint = new Vector3(citiesList[citiesList.Count - 1].transform.position.x, citiesList[citiesList.Count - 1].transform.position.y, newZPos);

            GameObject tempCity = cityPrefabs[GetRandomIndex(cityPrefabs.Length)];
            GameObject newCity = Instantiate(tempCity, spawnPoint, tempCity.transform.rotation);
            citiesList.Add(newCity);
        } else
        {
            GameObject tempCity = cityPrefabs[GetRandomIndex(cityPrefabs.Length)];
            GameObject newCity = Instantiate(tempCity, tempCity.transform.position, tempCity.transform.rotation);
            citiesList.Add(newCity);
        }

        spawnedCitiesCounter++;
    }

    public void RemoveCity()
    {
        int index = 0;
        if (citiesList.Count > 2)
        {
            index = spawnedCitiesCounter - 2;
        }

        // Destroy city game object after some time
        Destroy(citiesList[index].gameObject, delayDestroying);
    }

    public void SpawnObstacles()
    {
        if (obstaclesList.Count > 0)
        {
            float newZPos = obstaclesList[obstaclesList.Count - 1].transform.position.z + zOffset;
            Vector3 spawnPoint = new Vector3(obstaclesList[obstaclesList.Count - 1].transform.position.x, obstaclesList[obstaclesList.Count - 1].transform.position.y, newZPos);

            GameObject tempObstacle = obstaclePrefabs[GetRandomIndex(obstaclePrefabs.Length)];
            GameObject newObstacle = Instantiate(tempObstacle, spawnPoint, tempObstacle.transform.rotation);
            obstaclesList.Add(newObstacle);
        } else
        {
            GameObject tempObstacle = obstaclePrefabs[GetRandomIndex(obstaclePrefabs.Length)];
            GameObject newObstacle = Instantiate(tempObstacle, tempObstacle.transform.position, tempObstacle.transform.rotation);
            obstaclesList.Add(newObstacle);
        }

        spawnedObstaclesCounter++;
    }

    public void RemoveObstacles()
    {
        int index = 0;
        if (obstaclesList.Count > 2)
        {
            index = spawnedObstaclesCounter - 2;
        }

        // Destroy obstacles game object after some time
        Destroy(obstaclesList[index].gameObject, delayDestroying);
    }



    private int GetRandomIndex(int max)
    {
        return Random.Range(0, max);
    }

}
