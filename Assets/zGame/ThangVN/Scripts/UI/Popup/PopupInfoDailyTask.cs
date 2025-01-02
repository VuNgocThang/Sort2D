using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
public class PopupInfoDailyTask : Popup
{
    public List<GameObject> listObjs;

    public static async void Show()
    {
        PopupInfoDailyTask pop = await ManagerPopup.ShowPopup<PopupInfoDailyTask>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
    }
}
