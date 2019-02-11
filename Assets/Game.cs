using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game instance;
    internal bool gameOver;

    private float startTime;
    public Text distanceText;
    public Text speedText;
    public Text bottlesText;
    public Image bottleMeterBG;
    public RectTransform bottleMeter;

    private float bottleContent = 1;
    private float drinkingSpeed = 0.05f;
    private int bottlesLeft = 10;

    private float bottleMeterY, bottleMeterYDamp, bottleMeterYTarget = 0;
    private bool bottleChecksEnabled = true;

    void Start()
    {
        instance = this;
        startTime = Time.time;
        drinkingSpeed = 0.5f;
    }

    void Update()
    {
        distanceText.text = GetDistanceCovered().ToString("F1");
        speedText.text = ((int)Truck.instance.speed).ToString();

        if (!gameOver)
        {
            bottleContent -= drinkingSpeed * Time.deltaTime;
            bottleMeterBG.fillAmount = bottleContent;
            bottleMeterY = Mathf.SmoothDamp(bottleMeterY, bottleMeterYTarget, ref bottleMeterYDamp, 0.25f);
            bottleMeter.transform.localPosition = new Vector3(0, -bottleMeterY * 400, 0);

            if (bottleChecksEnabled && bottleContent < 0)
            {
                if (bottlesLeft == 0)
                {
                    GameOver();
                }
                else
                {
                    StartCoroutine(SwapBottles());
                }
            }

            bottlesText.text = "x" + bottlesLeft.ToString();
        }
    }

    IEnumerator SwapBottles ()
    {
        bottleChecksEnabled = false;
        bottleMeterYTarget = 1;
        yield return new WaitForSeconds(0.5f);
        bottleMeterYTarget = 0;
        bottleContent = 1;
        bottlesLeft--;
        bottleChecksEnabled = true;
        EmptyBottleManager.instance.SpawnBottle();
    }

    float GetDistanceCovered ()
    {
        return RoadManager.instance.position / 1000;
    }

    void GameOver ()
    {
        gameOver = true;
        Debug.Log("Game over");
    }
}
