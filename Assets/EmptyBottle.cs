using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBottle : MonoBehaviour
{
    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        body.AddForce(new Vector3(-Truck.instance.sideSpeed * 2, 0, 0), ForceMode.Acceleration); 
    }
}
