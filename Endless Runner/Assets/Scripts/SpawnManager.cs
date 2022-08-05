using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    Spawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<Spawner>();

        // Spawn random road, city and obstacles at start
        spawner.SpawnCity();
        spawner.SpawnRoad();
        spawner.SpawnObstacles();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTriggerActivated()
    {
        spawner.SpawnCity();
        spawner.SpawnRoad();
        spawner.SpawnObstacles();
    }

    public void RemoveTriggerActivated()
    {
        spawner.RemoveRoad();
        spawner.RemoveObstacles();
        spawner.RemoveCity();
    }
}
