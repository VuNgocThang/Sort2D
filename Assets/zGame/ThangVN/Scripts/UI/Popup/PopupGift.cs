using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ntDev;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupGift : Popup
{
    [SerializeField] private EasyButton btnClosePopup, btnGet;
    [SerializeField] private TextMeshProUGUI txtNameBooster;
    [SerializeField] private Image imgBooster;
    [SerializeField] private BoosterData boosterData;
    [SerializeField] private int indexBooster;

    private void Awake()
    {
        btnClosePopup.OnClick(Hide);
        btnGet.OnClick(() =>
        {
            AdsController.instance.ShowRewardedVideo((onCOmpleted) =>
            {
                ClaimReward(indexBooster);
                Hide();
            }, null, "Claim Gift Booster");
        });
    }

    public async static void Show(int index)
    {
        PopupGift pop = await ManagerPopup.ShowPopup<PopupGift>();
        pop.Init();
        pop.ShowBooster(index);
    }

    public override void Init()
    {
        base.Init();
    }

    private void ShowBooster(int index)
    {
        for (int i = 0; i < boosterData.listBooster.Count; i++)
        {
            if (index == (int)boosterData.listBooster[i].boosterEnum)
            {
                indexBooster = index;
                imgBooster.sprite = boosterData.listBooster[i].spriteBooster;
                imgBooster.SetNativeSize();
                txtNameBooster.text = boosterData.listBooster[i].nameBooster;
            }
        }
    }

    private void ClaimReward(int index)
    {
        switch (index)
        {
            case 0:
                SaveGame.Hammer += 2;
                break;
            case 1:
                SaveGame.Swap += 2;
                break;
            case 2:
                SaveGame.Refresh += 2;
                break;
            default:
                break;
        }
    }

    public override void Hide()
    {
        LogicGame.Instance.isPauseGame = false;
        base.Hide();
    }
}