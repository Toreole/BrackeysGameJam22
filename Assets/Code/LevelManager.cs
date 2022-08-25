using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Text;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private RescueObjectiveManager rescueObjective;
    [SerializeField]
    private RectTransform introTextTransform;
    [SerializeField]
    private TextMeshProUGUI introText;
    [SerializeField]
    private float timeForCompletion;
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private GameObject endingScreen;
    [SerializeField]
    private TextMeshProUGUI endingText;

    private StringBuilder strBuilder;

    private float remainingTime = 0f;

    private void Start()
    {
        DOTween.Init();
        strBuilder = new StringBuilder();
        rescueObjective.OnNPCsDeadOrRescued += OnLevelComplete;
        remainingTime = timeForCompletion;
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;
        SetTimeString();

        if(remainingTime < 0f)
        {
            OnLevelFailed();
        }
    }

    //update the visual timer.
    private void SetTimeString()
    {
        //if less than 15% of time is remaining, set the color of the text to red.
        if (remainingTime / timeForCompletion < 0.15f)
            timerText.color = Color.red;

        strBuilder.Clear();

        int minutes = (int)(remainingTime / 60f);
        int seconds = (int)(remainingTime % 60f);
        //int decis = ((int)(remainingTime * 100) % 100);

        strBuilder.Append(minutes).Append(':').Append(seconds.ToString("00"));//.Append('.').Append(decis.ToString("00"));
        timerText.text = strBuilder.ToString();
    }

    private void OnLevelComplete(int rescued, int dead)
    {

    }

    private void OnLevelFailed()
    {

    }
}
