using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using System.Threading.Tasks;

public class PopupPurchasing : Popup
{
    public static async Task<bool> Show()
    {
        PopupPurchasing pop = await ManagerPopup.ShowPopup<PopupPurchasing>();
        pop.Init();
        return true;
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
