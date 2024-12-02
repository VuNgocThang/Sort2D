using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupChallenges : Popup
{
    public static async void Show()
    {
        PopupChallenges pop = await ManagerPopup.ShowPopup<PopupChallenges>();

        pop.Init();
    }

    public override void Init()
    {
        base.Init();
    }
}