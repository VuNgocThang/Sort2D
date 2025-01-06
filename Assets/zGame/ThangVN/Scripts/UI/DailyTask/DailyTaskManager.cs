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
        //ManagerEvent.RegEvent(EventCMD.EVENT_DAILYTASK, SaveCurrentStar);

        dataSaved.listTaskSaved = dailyTaskData.listTasks;
        dataSaved.currentPoint = dailyTaskData.currentPoint;
        Debug.Log("Day: " + SaveGame.NewDay);
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
            //dataSaved.currentPoint = 10000;
            ManagerEvent.RaiseEvent(EventCMD.EVENT_DAILYTASK, (int)10);
            SaveData();
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
            dataSaved.listTaskSaved[i].isClaimed = false;
            dataSaved.listTaskSaved[i].taskGoal.ResetData();
        }

        dataSaved.currentPoint = 0;

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
