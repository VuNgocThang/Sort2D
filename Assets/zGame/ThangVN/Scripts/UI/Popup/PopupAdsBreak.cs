using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAdsBreak : Popup
{
    public static async void Show()
    {
        PopupAdsBreak pop = await ManagerPopup.ShowPopup<PopupAdsBreak>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
