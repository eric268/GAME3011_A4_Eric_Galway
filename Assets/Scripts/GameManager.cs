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
    public ActiveRowOrientation mActiveRow;
    public int mRowTilePos = 0;
    int mBufferIndex = 0;
    int mCorrectTilesNeeded;
    public TileType[] mSelectedTypeBuffer;
    public TileType[] mCorrectTileBuffer;

    public GameObject mSelectedHighlighter;
    public GameObject mAvailableHighlighter;

    private float mTileSize = 50.0f;

    private void Awake()
    {
        mDifficultyManager = GetComponent<DifficultyManager>();
        mGridGenerator = FindObjectOfType<GridGenerator>();
        ResetGameValues();
    }

    public void ResetGameValues()
    {
        mSelectedHighlighter.gameObject.SetActive(false);
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

        mBufferIndex = 0;
        mRowTilePos = 0;
        mActiveRow = ActiveRowOrientation.Horizontal;

        MoveHighlightedPosition(ref mAvailableHighlighter, mRowTilePos, mActiveRow);
    }

    public void TileSelected(int pos, TileType type)
    {
        mSelectedTypeBuffer[mBufferIndex++] = type;
        mActiveRow = (mActiveRow == ActiveRowOrientation.Horizontal) ? ActiveRowOrientation.Vertical : ActiveRowOrientation.Horizontal;
        mRowTilePos = pos;

        mSelectedHighlighter.gameObject.SetActive(true);
        mSelectedHighlighter.GetComponent<RectTransform>().anchoredPosition = mAvailableHighlighter.GetComponent<RectTransform>().anchoredPosition;
        mSelectedHighlighter.GetComponent<RectTransform>().sizeDelta = mAvailableHighlighter.GetComponent<RectTransform>().sizeDelta;

        MoveHighlightedPosition(ref mAvailableHighlighter, mRowTilePos, mActiveRow);
    }

    public void MoveHighlightedPosition(ref GameObject highligher, float gridPos, ActiveRowOrientation orientation)
    {
        RectTransform rectTransform = highligher.GetComponent<RectTransform>();
        float offset = 25.0f;
        float pos = (mGridGenerator.mGridDimensions * mGridGenerator.spacing - 75) /2.0f;
        if (orientation == ActiveRowOrientation.Horizontal)
        {
            rectTransform.sizeDelta = new Vector2(mGridGenerator.mGridDimensions * mGridGenerator.spacing - 25.0f, mTileSize);
            rectTransform.anchoredPosition = new Vector2(pos, -(gridPos) * (mTileSize + 25.0f));
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(mTileSize, mGridGenerator.mGridDimensions * mGridGenerator.spacing - 25.0f);
            rectTransform.anchoredPosition = new Vector2(gridPos * (mTileSize + 25.0f), -pos);
        }
    }
}
