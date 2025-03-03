using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupReward1 : PopupReward
{
    public static async void Show()
    {
        PopupReward1 pop = await ManagerPopup.ShowPopup<PopupReward1>();
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
        FirebaseCustom.LogDailyTaskRewardClaimed(1);
    }

    protected override void ReceiveReward(int countMagicWand, int countCrytalBall, int countMagicCard,
        bool isPopupRewardDecor = false)
    {
        base.ReceiveReward(countMagicWand, countCrytalBall, countMagicCard, isPopupRewardDecor);
    }

    public override void Hide()
    {
        base.Hide();
    }
}