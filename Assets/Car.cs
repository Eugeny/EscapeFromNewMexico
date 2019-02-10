using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoadObject))]
public class Car : MonoBehaviour
{
    public AudioClip[] crashSounds;
    internal float speed = 25;
    internal RoadObject ro;
    internal bool swerving = false;
    private int swerveDirection = 1;

    void Start()
    {
        ro = GetComponent<RoadObject>();
        gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        ro.position += speed * Time.deltaTime;        

        if (swerving)
        {
            ro.x += swerveDirection * 10 * Time.deltaTime;
            ro.rotation += swerveDirection * 10 * speed * Time.deltaTime;
            speed = Mathf.Max(0, speed - 10 * Time.deltaTime);
        }
    }

    public void SwerveAway(int dir)
    {
        if (!swerving)
        {
            if (Mathf.Abs(ro.position - RoadManager.instance.position) < 10)
            {
                GetComponent<AudioSource>().PlayOneShot(crashSounds[Random.Range(0, crashSounds.Length)]);
                GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1f);
                GetComponent<AudioSource>().volume = 0.5f;
            }
            swerving = true;
            swerveDirection = dir;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponent<Car>();
        if (car != null && !car.swerving)
        {
            car.SwerveAway((int)Mathf.Sign(car.ro.x - ro.x));
        }
    }
}
