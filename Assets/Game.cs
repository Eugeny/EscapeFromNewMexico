using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game instance;
    internal static bool gameOver;

    private float startTime;
    public Text distanceText;
    public Text speedText;
    public Text bottlesText;
    public Image bottleMeterBG;
    public RectTransform bottleMeter;
    public Text bottlePlusText;

    public PlayableDirector director;
    public PlayableAsset bottlePlusTimeline, gameOverTimeline;

    private float bottleContent = 1;
    private float drinkingSpeed = 0.05f;
    private int bottlesLeft = 10;

    private float bottleMeterY, bottleMeterYDamp, bottleMeterYTarget = 0;
    private bool bottleChecksEnabled = true;

    private float finalScore = 0;

    internal float drunkFactor = 0;
    internal float drunkInput;
    private float drunkFactorAccel = 0.1f;
    private float drunkInputTimeFactor = 0.1f;

    void Start()
    {
        instance = this;
        gameOver = false;
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


        var t = Time.time * drunkInputTimeFactor;
        drunkInput = Mathf.Sin(t * 3) + Mathf.Sin(t * 5) + Mathf.Sin(t * 13) + Mathf.Sin(t * 23);
        drunkInput *= drunkFactor;

        drunkFactor += Time.deltaTime * drunkFactorAccel;
    }

    public void AddBottles(int bottles)
    {
        bottlesLeft += bottles;
        bottlePlusText.text = "+" + bottles;
        director.Play(bottlePlusTimeline);
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
        if (finalScore != 0)
        {
            return finalScore;
        }
        return RoadManager.instance.position / 1000;
    }

    void GameOver ()
    {
        director.Play(gameOverTimeline);
        finalScore = GetDistanceCovered();
        gameOver = true;
        Debug.Log("Game over");
    }

    public void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
