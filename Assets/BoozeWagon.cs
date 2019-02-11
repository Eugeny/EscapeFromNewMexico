using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoozeWagon : MonoBehaviour
{
    public GameObject[] fillZones;
    private Car car;
    internal int bottles = 2;

    void Start()
    {
        car = GetComponentInParent<Car>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (fillZones != null && car.swerving)
        {
            foreach (var o in fillZones)
            {
                Destroy(o);
            }
            fillZones = null;
        }
    }
}
