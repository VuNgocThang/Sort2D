using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupWinMiniGame : Popup
{
    [SerializeField] EasyButton btnContinue, btnHome;

    private void Awake()
    {
        btnContinue.OnClick(() =>
        {
            PlayAgain();
        });

        btnHome.OnClick(() =>
        {
            Continue();
        });
    }

    public static async void Show()
    {
        PopupWinMiniGame pop = await ManagerPopup.ShowPopup<PopupWinMiniGame>();
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
        SaveGame.PlayBonus = false;
        SceneManager.LoadScene("SceneGame");
    }

    void Continue()
    {
        SaveGame.PlayBonus = false;
        SceneManager.LoadScene("SceneHome");
    }
}
