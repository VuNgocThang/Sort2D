using ntDev;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupWinMiniGame : Popup
{
    [SerializeField] EasyButton btnContinue, btnHome;
    [SerializeField] SkeletonGraphic spineBox;
    [SerializeField] Animator animShow;
    [SerializeField] TextMeshProUGUI txtCoin, txtPigment;
    [SerializeField] Reward rewardPrefab;
    [SerializeField] Transform nReward;

    [SerializeField] CustomerMissionData customerMissionData;

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
        InitReward();
        StartCoroutine(PlayAnimation());
    }

    void InitReward()
    {
        List<DataReward> rewards = customerMissionData.listLevelBonus[SaveGame.LevelBonus - 10001].listRewards;

        for (int i = 0; i < rewards.Count; i++)
        {
            Reward reward = Instantiate(rewardPrefab, nReward);
            reward.Init(rewards[i].typeReward, rewards[i].count);
        }
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
