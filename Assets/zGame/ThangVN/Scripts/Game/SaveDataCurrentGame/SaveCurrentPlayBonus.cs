using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveCurrentPlayBonus
{
    public List<ColorPlateInTable> ListColorPlate = new List<ColorPlateInTable>();
    public List<BonusMission> ListBonusMissions = new List<BonusMission>();
    public float currentTime;
}

