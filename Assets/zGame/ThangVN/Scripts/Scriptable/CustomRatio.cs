using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomRatio", menuName = "ScriptableObjects/CustomRatio")]
public class CustomRatio : ScriptableObject
{
    public List<LevelData> listLevelData;
}
