using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActiveRowOrientation
{
    Horizontal,
    Vertical
}

public class GameManager : MonoBehaviour
{
    public DifficultyManager mDifficultyManager;
    public GridGenerator mGridGenerator;
    public int mPlayerLevel;
    public DifficultyLevel mDifficultyLevel;
    public ActiveRowOrientation mActiveRow = ActiveRowOrientation.Horizontal;
    public int mRowTilePos = 0;
    int mBufferIndex = 0;
    int mCorrectTilesNeeded;
    public TileType[] mSelectedTypeBuffer;
    public TileType[] mCorrectTileBuffer;

    public GameObject mSelectedHighlighter;
    public GameObject mAvailableHighlighter;

    private void Awake()
    {
        mDifficultyManager = GetComponent<DifficultyManager>();
        mGridGenerator = FindObjectOfType<GridGenerator>();
        ResetGameValues();
    }

    public void ResetGameValues()
    {
        mCorrectTilesNeeded = (int)mDifficultyLevel + 2;

        mSelectedTypeBuffer = new TileType[mCorrectTilesNeeded];
        mCorrectTileBuffer = new TileType[mCorrectTilesNeeded];

        for (int i =0; i < mCorrectTilesNeeded; i++)
        {
            mCorrectTileBuffer[i] = (TileType)UnityEngine.Random.Range(0, (int)TileType.NUM_Tile_Types);
        }

        //Need to generate a grid with correct values here

        mPlayerLevel = UnityEngine.Random.Range(1, mDifficultyManager.mMaxSkillLevel + 1);
        mDifficultyLevel = (DifficultyLevel)UnityEngine.Random.Range((int)DifficultyLevel.Easy, (int)DifficultyLevel.NUM_DIFFICULTY_LEVELS);
        mGridGenerator.mGridDimensions = mDifficultyManager.mDifficultyGridDimension[(int)mDifficultyLevel];
    }

    public void TileSelected(int pos, TileType type)
    {
        //Add selected tile to buffer
        mSelectedTypeBuffer[mBufferIndex++] = type;
        mActiveRow = (mActiveRow == ActiveRowOrientation.Horizontal) ? ActiveRowOrientation.Vertical : ActiveRowOrientation.Horizontal;
        mRowTilePos = pos;
    }
}
