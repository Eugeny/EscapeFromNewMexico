using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static RoadManager instance;
    public RoadSegment[] segmentPrefabs;
    public List<RoadObject> roadObjects = new List<RoadObject>();
    public Transform environment;
    private float zCutoffForward = 300;
    private float zCutoffBack = 100;
    internal float position = 0;
    private List<RoadSegment> segments = new List<RoadSegment>();
    internal float globalStartAngle = 0;

    internal float currentFrameAngle;
    internal Vector3 currentFrameOffset;

    private void Start()
    {
        instance = this;
        SpawnNewSegment();
        SpawnNewSegment();
        position += 50;
    }

    void Update()
    {
        UpdateSegmentPositions();

        if (segments[0].position + GetTotalLength() - position < zCutoffForward)
        {
            SpawnNewSegment();
        }

        if (position - segments[0].position - segments[0].length > zCutoffBack)
        {
            RemoveSegment();
        }
    }

    private void UpdateSegmentPositions()
    {
        currentFrameOffset = GetOffsetAtPosition(position);
        currentFrameAngle = GetAngleAtPosition(position);
        foreach (var s in segments)
        {
            s.transform.position = Quaternion.AngleAxis(-currentFrameAngle, Vector3.up) * (GetOffsetAtPosition(s.position) - currentFrameOffset);
            s.transform.eulerAngles = new Vector3(0, GetAngleAtPosition(s.position) - currentFrameAngle, 0);
        }
        environment.transform.eulerAngles = new Vector3(0, -currentFrameAngle, 0);

        foreach (var o in roadObjects)
        {
            Reposition(o);
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
        UpdateSegmentPositions();
    }

    void RemoveSegment ()
    {
        if (segments.Count == 0) return;
        Destroy(segments[0].gameObject);
        globalStartAngle += segments[0].curvature;
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
        float a = globalStartAngle;
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
        float a = globalStartAngle;
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

    public void Reposition(RoadObject o)
    {
        var a = GetAngleAtPosition(o.position);
        o.transform.position = Quaternion.AngleAxis(-currentFrameAngle, Vector3.up) * (
            GetOffsetAtPosition(o.position)
            + Quaternion.AngleAxis(a, Vector3.up) * new Vector3(o.x, 0)
            - currentFrameOffset
        );
        o.transform.eulerAngles = new Vector3(0, a - currentFrameAngle + o.rotation, 0);
    }
}
