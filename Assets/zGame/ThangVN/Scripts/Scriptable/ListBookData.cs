using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BookData
{
    public int idBook;
    public string titleBook;
    public int totalParts;
}


[CreateAssetMenu(fileName = "ListBookData", menuName = "ScriptableObjects/ListBookData")]
public class ListBookData : ScriptableObject
{
    public List<BookData> listBookData;
}
