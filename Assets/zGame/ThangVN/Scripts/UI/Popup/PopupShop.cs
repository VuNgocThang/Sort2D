using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.UI;

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

    void BuyNoAdsBundle()
    {
        SaveGame.IsBoughtNoAds = true;
        UpdateButtonBoughtGrey();
        int coinBefore = SaveGame.Coin;
        int countCrytal = 1;
        int countMagicCard = 5;
        int countWand = 5;
        int countGold = 0;
        AddItem(countGold, countCrytal, countMagicCard, countWand);
        PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, false);

        Debug.Log("Buy No Ads Bundle");
        Config.SetRemoveAd();
    }

    void BuyNoAds()
    {
        SaveGame.IsBoughtNoAds = true;
        UpdateButtonBoughtGrey();
        Debug.Log("Buy No Ads");
        Config.SetRemoveAd();
    }

    void BuyPack1()
    {
        SaveGame.IsBoughtWelcomePack = true;
        UpdateButtonBoughtGrey();
        int coinBefore = SaveGame.Coin;
        int countCrytal = 1;
        int countMagicCard = 2;
        int countWand = 2;
        int countGold = 1200;
        AddItem(countGold, countCrytal, countMagicCard, countWand);
        PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

        Debug.Log("Buy WelcomePack");
    }

    void BuyPack2()
    {
        int coinBefore = SaveGame.Coin;
        int countCrytal = 3;
        int countMagicCard = 3;
        int countWand = 3;
        int countGold = 1500;
        AddItem(countGold, countCrytal, countMagicCard, countWand);
        PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

        Debug.Log("Buy CommonPack");
    }

    void BuyPack3()
    {
        int coinBefore = SaveGame.Coin;
        int countCrytal = 3;
        int countMagicCard = 5;
        int countWand = 5;
        int countGold = 2300;
        AddItem(countGold, countCrytal, countMagicCard, countWand);
        PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

        Debug.Log("Buy BookPack");
    }

    void BuyCoinPack1()
    {
        int coinBefore = SaveGame.Coin;
        int countCrytal = 0;
        int countMagicCard = 0;
        int countWand = 0;
        int countGold = 300;
        AddItem(countGold, countCrytal, countMagicCard, countWand);
        PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

        Debug.Log("Buy Coin Pack 1");
    }

    void BuyCoinPack2()
    {
        int coinBefore = SaveGame.Coin;
        int countCrytal = 0;
        int countMagicCard = 0;
        int countWand = 0;
        int countGold = 700;
        AddItem(countGold, countCrytal, countMagicCard, countWand);
        PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

        Debug.Log("Buy Coin Pack 2");

    }

    void BuyCoinPack3()
    {
        int coinBefore = SaveGame.Coin;
        int countCrytal = 0;
        int countMagicCard = 0;
        int countWand = 0;
        int countGold = 1200;
        AddItem(countGold, countCrytal, countMagicCard, countWand);
        PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

        Debug.Log("Buy Coin Pack 3");

    }

    void BuyCoinPack4()
    {
        int coinBefore = SaveGame.Coin;
        int countCrytal = 0;
        int countMagicCard = 0;
        int countWand = 0;
        int countGold = 2300;
        AddItem(countGold, countCrytal, countMagicCard, countWand);
        PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

        Debug.Log("Buy Coin Pack 4");

    }

    void BuyCoinPack5()
    {
        int coinBefore = SaveGame.Coin;
        int countCrytal = 0;
        int countMagicCard = 0;
        int countWand = 0;
        int countGold = 5000;
        AddItem(countGold, countCrytal, countMagicCard, countWand);
        PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

        Debug.Log("Buy Coin Pack 5");

    }

    void BuyCoinPack6()
    {
        int coinBefore = SaveGame.Coin;
        int countCrytal = 0;
        int countMagicCard = 0;
        int countWand = 0;
        int countGold = 12000;
        AddItem(countGold, countCrytal, countMagicCard, countWand);
        PopupRewardShop.Show(countCrytal, countMagicCard, countWand, coinBefore, true);

        Debug.Log("Buy Coin Pack 6");

    }

    private const float yCoin = 3200f;
    public void MoveToCoin()
    {
        RectTransform contentRect = scroll.content.GetComponent<RectTransform>();

        contentRect.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(contentRect.anchoredPosition.x, yCoin);
    }
}