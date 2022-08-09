using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] public float speed = 1;
    [SerializeField] public float turnSpeed = 5.0f;
    [SerializeField] public float jumpForce;
    [SerializeField] float horizontalInput;
    [SerializeField] bool isOnGround;
    [SerializeField] float leftBoundarie = -4.8f;
    [SerializeField] float rightBoundarie = 4.8f;

    Rigidbody rigidBody;
    Animator animator;

    GameManager gameManager;
    SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.gameOver)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            // Always move forward
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);

            // If player hit boundaries, return to prev position
            if (transform.position.x < leftBoundarie)
            {
                transform.position = new Vector3(leftBoundarie, transform.position.y, transform.position.z);
            }
            if (transform.position.x > rightBoundarie)
            {
                transform.position = new Vector3(rightBoundarie, transform.position.y, transform.position.z);
            }

            // Turn to right or left, depending on input
            transform.Translate(Vector3.right * turnSpeed * Time.deltaTime * horizontalInput);

            // Jump when user presses 'space' and is on ground
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                Jump();
            }

            // Pause game when user pressed P or Escape button
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                gameManager.PauseGame();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        } else if(collision.gameObject.CompareTag("Obstacle"))
        {
            gameManager.PlayCrashSound();
            animator.SetTrigger("Stumble_trig");
            gameManager.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Spawn new map and obstacles
        if (other.gameObject.CompareTag("SpawnTrigger"))
        {
            spawnManager.SpawnTriggerActivated();
        }
        // Remove old map and obstacles
        if (other.gameObject.CompareTag("RemoveTrigger"))
        {
            spawnManager.RemoveTriggerActivated();
        }
    }

    void Jump()
    {
        gameManager.PlayJumpSound();
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        animator.SetTrigger("Jump_trig");
    }
}
