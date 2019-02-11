using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBottleManager : MonoBehaviour
{
    public static EmptyBottleManager instance;
    public Vector3 spawnPoint;
    public EmptyBottle bottlePrefab;

    void Start()
    {
        instance = this;
    }

    public void SpawnBottle()
    {
        var bottle = Instantiate(bottlePrefab, transform.position + spawnPoint, Quaternion.Euler(0, 0, Random.Range(0, 360)), transform);
        bottle.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, Random.Range(-10, 10)));
    }

    public void Shake()
    {
        foreach (var bottle in FindObjectsOfType<EmptyBottle>())
        {
            bottle.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), Random.Range(1, 2), 0), ForceMode.Impulse);
        }
    }
}
