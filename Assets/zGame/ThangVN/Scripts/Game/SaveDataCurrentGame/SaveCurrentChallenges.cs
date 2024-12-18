using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveCurrentChallenges
{
    public List<ColorPlateInTable> ListColorPlate = new List<ColorPlateInTable>();
    public int currentPoint;
    public int countDiff;
}
