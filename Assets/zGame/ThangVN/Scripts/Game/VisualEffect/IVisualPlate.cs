using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisualPlate
{
    public void Execute(ColorPlate colorPlate, int count, ColorEnum colorEnum, bool plusPoint);
}
