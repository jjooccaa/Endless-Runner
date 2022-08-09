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
    [SerializeField] float leftBoundarie = -4.8f;  //FIXME Typo: Boundary
    [SerializeField] float rightBoundarie = 4.8f;  //FIXME Typo: Boundary

    Rigidbody rigidBody;
    Animator animator;

    GameManager gameManager;
    SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); //FIXME U idealnom slucaju PlayerController ne treba ni da zna za GameManager, vec obrnuto. Vidi komentare nanize.
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.gameOver) //FIXME gameOver je u nadleznosti GameManagera. Umesto da PlayerController pita GameManager da li je igra gotova, GameManager treba na GameOver() da obavesti PlayerController (a vec ima referencu na Playera iz koje moze da ga uzme) da je igra gotova.
        {
            horizontalInput = Input.GetAxis("Horizontal");
            // Always move forward 
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);

            // If player hit boundaries, return to prev position //FIXME Umesto komentara, izdvoji ovaj blok u sopstvenu funkciju i daj joj deskriptivno ime.
            if (transform.position.x < leftBoundarie)
            {
                transform.position = new Vector3(leftBoundarie, transform.position.y, transform.position.z);
            }
            if (transform.position.x > rightBoundarie)
            {
                transform.position = new Vector3(rightBoundarie, transform.position.y, transform.position.z);
            }

            // Turn to right or left, depending on input //FIXME Umesto komentara, izdvoji ovu liniju (zajedno sa inicijalizacijom horizontal imputa odozgo) u sopstvenu funkciju i daj joj deskriptivno ime.
            transform.Translate(Vector3.right * turnSpeed * Time.deltaTime * horizontalInput);

            // Jump when user presses 'space' and is on ground //FIXME Umesto komentara, izdvoji ovaj blok u sopstvenu funkciju i daj joj deskriptivno ime.
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                Jump();
            }

            // Pause game when user pressed P or Escape button
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) //FIXME Klasican Scope Envy iz Clean Code. Unpause dugme vec hvatas u GameManageru, i ova provera pripada tamo. Indikator ti je to sto zoves gameManagerovu funkciju (koja je u njegovoj nadleznosti, a ne u nadleznosti ove klase).
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
        } else if(collision.gameObject.CompareTag("Obstacle")) //FIXME else u novi red pls
        {
            gameManager.PlayCrashSound(); //FIXME Nema razloga da sav audio ide preko GameManagera, jer je njegova nadleznost gameplay, a ne zvuk. Na ovo cemo da dodjemo kad zavrsis ostale komentare.
            animator.SetTrigger("Stumble_trig");
            gameManager.GameOver(); //FIXME Ovde su eventi pravo resenje, radije nego referenca na GameManager. Za sada ga ostavi ovako pa cemo u kasnijoj fazi prakse kad stignemo do eventova da ga ispravimo.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Spawn new map and obstacles
        if (other.gameObject.CompareTag("SpawnTrigger"))
        {
            spawnManager.SpawnTriggerActivated(); //FIXME Umesto komentara iznad ovog bloka, samo treba ova funkcija da se zove deskriprivno. Vec znamo da se poziva kad se spawn trigger aktivira, tako da nam ovo ime nije korisno. "SpawnNextMapAndObstacles" bi bio primer deskriptivnog imena.
        }
        // Remove old map and obstacles
        if (other.gameObject.CompareTag("RemoveTrigger"))
        {
            spawnManager.RemoveTriggerActivated(); //FIXME Isto kao gore
        }
    }

    void Jump()
    {
        gameManager.PlayJumpSound(); //FIXME Kao i gore za zvuk, ovo cemo da refaktorisemo kad resis sve ostalo.
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        animator.SetTrigger("Jump_trig");
    }
}
