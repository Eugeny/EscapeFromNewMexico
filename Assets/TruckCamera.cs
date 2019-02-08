using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCamera : MonoBehaviour
{
    public float maxOffset;
    private Vector3 pos;

    void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        var offset = (Truck.instance.speed - Truck.instance.defaultSpeed) / Truck.instance.speedD;
        transform.position = pos + new Vector3(0, -offset, -offset) * maxOffset;        
    }
}
