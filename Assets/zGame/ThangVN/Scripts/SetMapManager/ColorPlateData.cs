using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SpecialData
{
    public int Row;
    public int Col;
    public int type;
    public int pointUnlock;
}

[Serializable]
public class ArrowData
{
    public int Row;
    public int Col;
    public int type;
}

[Serializable]
public class ExistedData
{
    public int Row;
    public int Col;
    public int type;
}
[Serializable]
public class EmptyData
{
    public int Row;
    public int Col;
    public int type;
}



[Serializable]
public class ColorPlateData
{
    public List<SpecialData> listSpecialData;
    public List<ArrowData> listArrowData;
    public List<ExistedData> listExistedData;
    public List<EmptyData> listEmptyData;
    public int rows;
    public int cols;
    public int goalScore;
    public int gold;
    public int pigment;
}
// pos, so mau

