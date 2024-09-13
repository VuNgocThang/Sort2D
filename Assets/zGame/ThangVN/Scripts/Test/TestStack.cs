using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestStack : MonoBehaviour
{
    public GameObject square;
    public List<GameObject> listSquare;
    public Transform parent;
    public GameObject test;

    public int rows = 9;
    public int cols = 8;
    public float cellSize = 1.2f;

    private void Start()
    {
        GenerateGrid();
        SetPos();
    }

    void GenerateGrid()
    {
        var offSetY = (((float)rows + 1) / 2.0f) * cellSize;
        var offsetX = (((float)cols - 1) / 2.0f) * cellSize;

        Vector3 startPosition = new Vector3(-offsetX, -offSetY, 0);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 position = new Vector3(col, row, 0) * cellSize + startPosition;

                GameObject colorPlate = Instantiate(square, parent);
                colorPlate.transform.localPosition = position;
                listSquare.Add(colorPlate);
            }
        }
    }

    void SetPos()
    {
        test.transform.position = new Vector3(-2.8f, listSquare[0].transform.position.y, 0);
    }

}
