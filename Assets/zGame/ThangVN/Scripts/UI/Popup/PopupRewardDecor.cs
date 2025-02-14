using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupRewardDecor : PopupReward
{
    public static async void Show()
    {
        PopupRewardDecor pop = await ManagerPopup.ShowPopup<PopupRewardDecor>();
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

    protected override void ReceiveReward(int countMagicWand, int countCrytalBall, int countMagicCard, bool isPopupRewardDecor = false)
    {
        base.ReceiveReward(countMagicWand, countCrytalBall, countMagicCard, true);
    }

    public override void Hide()
    {
        base.Hide();
    }
}
