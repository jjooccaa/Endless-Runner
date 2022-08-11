using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBack : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    { 
        transform.Translate(Vector3.back * GameManager.Instance.MovementSpeed * Time.deltaTime, Space.World);
    }
}
