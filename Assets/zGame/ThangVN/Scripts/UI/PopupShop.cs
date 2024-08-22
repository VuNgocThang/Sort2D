using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;

public class PopupShop : Popup
{
    public static async void Show()
    {
        PopupShop pop = await ManagerPopup.ShowPopup<PopupShop>();

        pop.Init();
    }

    public override void Init()
    {
        base.Init();
    }
}
