using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataLevel
{
    public int ID;
    public int CountDiff;
    public int[] Colors;
    public int[] Ratio;

    static List<DataLevel> listData;

    public static List<DataLevel> GetListData()
    {
        if (listData == null || listData.Count == 0)
        {
            listData = new List<DataLevel>();
            List<DataLevel> list = JsonHelper.GetJsonList<DataLevel>((Resources.Load<TextAsset>("DataLevel/DataLevel")).text);
            listData.AddRange(list);
        }
        return listData;
    }

    public static DataLevel GetData(int id)
    {
        List<DataLevel> list = GetListData();
        foreach (DataLevel d in list)
        {
            if (d.ID == id - 1)
                return d;
        }
        return null;
    }
}
