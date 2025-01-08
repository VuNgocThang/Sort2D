using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;

public class PopupNoAdsBundle : Popup
{
    [SerializeField] EasyButton btnBuy;

    private void Awake()
    {
        btnBuy.OnClick(BuyBundle);
    }
    public static async void Show()
    {
        PopupNoAdsBundle pop = await ManagerPopup.ShowPopup<PopupNoAdsBundle>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
    }

    void BuyBundle()
    {
        Debug.Log("Buy Bundle No Ads");
    }
}
