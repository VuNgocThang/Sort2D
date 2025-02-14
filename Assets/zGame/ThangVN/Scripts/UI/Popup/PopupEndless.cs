using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.SceneManagement;
using TMPro;

public class PopupEndless : Popup
{
    [SerializeField] EasyButton btnContinue;
    [SerializeField] TextMeshProUGUI txtBestScore;
    [SerializeField] GameObject imgGray, hand;

    public static async void Show()
    {
        PopupEndless pop = await ManagerPopup.ShowPopup<PopupEndless>();

        pop.Init();
    }

    public override void Init()
    {
        // base.Init();
        transform.localScale = Vector3.one;
        txtBestScore.text = SaveGame.BestScore.ToString();
        if (SaveGame.Level >= 15)
        {
            imgGray.SetActive(false);
            btnContinue.OnClick(() =>
            {
                SaveGame.Challenges = true;
                ManagerEvent.ClearEvent();
                SceneManager.LoadScene("SceneGame");
            });
        }
        else
        {
            imgGray.SetActive(true);
        }

        if (!SaveGame.IsTutChallenges && SaveGame.Level >= GameConfig.LEVEL_CHALLENGES)
        {
            SaveGame.IsTutChallenges = true;
            hand.SetActive(true);
        }
        else
            hand.SetActive(false);
    }

    public override void Hide()
    {
        base.Hide();
    }
}