using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGridMatrix
{
    ColorPlate[,] colorPlate;
    #region Init
    public void Init(int rows, int cols, Transform parent, List<ColorPlate> ListColorPlate, ColorPlate colorPlatePrefab)
    {

        GenerateGrid(rows, cols, parent, ListColorPlate, colorPlatePrefab);
    }
    void GenerateGrid(int rows, int cols, Transform parent, List<ColorPlate> ListColorPlate, ColorPlate colorPlatePrefab)
    {
        colorPlate = new ColorPlate[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 position = new Vector3(col * 1.1f, 0, row * 1.1f);

                InstantiateColorPlate(row, col, position, parent, ListColorPlate, colorPlatePrefab);
            }
        }


        LinkColorPlate(rows, cols);
    }
    private void InstantiateColorPlate(int row, int col, Vector3 position, Transform parent, List<ColorPlate> ListColorPlate, ColorPlate colorPlatePrefab)
    {
        ColorPlate colorPlate = null;
        colorPlate.status = Status.None;
        colorPlate.Init();
        colorPlate.Initialize(row, col);
        this.colorPlate[row, col] = colorPlate;
        ListColorPlate.Add(colorPlate);
    }

    void LinkColorPlate(int rows, int cols)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                ColorPlate currentCell = colorPlate[row, col];
                //Debug.Log(currentCell.name);

                if (col > 0)
                    currentCell.LinkColorPlate(colorPlate[row, col - 1]); //left
                if (col < cols - 1)
                    currentCell.LinkColorPlate(colorPlate[row, col + 1]); //right
                if (row > 0)
                    currentCell.LinkColorPlate(colorPlate[row - 1, col]); //bottom
                if (row < rows - 1)
                    currentCell.LinkColorPlate(colorPlate[row + 1, col]); //up
            }
        }
    }

    #endregion
}
