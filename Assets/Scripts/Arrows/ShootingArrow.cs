using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingArrow : PickUps
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagName.ENEMY_TAG))
        {
            Deactivate();
            EventManager.Instance.onArrowHitEnemy?.Invoke(collision.gameObject);
            EventManager.Instance.onEnemyKilled?.Invoke();
        }
        else
        {
            Deactivate();
        }
    }
}
