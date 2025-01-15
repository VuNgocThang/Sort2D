using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using TMPro;
using UnityEngine.UI;
using System;

public class PopupDailyTask : Popup
{
    const float maxDay = 120f;
    const float point1 = 30f;
    const float point2 = 80f;

    public List<DailyTask> listDailyTasks;
    public DailyTask dailyTaskPrefab;
    public Transform nHolderContent;
    [SerializeField] DailyTaskSaved data;
    [SerializeField] int currentPoint;
    [SerializeField] TextMeshProUGUI txtCurrentPoint, txtTimeRemain;
    [SerializeField] Image imgFillProgress;
    [SerializeField] EasyButton btnInfo, btnReward1, btnReward2, btnReward3;
    [SerializeField] GameObject Rewarded1, Rewarded2, Rewarded3, Locked1, Locked2, Locked3;
    [SerializeField] Animator anim1, anim2, anim3;

    private void Awake()
    {
        btnInfo.OnClick(PopupInfoDailyTask.Show);
        btnReward1.OnClick(() =>
        {
            if (CanClaimReward1())
            {
                SaveGame.ClaimReward1 = true;
                Rewarded1.SetActive(true);
                PopupReward1.Show();
            }
        });

        btnReward2.OnClick(() =>
        {
            if (CanClaimReward2())
            {
                SaveGame.ClaimReward2 = true;
                Rewarded2.SetActive(true);
                PopupReward2.Show();
            }
        });

        btnReward3.OnClick(() =>
        {
            if (CanClaimReward3())
            {
                SaveGame.ClaimReward3 = true;
                Rewarded3.SetActive(true);
                PopupReward3.Show();
            }
        });
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

        RefreshReward();

        CreateContent();
    }

    void RefreshPoint()
    {
        currentPoint = data.currentPoint;
        txtCurrentPoint.text = currentPoint.ToString();
    }

    void RefreshReward()
    {
        if (CanClaimReward1())
        {
            PlayAnimReward(Rewarded1, Locked1, anim1);
        }
        else
        {
            Rewarded1.SetActive(SaveGame.ClaimReward1);
            Locked1.SetActive(!SaveGame.ClaimReward1);

        };


        if (CanClaimReward2())
        {
            PlayAnimReward(Rewarded2, Locked2, anim2);
        }
        else
        {
            Rewarded2.SetActive(SaveGame.ClaimReward2);
            Locked2.SetActive(!SaveGame.ClaimReward2);

        };

        if (CanClaimReward3())
        {
            PlayAnimReward(Rewarded3, Locked3, anim3);
        }
        else
        {
            Rewarded3.SetActive(SaveGame.ClaimReward3);
            Locked3.SetActive(!SaveGame.ClaimReward3);

        };
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
            //Debug.Log("data.listTaskSaved[i].taskName: " + data.listTaskSaved[i].taskName);
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
        PlayAnim();

        if (imgFillProgress != null)
            imgFillProgress.fillAmount = (float)currentPoint / maxDay;

        DateTime now = DateTime.Now;

        DateTime midnight = now.Date.AddDays(1);
        TimeSpan timeRemaining = midnight - now;

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);

        txtTimeRemain.text = $"{formattedTime}";
    }

    void PlayAnim()
    {
        if (CanClaimReward1())
        {
            PlayAnimReward(Rewarded1, Locked1, anim1);
        }
        else
            anim1.Play("Default");

        if (CanClaimReward2())
        {
            PlayAnimReward(Rewarded2, Locked2, anim2);
        }
        else
            anim2.Play("Default");

        if (CanClaimReward3())
        {
            PlayAnimReward(Rewarded3, Locked3, anim3);
        }
        else
            anim3.Play("Default");
    }

    void PlayAnimReward(GameObject reward, GameObject locked, Animator animator)
    {
        reward.SetActive(false);
        locked.SetActive(false);
        animator.Play("Show");
    }

    bool CanClaimReward1()
    {
        return (ReachPoint(point1) && !SaveGame.ClaimReward1);
    }

    bool CanClaimReward2()
    {
        return (ReachPoint(point2) && !SaveGame.ClaimReward2);
    }
    bool CanClaimReward3()
    {
        return (ReachPoint(maxDay) && !SaveGame.ClaimReward3);
    }

    bool ReachPoint(float point)
    {
        return currentPoint >= point;
    }

    void ClaimReward()
    {
        Debug.Log("Claim");
        Rewarded1.SetActive(true);
        SaveGame.ClaimReward1 = true;
    }

}
