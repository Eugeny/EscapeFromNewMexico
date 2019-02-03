using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public Transform trailer;
    private float maxSideSpeed = 6;
    private float sideAccel = 8;
    private float sideSpeed = 0;
    private float sideSpeedDamp;

    private float trailerAngle;
    private float trailerAngleDamp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sideSpeed += Input.GetAxis("Horizontal") * Time.deltaTime * sideAccel;
        sideSpeed = Math.Max(-maxSideSpeed, Math.Min(maxSideSpeed, sideSpeed));

        transform.position += new Vector3(sideSpeed * Time.deltaTime, 0, 0);

        float oldSideSpeed = sideSpeed;
        sideSpeed = Mathf.SmoothDamp(sideSpeed, 0, ref sideSpeedDamp, 0.5f);
        float hAccel = (sideSpeed - oldSideSpeed) / Time.deltaTime;

        float truckAngle = sideSpeed * 5;
        transform.eulerAngles = new Vector3(0, truckAngle, 0);

        trailerAngle = Mathf.SmoothDamp(trailerAngle, truckAngle, ref trailerAngleDamp, 0.5f);
        trailer.transform.localEulerAngles = new Vector3(0, trailerAngle - truckAngle, -hAccel*2);
    }
}
