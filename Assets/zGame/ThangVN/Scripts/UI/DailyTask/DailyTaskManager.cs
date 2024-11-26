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
    }

    public DailyTaskData dailyTaskData;

    public DailyTaskSaved dataSaved;

    public bool IsNewDay;

    private void Start()
    {
        dataSaved.listTaskSaved = dailyTaskData.listTasks;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("ExecuteDailyTask");

            ExecuteDailyTask(TaskType.CompleteLevel, 1);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("ExecuteDailyTask");

            ExecuteDailyTask(TaskType.CountMerge, 2);
        }
    }

    public void ExecuteDailyTask(TaskType taskType, int amount)
    {
        TaskData taskData = dataSaved.listTaskSaved.Find(x => x.taskType == taskType);

        if (taskData == null) return;

        taskData.taskGoal.currentProgress += amount;
        SaveData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(new DailyTaskSaved()
        {
            listTaskSaved = dataSaved.listTaskSaved
        }, true);

        PlayerPrefs.SetString(GameConfig.TASK_DATA, json);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        string json = PlayerPrefs.GetString(GameConfig.TASK_DATA, "");

        Debug.Log(" LoadData: " + json);

        if (!string.IsNullOrEmpty(json))
        {
            DailyTaskSaved dailyTaskSaved = JsonUtility.FromJson<DailyTaskSaved>(json);
            dataSaved = dailyTaskSaved;
        }

    }

    void Test()
    {
        for (int i = 0; i < dataSaved.listTaskSaved.Count; i++)
        {
            if (dataSaved.listTaskSaved[i].taskGoal.IsCompleted())
            {
                Debug.Log("dataSaved.listTaskSaved[i]: " + dataSaved.listTaskSaved[i].taskType);
            }
        }
    }

    void RefreshData()
    {
        DailyTaskSaved dailyTaskSaved = new DailyTaskSaved();

        for (int i = 0; i < dataSaved.listTaskSaved.Count; i++)
        {
            dataSaved.listTaskSaved[i].taskGoal.ResetData();
        }

        dailyTaskSaved = dataSaved;

        SaveData();
    }

    void CheckNewDay()
    {
        if (SaveGame.NewDay != DateTime.Now.DayOfYear)
        {
            SaveGame.NewDay = DateTime.Now.DayOfYear;
            IsNewDay = true;
        }
    }
}
