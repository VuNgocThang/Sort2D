using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        btnGo.OnClick(() =>
        {
            SaveGame.PlayBonus = false;
            MoveToMissionDailyTask(taskType);
        });
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
        Debug.Log("starReward: " + starReward);
        ManagerEvent.RaiseEvent(EventCMD.EVENT_DAILYTASK, starReward);
        DailyTaskManager.Instance.dataSaved.currentPoint += starReward;
        SaveData();
    }


    void SaveData()
    {
        TaskData taskData = DailyTaskManager.Instance.dataSaved.listTaskSaved.Find(x => x.taskType == taskType);

        if (taskData == null) return;

        taskData.isClaimed = isClaimed;

        DailyTaskManager.Instance.SaveData();
    }

    void MoveToMissionDailyTask(TaskType taskType)
    {
        switch (taskType)
        {
            case TaskType.CollectFreeCoins:
                PopupFreeCoin.Show();
                break;
            case TaskType.DecorateBook:
                ManagerPopup.HidePopup<PopupDailyTask>();
                PopupDecor.Show();
                break;
            case TaskType.PlayChallenges:
                if (SaveGame.Level >= 15)
                {
                    ManagerEvent.ClearEvent();
                    SaveGame.Challenges = true;
                    SceneManager.LoadScene("SceneGame");
                }
                else
                {
                    ManagerEvent.ClearEvent();
                    SaveGame.Challenges = false;
                    SceneManager.LoadScene("SceneGame");  
                }
                break;
            default:
                ManagerEvent.ClearEvent();
                SceneManager.LoadScene("SceneGame");
                break;
        }
    }
}
