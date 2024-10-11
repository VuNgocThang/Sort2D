using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestStack : MonoBehaviour
{
    public SquareTest squarePrefab;
    public List<SquareTest> listSquare;
    public Transform nDesk;
    public Transform nParentArrow;
    public ColorPlate arrowPrefab;

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
        ResetNDesk();
        GenerateGrid();
        InitArrowPlates();
    }
    float scale;
    void ResetNDesk()
    {

        if (cols >= rows)
        {
            float y = 0.3f * (6 - cols);
            this.transform.position = new Vector3(0, 1.8f + y, 0);

            scale = 6f / cols;
            nDesk.localScale = new Vector3(scale, scale, scale);
            nParentArrow.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            float y = 0.3f * (6 - rows);
            this.transform.position = new Vector3(0, 1.8f + y, 0);

            scale = 6f / rows;
            nDesk.localScale = new Vector3(scale, scale, scale);
            nParentArrow.localScale = new Vector3(scale, scale, scale);
        }

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

                SquareTest colorPlate = Instantiate(squarePrefab, nDesk);
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
            ColorPlate arrow = Instantiate(arrowPrefab, nParentArrow);

            if (isHorizontal)
            {
                arrow.transform.position = new Vector3(listSquare[0].transform.position.x + i * offSetX, basePosition.y - 0.2f * scale, 0);
                arrow.Initialize(-1, i);
            }
            else
            {

                if (isArrowRight)
                {
                    arrow.transform.position = new Vector3(basePosition.x - 0.2f * scale, listSquare[0].transform.position.y + i * offSetY, 0);
                    arrow.Initialize(i, -1);
                }
                else
                {
                    arrow.transform.position = new Vector3(basePosition.x + 0.2f * scale, listSquare[0].transform.position.y + i * offSetY, 0);
                    arrow.Initialize(i, count + 1);
                }
            }

            if (count != 6)
            {
                arrow.logicVisual.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }


            arrow.transform.localEulerAngles = rotation;
            arrow.name = arrowName;
        }
    }

}
