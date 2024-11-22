using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DailyTask : MonoBehaviour
{
    public TaskType taskType;
    public int countStar;
    public TextMeshProUGUI txtNameTask, txtProgress;
    public Image imgProgress;
    [SerializeField] EasyButton btnClaim;

    private void Awake()
    {
        btnClaim.OnClick(ClaimRewardDailyTask);
    }


    void ClaimRewardDailyTask()
    {

    }


    void SaveData()
    {
        TaskData taskData = new TaskData();
    }
}
