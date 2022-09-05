using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 cameraOffset;

    private void Start()
    {
        SetPosition();
    }

    void FixedUpdate()
    {
        SetPosition();
    }

    void SetPosition()
    {
        transform.position = player.transform.position + cameraOffset;
    }
}
