using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    UseBoosters,
    CompleteLevel,
    CollectBooks,
    CountMerge,
    UseBoosterHammer,
    UseBoosterSwap,
    UseBoosterRefresh,
    DecorateBook,
    PlayBonusLevel,
    Revive,
    SpendGold,
    PlayChallenges,
    CollectFreeCoins,
}

[Serializable]
public class TaskGoal
{
    public float reach;
    public float currentProgress;

    public bool IsCompleted()
    {
        return currentProgress >= reach;
    }

    public void ResetData()
    {
        currentProgress = 0;
    }
}

[Serializable]
public class TaskData
{
    public TaskType taskType;
    public string taskName;
    public int starReward;
    public bool isClaimed;

    public TaskGoal taskGoal;

}

[Serializable]
public class DailyTaskSaved
{
    public List<TaskData> listTaskSaved;
}

[CreateAssetMenu(fileName = "DailyTaskData", menuName = "ScriptableObjects/DailyTaskData")]
public class DailyTaskData : ScriptableObject
{
    public List<TaskData> listTasks;
}
