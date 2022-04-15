using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    private GameManager mGameManager;
    private GridGenerator mGridGenerator;
    private DifficultyManager mDifficultyManager;
    public Action FRestartDelegate;
    private int mTimeToCompleteHack;

    public TextMeshProUGUI mTimerText;
    public TextMeshProUGUI mResultText;
    public TextMeshProUGUI mPlayerLevelText;
    public TextMeshProUGUI mDifficultyText;

    private void Start()
    {
        mGameManager = FindObjectOfType<GameManager>();
        mGridGenerator = FindObjectOfType<GridGenerator>();
        mDifficultyManager = FindObjectOfType<DifficultyManager>();
        FRestartDelegate += mGameManager.ResetGameValues;
        FRestartDelegate += mGridGenerator.GenerateNewGrid;
        FRestartDelegate += mGridGenerator.PopulateGrid;

        mTimeToCompleteHack = mDifficultyManager.mTimePerDifficulty[(int)mGameManager.mDifficultyLevel] + mGameManager.mPlayerLevel;
        UpdateText();
    }

    public void OnRestartPressed()
    {
        FRestartDelegate?.Invoke();
        mTimeToCompleteHack = mDifficultyManager.mTimePerDifficulty[(int)mGameManager.mDifficultyLevel] + mGameManager.mPlayerLevel;
        UpdateText();
        CancelInvoke();
    }

    public void OnMainMenuPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnGameStart()
    {
        print("Started");
        InvokeRepeating(nameof(CountdownTimer), 0.0f, 1.0f);
    }

    public void OnGameOver(bool result)
    {
        mResultText.text = (result) ? "Success" : "Failure";
        mResultText.color = (result) ? Color.green : Color.red;
        CancelInvoke();
    }

    public void CountdownTimer()
    {
        mTimeToCompleteHack--;
        mTimerText.text = "Timer: " + mTimeToCompleteHack;

        if (mTimeToCompleteHack <= 0)
        {
            CancelInvoke();
            mGameManager.mGameActive = false;
            mResultText.text =  "Failure";
            mResultText.color =  Color.red;
        }
    }

    private void UpdateText()
    {
        mTimerText.text = "Timer: " + mTimeToCompleteHack;
        mResultText.text = "";
        mPlayerLevelText.text ="Player Level: " + mGameManager.mPlayerLevel.ToString();
        mDifficultyText.text = "Difficulty: " + mGameManager.mDifficultyLevel.ToString();
    }

    private void OnDestroy()
    {
        FRestartDelegate -= mGameManager.ResetGameValues;
        FRestartDelegate -= mGridGenerator.PopulateGrid;
    }
}
