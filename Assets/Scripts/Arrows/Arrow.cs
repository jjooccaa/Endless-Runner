using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    private void Update()
    {
        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }

    protected void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
