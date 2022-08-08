using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] Vector3 cameraOffset = new Vector3(0,4,-6);

    // Update is called once per frame
    void Update()
    {
        // always follow player
        transform.position = player.transform.position + cameraOffset;
    }
}
