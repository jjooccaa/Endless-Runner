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
        // always follow player //FIXME Komentar treba da bude zadnja opcija za objasnjavanje koda. Ako smatras da linija ispod nije jasna sama po sebi (meni recimo jeste) izdvojis je u one-liner funkciju cije ce ime da opise sta radi.
        transform.position = player.transform.position + cameraOffset;
    }
}
