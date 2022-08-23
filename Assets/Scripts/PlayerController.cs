using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float turnSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float horizontalInput;
    [SerializeField] float leftBoundary = -4.8f;
    [SerializeField] float rightBoundary = 4.8f;

    bool isOnGround;
    bool movementDisabled = false;

    bool canJump = true;
    float jumpWaiter = 0.7f;

    bool canShoot = true;
    float shootWaiter = 0.5f;

    [SerializeField] ParticleSystem smokeParticle;
    Rigidbody rigidBody;
    Animator animator;

    private const string HORIZONTAL = "Horizontal";
    private const string JUMP_ANIM_TRIG = "Jump_trig";
    private const string STUMBLE_ANIM_TRIG = "Stumble_trig";
    private const string DEATH_ANIM_BOOL = "Is_Game_Over";
    private const string RUN_SPEED_F = "Run_Speed_f";
    private const string JUMP_SPEED_F = "Jump_Speed_f";
    private const string SHOOT_ANIM_TRIG = "Shoot_trig";

    private void OnEnable()
    {
        EventManager.Instance.onGameOver += DisableMovement;
        EventManager.Instance.onGameOver += DeathAnimation;
        EventManager.Instance.onMovementSpeedChange += SetMoveAnimationSpeed;
        EventManager.Instance.onMovementSpeedChange += SetJumpAnimationSpeed;
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

            JumpOnPressedKey(KeyCode.Space);

            ShootOnPressedKey(KeyCode.F);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagName.GROUND_TAG))
        {
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag(TagName.OBSTACLE_TAG) || collision.gameObject.CompareTag(TagName.ENEMY_TAG))
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
            EventManager.Instance.onSpawnTrigger?.Invoke();
        }
        // Remove old map and obstacles
        if (other.gameObject.CompareTag(TagName.REMOVE_TRIGGER_TAG))
        {
            EventManager.Instance.onRemoveTrigger?.Invoke();
        }
        if (other.gameObject.CompareTag(TagName.TUTORIAL_TRIGGER_TAG))
        {
            EventManager.Instance.onTutorialTrigger?.Invoke();
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

    void JumpOnPressedKey(KeyCode key)
    {
        if (Input.GetKeyDown(key) && isOnGround)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (canJump)
        {
            EventManager.Instance.onJump?.Invoke();
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            animator.SetTrigger(JUMP_ANIM_TRIG);
            StartCoroutine(JumpDelay());
        }
    }

    IEnumerator JumpDelay()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpWaiter);
        canJump = true;
    }

    void ShootOnPressedKey(KeyCode key)
    {
        if(Input.GetKeyDown(key))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (canShoot && GameManager.Instance.NumberOfArrows > 0)
        {
            animator.SetTrigger(SHOOT_ANIM_TRIG);
            EventManager.Instance.onSpawnShootingArrow?.Invoke(transform.position); //Spawn and shoot arrow from player current position
            EventManager.Instance.onArrowShoot?.Invoke();
            StartCoroutine(ShootDelay());
        }
    }

    IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootWaiter);
        canShoot = true;
    }

    public void DisableMovement()
    {
        movementDisabled = true;
        turnSpeed = 0;
        jumpForce = 0;
    }

    void DeathAnimation()
    {
        animator.SetBool(DEATH_ANIM_BOOL, true);
    }

    void SetMoveAnimationSpeed(float movementSpeed)
    {
        float runSpeed = movementSpeed * 0.08f;
        animator.SetFloat(RUN_SPEED_F, runSpeed);
    }

    void SetJumpAnimationSpeed(float movementSpeed)
    {
        float jumpSpeed = movementSpeed * 0.09f;
        animator.SetFloat(JUMP_SPEED_F, jumpSpeed);
    }

    public void PlaySmokeParticle()
    {
        smokeParticle.Play();
    }

    public void StopSmokeParticle()
    {
        smokeParticle.Stop();
    }
}
