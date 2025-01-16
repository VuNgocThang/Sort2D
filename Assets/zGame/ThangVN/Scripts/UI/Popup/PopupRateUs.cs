using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;

public class PopupRateUs : Popup
{
    [SerializeField] EasyButton btnSubmit;
    public static async void Show()
    {
        PopupRateUs pop = await ManagerPopup.ShowPopup<PopupRateUs>();
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
