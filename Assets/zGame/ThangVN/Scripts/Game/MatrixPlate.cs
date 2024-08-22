using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class MatrixPlate 
{
    #region Init
    //private void Init()
    //{
    //    cellSize = 1f;

    //    GenerateGrid();
    //}
    //void GenerateGrid()
    //{
    //    colorPlate = new ColorPlate[rows, cols];

    //    for (int row = 0; row < rows; row++)
    //    {
    //        for (int col = 0; col < cols; col++)
    //        {
    //            Vector3 position = new Vector3(col * cellSize * 1.1f, 0, row * cellSize * 1.1f);
    //            if (IsCorner(row, col))
    //            {
    //                InstantiateEmptyHolder(row, col, position);
    //            }
    //            else if (IsEdge(row, col))
    //            {
    //                InstantiateArrowHolder(row, col, position);
    //            }
    //            else
    //            {
    //                InstantiateColorPlate(row, col, position);
    //            }
    //        }
    //    }


    //    LinkColorPlate();
    //}
    //private void InstantiateColorPlate(int row, int col, Vector3 position)
    //{
    //    ColorPlate colorPlate = Instantiate(colorPlatePrefab, position, Quaternion.identity, holderColorPlate);
    //    colorPlate.status = Status.None;
    //    colorPlate.Init();
    //    colorPlate.Initialize(row, col);
    //    this.colorPlate[row, col] = colorPlate;
    //    ListColorPlate.Add(colorPlate);
    //}
    //bool IsCorner(int row, int col)
    //{
    //    return (row == 0 && col == 0) || (row == 0 && col == cols - 1) || (row == rows - 1 && col == 0) || (row == rows - 1 && col == cols - 1);
    //}
    //bool IsEdge(int row, int col)
    //{
    //    return (row == 0 && col != 0 && col < cols) || (col == 0 && row != 0 && row < rows) || (row == rows - 1 && col != 0 && col < cols) || (col == cols - 1 && row != 0 && row < rows);
    //}
    //void InstantiateEmptyHolder(int row, int col, Vector3 position)
    //{
    //    ColorPlate empty = Instantiate(emptyHolder, position, Quaternion.identity, holderEmpty);
    //    empty.Init();
    //    empty.status = Status.Empty;
    //    empty.Initialize(row, col);
    //    this.colorPlate[row, col] = empty;
    //}
    //void InstantiateArrowHolder(int row, int col, Vector3 position)
    //{
    //    ColorPlate arrow = Instantiate(arrowPlatePrefab, position, Quaternion.identity, holderArrow);
    //    arrow.Init();
    //    arrow.Initialize(row, col);
    //    this.colorPlate[row, col] = arrow;
    //    ListColorPlate.Add(arrow);
    //    if (arrow.Col == 0)
    //    {
    //        arrow.status = Status.Right;
    //    }
    //    else if (arrow.Row == 0)
    //    {
    //        arrow.status = Status.Up;
    //    }
    //    else if (arrow.Col == cols - 1)
    //    {
    //        arrow.status = Status.Left;
    //    }
    //    else if (arrow.Row == rows - 1)
    //    {
    //        arrow.status = Status.Down;
    //    }
    //    arrow.name = $"arrow_{arrow.status}  +  [{arrow.Row},{arrow.Col}]";
    //}
    //void LinkColorPlate()
    //{
    //    for (int row = 0; row < rows; row++)
    //    {
    //        for (int col = 0; col < cols; col++)
    //        {
    //            if (IsCorner(row, col)) continue;
    //            ColorPlate currentCell = colorPlate[row, col];
    //            //Debug.Log(currentCell.name);

    //            if (col > 0)
    //                currentCell.LinkColorPlate(colorPlate[row, col - 1]); //left
    //            if (col < cols - 1)
    //                currentCell.LinkColorPlate(colorPlate[row, col + 1]); //right
    //            if (row > 0)
    //                currentCell.LinkColorPlate(colorPlate[row - 1, col]); //bottom
    //            if (row < rows - 1)
    //                currentCell.LinkColorPlate(colorPlate[row + 1, col]); //up
    //        }
    //    }
    //}
    #endregion


}
