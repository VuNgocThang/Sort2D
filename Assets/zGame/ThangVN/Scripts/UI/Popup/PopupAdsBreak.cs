using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAdsBreak : Popup
{
    public static async void Show(string pWhere)
    {
        PopupAdsBreak pop = await ManagerPopup.ShowPopup<PopupAdsBreak>();
        pop.Init();
        pop.Initialized(pWhere);
    }

    public override void Init()
    {
        base.Init();
    }

    private void Initialized(string pWhere)
    {
        StartCoroutine(ShowInterAds(pWhere));
    }

    IEnumerator ShowInterAds(string pWhere)
    {
        yield return new WaitForSeconds(1f);
        ManagerEvent.RaiseEvent(EventCMD.EVENT_INTER_ADS, pWhere);
        Hide();
        // AdsController.instance.ShowInterAd(null, pWhere);
        // HomeUI.Instance.InitFirstDecor();
    }

    public override void Hide()
    {
        base.Hide();
    }
}