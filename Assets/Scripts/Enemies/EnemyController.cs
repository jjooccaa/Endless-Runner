using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]public Enemy enemy;

    bool movementDisabled = false;
    float magnitude = 4.0f; // Size of sine movement
    Vector3 currentPosition;
    Vector3 axis;

    private void OnEnable()
    {
        EventManager.Instance.onGameOver += DisableMovement;
    }

    void Start()
    {
        transform.localScale *= enemy.scale;
        currentPosition = transform.position;
        axis = transform.right;

        Physics.IgnoreLayerCollision(7, 8); // Ignore collision with obstacles
    }

    void Update()
    {
        if (!movementDisabled)
        {
            Move();
        }

        Deactivate();
    }

    void Move()
    {
        if (enemy.canMoveSideways)
        {
            MoveZigZag();
        } 
        else
        {
            transform.Translate(Vector3.back * Time.deltaTime * (enemy.speed * GameManager.Instance.MovementSpeed));
        }
    }

    void MoveZigZag()
    {
        currentPosition += Vector3.back * Time.deltaTime * (enemy.speed * GameManager.Instance.MovementSpeed);
        transform.position = currentPosition + axis * Mathf.Sin(Time.time * enemy.sidewaysSpeed) * magnitude;
    }

    void Deactivate()
    {
        if (transform.position.z < -20)
        {
            gameObject.SetActive(false);
        }
    }

    void DisableMovement()
    {
        movementDisabled = true;
    }
}
