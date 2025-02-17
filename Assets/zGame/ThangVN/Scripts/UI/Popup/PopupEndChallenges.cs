using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PopupEndChallenges : Popup
{
    [FormerlySerializedAs("btnHome")] [SerializeField]
    EasyButton btnContinue;

    [SerializeField] TextMeshProUGUI txtCoin, txtColorPlate, txtScore;
    [SerializeField] GameObject imgBest;

    [SerializeField] int score;

    private void Awake()
    {
        btnContinue.OnClick(PopupRanking.Show);
    }

    public static async void Show()
    {
        PopupEndChallenges pop = await ManagerPopup.ShowPopup<PopupEndChallenges>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();

        if (DailyTaskManager.Instance != null)
            DailyTaskManager.Instance.ExecuteDailyTask(TaskType.PlayChallenges, 1);

        txtCoin.text = SaveGame.Coin.ToString();

        txtColorPlate.text = SaveGame.Pigment.ToString();

        score = LogicGame.Instance.point;

        if (score >= SaveGame.BestScore) imgBest.SetActive(true);
        else imgBest.SetActive(false);

        txtScore.text = score.ToString();
        SaveGame.CurrentScore = score;
        // ManagerEvent.RaiseEvent(EventCMD.EVENT_SCORE_CHALLENGES, score);
    }
}