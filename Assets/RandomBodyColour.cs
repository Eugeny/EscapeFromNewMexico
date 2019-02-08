using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBodyColour : MonoBehaviour
{
    public static Color[] colors =
    {
        new Color(1f, 0.4f, 0.388f),
        new Color(0.647f, 0.063f, 0.502f),
        new Color(0.2f, 0.2f, 0.2f),
        new Color(0.9f, 0.9f, 0.9f),
        new Color(0, 0.153f, 0.169f),
        new Color(0.129f, 0.463f, 0.682f),
        new Color(0.984f, 0.694f, 0.235f),
        new Color(0.212f, 0.224f, 0.275f),
    };

    void Start()
    {
        var color = colors[Random.Range(0, colors.Length)];
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            foreach (var m in mr.materials)
            {
                if (m.name.ToLower() == "body (instance)")
                {
                    m.color = color;
                }
            }
        }
    }
}
