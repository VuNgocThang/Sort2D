using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisualPlate
{
    public void Execute(ColorPlate colorPlate, int count, int countClear, ColorEnum colorEnum, bool plusPoint,
        bool playSound);
}