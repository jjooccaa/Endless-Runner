using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 cameraOffset;

    // Update is called once per frame //FIXME nepotrebann kom, proveri i u ostalim klasama
    void Update()
    {
        transform.position = player.transform.position + cameraOffset;
    }
}
