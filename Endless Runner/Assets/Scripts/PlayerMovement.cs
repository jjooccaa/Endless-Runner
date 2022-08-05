using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rigidbody;
    Animator animator;
    

    [SerializeField] public float speed = 1;
    [SerializeField] public float turnSpeed = 5.0f;
    [SerializeField] public float jumpForce;
    [SerializeField] float horizontalInput;
    [SerializeField] bool isOnGround;

    GameManager gameManager;
    SpawnManager spawnManager;

    float leftBoundarie = -4.8f;
    float rightBoundarie = 4.8f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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


            // Move forward
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);

            // Boundaries, return to prev position
            if (transform.position.x < leftBoundarie)
            {
                transform.position = new Vector3(leftBoundarie, transform.position.y, transform.position.z);
            }
            if (transform.position.x > rightBoundarie)
            {
                transform.position = new Vector3(rightBoundarie, transform.position.y, transform.position.z);
            }

            // Turn to right or left, depending on Input
            transform.Translate(Vector3.right * turnSpeed * Time.deltaTime * horizontalInput);

            // Jump when user presses 'space' and is on ground
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                Jump();
            }

            // Slide when user presses 'Control' and is on ground 
            if (Input.GetKeyDown(KeyCode.LeftControl) && isOnGround)
            {
                Slide();
            }

            // Pause game when P has been pressed
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
        // Spawn new road
        if (other.gameObject.CompareTag("SpawnTrigger"))
        {
            spawnManager.SpawnTriggerActivated();
        }
        // Remove old road
        if (other.gameObject.CompareTag("RemoveTrigger"))
        {
            spawnManager.RemoveTriggerActivated();
        }
    }

    void Jump()
    {
        gameManager.PlayJumpSound();
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        animator.SetTrigger("Jump_trig");
    }

    void Slide()
    {

    }

}
