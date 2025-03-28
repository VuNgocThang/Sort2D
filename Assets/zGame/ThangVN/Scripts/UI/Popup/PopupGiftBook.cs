using System;
using System.Collections;
using System.Collections.Generic;
using ntDev;
using UnityEngine;

public class PopupGiftBook : Popup
{
    [SerializeField] private EasyButton btnClosePopup, btnClaim;

    private void Awake()
    {
        btnClosePopup.OnClick(Hide);
        btnClaim.OnClick(() =>
        {
            if (!AdsController.instance.IsRewardedVideoAvailable())
            {
                EasyUI.Toast.Toast.Show("No Ads Now", 1f);
            }
            else
            {
                AdsController.instance.ShowRewardedVideo(successful =>
                {
                    if (successful)
                    {
                        ClaimReward();
                    }
                }, null, "Claim Gift Book");
            }
        });
    }

    public async static void Show()
    {
        PopupGiftBook pop = await ManagerPopup.ShowPopup<PopupGiftBook>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        SaveGame.CanShowGiftBook = false;
    }

    private void ClaimReward()
    {
        SaveGame.Pigment += 50;
        Hide();
    }

    public override void Hide()
    {
        base.Hide();
    }
}