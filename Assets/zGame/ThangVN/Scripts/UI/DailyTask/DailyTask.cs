using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DailyTask : MonoBehaviour
{
    public TaskType taskType;
    public string taskName;
    public int starReward;
    public float currentProgress;
    public float reach;
    public bool isClaimed;

    public TextMeshProUGUI txtNameTask, txtProgress, txtStar;
    public Image imgProgress;
    [SerializeField] EasyButton btnClaim, btnGo;
    [SerializeField] GameObject canClaim, claimed, imgStarClaimed;

    public bool CanClaim()
    {
        return currentProgress >= reach;
    }

    private void Awake()
    {
        btnClaim.OnClick(ClaimRewardDailyTask);
    }

    public void Init()
    {
        if (currentProgress >= reach) currentProgress = reach;

        txtNameTask.text = $"{taskName}";
        txtStar.text = starReward.ToString();
        txtProgress.text = $"{currentProgress} / {reach}";
        imgProgress.fillAmount = currentProgress / reach;

        btnGo.gameObject.SetActive(!CanClaim());

        if (CanClaim())
        {
            canClaim.SetActive(!isClaimed);
            claimed.SetActive(isClaimed);
            imgStarClaimed.SetActive(isClaimed);
        }
       
        //if (isClaimed) btnClaim.gameObject.SetActive(false);
    }


    void ClaimRewardDailyTask()
    {
        if (!CanClaim()) return;
        if (isClaimed) return;

        isClaimed = true;

        canClaim.SetActive(!isClaimed);
        claimed.SetActive(isClaimed);
        imgStarClaimed.SetActive(isClaimed);
        ManagerEvent.RaiseEvent(EventCMD.EVENT_DAILYTASK, starReward);
        SaveData();
    }


    void SaveData()
    {
        TaskData taskData = DailyTaskManager.Instance.dataSaved.listTaskSaved.Find(x => x.taskType == taskType);

        if (taskData == null) return;

        taskData.isClaimed = isClaimed;

        DailyTaskManager.Instance.SaveData();
    }
}
