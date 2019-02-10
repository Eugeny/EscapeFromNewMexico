using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public static CarManager instance;
    public Car carPrefab;
    private List<Car> cars = new List<Car>();
    internal float laneWidth = 3;
    internal int lanes = 4;
    private float zCutoffForward = 200;
    private float zCutoffBack = 100;
    private float[] lastCarPosition;
    private float[] laneSpeeds = { 25, 20, 15, 10 };

    void Start()
    {
        instance = this;
        lastCarPosition = new float[lanes];
    }

    private void Update()
    {
        for (int i = 0; i < lanes; i++)
        {
            lastCarPosition[i] += laneSpeeds[i] * Time.deltaTime;
            if (lastCarPosition[i] - RoadManager.instance.position < zCutoffForward)
            {
                SpawnCar(i, Random.Range(20f, 45f));
            }
        }

        foreach (var car in cars) {
            if (RoadManager.instance.position - car.ro.position > zCutoffBack)
            {
                cars.Remove(car);
                Destroy(car.gameObject);
            }
        }
    }


    void SpawnCar(int lane, float distance)
    {
        float position = lastCarPosition[lane] + distance;
        float x = laneWidth * (lane - (lanes - 1) / 2f) + Random.Range(-laneWidth * 0.1f, laneWidth * 0.1f);
        var car = Instantiate<Car>(carPrefab);
        car.GetComponent<RoadObject>().x = x;
        car.GetComponent<RoadObject>().position = position;
        car.speed = laneSpeeds[lane];

        lastCarPosition[lane] = position;
        RoadManager.instance.Reposition(car.GetComponent<RoadObject>());
    }
}
