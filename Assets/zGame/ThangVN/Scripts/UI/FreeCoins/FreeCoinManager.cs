using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCoinManager : MonoBehaviour
{
    public static FreeCoinManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    public void Init()
    {

        if (SaveGame.NewDayFreeCoin == DateTime.Now.DayOfYear) return;

        SaveGame.ShowFreeCoin = false;

        SaveGame.NewDayFreeCoin = DateTime.Now.DayOfYear;
        SaveGame.DataFreeCoin.listDataFreeCoin.Clear();
        SaveGame.DataFreeCoin.currentIndex = 0;
        SaveGame.DataFreeCoin.isClaimed50 = false;

        SaveGame.DataFreeCoin = SaveGame.DataFreeCoin;
    }
}
