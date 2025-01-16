using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using Spine.Unity;
using TMPro;

public class PopupReward : Popup
{
    [SerializeField] protected EasyButton btnClaim, btnClaimx2;
    [SerializeField] protected Animator animShow;
    [SerializeField] protected int gold, countMagicWand, countCrytalBall, countMagicCard;
    [SerializeField] protected SkeletonGraphic spineBox;
    const string DROP = "drop";

    private void Awake()
    {
        btnClaimx2.OnClick(() =>
        {
            ClaimReward();
            Hide();
        });
    }

    public static async void Show()
    {
        PopupReward pop = await ManagerPopup.ShowPopup<PopupReward>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        ClaimReward();
        StartCoroutine(PlayAnimation());
    }

    protected virtual void ClaimReward()
    {
        SaveGame.Coin += gold;
        SaveGame.Hammer += countMagicWand;
        SaveGame.Refresh += countCrytalBall;
        SaveGame.Swap += countMagicCard;
    }

    public virtual IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        spineBox.gameObject.SetActive(true);
        PlaySpine(DROP, false);
        yield return new WaitForSeconds(1f);
        if (animShow != null)
            animShow.Play("Show");
    }

    public void PlaySpine(string animationName, bool isLoop = false)
    {
        if (spineBox.AnimationState != null)
        {
            Debug.Log("Play spine");
            spineBox.AnimationState.SetAnimation(0, animationName, isLoop);
        }
    }


    public override void Hide()
    {
        base.Hide();
    }
}
