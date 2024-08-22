using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUnlockColor : Popup
{
    [SerializeField] Image icon;
    [SerializeField] NewColorData newColorData;

    public static async void Show(int index)
    {
        PopupUnlockColor pop = await ManagerPopup.ShowPopup<PopupUnlockColor>();

        pop.Initialized(index);
    }

    public void Initialized(int index)
    {
        base.Init();
        LogicGame.Instance.isPauseGame = true;
        Debug.Log("Show" + index);

        for (int i = 0; i < newColorData.listNewColorData.Count; i++)
        {
            if (index == (int)newColorData.listNewColorData[i].newColorEnum)
            {
                icon.sprite = newColorData.listNewColorData[i].spriteIcon;
            }
        }
    }

    public override void Hide()
    {
        base.Hide();
        LogicGame.Instance.isPauseGame = false;
        //ManagerEvent.RaiseEvent(EventCMD.EVENT_SPAWN_PLATE);

    }
}
