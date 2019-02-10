using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckSound : MonoBehaviour
{
    public AudioClip engineClip, shiftClip;
    private AudioSource engine, shift;
    private float[] gearRatios = { 1f, 1.5f, 2.25f, 3.4f, 5f, 7.5f };
    private const float speedToRPM = 1500 / 15f;
    private float engineClipRPM = 1500;
    private float engineRPM = 0;
    private float engineRPMDamp;
    private float minRPM = 799;
    private float maxRPM = 2000;
    private int lastGear = 1;

    // Start is called before the first frame update
    void Start()
    {
        engine = gameObject.AddComponent<AudioSource>();
        engine.clip = engineClip;
        engine.loop = true;
        engine.Play();
        shift = gameObject.AddComponent<AudioSource>();
        shift.clip = shiftClip;
        shift.loop = false;
        shift.volume = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        var speed = Truck.instance.speed;
        int gear = gearRatios.Length - 1;

        var driveShaftRPM = speed * speedToRPM;
        var targetEngineRPM = 0f;
        for (int i = 0; i < gearRatios.Length; i++)
        {
            targetEngineRPM = driveShaftRPM / gearRatios[i];
            if (targetEngineRPM > minRPM && targetEngineRPM <= maxRPM)
            {
                gear = i;
                break;
            }
        }

        engineRPM = Mathf.SmoothDamp(engineRPM, targetEngineRPM, ref engineRPMDamp, 0.2f);

        engine.pitch = engineRPM / engineClipRPM;

        if (lastGear != gear)
        {
            shift.Play();
        }

        lastGear = gear;
    }
}
