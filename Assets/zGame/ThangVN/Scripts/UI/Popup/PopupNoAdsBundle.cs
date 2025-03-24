using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;

public class PopupNoAdsBundle : Popup
{
    [SerializeField] EasyButton btnBuy;

    public static async void Show()
    {
        PopupNoAdsBundle pop = await ManagerPopup.ShowPopup<PopupNoAdsBundle>();
        pop.Init();
    }

    private void Awake()
    {
        btnBuy.OnClick(BuyBundle);
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void BuyBundle()
    {
        SaveGame.IsBoughtNoAds = true;
        HomeUI.Instance.btnNoAdsBundle.gameObject.SetActive(false);
        Debug.Log("Buy Bundle No Ads");
        Config.SetRemoveAd();
        Hide();
    }
}