using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPusherCollider : MonoBehaviour
{
    public ParticleSystem particlesL, particlesR;
    private Truck truck;
    private new BoxCollider collider;

    void Start()
    {
        truck = GetComponentInParent<Truck>();
        collider = GetComponent<BoxCollider>();

        particlesL.enableEmission = false;
        particlesR.enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponent<Car>();
        if (car != null)
        {
            (car.ro.x > truck.x ? particlesR : particlesL).enableEmission = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        var car = other.GetComponent<Car>();
        if (car != null)
        {
            car.ro.x += Mathf.Abs(truck.sideSpeed) * Mathf.Sign(car.ro.x - truck.x) * Time.deltaTime * 3;
        }
    }

    void OnTriggerExit(Collider other)
    {
        var car = other.GetComponent<Car>();
        if (car != null)
        {
            particlesL.enableEmission = false;
            particlesR.enableEmission = false;
        }
    }


}
