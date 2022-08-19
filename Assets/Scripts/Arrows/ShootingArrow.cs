using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingArrow : Arrow
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagName.ENEMY_TAG))
        {
            Deactivate();
            EventManager.Instance.onArrowHit?.Invoke(collision.gameObject); // Deactivate enemy on hit
        }
        else
        {
            Deactivate();
        }
    }
}
