using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoosterEnum
{
    BoosterHammer,
    BoosterSwap,
    BoosterRefresh
}

[Serializable]
public class Booster
{
    public string nameBooster;
    public Sprite spriteBooster;
    public Sprite spriteIcon;
    public Sprite spriteText;
    public BoosterEnum boosterEnum;
    public string textExplain;
    public int cost;
}

[CreateAssetMenu(fileName = "BoosterData", menuName = "ScriptableObjects/BoosterData")]
public class BoosterData : ScriptableObject
{
    public List<Booster> listBooster;
}
