using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
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
        // Pool road, city and obstacles with zero Z offset //FIXME Nepotreban komentar
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

    void PoolRoad(float zOffset) //FIXME Ova funkcija i sledece tri pripadaju ispod SpawnTriggerActivated (jer im je to poslednje mesto poziva u fajlu)
    {
        if (pooledRoad != null)
        {
            previousRoad = pooledRoad;
            pooledRoad = null; //FIXME Nepotrebna naredba, s obzirom da u sledecoj naredbi vec postavljas novu vrednost u pooledRoad
        }

        // Get new pooled road obj, make it active and position it based on last Z position //FIXME Nepotreban komentar, sve je ovo jasno iz koda
        pooledRoad = ObjectPooler.SharedInstance.GetPooledRoad();
        pooledRoad.SetActive(true);
        MoveToNewZPosition(pooledRoad, previousRoad, zOffset);
    }

    void PoolCity(float zOffset) //FIXME Verovatno uocavas i sam da su ove tri funkcije plodne za refaktor jer rade uglavnom istu stvar. Razmisli kako moze da se izdvoji zajednicka logika. Ako se ne snadjes (ili budes hteo sanity check od mene), i o ovome pricamo kad zavrsis ostale komentare.
    {
        if (pooledCity != null)
        {
            previousCity = pooledCity;
            pooledCity = null;
        }

        // Get new pooled city obj, make it active and position it based on last Z position
        pooledCity = ObjectPooler.SharedInstance.GetPooledCity();
        pooledCity.SetActive(true);
        MoveToNewZPosition(pooledCity, previousCity, zOffset);
    }

    void PoolObstacles(float zOffset)
    {
        if (pooledObstacles != null)
        {
            previousObstacles = pooledObstacles;
            pooledObstacles = null;
        }

        // Get new pooled obstacles obj, make it active and position it based on last Z position
        pooledObstacles = ObjectPooler.SharedInstance.GetPooledObstacles();
        pooledObstacles.SetActive(true);
        MoveToNewZPosition(pooledObstacles, previousObstacles, zOffset);
    }

    void MoveToNewZPosition(GameObject pooledObj, GameObject previousPooledObj, float zOffset)
    {
        Vector3 newZPos = new(0, 0, zOffset);

        // If we have previous object, add z offset to that object's position and if we don't, add it to current object position
        pooledObj.transform.position = (previousPooledObj != null) ? previousPooledObj.transform.position + newZPos
            : pooledObj.transform.position + newZPos;
    }

    void DeactivateObject(GameObject obj)
    {
        if(obj != null)
        {
            obj.SetActive(false);
        }
    }
}
