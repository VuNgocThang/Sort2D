using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using TMPro;
using UnityEngine.SceneManagement;

public class PopupLoseMiniGame : Popup
{
    [SerializeField] EasyButton btnPlayAgain, btnContinue;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        btnPlayAgain.OnClick(() =>
        {
            RefreshButton(false);
            PlayAgain();
        });

        btnContinue.OnClick(() => 
        {
            RefreshButton(false);
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
        RefreshButton(true);
        // StartCoroutine(StopAnimator());
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void RefreshButton(bool enabled)
    {
        btnContinue.enabled = enabled;
        btnPlayAgain.enabled = enabled;
    }

    public void StopAnimation()
    {
        anim.enabled = false;
    }

    IEnumerator StopAnimator()
    {
        yield return new WaitForSeconds(1.4f);
        anim.enabled = false;
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