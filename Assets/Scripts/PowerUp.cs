using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PowerUp : MonoBehaviour
{
    [Range(1, 3)]
    [Tooltip("Invisibility = 1, Extra Life = 2, Fly = 3")]
    [SerializeField] int powerUpID;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ActivatePowerUp(this.gameObject, powerUpID); //FIXME ovde se verv moze iskoristiti event
        }
    }

}
