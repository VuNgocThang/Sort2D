using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicVisualPlate : MonoBehaviour
{
    //visual gameplay
    //public GameObject hightlight;
    //public GameObject target;
    public GameObject circle;

    //visual plate spawn
    public GameObject normal;
    public GameObject arrow;
    public GameObject arrowClick;
    public GameObject arrowCannotClick;
    public GameObject lockCoin;
    public GameObject cannotPlace;
    public List<GameObject> listForzen;
    public GameObject existed;
    public GameObject ads;


    //public Animator animLockCoin;

    //logic visual ingame
    public void PlayNormal(bool isArrow)
    {
        if (!isArrow)
        {
            normal.SetActive(true);
            arrow.SetActive(false);
            //target.SetActive(false);
            //hightlight.SetActive(false);
        }
        else
        {
            normal.SetActive(false);
            arrow.SetActive(true);
            //target.SetActive(false);
            //hightlight.SetActive(false);
        }
    }
    public void PlayHighLight()
    {
        normal.SetActive(false);
        //target.SetActive(false);
        //hightlight.SetActive(true);
    }
    public void PlayTarget()
    {
        arrow.SetActive(false);
        normal.SetActive(false);
        //hightlight.SetActive(false);

        //target.SetActive(true);
    }

    public void PlayArrowCannotClick()
    {
        arrow.SetActive(false);
        arrowClick.SetActive(false);
        arrowCannotClick.SetActive(true);
    }

    public void PlayArrowNormal()
    {
        arrow.SetActive(true);
        arrowClick.SetActive(false);
        arrowCannotClick.SetActive(false);
    }

    public void PlayArrowClicked()
    {
        StartCoroutine(ClickArrow());
    }

    IEnumerator ClickArrow()
    {
        arrow.SetActive(false);
        arrowClick.SetActive(true);
        arrowCannotClick.SetActive(false);

        arrowClick.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.15f)
            .OnComplete(() =>
            {
                arrowClick.transform.localScale = Vector3.one;
            });
        yield return new WaitForSeconds(0.15f);

        arrow.SetActive(true);
        arrowClick.SetActive(false);
        arrowCannotClick.SetActive(false);

    }

    public void SetDirectionArrow(Status stt, bool isLocked)
    {
        normal.SetActive(false);

        if (isLocked) arrow.SetActive(false);
        else arrow.SetActive(true);

        switch (stt)
        {
            case Status.Left:
                arrow.transform.localEulerAngles = new Vector3(0, 90f, 0);
                arrowClick.transform.localEulerAngles = new Vector3(0, 90f, 0);
                arrowCannotClick.transform.localEulerAngles = new Vector3(0, 90f, 0);
                break;

            case Status.Right:
                arrow.transform.localEulerAngles = new Vector3(0, -90f, 0);
                arrowClick.transform.localEulerAngles = new Vector3(0, -90f, 0);
                arrowCannotClick.transform.localEulerAngles = new Vector3(0, -90f, 0);
                break;

            case Status.Up:
                arrow.transform.localEulerAngles = new Vector3(0, 180f, 0);
                arrowClick.transform.localEulerAngles = new Vector3(0, 180f, 0);
                arrowCannotClick.transform.localEulerAngles = new Vector3(0, 180f, 0);
                break;

            case Status.Down:
                arrow.transform.localEulerAngles = new Vector3(0, 0, 0);
                arrowClick.transform.localEulerAngles = new Vector3(0, 0, 0);
                arrowCannotClick.transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            default:
                return;
        }
    }

    public void SetSpecialSquare(Status stt, int RowOffset)
    {
        switch (stt)
        {
            case Status.Frozen:
                SetFrozenVisual(RowOffset);
                break;
            case Status.CannotPlace:
                SetCannotPlace();
                break;
            case Status.LockCoin:
                SetLockCoin();
                break;
            case Status.Ads:
                SetAds();
                break;
            default:
                return;
        }
    }

    public void SetSpecialSquareExisted(Status stt, int countFrozen, int RowOffset)
    {
        switch (stt)
        {
            case Status.Frozen:
                SetFrozen(countFrozen, RowOffset);
                break;
            case Status.CannotPlace:
                SetCannotPlace();
                break;
            case Status.LockCoin:
                SetLockCoin();
                break;
            case Status.Ads:
                SetAds();
                break;
            default:
                return;
        }
    }

    public void SetVisualAfterUnlock(Status stt)
    {
        switch (stt)
        {
            case Status.Left:
                lockCoin.SetActive(false);
                arrow.SetActive(true);
                break;

            case Status.Right:
                lockCoin.SetActive(false);
                arrow.SetActive(true);
                break;

            case Status.Up:
                lockCoin.SetActive(false);
                arrow.SetActive(true);
                break;

            case Status.Down:
                lockCoin.SetActive(false);
                arrow.SetActive(true);
                break;

            case Status.LockCoin:
                lockCoin.SetActive(false);
                arrow.SetActive(false);
                normal.SetActive(true);
                break;

            default:
                return;
        }

        //lockCoin.SetActive(false);
        //normal.SetActive(true);
    }

    //logic setmap

    public void DeletePlate()
    {
        normal.SetActive(false);
        arrow.SetActive(false);
        lockCoin.SetActive(false);
        existed.SetActive(false);
        for (int i = 0; i < listForzen.Count; i++)
        {
            listForzen[i].SetActive(false);
        }

        cannotPlace.SetActive(false);
        ads.SetActive(false);
    }

    public void Refresh()
    {
        normal.SetActive(true);
        arrow.SetActive(false);
        lockCoin.SetActive(false);
        existed.SetActive(false);
        for (int i = 0; i < listForzen.Count; i++)
        {
            listForzen[i].SetActive(false);
        }

        cannotPlace.SetActive(false);
        ads.SetActive(false);
    }
    public void SetExistedPlate()
    {
        normal.SetActive(true);
        existed.SetActive(true);
        cannotPlace.SetActive(false);
        lockCoin.SetActive(false);
        ads.SetActive(false);

        existed.transform.localPosition = new Vector3(0, 0.5f, 0);
    }
    public void SetPlateArrow()
    {
        normal.SetActive(false);

        arrow.SetActive(true);
    }
    public void SetLockCoin()
    {
        normal.SetActive(false);
        arrow.SetActive(false);
        cannotPlace.SetActive(false);
        existed.SetActive(false);
        ads.SetActive(false);

        for (int i = 0; i < listForzen.Count; i++)
        {
            listForzen[i].SetActive(false);
        }

        lockCoin.SetActive(true);
    }


    public void SetCannotPlace()
    {
        normal.SetActive(false);
        arrow.SetActive(false);
        lockCoin.SetActive(false);
        existed.SetActive(false);
        ads.SetActive(false);

        for (int i = 0; i < listForzen.Count; i++)
        {
            listForzen[i].SetActive(false);
        }

        cannotPlace.SetActive(true);
    }

    public void SetFrozenVisual(int RowOffset)
    {
        normal.SetActive(true);
        cannotPlace.SetActive(false);
        lockCoin.SetActive(false);
        ads.SetActive(false);

        for (int i = 0; i < listForzen.Count; i++)
        {
            listForzen[i].SetActive(true);
            Debug.Log((7 - RowOffset) * 10 - 5);
            listForzen[i].transform.localPosition = new Vector3(0, 0.2f, -(7 - RowOffset) * 10 - 5);
        }
    }

    public void SetFrozen(int countFrozen, int RowOffset)
    {
        normal.SetActive(true);
        cannotPlace.SetActive(false);
        lockCoin.SetActive(false);
        ads.SetActive(false);

        for (int i = 0; i < listForzen.Count; i++)
        {
            listForzen[i].SetActive(true);
            listForzen[i].transform.localPosition = new Vector3(0, 0.2f, -(7 - RowOffset) * 10 - 5);

        }

        if (countFrozen == 2)
        {
            listForzen[0].SetActive(false);
        }
        else if (countFrozen == 1)
        {
            listForzen[0].SetActive(false);
            listForzen[1].SetActive(false);
        }
    }

    public void SetAds()
    {
        normal.SetActive(false);
        arrow.SetActive(false);
        cannotPlace.SetActive(false);
        lockCoin.SetActive(false);

        for (int i = 0; i < listForzen.Count; i++)
        {
            listForzen[i].SetActive(false);
        }

        ads.SetActive(true);
    }
}
