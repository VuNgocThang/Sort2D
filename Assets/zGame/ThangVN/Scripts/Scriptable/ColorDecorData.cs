using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumColor
{
    blue,
    purple,
    red
}

[Serializable]
public class ColorData
{
    public EnumColor enumColor;
    public Color color;
}

[CreateAssetMenu(fileName = "ColorDecorData", menuName = "ScriptableObjects/ColorDecorData")]
public class ColorDecorData : ScriptableObject
{
    public List<ColorData> listColorData;
}
