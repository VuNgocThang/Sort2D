using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.SceneManagement;

public class PopupBonusLevel : Popup
{
    [SerializeField] EasyButton btnHelp, btnNo;

    private void Awake()
    {
        btnHelp.OnClick(() =>
        {
            if (!AdsController.instance.IsRewardedVideoAvailable())
            {
                EasyUI.Toast.Toast.Show("No Ads Now", 1f);
            }
            else
            {
                AdsController.instance.ShowRewardedVideo(successful =>
                    {
                        if (successful)
                        {
                            MoveToLevelBonus();
                        }
                    }, null,
                    "Reward Play Bonus");
            }
        });

        btnNo.OnClick(MoveToHome);
    }

    public static async void Show()
    {
        PopupBonusLevel pop = await ManagerPopup.ShowPopup<PopupBonusLevel>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
    }

    void MoveToLevelBonus()
    {
        SaveGame.PlayBonus = true;
        if (DailyTaskManager.Instance != null)
            DailyTaskManager.Instance.ExecuteDailyTask(TaskType.PlayBonusLevel, 1);

        // ManagerEvent.ClearEvent();
        // SceneManager.LoadScene("SceneGame");
        Hide();
        ManagerEvent.RaiseEvent(EventCMD.EVENT_RECEIVE_REWARD, "SceneGame");
    }

    void MoveToHome()
    {
        SaveGame.PlayBonus = false;

        // ManagerEvent.ClearEvent();
        // SceneManager.LoadScene("SceneHome");
        Hide();
        ManagerEvent.RaiseEvent(EventCMD.EVENT_RECEIVE_REWARD, "SceneHome");
    }

    public override void Hide()
    {
        base.Hide();
    }
}