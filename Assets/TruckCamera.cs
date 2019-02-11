using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCamera : MonoBehaviour
{
    public float maxOffset;
    private Vector3 pos, rot;

    private float offset;
    private float offsetDamp;

    private float angle;
    private float angleDamp;

    void Start()
    {
        pos = transform.position;
        rot = transform.eulerAngles;
    }

    void Update()
    {
        angle = Mathf.SmoothDampAngle(angle, RoadManager.instance.currentFrameAngle, ref angleDamp, 1f);

        var offsetTarget = (Truck.instance.speed - Truck.instance.defaultSpeed) / Truck.maxSpeed;
        offset = Mathf.SmoothDamp(offset, offsetTarget, ref offsetDamp, 0.25f);

        var offsetAngle = angle - RoadManager.instance.currentFrameAngle;
        offsetAngle /= 2;
        transform.position = Quaternion.AngleAxis(-offsetAngle, Vector3.up) * (pos + new Vector3(0, -offset, -offset) * maxOffset);

        transform.eulerAngles = rot - new Vector3(0, offsetAngle, Game.instance.drunkInput * 0.5f);
    }
}
