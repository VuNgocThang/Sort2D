using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.UI;

public class PopupShop : MonoBehaviour
{
    [SerializeField] EasyButton btnNoAdsBundle,
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


    void BuyNoAdsBundle()
    {
        //PopupNoAdsBundle.Show();
    }

    void BuyNoAds()
    {
    }

    void BuyPack1()
    {
    }

    void BuyPack2()
    {
    }

    void BuyPack3()
    {
    }

    void BuyCoinPack1()
    {
    }

    void BuyCoinPack2()
    {
    }

    void BuyCoinPack3()
    {
    }

    void BuyCoinPack4()
    {
    }

    void BuyCoinPack5()
    {
    }

    void BuyCoinPack6()
    {
    }

    private const float yCoin = 3200f;
    public void MoveToCoin()
    {
        RectTransform contentRect = scroll.content.GetComponent<RectTransform>();
       
        contentRect.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(contentRect.anchoredPosition.x, yCoin);
    }
}