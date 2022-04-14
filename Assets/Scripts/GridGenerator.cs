using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject tileSlotPrefab;
    int rowCounter = 0;
    int columnCounter = 0;
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
        tileArray = new GameObject[mGridDimensions, mGridDimensions];
    }
    private void Start()
    {
        GenerateNewGrid();
    }
    public void CreateGrid()
    {
        startingPos = new Vector2(transform.position.x, transform.position.y);
        tileArray = new GameObject[mGridDimensions, mGridDimensions];
        int numCells = mGridDimensions * mGridDimensions;
        rowCounter = 0;
        columnCounter = 0;

        int counter = 0;

        while (counter < numCells)
        {
            counter++;
            //Provides information to tiles
            GameObject newObject = Instantiate(tileSlotPrefab, this.transform);
            //newObject.GetComponent<Tile>().tileTypes = m_tileType;
            newObject.name = "Tile x: " + (rowCounter) + " y: " + (columnCounter);
            newObject.GetComponent<Tile>().mXPos = rowCounter;
            newObject.GetComponent<Tile>().mYPos = columnCounter;
            tileArray[rowCounter, columnCounter] = newObject;

            newObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(rowCounter * spacing, columnCounter * spacing * -1);

            rowCounter++;
            if (rowCounter >= mGridDimensions)
            {
                rowCounter = 0;
                columnCounter++;
            }
        }
    }

    public void DestroyGrid()
    {
        //int numChildren = transform.childCount;
        //for (int i = numChildren-1; i >= 0; i--)
        //{
        //    Destroy(gameObject.transform.GetChild(i).gameObject);
        //}
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
}
