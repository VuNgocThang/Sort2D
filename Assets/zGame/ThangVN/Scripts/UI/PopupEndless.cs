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
    [SerializeField] GameObject imgGray;

    public static async void Show()
    {
        PopupEndless pop = await ManagerPopup.ShowPopup<PopupEndless>();

        pop.Init();
    }

    public override void Init()
    {
        base.Init();
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
    }

    public override void Hide()
    {
        base.Hide();
    }
}
