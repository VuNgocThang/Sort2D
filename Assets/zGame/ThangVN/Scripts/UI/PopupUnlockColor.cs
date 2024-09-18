using ntDev;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUnlockColor : Popup
{
    [SerializeField] Image bg, icon;
    [SerializeField] NewColorData newColorData;
    [SerializeField] SkeletonGraphic spineBox;
    [SerializeField] Animator animShow;

    const string DROP = "box_drop";
    const string UNLOCK = "box_opening";

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
                bg.sprite = newColorData.listNewColorData[i].spriteBg;
                icon.sprite = newColorData.listNewColorData[i].spriteIcon;
            }
        }

        StartCoroutine(PlayAnimation());
    }

    public override void Hide()
    {
        base.Hide();
        LogicGame.Instance.isPauseGame = false;
        //ManagerEvent.RaiseEvent(EventCMD.EVENT_SPAWN_PLATE);

    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        spineBox.gameObject.SetActive(true);
        PlaySpine(DROP, false);
        yield return new WaitForSeconds(0.8f);
        PlaySpine(UNLOCK, false);
        if (animShow != null)
            animShow.Play("Show");

        yield return new WaitForSeconds(1f);
        if (animShow != null)
            animShow.Play("Move");


    }

    public void PlaySpine(string animationName, bool isLoop = false)
    {
        if (spineBox.AnimationState != null)
        {
            Debug.Log("Play spine");
            spineBox.AnimationState.SetAnimation(0, animationName, isLoop);
        }
    }
}
