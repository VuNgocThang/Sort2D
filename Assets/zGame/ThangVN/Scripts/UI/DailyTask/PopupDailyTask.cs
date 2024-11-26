using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;


public class PopupDailyTask : Popup
{
    public List<DailyTask> listDailyTasks;
    public DailyTask dailyTaskPrefab;
    public Transform nHolderContent;
    [SerializeField] DailyTaskSaved data;
    public static async void Show()
    {
        PopupDailyTask pop = await ManagerPopup.ShowPopup<PopupDailyTask>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();

        data = DailyTaskManager.Instance.dataSaved;

        RefreshList();

        CreateContent();
    }

    private void RefreshList()
    {
        if (listDailyTasks.Count > 0)
        {
            for (int i = 0; i < listDailyTasks.Count; i++)
            {
                Destroy(listDailyTasks[i].gameObject);
            }
        }

        listDailyTasks.Clear();
    }

    private void CreateContent()
    {
        for (int i = 0; i < data.listTaskSaved.Count; i++)
        {
            DailyTask task = Instantiate(dailyTaskPrefab, nHolderContent);
            task.taskType = data.listTaskSaved[i].taskType;
            Debug.Log("data.listTaskSaved[i].taskName: " + data.listTaskSaved[i].taskName);
            task.taskName = data.listTaskSaved[i].taskName;
            task.starReward = data.listTaskSaved[i].starReward;
            task.currentProgress = data.listTaskSaved[i].taskGoal.currentProgress;
            task.reach = data.listTaskSaved[i].taskGoal.reach;
            task.isClaimed = data.listTaskSaved[i].isClaimed;

            task.Init();
            listDailyTasks.Add(task);
        }
    }


}
