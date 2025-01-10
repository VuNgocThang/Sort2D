using BaseGame;
using ntDev;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class PopupWinMiniGame : Popup
{
    [SerializeField] EasyButton btnContinue, btnHome;
    [SerializeField] SkeletonGraphic spineBox;
    [SerializeField] Animator animShow;
    [SerializeField] TextMeshProUGUI txtCoin, txtPigment;

    const string DROP = "drop";
    const string IDLE = "idle";

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
        txtCoin.text = $"{SaveGame.Coin}";
        txtPigment.text = $"{SaveGame.Pigment}";
        StartCoroutine(PlayAnimation());
    }

    public override void Hide()
    {
        base.Hide();
    }

    void PlayAgain()
    {
        SaveGame.PlayBonus = false;
        ManagerEvent.ClearEvent();
        SceneManager.LoadScene("SceneGame");
    }

    void Continue()
    {
        SaveGame.PlayBonus = false;
        ManagerEvent.ClearEvent();
        SceneManager.LoadScene("SceneHome");
    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        spineBox.gameObject.SetActive(true);
        PlaySpine(DROP, false);
        yield return new WaitForSeconds(1f);
        if (animShow != null)
            animShow.Play("Show");

        yield return new WaitForSeconds(0.5f);
        if (animShow != null)
            animShow.Play("Move");
    }

    public void PlaySpine(string animationName, bool isLoop = false)
    {
        if (spineBox.AnimationState != null)
        {
            Debug.Log("Play spine");
            spineBox.AnimationState.SetAnimation(0, animationName, isLoop);
        }
    }
}
