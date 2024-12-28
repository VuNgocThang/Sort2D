using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionProgress
{
    public ColorEnum missionType;
    public int current;

    public MissionProgress(ColorEnum missionType, int current)
    {
        this.missionType = missionType;
        this.current = current;
    }
}
