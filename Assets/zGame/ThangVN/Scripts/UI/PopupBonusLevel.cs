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
        btnHelp.OnClick(MoveToLevelBonus);
        btnNo.OnClick(MoveToLevelNormal);
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
        ManagerEvent.ClearEvent();
        SceneManager.LoadScene("SceneGame");
    }

    void MoveToLevelNormal()
    {
        SaveGame.PlayBonus = false;
        ManagerEvent.ClearEvent();
        SceneManager.LoadScene("SceneGame");
    }
}
