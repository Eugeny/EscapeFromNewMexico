using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObject : MonoBehaviour
{
    public float position;
    public float x;

    void Start()
    {
        RoadManager.instance.roadObjects.Add(this);   
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        RoadManager.instance.roadObjects.Remove(this);
    }
}
