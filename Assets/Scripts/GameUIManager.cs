using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    private GameManager mGameManager;
    private GridGenerator mGridGenerator;

    public Action FRestartDelegate;

    private void Start()
    {
        mGameManager = FindObjectOfType<GameManager>();
        mGridGenerator = FindObjectOfType<GridGenerator>();
        FRestartDelegate += mGameManager.ResetGameValues;
        FRestartDelegate += mGridGenerator.GenerateNewGrid;
        FRestartDelegate += mGridGenerator.PopulateGrid;
    }

    public void OnRestartPressed()
    {
        FRestartDelegate?.Invoke();
    }

    private void OnDestroy()
    {
        FRestartDelegate -= mGameManager.ResetGameValues;
        FRestartDelegate -= mGridGenerator.PopulateGrid;
    }
}
