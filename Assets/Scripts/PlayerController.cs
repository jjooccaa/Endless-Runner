using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] public float turnSpeed;
    [SerializeField] public float jumpForce;
    [SerializeField] float horizontalInput;
    [SerializeField] bool isOnGround;
    [SerializeField] float leftBoundary = -4.8f;
    [SerializeField] float rightBoundary = 4.8f;
    [SerializeField] public bool movementDisabled = false;

    Rigidbody rigidBody;
    Animator animator;

    private const string HORIZONTAL = "Horizontal";
    private const string JUMP_ANIM_TRIG = "Jump_trig";
    private const string STUMBLE_ANIM_TRIG = "Stumble_trig";

    private void OnEnable()
    {
        EventManager.Instance.onGameOver += DisableMovement;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!movementDisabled)
        {
            ReturnWhenReachBoundaries();

            TurnOnHorizontalInput();

            JumpOnSpace();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagName.GROUND_TAG))
        {
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag(TagName.OBSTACLE_TAG))
        {
            animator.SetTrigger(STUMBLE_ANIM_TRIG);
            EventManager.Instance.onPlayerCrash?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Spawn new map and obstacles
        if (other.gameObject.CompareTag(TagName.SPAWN_TRIGGER_TAG))
        {
            SpawnManager.Instance.SpawnNextMapObstaclesAndPowerUps();
        }
        // Remove old map and obstacles
        if (other.gameObject.CompareTag(TagName.REMOVE_TRIGGER_TAG))
        {
            SpawnManager.Instance.DeactivatePreviousMapAndObstacles();
        }
    }

    void ReturnWhenReachBoundaries()
    {
        if (transform.position.x < leftBoundary)
        {
            transform.position = new Vector3(leftBoundary, transform.position.y, transform.position.z);
        }
        if (transform.position.x > rightBoundary)
        {
            transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);
        }
    }

    void TurnOnHorizontalInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);

        transform.Translate(Vector3.right * turnSpeed * Time.deltaTime * horizontalInput);
    }

    void JumpOnSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            Jump();
        }
    }

    void Jump()
    {
        SoundManager.Instance.PlayJumpSound();
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        animator.SetTrigger(JUMP_ANIM_TRIG);
    }

    public void DisableMovement()
    {
        movementDisabled = true;
        turnSpeed = 0;
        jumpForce = 0;
    }
}
