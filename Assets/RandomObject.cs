using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObject : MonoBehaviour
{
    public GameObject[] objects;

    void Start()
    {
        int k = Random.Range(0, objects.Length);
        for (int i = 0; i < objects.Length; i++)
        {
            if (i != k)
            {
                Destroy(objects[i]);
            }
        }
    }
}
