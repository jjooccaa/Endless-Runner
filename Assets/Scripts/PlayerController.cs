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

    SpawnManager spawnManager;
    SoundManager soundManager;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
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
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        } 
        else if(collision.gameObject.CompareTag("Obstacle"))
        {
            soundManager.PlayCrashSound();
            animator.SetTrigger("Stumble_trig");
            GameManager.Instance.GameOver(); //FIXME Ovde su eventi pravo resenje, radije nego referenca na GameManager. Za sada ga ostavi ovako pa cemo u kasnijoj fazi prakse kad stignemo do eventova da ga ispravimo.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Spawn new map and obstacles
        if (other.gameObject.CompareTag("SpawnTrigger"))
        {
            spawnManager.SpawnNextMapAndObstacles();
        }
        // Remove old map and obstacles
        if (other.gameObject.CompareTag("RemoveTrigger"))
        {
            spawnManager.DeactivatePreviousMapAndObstacles();
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
        horizontalInput = Input.GetAxis("Horizontal");

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
        soundManager.PlayJumpSound();
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        animator.SetTrigger("Jump_trig");
    }

    public void DisableMovement()
    {
        movementDisabled = true;
        turnSpeed = 0;
        jumpForce = 0;
    }
}
