using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using TMPro;
using UnityEngine.UI;

public class PopupDailyTask : Popup
{
    float maxDay = 150f;
    public List<DailyTask> listDailyTasks;
    public DailyTask dailyTaskPrefab;
    public Transform nHolderContent;
    [SerializeField] DailyTaskSaved data;
    [SerializeField] int currentPoint;
    [SerializeField] TextMeshProUGUI txtCurrentPoint;
    [SerializeField] Image imgFillProgress;
    [SerializeField] EasyButton btnInfo;

    private void Awake()
    {
        btnInfo.OnClick(PopupInfoDailyTask.Show);
    }

    public static async void Show()
    {
        PopupDailyTask pop = await ManagerPopup.ShowPopup<PopupDailyTask>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();

        ManagerEvent.RegEvent(EventCMD.EVENT_DAILYTASK, UpdateCountStar);

        data = DailyTaskManager.Instance.dataSaved;

        RefreshList();

        RefreshPoint();

        CreateContent();
    }

    void RefreshPoint()
    {
        currentPoint = data.currentPoint;
        txtCurrentPoint.text = currentPoint.ToString();
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

    void UpdateCountStar(object e)
    {
        currentPoint += (int)e;
        txtCurrentPoint.text = currentPoint.ToString();
    }


    private void Update()
    {
        if (imgFillProgress != null)
            imgFillProgress.fillAmount = (float)currentPoint / maxDay;
    }
}
