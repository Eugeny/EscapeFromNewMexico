using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public static Truck instance;
    public Transform trailer;
    internal float x;
    private float maxSideSpeed = 6;
    private float sideAccel = 8;
    internal float sideSpeed = 0;
    private float sideSpeedDamp;

    internal float speed = 0;
    internal float defaultSpeed = 30;
    internal float speedD = 15;
    private float speedDamp;
    private float speedTarget;

    private float trailerAngle;
    private float trailerAngleDamp;

    void Start()
    {
        instance = this;
        speedTarget = speed;
    }

    // Update is called once per frame
    void Update()
    {
        var movement = speed * Time.deltaTime;
        RoadManager.instance.position += movement;

        speedTarget = defaultSpeed + Input.GetAxis("Vertical") * speedD;
        speed = Mathf.SmoothDamp(speed, speedTarget, ref speedDamp, 1f);

        sideSpeed += Input.GetAxis("Horizontal") * Time.deltaTime * sideAccel;
        sideSpeed = Math.Max(-maxSideSpeed, Math.Min(maxSideSpeed, sideSpeed));
        x += sideSpeed * Time.deltaTime;

        transform.position = new Vector3(x, 0, 0);

        float oldSideSpeed = sideSpeed;
        sideSpeed = Mathf.SmoothDamp(sideSpeed, 0, ref sideSpeedDamp, 0.5f);
        float hAccel = (sideSpeed - oldSideSpeed) / Time.deltaTime;

        float truckAngle = sideSpeed * 5;
        transform.eulerAngles = new Vector3(0, truckAngle, 0);

        trailerAngle = Mathf.SmoothDamp(trailerAngle, truckAngle, ref trailerAngleDamp, 0.5f);
        trailer.transform.localEulerAngles = new Vector3(0, trailerAngle - truckAngle, -hAccel*2);
    }
}
