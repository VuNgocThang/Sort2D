using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ntDev;
[Serializable]
public class DataLand
{
    // mỗi cột trong excel là 1 Dataland

    public int ID;
    public int[] Type;
    static List<DataLand> listData;
    public async static Task<List<DataLand>> GetListData()
    {
        if (listData == null || listData.Count == 0)
        {
            listData = new List<DataLand>();
            List<DataLand> list = JsonHelper.GetJsonList<DataLand>((await ManagerAsset.LoadAssetAsync<TextAsset>("DataLand")).text);
            listData.AddRange(list);
        }
        return listData;
    }
    public async static Task<DataLand> GetData(int id)
    {
        List<DataLand> list = await GetListData();
        foreach (DataLand d in list)
        {
            if (d.ID == id)
                return d;
        }
        return null;
    }
}