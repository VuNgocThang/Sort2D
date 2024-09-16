using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestStack : MonoBehaviour
{
    public SquareTest squarePrefab;
    public List<SquareTest> listSquare;
    public Transform parent;
    public Transform parentArrow;
    public ArrowTest arrowPrefab;

    public int rows = 6;
    public int cols = 6;
    public float cellSize = 1.2f;

    public float offSetX;
    public float offSetY;
    // up  y = -1.44f;
    // right x = -2.5; rotation z = 90;
    // left  x = 2.5; rotation z = 180;

    private void Start()
    {
        //GenerateGrid();
        //InitArrowPlates();
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

                SquareTest colorPlate = Instantiate(squarePrefab, parent);
                colorPlate.Init(row, col);
                colorPlate.transform.localPosition = position;
                listSquare.Add(colorPlate);
            }
        }


    }


    void InitArrowPlates()
    {
        offSetX = listSquare[1].transform.position.x - listSquare[0].transform.position.x;
        offSetY = listSquare[cols].transform.position.y - listSquare[0].transform.position.y;

        InitArrows(cols, new Vector3(0, 0, 90f), "ArrowUp", new Vector3(0, -1.44f, 0), true, true);
        InitArrows(rows, new Vector3(0, 0, 0), "ArrowRight", new Vector3(-2.5f, 0, 0), false, true);
        InitArrows(rows, new Vector3(0, 0, 180f), "ArrowLeft", new Vector3(2.5f, 0, 0), false, false);
    }

    void InitArrows(int count, Vector3 rotation, string arrowName, Vector3 basePosition, bool isHorizontal, bool isArrowRight)
    {
        for (int i = 0; i < count; i++)
        {
            ArrowTest arrow = Instantiate(arrowPrefab, parentArrow);

            if (isHorizontal)
            {
                arrow.transform.position = new Vector3(listSquare[0].transform.position.x + i * offSetX, basePosition.y, 0);
                arrow.Init(-1, i);
            }
            else
            {
                arrow.transform.position = new Vector3(basePosition.x, listSquare[0].transform.position.y + i * offSetY, 0);

                if (isArrowRight)
                {
                    arrow.Init(i, -1);
                }
                else
                {
                    arrow.Init(i, count + 1);
                }
            }

            

            arrow.transform.localEulerAngles = rotation;
            arrow.name = arrowName;
        }
    }

}
