using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class DataItemDecor
{
    public int id;
    public int cost;
    public Sprite spriteIcon;
    public Sprite sprite;
    public bool isPainted;
}

[Serializable]
public class DataSlot
{
    public int id;
    public Sprite spriteLine;
    public Vector2 pos;
}

[Serializable]
public class DataBook
{
    public int idBook;
    public List<DataItemDecor> listDataItemDecor;
    public List<DataSlot> listDataSlots;
}

[CreateAssetMenu(fileName = "DataConfigDecor", menuName = "ScriptableObjects/DataConfigDecor")]

public class DataConfigDecor : ScriptableObject
{
    public List<DataBook> listDataBooks;
}
