using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveCurrentDataGame
{
    public int currentLevel;
    public List<ColorPlateInTable> ListColorPlate = new List<ColorPlateInTable>();
    public int currentPoint;
    public int countDiff;
}