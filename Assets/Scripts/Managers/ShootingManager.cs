using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : Singleton<ShootingManager>
{
    [SerializeField] float launchVelocity;

    private void OnEnable()
    {
        EventManager.Instance.onShoot += Launch;
    }

    void Launch(GameObject arrow)
    {
        if (arrow.activeInHierarchy)
        {
            arrow.GetComponent<Rigidbody>().AddForce(Vector3.forward * launchVelocity, ForceMode.Impulse);
        }
        
    }
}
