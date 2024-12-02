using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionProgress
{
    public MissionType missionType;
    public int current;

    public MissionProgress(MissionType missionType, int current)
    {
        this.missionType = missionType;
        this.current = current;
    }
}
