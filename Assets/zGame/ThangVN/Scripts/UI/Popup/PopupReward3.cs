using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupReward3 : PopupReward
{
    public static async void Show()
    {
        PopupReward3 pop = await ManagerPopup.ShowPopup<PopupReward3>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
    }

    public override IEnumerator PlayAnimation()
    {
        return base.PlayAnimation();
    }

    protected override void ClaimReward(int multi)
    {
        base.ClaimReward(multi);
    }

    public override void Hide()
    {
        base.Hide();
    }
}