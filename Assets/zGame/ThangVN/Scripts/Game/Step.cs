using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Step
{
    public ColorPlate rootColorPlate;
    public ColorPlate nearByColorPlate;

    public ColorPlate ColorNearByColorRoot(ColorPlate colorPlate, List<ColorPlate> listDataConnect, HashSet<ColorPlate> processedNearBy)
    {
        ColorPlate colorNearBy = null;
        foreach (var c in colorPlate.ListConnect)
        {
            if (c.countFrozen != 0) continue;

            if (listDataConnect.Contains(c))
            {
                if (processedNearBy.Contains(c)) continue;

                colorNearBy = c;
            }
        }
        return colorNearBy;
    }
}