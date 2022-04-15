using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ActiveRowOrientation
{
    Horizontal,
    Vertical
}

public class GameManager : MonoBehaviour
{
    public DifficultyManager mDifficultyManager;
    public GridGenerator mGridGenerator;
    public GameUIManager mUIManager;
    public int mPlayerLevel;
    public DifficultyLevel mDifficultyLevel;
    public ActiveRowOrientation mActiveRow;
    public int mRowTilePos = 0;
    public int mBufferIndex = 0;
    public int mCorrectTilesNeeded;
    public int mNumCorrectSelections;
    public TileType[] mSelectedTypeBuffer;
    public TileType[] mCorrectTileBuffer;

    public GameObject mSelectedHighlighter;
    public GameObject mAvailableHighlighter;
    public bool mGameActive = false;
    public bool mGameOver = false;
    private float mTileSize = 50.0f;

    private void Awake()
    {
        mDifficultyManager = GetComponent<DifficultyManager>();
        mGridGenerator = FindObjectOfType<GridGenerator>();
        mUIManager = FindObjectOfType<GameUIManager>();
        ResetGameValues();
    }

    public void ResetGameValues()
    {
        mDifficultyLevel = (DifficultyLevel)UnityEngine.Random.Range((int)DifficultyLevel.Easy, (int)DifficultyLevel.NUM_DIFFICULTY_LEVELS);
        mGridGenerator.mGridDimensions = mDifficultyManager.mDifficultyGridDimension[(int)mDifficultyLevel];
        mPlayerLevel = UnityEngine.Random.Range(1, mDifficultyManager.mMaxSkillLevel + 1);

        mSelectedHighlighter.gameObject.SetActive(false);
        mCorrectTilesNeeded = (int)mDifficultyLevel + 3;
        mNumCorrectSelections = 0;
        mCorrectTileBuffer = new TileType[mCorrectTilesNeeded];

        for (int i =0; i < mCorrectTilesNeeded; i++)
        {
            mCorrectTileBuffer[i] = (TileType)UnityEngine.Random.Range(0, (int)TileType.NUM_Tile_Types);
        }

        mBufferIndex = 0;
        mRowTilePos = 0;
        mActiveRow = ActiveRowOrientation.Horizontal;
        mGameActive = true;
        mUIManager.OnGameStart();
        MoveHighlightedPosition(ref mAvailableHighlighter, mRowTilePos, mActiveRow);
    }

    public void TileSelected(int pos, TileType type)
    {
        UpdateBuffers(type);

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
            rectTransform.sizeDelta = new Vector2(mGridGenerator.mGridDimensions * mGridGenerator.spacing - offset, mTileSize);
            rectTransform.anchoredPosition = new Vector2(pos, -(gridPos) * (mTileSize + offset));
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(mTileSize, mGridGenerator.mGridDimensions * mGridGenerator.spacing - offset);
            rectTransform.anchoredPosition = new Vector2(gridPos * (mTileSize + offset), -pos);
        }
    }

    public void UpdateBuffers(TileType type)
    {
        Tile correctBufferTile = mGridGenerator.mCorrectInputBuffer.transform.GetChild(mBufferIndex).GetComponent<Tile>();
        Tile playerInputTile = mGridGenerator.mPlayerInputBuffer.transform.GetChild(mBufferIndex).GetComponent<Tile>();
        playerInputTile.GetComponentInChildren<TextMeshProUGUI>().text = type.ToString();

        if (type == mCorrectTileBuffer[mBufferIndex])
        {
            correctBufferTile.mTileImage.color = new Color(0, 1, 0, correctBufferTile.mHoveredOpacity);
            mNumCorrectSelections++;
            if (mNumCorrectSelections == mCorrectTilesNeeded)
            {
                mGameActive = false;
                mUIManager.OnGameOver(true);
            }
        }
        else
        {
            correctBufferTile.mTileImage.color = new Color(1, 0, 0, correctBufferTile.mHoveredOpacity);
            mGameActive = false;
            mUIManager.OnGameOver(false);
        }
        mBufferIndex++;
    }
}
