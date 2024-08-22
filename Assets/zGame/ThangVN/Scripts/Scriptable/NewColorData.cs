using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NewColorEnum
{
    ColorYellow,
    ColorPurple,
    ColorPink,
    ColorRandom,
    ColorOrange
}

[Serializable]
public class NewColor
{
    public int indexLevelUnlock;
    public Sprite spriteIcon;
    public NewColorEnum newColorEnum;
}

[CreateAssetMenu(fileName = "NewColorData", menuName = "ScriptableObjects/NewColorData")]

public class NewColorData : ScriptableObject
{
    public List<NewColor> listNewColorData;
}
