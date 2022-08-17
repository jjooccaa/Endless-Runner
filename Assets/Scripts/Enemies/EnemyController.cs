using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    bool movementDisabled = false;
    float magnitude = 4.0f; // Size of sine movement
    Vector3 pos;
    Vector3 axis;

    private void OnEnable()
    {
        EventManager.Instance.onGameOver += DisableMovement;
    }

    void Start()
    {
        Physics.IgnoreLayerCollision(7, 8); // Ignore collision with obstacles

        pos = transform.position;
        axis = transform.right;
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
            transform.Translate(Vector3.back * Time.deltaTime * enemy.speed);
        }
    }

    void MoveZigZag()
    {
        pos += Vector3.back * Time.deltaTime * enemy.speed;
        transform.position = pos + axis * Mathf.Sin(Time.time * enemy.sidewaysSpeed) * magnitude;
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
