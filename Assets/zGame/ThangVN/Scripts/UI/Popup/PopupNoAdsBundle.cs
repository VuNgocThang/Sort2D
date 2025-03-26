using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using TMPro;
using static PurchaserManager;

public class PopupNoAdsBundle : Popup
{
    [SerializeField] EasyButton btnBuy;
    [SerializeField] TextMeshProUGUI txtCost;
    [SerializeField] GameObject btnNoAdsBundleGrey;

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

    async void BuyBundle()
    {
        //SaveGame.IsBoughtNoAds = true;
        //HomeUI.Instance.btnNoAdsBundle.gameObject.SetActive(false);
        //Debug.Log("Buy Bundle No Ads");
        //Config.SetRemoveAd();
        //Hide();

        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.ads_bundle_7, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();
                HomeUI.Instance.btnNoAdsBundle.gameObject.SetActive(false);
                SaveGame.IsBoughtNoAds = true;
                UpdateButtonBoughtGrey();
                int coinBefore = SaveGame.Coin;
                int countCrytal = 1;
                int countMagicCard = 5;
                int countWand = 5;
                int countGold = 0;
                AddItem(countGold, countCrytal, countMagicCard, countWand);
                PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, false);

                Config.SetRemoveAd();
            }
            else if (state == IAP_CALLBACK_STATE.FAIL)
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });
    }

    void AddItem(int gold, int crytal, int magicCard, int wand)
    {
        SaveGame.Coin += gold;
        SaveGame.Refresh += crytal;
        SaveGame.Swap += magicCard;
        SaveGame.Hammer += wand;
    }

    private void UpdateButtonBoughtGrey()
    {
        btnNoAdsBundleGrey.SetActive(SaveGame.IsBoughtNoAds);
    }
}