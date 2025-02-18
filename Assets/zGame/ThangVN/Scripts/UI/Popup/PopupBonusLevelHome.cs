using System.Collections;
using System.Collections.Generic;
using ntDev;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupBonusLevelHome : Popup
{
    [SerializeField] EasyButton btnHelp, btnNo;

    private void Awake()
    {
        btnHelp.OnClick(MoveToLevelBonus);
        btnNo.OnClick(MoveToHome);
    }

    public static async void Show()
    {
        PopupBonusLevelHome pop = await ManagerPopup.ShowPopup<PopupBonusLevelHome>();
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

        ManagerEvent.ClearEvent();
        SceneManager.LoadScene("SceneGame");
    }

    void MoveToHome()
    {
        SaveGame.PlayBonus = false;

        ManagerEvent.ClearEvent();
        SceneManager.LoadScene("SceneGame");
    }

    public override void Hide()
    {
        base.Hide();
    }
}