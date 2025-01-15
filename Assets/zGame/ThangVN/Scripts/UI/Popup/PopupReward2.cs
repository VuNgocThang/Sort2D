using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupReward2 : PopupReward
{
    public static async void Show()
    {
        PopupReward2 pop = await ManagerPopup.ShowPopup<PopupReward2>();
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

    protected override void ClaimReward()
    {
        base.ClaimReward();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
