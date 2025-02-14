using System.Collections;
using System.Collections.Generic;
using ntDev;
using UnityEngine;

public class PopupRanking : Popup
{
    public static async void Show()
    {
        PopupRanking pop = await ManagerPopup.ShowPopup<PopupRanking>();
        pop.Init();
    }

    public override void Init()
    {
        transform.localScale = Vector3.one;
    }

    public override void Hide()
    {
        base.Hide();
    }
}
