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
    public float percent;
    public Sprite spriteIcon;
    public Sprite sprite;
    public bool isPainted;
    public bool isBought;
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
public class ColorDecor
{
    public Color color;
    public bool CanSelect;
}

[Serializable]
public class DataBook
{
    public int idBook;
    public string titleBook;
    public Sprite sprite;
    public int totalParts;
    public List<DataItemDecor> listDataItemDecor;
    public List<DataSlot> listDataSlots;
    public List<ColorDecor> listColorDecor;

    //public bool IsPainted()
    //{
    //    return ()
    //}
}

//data save
//public class ItemPosition
//{
//    public float x;
//    public float y;
//}

[Serializable]
public class ItemDecorated
{
    public int idItemDecorated;
    public bool isPainted;
    public bool isBought;
    public bool isTruePos;
    public float percent;
    //public ItemPosition position;
    public float x;
    public float y;
}

[Serializable]
public class BookDecorated
{
    public int idBookDecorated;
    public float progress;
    public bool isPainted;
    public bool isCollectedReward;
    public Color colorPainted;
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
