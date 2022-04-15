using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject tileSlotPrefab;
    int colCounter = 0;
    int rowCounter = 0;
    private Vector2 startingPos;
    public float spacing = 50.0f;
    [SerializeField]
    public int mGridDimensions;
    public GameObject [,] tileArray;

    private GameManager mGameManager;

    //[SerializeField]
    //private TileTypes m_tileType;

    private void Awake()
    {
        mGameManager = FindObjectOfType<GameManager>();
        tileArray = new GameObject[mGridDimensions, mGridDimensions];
    }
    private void Start()
    {
        GenerateNewGrid();
        PopulateGrid();
    }

    public void CreateGrid()
    {
        startingPos = new Vector2(transform.position.x, transform.position.y);
        tileArray = new GameObject[mGridDimensions, mGridDimensions];
        int numCells = mGridDimensions * mGridDimensions;
        colCounter = 0;
        rowCounter = 0;

        int counter = 0;

        while (counter < numCells)
        {
            counter++;
            //Provides information to tiles
            GameObject newObject = Instantiate(tileSlotPrefab, this.transform);
            newObject.GetComponent<Tile>().mTileType = TileType.None;
            newObject.name = "Tile x: " + (colCounter) + " y: " + (rowCounter);
            newObject.GetComponent<Tile>().mXPos = colCounter;
            newObject.GetComponent<Tile>().mYPos = rowCounter;
            tileArray[colCounter, rowCounter] = newObject;

            newObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(rowCounter * spacing, colCounter * spacing * -1);

            rowCounter++;
            if (rowCounter >= mGridDimensions)
            {
                rowCounter = 0;
                colCounter++;
            }
        }
    }

    public void DestroyGrid()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void GenerateNewGrid()
    {
        Array.Clear(tileArray, 0, tileArray.Length);
        DestroyGrid();
        CreateGrid();
    }

    public void PopulateGrid()
    {
        GenerateHackingPath();
        FillDummyTiles();
    }

    public void GenerateHackingPath()
    {
        int gridDimensions = mGridDimensions;
        int ranVal = UnityEngine.Random.Range(0, gridDimensions);
        tileArray[0, ranVal].GetComponent<Tile>().mTileType = mGameManager.mCorrectTileBuffer[mGameManager.mBufferIndex++];
        tileArray[0, ranVal].GetComponentInChildren<TextMeshProUGUI>().text = tileArray[0, ranVal].GetComponent<Tile>().mTileType.ToString();
        
        int oldPos = 0;
        int keepPos = ranVal;

        string w1 = 0.ToString() + ", " + ranVal;
        print(w1);
        
        //Starting position is on top now and in random column
        for (int i = 1; i < mGameManager.mCorrectTilesNeeded; i++)
        {
            if (i % 2 == 0)
            {
                while (true)
                {
                    int newPos = UnityEngine.Random.Range(0, gridDimensions);
                    if (newPos != oldPos)
                    {
                        tileArray[keepPos, newPos].GetComponent<Tile>().mTileType = mGameManager.mCorrectTileBuffer[mGameManager.mBufferIndex++];
                        tileArray[keepPos, newPos].GetComponentInChildren<TextMeshProUGUI>().text = tileArray[keepPos, newPos].GetComponent<Tile>().mTileType.ToString();
                        oldPos = keepPos;
                        keepPos = newPos;
                        break;
                    }
                }
            }
            else
            {
                while (true)
                {
                    int newPos = UnityEngine.Random.Range(0, gridDimensions);
                    if (newPos != oldPos)
                    {
                        tileArray[newPos, keepPos].GetComponent<Tile>().mTileType = mGameManager.mCorrectTileBuffer[mGameManager.mBufferIndex++];
                        tileArray[newPos, keepPos].GetComponentInChildren<TextMeshProUGUI>().text = tileArray[newPos, keepPos].GetComponent<Tile>().mTileType.ToString();
                        string w2 = newPos.ToString() + ", " + keepPos;
                        print(w2);
                        oldPos = keepPos;
                        keepPos = newPos;
                        break;
                    }
                }
            }
        }
        mGameManager.mBufferIndex = 0;
    }

    void FillDummyTiles()
    {
        foreach(GameObject tile in tileArray)
        {
            Tile t = tile.GetComponent<Tile>();
            if (t.mTileType == TileType.None)
            {
                TileType ranType = (TileType)UnityEngine.Random.Range(0, (int)TileType.NUM_Tile_Types);
                t.mTileType = ranType;
                t.GetComponentInChildren<TextMeshProUGUI>().text = t.mTileType.ToString();
            }
        }
    }
}
