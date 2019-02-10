using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private float startTime;
    public Text timerText;
    public Text speedText;

    void Start()
    {
        startTime = Time.time;    
    }

    void Update()
    {
        var t = "";
        var time = (int)(Time.time - startTime);
        if ((time / 60) < 10)
        {
            t += "0";
        }
        t += (time / 60).ToString();
        t += ":";
        if ((time % 60) < 10)
        {
            t += "0";
        }
        t += (time % 60).ToString();
        timerText.text = t;

        speedText.text = ((int)Truck.instance.speed).ToString();
    }
}
