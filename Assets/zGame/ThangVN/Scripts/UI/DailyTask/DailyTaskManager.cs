using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyTaskManager : MonoBehaviour
{
    public static DailyTaskManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(Instance);
    }

    public DailyTaskData dailyTaskData;

    public DailyTaskSaved dataSaved;

    public bool IsNewDay;

    private void Start()
    {
        // ManagerEvent.RegEvent(EventCMD.EVENT_DAILYTASK, SaveCurrentStar);

        Init();
    }

    private void Init()
    {
        dataSaved.listTaskSaved = dailyTaskData.listTasks;
        dataSaved.currentPoint = dailyTaskData.currentPoint;
        // Debug.Log("Day: " + SaveGame.NewDay);
        CheckNewDay();

        if (IsNewDay)
        {
            RefreshData();
        }
        else
        {
            LoadData();
        }
    }

    public void ExecuteDailyTask(TaskType taskType, int amount)
    {
        TaskData taskData = dataSaved.listTaskSaved.Find(x => x.taskType == taskType);
        if (taskData == null) return;
        taskData.taskGoal.currentProgress += amount;
        SaveData();
    }

    //public void SaveCurrentStar(object e)
    //{
    //    dataSaved.currentPoint += (int)e;
    //    Debug.Log("point: " + dataSaved.currentPoint);

    //    SaveData();
    //}

    public void SaveData()
    {
        string json = JsonUtility.ToJson(new DailyTaskSaved()
        {
            listTaskSaved = dataSaved.listTaskSaved,
            currentPoint = dataSaved.currentPoint
        }, true);

        PlayerPrefs.SetString(GameConfig.TASK_DATA, json);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        string json = PlayerPrefs.GetString(GameConfig.TASK_DATA, "");

        // Debug.Log(" LoadData: " + json);

        if (!string.IsNullOrEmpty(json))
        {
            DailyTaskSaved dailyTaskSaved = JsonUtility.FromJson<DailyTaskSaved>(json);
            dataSaved = dailyTaskSaved;
        }
    }

    private void RefreshData()
    {
        for (int i = 0; i < dataSaved.listTaskSaved.Count; i++)
        {
            dataSaved.listTaskSaved[i].isClaimed = false;
            dataSaved.listTaskSaved[i].taskGoal.ResetData();
        }

        dataSaved.currentPoint = 0;
        ResetRewardClaim();
        SaveData();
    }

    void ResetRewardClaim()
    {
        SaveGame.ClaimReward1 = false;
        SaveGame.ClaimReward2 = false;
        SaveGame.ClaimReward3 = false;
    }

    private void CheckNewDay()
    {
        if (SaveGame.NewDay != DateTime.Now.DayOfYear)
        {
            SaveGame.NewDay = DateTime.Now.DayOfYear;
            IsNewDay = true;
        }
    }

    private bool NotYetClaimedAllReward()
    {
        bool isNotClaimedAllReward = false;

        if (dataSaved.currentPoint >= 30f && !SaveGame.ClaimReward1)
            return true;
        if (dataSaved.currentPoint >= 80f && !SaveGame.ClaimReward2)
            return true;
        if (dataSaved.currentPoint >= 120f && !SaveGame.ClaimReward3)
            return true;

        //if (dataSaved.currentPoint >= 30f)
        //{
        //    if (SaveGame.ClaimReward1 && SaveGame.ClaimReward2 && SaveGame.ClaimReward3) return false;
        //}
        //else
        //    return false;

        return isNotClaimedAllReward;
    }

    private bool HasMissionDone()
    {
        bool hasMissionDone = false;

        for (int i = 0; i < dataSaved.listTaskSaved.Count; i++)
        {
            if (dataSaved.listTaskSaved[i].isClaimed) continue;

            if (dataSaved.listTaskSaved[i].taskGoal.IsCompleted()) hasMissionDone = true;
        }

        return hasMissionDone;
    }

    public bool CheckNotice()
    {
        return (NotYetClaimedAllReward() || HasMissionDone());
    }
}