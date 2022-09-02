using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy enemy;

    bool movementDisabled = false;
    float magnitude = 4.0f; // Size of sine movement
    Vector3 currentPosition;
    Vector3 axis;

    Animator animator;

    const string ATTACK_TRIG = "Attack_trig";
    const string IDLE_TRIG = "Idle_trig";

    private void OnEnable()
    {
        EventManager.Instance.onPlayerCrash += PlayAttackAnimation;
        EventManager.Instance.onEnemySpawn += SetPosition;
        EventManager.Instance.onGameOver += DisableMovement;
        EventManager.Instance.onGameOver += PlayIdleAnimation;
    }

    void Start()
    {
        transform.localScale *= enemy.scale;
        SetPosition();
        animator = gameObject.GetComponent<Animator>();

        Physics.IgnoreLayerCollision(8, 7); // Ignore collision with obstacles
        Physics.IgnoreLayerCollision(8, 9); // Ignore collision with pick ups
        Physics.IgnoreLayerCollision(8, 8); // Ignore collision with other enemies
    }

    void Update()
    {
        if (!movementDisabled)
        {
            Move();
        }
        DeactivateOnOutOfBounds();
    }

    void SetPosition()
    {
        currentPosition = transform.position;
        axis = transform.right;
    }

    void Move()
    {
        if (enemy.canMoveSideways)
        {
            MoveZigZag();
        } 
        else
        {
            MoveStraight();
        }
    }

    void MoveZigZag()
    {
        currentPosition += Vector3.back * Time.deltaTime * (enemy.speed * GameManager.Instance.MovementSpeed);
        transform.position = currentPosition + axis * Mathf.Sin(Time.time * enemy.sidewaysSpeed) * magnitude;
    }

    void MoveStraight()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * (enemy.speed * GameManager.Instance.MovementSpeed));
    }

    void DeactivateOnOutOfBounds()
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

    void PlayAttackAnimation()
    {
        if (gameObject.activeInHierarchy)
        {
            animator.SetTrigger(ATTACK_TRIG);
        }
    }

    void PlayIdleAnimation()
    {
        if (gameObject.activeInHierarchy)
        {
            animator.SetTrigger(IDLE_TRIG);
        }
    }
}
