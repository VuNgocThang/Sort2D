using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class DataFreeCoin
{
    public int index;
    public int countCoin;
    public bool isClaimed;
}
[Serializable]
public class DataClaimedFreecoin
{
    public List<DataFreeCoin> listDataFreeCoin;
    //public List<int> listDataFreeCoin;
    public int currentIndex;
    public bool isClaimed50;
}

[CreateAssetMenu(fileName = "FreeCoinData", menuName = "ScriptableObjects/FreeCoinData")]
public class FreeCoinData : ScriptableObject
{
    public List<DataFreeCoin> listDataFreeCoin;
    public int currentIndex;
}
