using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBack : MonoBehaviour
{

    float playerSpeed;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerSpeed = playerController.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.movementDisabled)
        {
            transform.Translate(Vector3.back * playerSpeed * Time.deltaTime, Space.World);
        }
    }
}
