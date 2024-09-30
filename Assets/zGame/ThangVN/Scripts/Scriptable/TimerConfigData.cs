using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimerConfigData", menuName = "ScriptableObjects/TimerConfigData")]
public class TimerConfigData : ScriptableObject
{
    public float timeMove;
    public float timeMerge;
    public float timeRun;
}
