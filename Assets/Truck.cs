using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public static Truck instance;
    public Transform trailer;
    internal float x;
    private float maxSideSpeed = 12;
    private float sideAccel = 12;
    internal float sideSpeed = 0;
    private float sideSpeedDamp;

    internal float speed = 0;
    internal const float minSpeed = 5;
    internal const float maxSpeed = 50;
    internal float defaultSpeed = 30;
    internal float accel = 0;
    internal const float maxAccel = 5;
    private float accelDamp;
    private float accelTarget;

    private float trailerAngle;
    private float trailerAngleDamp;

    public AudioSource bottleClinkSound;

    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        var hInput = Input.GetAxis("Horizontal") + Mathf.Clamp(Game.instance.drunkInput * 0.02f, -0.85f, 0.85f);

        var movement = speed * Time.deltaTime;
        RoadManager.instance.position += movement;

        accelTarget = Input.GetAxis("Vertical") * maxAccel;
        accel = Mathf.SmoothDamp(accel, accelTarget, ref accelDamp, 0.5f);

        if (!Game.gameOver)
        {
            speed += accel * Time.deltaTime;
        }

        speed = Mathf.Max(minSpeed, Mathf.Min(maxSpeed, speed));

        float maxX = CarManager.instance.lanes * CarManager.instance.laneWidth / 2f;

        if (!Game.gameOver)
        {
            if ((x > -maxX + 0.1f && hInput < 0) || (x < maxX - 0.1f && hInput > 0))
            {
                sideSpeed += hInput * Time.deltaTime * sideAccel;
            }
         
            sideSpeed = Math.Max(-maxSideSpeed, Math.Min(maxSideSpeed, sideSpeed));

            x += sideSpeed * Time.deltaTime;
        }

        x = Mathf.Min(maxX, Mathf.Max(-maxX, x));

        transform.position = new Vector3(x, 0, 0);

        float oldSideSpeed = sideSpeed;
        sideSpeed = Mathf.SmoothDamp(sideSpeed, 0, ref sideSpeedDamp, 0.5f);
        float hAccel = (sideSpeed - oldSideSpeed) / Time.deltaTime;

        float truckAngle = sideSpeed * 5;
        transform.eulerAngles = new Vector3(0, truckAngle, 0);

        trailerAngle = Mathf.SmoothDamp(trailerAngle, truckAngle, ref trailerAngleDamp, 0.5f);
        trailer.transform.localEulerAngles = new Vector3(0, trailerAngle - truckAngle, -hAccel*2);
    }


    private void OnTriggerEnter(Collider other)
    {
        var bw = other.GetComponentInParent<BoozeWagon>();
        if (bw != null)
        {
            bottleClinkSound.Play();
            Game.instance.AddBottles(bw.bottles);
            Destroy(other.gameObject);
        }
    }
}
