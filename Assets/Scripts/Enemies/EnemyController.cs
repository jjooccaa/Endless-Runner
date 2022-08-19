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

    private void OnEnable()
    {
        EventManager.Instance.onGameOver += DisableMovement;
        EventManager.Instance.onPlayerCrash += PlayAttackAnimation;
    }

    void Start()
    {
        transform.localScale *= enemy.scale;
        currentPosition = transform.position;
        axis = transform.right;

        animator = gameObject.GetComponent<Animator>();

        Physics.IgnoreLayerCollision(8, 7); // Ignore collision with obstacles
        Physics.IgnoreLayerCollision(8, 9); // Ignore collision with pick ups
    }

    void Update()
    {
        if (!movementDisabled)
        {
            Move();
        }
        DeactivateOnOutOfBounds();
    }

    void Move()
    {
        if (enemy.canMoveSideways)
        {
            MoveZigZag();
        } 
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * (enemy.speed * GameManager.Instance.MovementSpeed));
        }
    }

    void MoveZigZag()
    {
        currentPosition += Vector3.back * Time.deltaTime * (enemy.speed * GameManager.Instance.MovementSpeed);
        transform.position = currentPosition + axis * Mathf.Sin(Time.time * enemy.sidewaysSpeed) * magnitude;
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
}
