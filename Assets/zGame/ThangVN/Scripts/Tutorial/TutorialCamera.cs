using DG.Tweening;
using ntDev;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;

public class TutorialCamera : MonoBehaviour
{
    public static TutorialCamera Instance;
    [SerializeField] Camera cam;
    [SerializeField] RectTransform hand;
    [SerializeField] Transform handObject;
    [SerializeField] GameObject particleHand;
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] List<GameObject> listSteps;
    [SerializeField] PopupHome popupHome;
    [SerializeField] Transform nBlack, nBlackTut;
    [SerializeField] EasyButton btnContinue, btnContinueBlack, btnContinueBlackBooster, btnContinueLockCoin, btnContinueFrozen;
    public bool isDoneStep2;

    private void Awake()
    {
        Instance = this;
        btnContinue.OnClick(() =>
        {
            SaveGame.IsDoneTutorial = true;
            LogicGame.Instance.isPauseGame = false;
            LogicGame.Instance.CheckClear();
            HideHandTut();
            btnContinue.gameObject.SetActive(false);
        });

        btnContinueBlack.OnClick(() =>
        {
            SaveGame.IsDoneTutPoint = true;
            popupHome.ResetNBar();
            HideHandTut();
            RefreshArrow();
            nBlack.gameObject.SetActive(false);
        });

        btnContinueLockCoin.OnClick(() =>
        {
            SaveGame.IsDoneTutLockCoin = true;
            LogicGame.Instance.isPauseGame = false;
            RefreshLockCoin();

            btnContinueLockCoin.gameObject.SetActive(false);
        });

        btnContinueFrozen.OnClick(() =>
        {
            SaveGame.IsDoneTutFrozen = true;
            LogicGame.Instance.isPauseGame = false;
            RefreshFrozen();
            btnContinueFrozen.gameObject.SetActive(false);
        });

        btnContinueBlackBooster.OnClick(() =>
        {
            CloseTutorialBooster();
        });
    }

    public void CloseTutorialBooster()
    {
        HideHandTut();
        LogicGame.Instance.isPauseGame = false;
        btnContinueBlackBooster.gameObject.SetActive(false);
        LogicGame.Instance.homeInGame.ResetPositionAfterTutorial();
    }

    private void Start()
    {
        particleHand.gameObject.SetActive(false);
    }

    public void MoveHand(int index, int indexStep)
    {
        if (index == 0) isDoneStep2 = true;
        LogicGame.Instance.ListArrowPlate[index].canClick = true;
        Vector3 pos = LogicGame.Instance.ListArrowPlate[index].transform.position;
        SetPositionHand(pos);
        listSteps[indexStep].SetActive(true);
    }

    private void SetPositionHand(Vector3 pos)
    {
        Vector3 position = cam.WorldToScreenPoint(pos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            position,
            cam,
            out Vector2 localPoint
        );
        hand.anchoredPosition = localPoint;
        hand.gameObject.SetActive(true);
        particleHand.gameObject.SetActive(true);
    }

    public void PlayTut3()
    {
        btnContinue.gameObject.SetActive(true);
        listSteps[2].SetActive(true);
    }

    public void PlayTut4()
    {
        nBlack.gameObject.SetActive(true);
        popupHome.nBar.transform.SetParent(nBlack);
        listSteps[3].SetActive(true);
    }

    public void HideHandTut()
    {
        for (int i = 0; i < listSteps.Count; i++)
        {
            listSteps[i].SetActive(false);
        }

        hand.gameObject.SetActive(false);
    }

    public void RefreshArrow()
    {
        for (int i = 0; i < LogicGame.Instance.ListArrowPlate.Count; i++)
        {
            LogicGame.Instance.ListArrowPlate[i].canClick = true;
        }
    }

    public void InitTutorialLockCoin()
    {
        btnContinueLockCoin.gameObject.SetActive(true);

        for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
        {
            ColorPlate colorPlate = LogicGame.Instance.ListColorPlate[i];
            if (colorPlate.status == Status.LockCoin)
            {
                colorPlate.logicVisual.SetTutLockCoin();
                colorPlate.txtPointUnlock.sortingOrder = 17;

            }
        }
    }

    public void RefreshLockCoin()
    {
        for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
        {
            ColorPlate colorPlate = LogicGame.Instance.ListColorPlate[i];
            if (colorPlate.status == Status.LockCoin)
            {
                colorPlate.logicVisual.ResetTut(colorPlate.Row);
                colorPlate.txtPointUnlock.sortingOrder = 0;

            }
        }
    }

    public void InitTutorialFrozen()
    {
        btnContinueFrozen.gameObject.SetActive(true);

        for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
        {
            ColorPlate colorPlate = LogicGame.Instance.ListColorPlate[i];
            if (colorPlate.status == Status.Frozen)
            {
                colorPlate.logicVisual.SetTutFrozen();
            }
        }
    }

    public void RefreshFrozen()
    {
        for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
        {
            ColorPlate colorPlate = LogicGame.Instance.ListColorPlate[i];
            if (colorPlate.status == Status.Frozen)
            {
                colorPlate.logicVisual.ResetTut(colorPlate.Row);
            }
        }
    }

    public void InitTutorialBoosterRefresh()
    {
        hand.position = LogicGame.Instance.homeInGame.btnRefresh.GetComponent<RectTransform>().position;
        LogicGame.Instance.homeInGame.btnRefresh.transform.SetParent(btnContinueBlackBooster.transform);
        btnContinueBlackBooster.gameObject.SetActive(true);
        hand.gameObject.SetActive(true);
        listSteps[4].SetActive(true);
    }

    public void InitTutorialBoosterHammer()
    {
        hand.position = LogicGame.Instance.homeInGame.btnHammer.GetComponent<RectTransform>().position;
        LogicGame.Instance.homeInGame.btnHammer.transform.SetParent(btnContinueBlackBooster.transform);
        btnContinueBlackBooster.gameObject.SetActive(true);
        hand.gameObject.SetActive(true);
        listSteps[5].SetActive(true);

    }

    public void InitTutorialBoosterSwap()
    {
        hand.position = LogicGame.Instance.homeInGame.btnSwap.GetComponent<RectTransform>().position;
        LogicGame.Instance.homeInGame.btnSwap.transform.SetParent(btnContinueBlackBooster.transform);
        btnContinueBlackBooster.gameObject.SetActive(true);
        hand.gameObject.SetActive(true);
        listSteps[7].SetActive(true);

    }


    public void TutorialHammer()
    {
        LogicGame.Instance.homeInGame.itemObj.transform.SetParent(nBlackTut);
        handObject.position = LogicGame.Instance.ListColorPlate[16].transform.position;
        handObject.SetActive(true);
        nBlackTut.SetActive(true);
        listSteps[6].SetActive(true);

        for (int i = 0; i < LogicGame.Instance.ListColorPlate[16].ListColor.Count; i++)
        {
            LogicColor c = LogicGame.Instance.ListColorPlate[16].ListColor[i];
            c.spriteRender.sortingOrder = 17;
        }
    }

    public void TutorialSwap()
    {
        LogicGame.Instance.homeInGame.itemObj.transform.SetParent(nBlackTut);
        handObject.position = LogicGame.Instance.ListColorPlate[7].transform.position;
        handObject.SetActive(true);
        nBlackTut.SetActive(true);
        listSteps[8].SetActive(true);
        a = LogicGame.Instance.ListColorPlate[7].transform;
        b = LogicGame.Instance.ListColorPlate[10].transform;
        for (int i = 0; i < LogicGame.Instance.ListColorPlate[7].ListColor.Count; i++)
        {
            LogicColor c = LogicGame.Instance.ListColorPlate[7].ListColor[i];
            c.spriteRender.sortingOrder = 17;
        }

        for (int i = 0; i < LogicGame.Instance.ListColorPlate[10].ListColor.Count; i++)
        {
            LogicColor c = LogicGame.Instance.ListColorPlate[10].ListColor[i];
            c.spriteRender.sortingOrder = 17;
        }

        MoveHand();
    }

    public void EndTut()
    {
        LogicGame.Instance.homeInGame.itemObj.transform.SetParent(LogicGame.Instance.homeInGame.transform);
        HideHandTut();
        handObject.SetActive(false);
        nBlackTut.SetActive(false);
    }

    Transform a;
    Transform b;

    void MoveHand()
    {
        handObject.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "nothing", false);

        handObject.transform.DOMove(b.position, 0.75f)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
    }

}

