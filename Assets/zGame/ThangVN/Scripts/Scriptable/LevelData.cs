using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int indexLevel;
    public int countColor;
    public List<int> listRatioSpawnColor;
    public List<int> listRatioCountInStack;
    public int maxPoint;
}
