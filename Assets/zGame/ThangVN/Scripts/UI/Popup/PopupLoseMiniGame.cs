using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using TMPro;
using UnityEngine.SceneManagement;

public class PopupLoseMiniGame : Popup
{
    [SerializeField] EasyButton btnPlayAgain, btnContinue;

    private void Awake()
    {
        btnPlayAgain.OnClick(() =>
        {
            PlayAgain();
        });

        btnContinue.OnClick(() =>
        {
            Continue();
        });
    }

    public static async void Show()
    {
        PopupLoseMiniGame pop = await ManagerPopup.ShowPopup<PopupLoseMiniGame>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Hide()
    {
        base.Hide();
    }

    void PlayAgain()
    {
        SaveGame.PlayBonus = true;
        ManagerEvent.ClearEvent();
        SceneManager.LoadScene("SceneGame");
    }

    void Continue()
    {
        SaveGame.PlayBonus = false;
        ManagerEvent.ClearEvent();
        SceneManager.LoadScene("SceneHome");
    }
}
