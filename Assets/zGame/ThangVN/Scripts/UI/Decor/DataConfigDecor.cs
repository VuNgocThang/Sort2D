using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//data config
[Serializable]
public class DataItemDecor
{
    public int idItemDecor;
    public int cost;
    public Sprite spriteIcon;
    public Sprite sprite;
    public bool isPainted;
}

[Serializable]
public class DataSlot
{
    public int idSlot;
    public Sprite spriteLine;
    public Vector2 pos;
}

[Serializable]
public class DataColor
{
    public int idColor;
    public Color color;
}

[Serializable]
public class DataBook
{
    public int idBook;
    public List<DataItemDecor> listDataItemDecor;
    public List<DataSlot> listDataSlots;
}

//data save
[Serializable]
public class ItemDecorated
{
    public int idItemDecorated;
    public bool isPainted;
}

[Serializable]
public class BookDecorated
{
    public int idBookDecorated;
    public float progress;
    public bool isPainted;
    public List<ItemDecorated> listItemDecorated;
}

[Serializable]
public class ColorPainted
{
    public int idColorPainted;
}

[Serializable]
public class ListBookDecorated
{
    public List<BookDecorated> listBookDecorated;
}



[CreateAssetMenu(fileName = "DataConfigDecor", menuName = "ScriptableObjects/DataConfigDecor")]

public class DataConfigDecor : ScriptableObject
{
    public List<DataBook> listDataBooks;
}
