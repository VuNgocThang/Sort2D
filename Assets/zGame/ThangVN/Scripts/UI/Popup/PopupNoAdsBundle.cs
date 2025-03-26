using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using TMPro;

public class PopupNoAdsBundle : Popup
{
    [SerializeField] EasyButton btnBuy;
    [SerializeField] TextMeshProUGUI txtCost;

    public static async void Show()
    {
        PopupNoAdsBundle pop = await ManagerPopup.ShowPopup<PopupNoAdsBundle>();
        pop.Init();
    }

    private void Awake()
    {
        btnBuy.OnClick(BuyBundle);
        txtCost.text = PurchaserManager.instance.GetLocalizedPriceString(PurchaserManager.GetStringIapId(PurchaserManager.IAP_ID.ads_bundle_7), "6.99$");
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