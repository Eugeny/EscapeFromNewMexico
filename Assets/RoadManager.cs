using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public RoadSegment[] segmentPrefabs;
    public Transform environment;
    private float zCutoffForward = 200;
    private float zCutoffBack = 100;
    private float speed = 30;
    private float position = 0;
    private List<RoadSegment> segments = new List<RoadSegment>();

    private void Start()
    {
        SpawnNewSegment();
        SpawnNewSegment();
    }

    void Update()
    {
        var movement = speed * Time.deltaTime;
        position += movement;

        var curPosition = GetOffsetAtPosition(position);
        var curAngle = GetAngleAtPosition(position);
        foreach (var s in segments)
        {
            s.transform.position = Quaternion.AngleAxis(-curAngle, Vector3.up) * (GetOffsetAtPosition(s.position) - curPosition);
            s.transform.eulerAngles = new Vector3(0, GetAngleAtPosition(s.position) - curAngle, 0);
        }
        environment.transform.eulerAngles = new Vector3(0, -curAngle, 0);

        if (segments[0].position + GetTotalLength() - position < zCutoffForward)
        {
            SpawnNewSegment();
        }

        if (position - segments[0].position > zCutoffBack)
        {
            RemoveSegment();
        }
    }

    void SpawnNewSegment()
    {
        var s = Instantiate<RoadSegment>(segmentPrefabs[Random.Range(0, segmentPrefabs.Length)]);   
        if (segments.Count > 0)
        {
            s.position = GetTotalLength() + segments[0].position;
        }
        segments.Add(s);
    }

    void RemoveSegment ()
    {
        if (segments.Count == 0) return;
        Destroy(segments[0].gameObject);
        segments.RemoveAt(0);
    }


    RoadSegment GetCurrentSegment ()
    {
        foreach (var s in segments) {
            if (s.position <= position && s.position + s.length > position)
            {
                return s;
            }
        }

        return null;
    }

    float GetTotalLength ()
    {
        float l = 0;
        foreach (var s in segments)
        {
            l += s.length;
        }
        return l;
    }

    float GetAngleAtPosition(float p)
    {
        float a = 0;
        foreach (var s in segments)
        {
            a += s.GetBendAtPosition(Mathf.Min(s.length, p - s.position));
            if (s.position <= p && s.position + s.length > p)
            {
                return a;
            }
        }
        return a;
    }

    Vector3 GetOffsetAtPosition(float p)
    {
        Vector3 o = Vector3.zero;
        float a = 0;
        foreach (var s in segments)
        {
            o += Quaternion.AngleAxis(a, Vector3.up) * s.GetOffsetAtPosition(Mathf.Min(s.length, p - s.position));
            if (s.position <= p && s.position + s.length > p)
            {
                return o;
            }
            a += s.curvature;
        }
        return o;
    }
}
