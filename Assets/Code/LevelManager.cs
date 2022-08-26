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
    private int levelID = 0;
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

    [SerializeField]
    private string menuScene;
    [SerializeField]
    private string nextLevel;
    [SerializeField]
    private GameObject nextLevelButton;

    private StringBuilder strBuilder;

    private float remainingTime = 0f;

    private bool levelOver = false;

    private void Start()
    {
        DOTween.Init();
        strBuilder = new StringBuilder();
        rescueObjective.OnNPCsDeadOrRescued += OnLevelComplete;
        remainingTime = timeForCompletion;

        nextLevelButton.SetActive(!string.IsNullOrEmpty(nextLevel));

        introTextTransform.DOScale(Vector3.zero, 0.75f).SetDelay(2.5f);
    }

    private void Update()
    {
        if (levelOver)
            return;
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

    readonly string[] winMessages = { "Fantastic job!", "Perfect!!!", "Well done!", "Congratulations!" };
    readonly string[] failMessages = { "You monster.", "Congratulations.", "We'll remember this.", "Horribly done." };
    readonly string[] mixedMessages = { "That's something.", "You tried.", "Decent work.", "We've all been there." };

    private void OnLevelComplete(int rescued, int dead)
    {
        if (levelOver)
            return;
        Game.MarkLevelComplete(this.levelID);
        levelOver = true;
        bool allRescued = dead == 0;
        bool allDead = rescued == 0;
        string tempText = $"{rescued}/{rescued + dead} rescued.\n";
        if (allRescued)
            tempText += winMessages[Random.Range(0, winMessages.Length)];
        else if (allDead)
            tempText += failMessages[Random.Range(0, failMessages.Length)];
        else
            tempText += mixedMessages[Random.Range(0, mixedMessages.Length)];
        endingText.text = tempText;
        ShowEndScreen();
    }

    private void ShowEndScreen()
    {
        endingScreen.SetActive(true);
        RectTransform rt = endingScreen.transform as RectTransform;

        rt.DOJumpAnchorPos(new Vector2(0, 0), 150, 1, 1f).Prepend(rt.DOAnchorPosY(0, 0.8f)).Append(rt.DOJumpAnchorPos(new Vector2(0, 0), 50, 1, 0.7f));
    }

    private void OnLevelFailed()
    {
        levelOver = true;
        ShowEndScreen();
        endingText.text = "You ran out of time!";
    }

    public void GoToMenu()
    {
        LoadingScreen.GoToScene(menuScene);
    }

    public void GoToNextLevel()
    {
        LoadingScreen.GoToScene(nextLevel);
    }
}
