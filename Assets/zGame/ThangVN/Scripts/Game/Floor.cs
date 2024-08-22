using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Floor
{
    public List<Step> Steps = new List<Step>();

    public void AddStepRecursively(ColorPlate colorRoot, List<ColorPlate> listDataConnect, HashSet<ColorPlate> processedNearBy)
    {
        ColorPlate colorNearBy = new Step().ColorNearByColorRoot(colorRoot, listDataConnect, processedNearBy);

        if (colorNearBy == null || processedNearBy.Contains(colorNearBy))
        {
            return;
        }

        processedNearBy.Add(colorNearBy);

        Steps.Add(new Step
        {
            rootColorPlate = colorRoot,
            nearByColorPlate = colorNearBy
        });

        AddStepRecursively(colorRoot, listDataConnect, processedNearBy);
    }
}
