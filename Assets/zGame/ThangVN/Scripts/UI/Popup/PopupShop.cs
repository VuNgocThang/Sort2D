using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;

public class PopupShop : MonoBehaviour
{
    [SerializeField] EasyButton btnNoAdsBundle, btnNoAds;

    private void Awake()
    {
        btnNoAdsBundle.OnClick(() => { Debug.Log("NoAdsBundle"); });
        btnNoAds.OnClick(() => { Debug.Log("NoAdsBundle"); });
    }
   
}
