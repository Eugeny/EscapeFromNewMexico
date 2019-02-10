using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoadObject))]
public class Car : MonoBehaviour
{
    internal float speed = 25;
    internal RoadObject ro;
    internal bool swerving = false;
    private int swerveDirection = 1;

    void Start()
    {
        ro = GetComponent<RoadObject>();
    }

    void Update()
    {
        ro.position += speed * Time.deltaTime;        

        if (swerving)
        {
            ro.x += swerveDirection * 10 * Time.deltaTime;
            ro.rotation += swerveDirection * 10 * speed * Time.deltaTime;
            speed = Mathf.Max(0, speed - 10 * Time.deltaTime);
        }
    }

    public void SwerveAway(int dir)
    {
        swerving = true;
        swerveDirection = dir;
    }


    void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponent<Car>();
        if (car != null && !car.swerving)
        {
            car.SwerveAway((int)Mathf.Sign(car.ro.x - ro.x));
        }
    }
}
