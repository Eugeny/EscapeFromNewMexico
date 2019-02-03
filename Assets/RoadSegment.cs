using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    public float length, curvature;
    internal float position;

    public float GetBendAtPosition(float p)
    {
        return Mathf.Lerp(0, curvature, p / length);
    }

    public Vector3 GetOffsetAtPosition (float p)
    {
        var r = (360 / curvature) * length / (Mathf.PI * 2);
        if (curvature == 0)
        {
            return new Vector3(0, 0, p);
        }

        var a = GetBendAtPosition(p);
        return new Vector3(
            (1f - Mathf.Cos(Mathf.Deg2Rad * a)) * r,
            0,
            Mathf.Sin(Mathf.Deg2Rad * a) * r
        );
    }
}
