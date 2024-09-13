using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTest : MonoBehaviour
{
    public int Row;
    public int Col;

    public void Init(int row, int col)
    {
        this.Row = row;
        this.Col = col;
        gameObject.name = $"Cell {Row}-{Col}";
    }
}
