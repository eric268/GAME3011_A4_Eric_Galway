using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TileType
{
    None = -1,
    E1,
    F9,
    A0,
    D3,
    C7,
    B2,
    NUM_Tile_Types
}


public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int mXPos;
    public int mYPos;
    public TileType mTileType;
    public float mBaseOpacity;
    public float mHoveredOpacity;
    public GameManager mGameManager;

    public Action<int, TileType> FTileSelected;

    private Image mTileImage;

    void Awake()
    {
        mTileImage = GetComponent<Image>();
        mGameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        FTileSelected = mGameManager.TileSelected;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        FTileSelected -= mGameManager.TileSelected;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //Check if tile is allowed to be pressed
        if (mGameManager.mActiveRow == ActiveRowOrientation.Horizontal && mYPos == mGameManager.mRowTilePos)
        {
            FTileSelected(mXPos, mTileType);
            gameObject.SetActive(false);
        }
        else if (mGameManager.mActiveRow == ActiveRowOrientation.Vertical && mXPos == mGameManager.mRowTilePos)
        {
            FTileSelected(mYPos, mTileType);
            gameObject.SetActive(false);
        }

        //Do something with game
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Check if tile is allowed to be pressed
        if (mGameManager.mActiveRow == ActiveRowOrientation.Horizontal && mYPos == mGameManager.mRowTilePos ||
            mGameManager.mActiveRow == ActiveRowOrientation.Vertical && mXPos == mGameManager.mRowTilePos)
        {
            mTileImage.color = new Color(mTileImage.color.r, mTileImage.color.g, mTileImage.color.b, mHoveredOpacity);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Check if tile is allowed to be pressed

        mTileImage.color = new Color(mTileImage.color.r, mTileImage.color.g, mTileImage.color.b, mBaseOpacity);
    }
}
