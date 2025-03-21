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
    public GameObject grow;
    public GameObject lockCoin;
    public GameObject cannotPlace;
    public List<GameObject> listForzen;
    public GameObject existed;
    public GameObject ads;
    public GameObject wood;
    public List<GameObject> listBags;
    public GameObject poison;

    public AnimationCurve animCurve;
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
    public float moveDistance = 0.2f;
    public float duration = 0.3f;
    public Vector3 smallScale = new Vector3(0.8f, 1.2f, 1);
    public Vector3 bigScale = new Vector3(1.2f, 0.8f, 1);
    public Vector3 startPosition = Vector3.zero;
    private Tween arrowTween;
    private Tween growTween;
    public void PlayAnimationArrowPending()
    {
        if (arrow == null || grow == null)
            return;

        if (arrowTween != null) arrowTween.Kill();
        if (growTween != null) growTween.Kill();

        arrowTween = DOTween.Sequence()
           .Append(arrow.transform.DOLocalMoveY(startPosition.z - moveDistance, duration).SetEase(Ease.InOutSine))
           .Join(arrow.transform.DOScale(bigScale, duration))
           .Append(arrow.transform.DOLocalMoveY(startPosition.z, duration).SetEase(Ease.InOutSine))
           .Join(arrow.transform.DOScale(smallScale, duration))
           .SetLoops(-1, LoopType.Yoyo);


        growTween = DOTween.Sequence()
         .Append(grow.transform.DOLocalMoveY(startPosition.z - moveDistance, duration).SetEase(Ease.InOutSine))
         .Join(grow.transform.DOScale(bigScale, duration))
         .Append(grow.transform.DOLocalMoveY(startPosition.z, duration).SetEase(Ease.InOutSine))
         .Join(grow.transform.DOScale(smallScale, duration))
         .SetLoops(-1, LoopType.Yoyo);
    }

    public void RefreshAnimation()
    {
        arrow.transform.localPosition = startPosition;
        arrow.transform.localScale = Vector3.one;

        grow.transform.localPosition = startPosition;
        grow.transform.localScale = Vector3.one;

        if (arrowTween != null) arrowTween.Kill();

        if (growTween != null) growTween.Kill();


    }

    //public void PlayArrowClicked()
    //{
    //    StartCoroutine(ClickArrow());
    //}

    //IEnumerator ClickArrow()
    //{
    //    arrow.SetActive(false);
    //    arrowClick.SetActive(true);
    //    arrowCannotClick.SetActive(false);

    //    arrowClick.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.15f)
    //        .OnComplete(() => { arrowClick.transform.localScale = Vector3.one; });
    //    yield return new WaitForSeconds(0.15f);

    //    arrow.SetActive(true);
    //    arrowClick.SetActive(false);
    //    arrowCannotClick.SetActive(false);
    //}

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
            case Status.Wood:
                SetWood();
                break;
            case Status.Poison:
                SetPoison();
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
        foreach (var t in listForzen)
        {
            t.SetActive(false);
        }

        cannotPlace.SetActive(false);
        ads.SetActive(false);
        wood.SetActive(false);
        poison.SetActive(false);
        foreach (var t in listBags)
        {
            t.SetActive(false);
        }
    }

    public void Refresh()
    {
        DeletePlate();
        normal.SetActive(true);
    }

    public void SetExistedPlate()
    {
        DeletePlate();
        normal.SetActive(true);
        existed.SetActive(true);
        existed.transform.localPosition = new Vector3(0, 0.5f, 0);
    }

    public void SetPlateArrow()
    {
        normal.SetActive(false);

        arrow.SetActive(true);
    }

    public void SetLockCoin()
    {
        DeletePlate();
        lockCoin.SetActive(true);
    }


    public void SetCannotPlace()
    {
        DeletePlate();
        cannotPlace.SetActive(true);
    }

    public void SetFrozenVisual(int RowOffset)
    {
        DeletePlate();
        normal.SetActive(true);
        for (int i = 0; i < listForzen.Count; i++)
        {
            listForzen[i].SetActive(true);
            var layer = (GameConfig.OFFSET_LAYER - RowOffset) > 1 ? GameConfig.OFFSET_LAYER - RowOffset : 1;
            listForzen[i].GetComponent<SpriteRenderer>().sortingOrder = layer;
        }
    }

    private void SetFrozen(int countFrozen, int RowOffset)
    {
        DeletePlate();
        normal.SetActive(true);

        for (int i = 0; i < listForzen.Count; i++)
        {
            listForzen[i].SetActive(true);
            var layer = (GameConfig.OFFSET_LAYER - RowOffset) > 1 ? GameConfig.OFFSET_LAYER - RowOffset : 1;
            listForzen[i].GetComponent<SpriteRenderer>().sortingOrder = layer;
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
        DeletePlate();
        ads.SetActive(true);
    }

    public void SetWood()
    {
        DeletePlate();
        wood.SetActive(true);
    }

    public void SetPoison()
    {
        DeletePlate();
        poison.SetActive(true);
    }

    public void SetBag(int typeBag)
    {
        DeletePlate();
        for (int i = 0; i < listBags.Count; i++)
        {
            if (i == typeBag)
            {
                listBags[i].SetActive(true);
            }
        }
    }

    public void SetTutLockCoin()
    {
        lockCoin.GetComponent<SpriteRenderer>().sortingOrder = 17;
    }

    public void SetTutFrozen()
    {
        for (int i = 0; i < listForzen.Count - 1; i++)
        {
            listForzen[i].GetComponent<SpriteRenderer>().sortingOrder = 17;
        }
    }

    public void ResetTut(int RowOffset)
    {
        lockCoin.GetComponent<SpriteRenderer>().sortingOrder = 0;
        for (int i = 0; i < listForzen.Count - 1; i++)
        {
            int layer = (GameConfig.OFFSET_LAYER - RowOffset) > 1 ? GameConfig.OFFSET_LAYER - RowOffset : 1;

            listForzen[i].GetComponent<SpriteRenderer>().sortingOrder = layer;
        }
    }
}