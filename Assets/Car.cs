using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoadObject))]
public class Car : MonoBehaviour
{
    internal float speed = 25;
    internal RoadObject ro;

    void Start()
    {
        ro = GetComponent<RoadObject>();    
    }

    void Update()
    {
        ro.position += speed * Time.deltaTime;        
    }
}
