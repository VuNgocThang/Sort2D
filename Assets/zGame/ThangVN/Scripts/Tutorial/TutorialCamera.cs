using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public static TutorialCamera Instance;
    [SerializeField] Camera cam;
    [SerializeField] RectTransform hand;
    [SerializeField] GameObject particleHand;
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] List<GameObject> listSteps;
    [SerializeField] PopupHome popupHome;
    [SerializeField] Transform nBlack;
    [SerializeField] EasyButton btnContinue, btnContinueBlack, btnContinueLockCoin, btnContinueFrozen;
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

        Vector3 position = cam.WorldToScreenPoint(pos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            position,
            cam,
            out Vector2 localPoint
        );
        hand.anchoredPosition = localPoint;
        hand.gameObject.SetActive(true);
        listSteps[indexStep].SetActive(true);
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

    }
}
