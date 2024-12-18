using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[Serializable]
public class DataLevel
{
    public int ID;
    public int CountDiff;
    public int[] Colors;
    public float[] Ratio;
    public int[] RatioInStacks;

    static List<DataLevel> listData;

    public async static Task<List<DataLevel>> GetListData()
    {
        if (listData == null || listData.Count == 0)
        {
            listData = new List<DataLevel>();
            //List<DataLevel> list = JsonHelper.GetJsonList<DataLevel>((Resources.Load<TextAsset>("DataLevel/DataLevel")).text);
            List<DataLevel> list = JsonHelper.GetJsonList<DataLevel>((await ManagerAsset.LoadAssetAsync<TextAsset>("DataLevel")).text);
            listData.AddRange(list);
        }
        return listData;
    }

    public async static Task<DataLevel> GetData(int id)
    {
        List<DataLevel> list = await GetListData();
        foreach (DataLevel d in list)
        {
            // start ID from zero 
            if (d.ID == id/* - 1*/)
                return d;
        }
        return null;
    }
}
