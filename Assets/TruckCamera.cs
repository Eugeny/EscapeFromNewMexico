using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCamera : MonoBehaviour
{
    public float maxOffset;
    private Vector3 pos, rot;

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

        var offset = (Truck.instance.speed - Truck.instance.defaultSpeed) / Truck.instance.speedD;
        var offsetAngle = angle - RoadManager.instance.currentFrameAngle;
        transform.position = Quaternion.AngleAxis(-offsetAngle, Vector3.up) * (pos + new Vector3(0, -offset, -offset) * maxOffset);

        transform.eulerAngles = rot - new Vector3(0, offsetAngle, 0);
    }
}
