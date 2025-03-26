using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.UI;
using static PurchaserManager;

public class PopupShop : MonoBehaviour
{
    [SerializeField]
    EasyButton btnNoAdsBundle,
        btnNoAds,
        btnPack1,
        btnPack2,
        btnPack3,
        btnCoinPack1,
        btnCoinPack2,
        btnCoinPack3,
        btnCoinPack4,
        btnCoinPack5,
        btnCoinPack6;

    [SerializeField] private ScrollRect scroll;

    [SerializeField] GameObject btnNoAdsBundleGrey, btnNoAdsGrey, btnPack1Grey;

    private void Awake()
    {
        btnNoAdsBundle.OnClick(() => { BuyNoAdsBundle(); });
        btnNoAds.OnClick(() => { BuyNoAds(); });
        btnPack1.OnClick(() => { BuyPack1(); });
        btnPack2.OnClick(() => { BuyPack2(); });
        btnPack3.OnClick(() => { BuyPack3(); });
        btnCoinPack1.OnClick(() => { BuyCoinPack1(); });
        btnCoinPack2.OnClick(() => { BuyCoinPack2(); });
        btnCoinPack3.OnClick(() => { BuyCoinPack3(); });
        btnCoinPack4.OnClick(() => { BuyCoinPack4(); });
        btnCoinPack5.OnClick(() => { BuyCoinPack5(); });
        btnCoinPack6.OnClick(() => { BuyCoinPack6(); });
    }

    private void OnEnable()
    {
        UpdateButtonBoughtGrey();
    }

    private void UpdateButtonBoughtGrey()
    {
        btnNoAdsBundleGrey.SetActive(SaveGame.IsBoughtNoAds);
        btnNoAdsGrey.SetActive(SaveGame.IsBoughtNoAds);
        btnPack1Grey.SetActive(SaveGame.IsBoughtWelcomePack);
    }

    void AddItem(int gold, int crytal, int magicCard, int wand)
    {
        SaveGame.Coin += gold;
        SaveGame.Refresh += crytal;
        SaveGame.Swap += magicCard;
        SaveGame.Hammer += wand;
    }

    async void BuyNoAdsBundle()
    {
        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.ads_bundle_7, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

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

    async void BuyNoAds()
    {
        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.ads_pass_6, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

                SaveGame.IsBoughtNoAds = true;
                UpdateButtonBoughtGrey();
                Config.SetRemoveAd();
            }
            else
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });
    }

    async void BuyPack1()
    {
        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.pack_welcome_5, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

                SaveGame.IsBoughtWelcomePack = true;
                UpdateButtonBoughtGrey();
                int coinBefore = SaveGame.Coin;
                int countCrytal = 1;
                int countMagicCard = 2;
                int countWand = 2;
                int countGold = 1200;
                AddItem(countGold, countCrytal, countMagicCard, countWand);
                PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);
            }
            else
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });
    }

    async void BuyPack2()
    {
        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.pack_common_7, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

                int coinBefore = SaveGame.Coin;
                int countCrytal = 3;
                int countMagicCard = 3;
                int countWand = 3;
                int countGold = 1500;
                AddItem(countGold, countCrytal, countMagicCard, countWand);
                PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);
            }
            else
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });
    }

    async void BuyPack3()
    {
        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.pack_book_9, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

                int coinBefore = SaveGame.Coin;
                int countCrytal = 3;
                int countMagicCard = 5;
                int countWand = 5;
                int countGold = 2300;
                AddItem(countGold, countCrytal, countMagicCard, countWand);
                PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);
            }
            else
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });
    }

    async void BuyCoinPack1()
    {
        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.pack_mini_2, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

                int coinBefore = SaveGame.Coin;
                int countCrytal = 0;
                int countMagicCard = 0;
                int countWand = 0;
                int countGold = 300;
                AddItem(countGold, countCrytal, countMagicCard, countWand);
                PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);
            }
            else
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });
    }

    async void BuyCoinPack2()
    {
        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.pack_small_4, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

                int coinBefore = SaveGame.Coin;
                int countCrytal = 0;
                int countMagicCard = 0;
                int countWand = 0;
                int countGold = 700;
                AddItem(countGold, countCrytal, countMagicCard, countWand);
                PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

            }
            else
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });
    }

    async void BuyCoinPack3()
    {
        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.pack_medium_6, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

                int coinBefore = SaveGame.Coin;
                int countCrytal = 0;
                int countMagicCard = 0;
                int countWand = 0;
                int countGold = 1200;
                AddItem(countGold, countCrytal, countMagicCard, countWand);
                PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

            }
            else
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });
    }

    async void BuyCoinPack4()
    {
        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.pack_large_9, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

                int coinBefore = SaveGame.Coin;
                int countCrytal = 0;
                int countMagicCard = 0;
                int countWand = 0;
                int countGold = 2300;
                AddItem(countGold, countCrytal, countMagicCard, countWand);
                PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);
            }
            else
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });

    }

    async void BuyCoinPack5()
    {
        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.pack_huge_18, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

                int coinBefore = SaveGame.Coin;
                int countCrytal = 0;
                int countMagicCard = 0;
                int countWand = 0;
                int countGold = 5000;
                AddItem(countGold, countCrytal, countMagicCard, countWand);
                PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

            }
            else
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });
    }

    async void BuyCoinPack6()
    {

        bool b = await PopupPurchasing.Show();

        PurchaserManager.instance.BuyConsumable(PurchaserManager.IAP_ID.pack_ultimate_33, (string str, IAP_CALLBACK_STATE state) =>
        {
            if (state == IAP_CALLBACK_STATE.SUCCESS)
            {
                //ManagerPopup.HidePopup<PopupPurchasing>();

                int coinBefore = SaveGame.Coin;
                int countCrytal = 0;
                int countMagicCard = 0;
                int countWand = 0;
                int countGold = 12000;
                AddItem(countGold, countCrytal, countMagicCard, countWand);
                PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);
            }
            else
            {
                ManagerPopup.HidePopup<PopupPurchasing>();

                return;
            }
        });
    }

    private const float yCoin = 3200f;
    public void MoveToCoin()
    {
        RectTransform contentRect = scroll.content.GetComponent<RectTransform>();

        contentRect.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(contentRect.anchoredPosition.x, yCoin);
    }
}