using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpArrow : PickUps
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.PLAYER_TAG))
        {
            Deactivate();
            EventManager.Instance.onArrowPickUp?.Invoke();
        }
    }
}
