﻿using System;
using System.Collections;
using System.Collections.Generic;
using ntDev;
using UnityEngine;
using DG.Tweening;
using TMPro;

public enum ColorEnum
{
    Green,
    Blue,
    Red,
    Yellow,
    Purple,
    Pink,
    Orange,
    Random
}
public enum Status
{
    Right,
    Left,
    Up,
    Down,
    Empty,
    LockCoin,
    Frozen,
    CannotPlace,
    None,
    Ads,
    Existed,
    SpeicalArrowRight,
    SpecialArrowLeft,
    SpecialArrowUp,
    SpecialArrowDown
}

[Serializable]
public class GroupEnum
{
    public ColorEnum type;
    public List<ColorEnum> listPlates = new List<ColorEnum>();
}

public delegate LogicColor GetColorNew();

public class ColorPlate : MonoBehaviour
{
    public int Row;
    public int Col;

    public LogicVisualPlate logicVisual;
    public List<ColorPlate> ListConnect;
    public List<LogicColor> ListColor;
    public Status status = Status.None;
    public List<ColorEnum> ListValue = new List<ColorEnum>();
    public ColorEnum TopValue => ListValue[ListValue.Count - 1];
    public LogicColor TopColor => ListColor[ListColor.Count - 1];
    GetColorNew GetColorNew;
    public int count => listTypes[listTypes.Count - 1].listPlates.Count;
    public List<GroupEnum> listTypes;
    [SerializeField] public Animator anim;
    [SerializeField] public Transform targetUIPosition;
    [SerializeField] CustomRatio customRatio;
    public GameObject circleZZZ;
    public TextMeshPro txtPointUnlock;
    public bool isLocked;
    public int pointToUnLock;
    public int countFrozen;
    public int countMaxDiff;
    public AnimationCurve curve;
    public AnimationCurve curveMoveUp;
    public TimerConfigData timerConfigData;

    private void Start()
    {
        if (LogicGame.Instance != null)
            targetUIPosition = LogicGame.Instance.targetUIPosition;

    }

    public void Init(GetColorNew getColorNew)
    {
        GetColorNew = getColorNew;
        ListColor.Refresh();
        ListColor.Clear();
        listTypes.Clear();
        ListValue.Clear();
    }
    public void Init()
    {
        ListColor.Refresh();
        ListValue.Clear();
    }

    public void Initialize(int row, int col)
    {
        Row = row;
        Col = col;
        gameObject.name = $"Cell ({row}, {col})";
    }

    public void InitColor()
    {
        LevelData levelData = customRatio.listLevelData[SaveGame.Level];

        listTypes.Clear();
        List<int> listDiff = new List<int>();
        int randomListTypeCount = -1;
        int rdRatioCountInStack = UnityEngine.Random.Range(0, 100);
        for (int i = 0; i < levelData.listRatioCountInStack.Count; i++)
        {
            if (levelData.listRatioCountInStack[i] > rdRatioCountInStack)
            {
                randomListTypeCount = i + 1;
                break;
            }

            randomListTypeCount = levelData.listRatioCountInStack.Count + 1;
        }

        randomListTypeCount = randomListTypeCount > LogicGame.Instance.countDiff ? LogicGame.Instance.countDiff : randomListTypeCount;


        int t = 0;
        while (listDiff.Count < randomListTypeCount && t < 1000)
        {
            int type = UnityEngine.Random.Range(0, LogicGame.Instance.countDiff);
            if (!listDiff.Contains(type))
                listDiff.Add(type);
            ++t;
        }

        foreach (int type in listDiff)
        {
            GroupEnum group = new GroupEnum { type = (ColorEnum)type };
            listTypes.Add(group);

            int remainingCount = 10 - ListValue.Count;
            int maxPossibleAdditions = listDiff.Count > 3 ? remainingCount / listDiff.Count : 3;
            int randomCount = UnityEngine.Random.Range(2, Mathf.Min(4, maxPossibleAdditions + 1));

            for (int j = 0; j < randomCount; j++)
            {
                if (ListValue.Count >= 10)
                {
                    break;
                }

                group.listPlates.Add(group.type);
                ListValue.Add(group.type);
            }
        }

        //foreach (int type in listDiff)
        //{
        //    GroupEnum group = new GroupEnum { type = (ColorEnum)type };
        //    listTypes.Add(group);

        //    group.listPlates.Add(group.type);
        //    ListValue.Add(group.type);
        //}

        InitValue(this.transform);
    }

    public void SpawnSpecialColor(GetColorNew getColorNew)
    {
        Init(getColorNew);
        GroupEnum group = new GroupEnum { type = ColorEnum.Random };
        listTypes.Add(group);
        group.listPlates.Add(group.type);
        ListValue.Add(group.type);
        InitValue(this.transform, true);
    }

    #region old gameplay spawn
    public void InitRandom(bool isFirst = true)
    {
        InitGroupEnum();
        InitValue(this.transform, isFirst);
    }

    public void InitColorExisted(List<CurrentEnum> listCurrentEnum)
    {
        InitExistedValue(listCurrentEnum);

        InitValue(this.transform);
    }

    public void InitExistedValue(List<CurrentEnum> listCurrentEnum)
    {
        listTypes.Clear();

        for (int i = 0; i < listCurrentEnum.Count; i++)
        {
            GroupEnum group = new GroupEnum { type = (ColorEnum)listCurrentEnum[i].indexEnum };
            listTypes.Add(group);

            for (int j = 0; j < listCurrentEnum[i].countEnum; j++)
            {
                group.listPlates.Add(group.type);
                ListValue.Add(group.type);
            }
        }
    }

    public void InitGroupEnum()
    {
        LevelData levelData = customRatio.listLevelData[0];

        listTypes.Clear();
        HashSet<int> listDiff = new HashSet<int>();

        int randomListTypeCount = -1;
        int rdRatioCountInStack = UnityEngine.Random.Range(0, 100);


        for (int i = 0; i < levelData.listRatioCountInStack.Count; i++)
        {
            if (levelData.listRatioCountInStack[i] > rdRatioCountInStack)
            {
                randomListTypeCount = i + 1;
                break;
            }

            randomListTypeCount = levelData.listRatioCountInStack.Count + 1;
        }
        //Debug.Log("rdRatioCountInStack: " + rdRatioCountInStack);
        //Debug.Log("số lượng màu khác nhau: " + randomListTypeCount);

        while (listDiff.Count < randomListTypeCount)
        {
            int randomCountPerStack = -1;
            int rdRatioColorInStack = UnityEngine.Random.Range(0, 100);
            //Debug.Log("rdRatioColorInStack: " + rdRatioColorInStack);

            for (int i = 0; i < levelData.listRatioSpawnColor.Count; i++)
            {
                if (levelData.listRatioSpawnColor[i] > rdRatioColorInStack)
                {
                    randomCountPerStack = i;
                    break;
                }

                randomCountPerStack = levelData.listRatioSpawnColor.Count;
            }
            //Debug.Log("màu được spawn: " + randomCountPerStack);

            listDiff.Add(randomCountPerStack);
        }

        //int randomListTypeCount = 3;

        //while (listDiff.Count < randomListTypeCount)
        //{
        //    listDiff.Add(UnityEngine.Random.Range(0, 7));
        //}

        foreach (int type in listDiff)
        {
            GroupEnum group = new GroupEnum { type = (ColorEnum)type };
            listTypes.Add(group);

            //int randomCount = UnityEngine.Random.Range(1, 4);

            //for (int j = 0; j < randomCount; j++)
            //{
            //    group.listPlates.Add(group.type);
            //    ListValue.Add(group.type);
            //}

            int remainingCount = 10 - ListValue.Count;
            int maxPossibleAdditions = listDiff.Count > 3 ? remainingCount / listDiff.Count : 3;
            int randomCount = UnityEngine.Random.Range(2, Mathf.Min(4, maxPossibleAdditions + 1));

            for (int j = 0; j < randomCount; j++)
            {
                if (ListValue.Count >= 10)
                {
                    break;
                }

                group.listPlates.Add(group.type);
                ListValue.Add(group.type);
            }
        }
    }
    #endregion
    public void ChangeSpecialColorPLate(ColorEnum colorEnum)
    {
        listTypes.Clear();
        ListValue.Clear();

        GroupEnum group = new GroupEnum { type = colorEnum };
        listTypes.Add(group);

        int randomCount = UnityEngine.Random.Range(6, 7);

        for (int j = 0; j < randomCount; j++)
        {
            group.listPlates.Add(group.type);
            ListValue.Add(group.type);
        }

        InitValue(this.transform, true);

    }

    public AnimationCurve customCurve;

    public void InitValue(Transform transform = null, bool isFirst = true, int index = -1)
    {
        for (int i = 0; i < ListValue.Count; ++i)
        {
            if (i >= ListColor.Count)
            {
                LogicColor color = GetColorNew();
                int layer = (8 - Row) > 1 ? 8 - Row : 1;
                Debug.Log("layer: " + layer);
                color.Init((int)ListValue[i], layer);
                color.transform.SetParent(transform);
                color.transform.localRotation = Quaternion.identity;

                color.transform.localScale = Vector3.one;
                float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);
                color.transform.localPosition = new Vector3(randomX, i * GameConfig.OFFSET_PLATE, -i * GameConfig.OFFSET_PLATE);
                ListColor.Add(color);
            }
            else
            {

                if (Math.Abs(ListColor[i].transform.localPosition.x) > 1 || Math.Abs(ListColor[i].transform.localPosition.y) > 1)
                {
                    List<LogicColor> ListColorVisual = new List<LogicColor>();
                    ListColorVisual.Add(ListColor[i]);
                    // Bieu dien o day

                    //Vector3 currentPos = ListColor[i].transform.localPosition;
                    float jumpPower = 0.5f + i * 0.1f;
                    if (index == 0)
                    {
                        Debug.Log("000000000000000");

                        LogicColor colorZ = ListColor[i];
                        float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);

                        colorZ.transform.DOLocalJump(new Vector3(randomX, i * GameConfig.OFFSET_PLATE, -i * GameConfig.OFFSET_PLATE), jumpPower, 1, timerConfigData.timeMove)
                            .OnStart(() =>
                            {
                                colorZ.spriteRender.sortingOrder = 11;
                            })
                            .OnComplete(() =>
                            {
                                int layer = (8 - Row) > 1 ? 8 - Row : 1;
                                colorZ.spriteRender.sortingOrder = layer;
                            })
                            ;
                    }
                    else if (index == 1)
                    {
                        Debug.Log("111111111111");

                        float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);
                        LogicColor colorZ = ListColor[i];

                        colorZ.transform.DOLocalMove(new Vector3(randomX, i * GameConfig.OFFSET_PLATE, -i * GameConfig.OFFSET_PLATE), timerConfigData.timeMove)
                            .SetEase(curve)
                            //.SetEase(curveMoveUp)
                            .OnStart(() =>
                            {
                                colorZ.spriteRender.sortingOrder = 11;
                            })
                            .OnComplete(() =>
                            {
                                int layer = (8 - Row) > 1 ? 8 - Row : 1;

                                colorZ.spriteRender.sortingOrder = layer;
                            })
                            ;
                        ;
                    }

                    for (int j = 0; j < ListColorVisual.Count; j++)
                    {
                        float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);

                        ListColorVisual[j].transform.localPosition = new Vector3(randomX, ListColorVisual[j].transform.localPosition.y, ListColorVisual[j].transform.localPosition.z);
                    }
                }
                else
                {
                    //Debug.Log("wtf");
                    int layer = (8 - Row) > 1 ? 8 - Row : 1;

                    ListColor[i].spriteRender.sortingOrder = layer;
                    ListColor[i].transform.localPosition = new Vector3(ListColor[i].transform.localPosition.x, i * GameConfig.OFFSET_PLATE, -i * GameConfig.OFFSET_PLATE);
                }



            }
        }
    }


    private void MoveDirection(int index, int i)
    {
        if (index == 0)
        {
            // same col, row end > row start
            ListColor[i].transform.localEulerAngles
                                    = new Vector3(ListColor[i].transform.localEulerAngles.x, ListColor[i].transform.localEulerAngles.y, ListColor[i].transform.localEulerAngles.z - 180f);

            ListColor[i].transform.DOLocalRotate
               (new Vector3(ListColor[i].transform.localEulerAngles.x, ListColor[i].transform.localEulerAngles.y, ListColor[i].transform.localEulerAngles.z + 180f), GameConfig.TIME_MOVE, RotateMode.Fast)
               .OnComplete(() =>
               {
                   ListColor[i].transform.localEulerAngles = Vector3.zero;
               });
        }
        else if (index == 1)
        {
            // same row, col end < col start
            ListColor[i].transform.localEulerAngles
                                   = new Vector3(ListColor[i].transform.localEulerAngles.x - 180f, ListColor[i].transform.localEulerAngles.y, ListColor[i].transform.localEulerAngles.z);

            ListColor[i].transform.DOLocalRotate
               (new Vector3(ListColor[i].transform.localEulerAngles.x + 180f, ListColor[i].transform.localEulerAngles.y, ListColor[i].transform.localEulerAngles.z), GameConfig.TIME_MOVE, RotateMode.Fast)
               .OnComplete(() =>
               {
                   ListColor[i].transform.localEulerAngles = Vector3.zero;
               });
        }
        else if (index == 2)
        {
            // same row, col end > col start
            ListColor[i].transform.localEulerAngles
                                   = new Vector3(ListColor[i].transform.localEulerAngles.x + 180f, ListColor[i].transform.localEulerAngles.y, ListColor[i].transform.localEulerAngles.z);

            ListColor[i].transform.DOLocalRotate
               (new Vector3(ListColor[i].transform.localEulerAngles.x - 180f, ListColor[i].transform.localEulerAngles.y, ListColor[i].transform.localEulerAngles.z), GameConfig.TIME_MOVE, RotateMode.Fast)
               .OnComplete(() =>
               {
                   ListColor[i].transform.localEulerAngles = Vector3.zero;
               });
        }
        else
        {
            // same col, row end < row start
            ListColor[i].transform.localEulerAngles
                                    = new Vector3(ListColor[i].transform.localEulerAngles.x, ListColor[i].transform.localEulerAngles.y, ListColor[i].transform.localEulerAngles.z + 180f);

            ListColor[i].transform.DOLocalRotate
               (new Vector3(ListColor[i].transform.localEulerAngles.x, ListColor[i].transform.localEulerAngles.y, ListColor[i].transform.localEulerAngles.z - 180f), GameConfig.TIME_MOVE, RotateMode.Fast)
               .OnComplete(() =>
               {
                   ListColor[i].transform.localEulerAngles = Vector3.zero;
               });
        }
    }


    public ColorEnum CheckClearEnum()
    {
        if (listTypes.Count <= 0)
        {
            throw new InvalidOperationException("listTypes is empty.");
        }

        int count = listTypes[listTypes.Count - 1].listPlates.Count;

        if (count < LogicGame.RULE_COMPLETE)
        {
            throw new InvalidOperationException("Not enough");
        }

        if (count >= LogicGame.RULE_COMPLETE)
        {
            Debug.Log("CountClear: " + count + " at: " + transform.name);
            return TopValue;
        }

        throw new InvalidOperationException("Unexpected");
    }

    public void InitClear(bool plusPoint = false)
    {
        ColorEnum colorEnum = listTypes[listTypes.Count - 1].type;
        int count = listTypes[listTypes.Count - 1].listPlates.Count;

        ListValue.RemoveRange(ListValue.Count - count, count);
        listTypes.RemoveAt(listTypes.Count - 1);

        IVisualPlate visual = new DefaultFinishPlate();
        visual.Execute(this, count, colorEnum, plusPoint);

    }

    public void ClearLastType()
    {
        if (listTypes[listTypes.Count - 1].listPlates.Count == 0)
        {
            Debug.Log(transform.name + " plate clear listLastType");
            listTypes.RemoveAt(listTypes.Count - 1);
        }

    }
    public void LinkColorPlate(ColorPlate colorPlate)
    {
        if (colorPlate != null && !ListConnect.Contains(colorPlate) && colorPlate.status != Status.Empty)
        {
            ListConnect.Add(colorPlate);
        }
    }

    public void LinkColorPlateArrow(ColorPlate colorPlate)
    {
        if (colorPlate != null && !ListConnect.Contains(colorPlate))
        {
            ListConnect.Add(colorPlate);
        }
    }

    public List<ColorPlate> CheckNearByCanConnect(/*ColorPlate colorPlate*/)
    {
        List<ColorPlate> listSame = new List<ColorPlate>();

        foreach (var c in ListConnect)
        {
            //Debug.Log(c.name + " ___ " + c.countFrozen);
            if (c.ListValue.Count == 0 || c.countFrozen != 0) continue;

            if (c.TopValue == TopValue)
            {
                if (!listSame.Contains(c))
                {
                    listSame.Add(c);
                }
            }
        }

        return listSame;
    }

    public int CountHasSameTopValueInConnect()
    {
        int count = 0;
        foreach (var c in ListConnect)
        {
            if (c.ListValue.Count == 0 || c.countFrozen != 0) continue;
            if (c.TopValue == TopValue)
            {
                count++;
            }
        }

        return count;
    }


    public bool isPlayingOnClick = false;
    public void PlayAnimOnClick()
    {
        //if (anim != null)
        //{
        //    anim.Play("OnClick");
        //    isPlayingOnClick = true;
        //}

        //StartCoroutine(PlayAnim());
        StartCoroutine(PlayAnimClick());
    }

    public bool IsPlayingOnClick()
    {
        return isPlayingOnClick;
    }

    public void PlayAnimNormal()
    {
        if (anim != null)
            anim.Play("Normal");
    }

    public void PlayAnimCanClick()
    {
        if (anim != null)
            anim.Play("CanClick");
    }

    IEnumerator PlayAnim()
    {
        if (anim != null)
        {
            anim.Play("OnClick");
            isPlayingOnClick = true;
            yield return new WaitForSeconds(0.3f);
            anim.Play("Normal");
            isPlayingOnClick = false;
        }
    }

    public void PlayAnimArrow()
    {
        if (CheckArrow(ListConnect[0]) && !isPlayingOnClick)
        {
            logicVisual.PlayArrowCannotClick();
        }

        if (!CheckArrow(ListConnect[0]) && !isPlayingOnClick)
        {
            logicVisual.PlayArrowNormal();
        }
    }

    bool CheckArrow(ColorPlate c)
    {
        if (c.ListValue.Count > 0 || c.status == Status.Frozen || c.status == Status.LockCoin || c.status == Status.CannotPlace || c.status == Status.Ads || c.status == Status.Empty) return true;
        else return false;
    }

    IEnumerator PlayAnimClick()
    {
        isPlayingOnClick = true;
        logicVisual.arrow.SetActive(false);
        logicVisual.arrowClick.SetActive(true);
        logicVisual.arrowCannotClick.SetActive(false);

        logicVisual.arrowClick.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.15f)
            .OnComplete(() =>
            {
                logicVisual.arrowClick.transform.localScale = Vector3.one;
            });
        yield return new WaitForSeconds(0.15f);

        isPlayingOnClick = false;
    }

    public void PlayAnimScale()
    {
        if (anim != null) anim.Play("Scale", -1, 0);
    }

    public void ClearAll()
    {
        ListValue.Clear();
        listTypes.Clear();

        Sequence sq = DOTween.Sequence();
        float delay = 0f;

        List<LogicColor> listTest = new List<LogicColor>();

        for (int i = ListColor.Count - 1; i >= 0; --i)
        {
            LogicColor color = ListColor[i];
            if (i != 0) listTest.Add(color);
            ListColor.Remove(color);

            if (i == 0)
            {
                sq.Insert(delay, color.transform.DOScale(0.5f, 0.3f).OnComplete(() =>
                {
                    if (color.trail != null) color.trail.enabled = true;

                    // Camera overlay
                    Vector3 viewportPos = new Vector3(targetUIPosition.position.x / Screen.width, targetUIPosition.position.y / Screen.height, Camera.main.nearClipPlane);
                    Vector3 targetPos = Camera.main.ViewportToWorldPoint(viewportPos);

                    //Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, targetUIPosition.position);
                    //Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, Camera.main.nearClipPlane));

                    color.transform.DOMove(targetPos, 0.3f).OnComplete(() =>
                    {
                        if (color.trail != null) color.trail.enabled = false;
                        color.gameObject.SetActive(false);
                    });
                }));
            }
            else
            {
                sq.Insert(delay, color.transform.DOScale(0, 0.3f));
                delay += 0.05f;
                sq.OnComplete(() =>
                {
                    for (int i = 0; i < listTest.Count; ++i)
                    {
                        listTest[i].gameObject.SetActive(false);
                    }
                });
            }
        }
    }
    public void DecreaseCountFrozenNearBy()
    {
        for (int i = 0; i < ListConnect.Count; ++i)
        {
            if (ListConnect[i].ListValue.Count == 0) continue;

            if (ListConnect[i].countFrozen == 0) continue;

            LogicGame.Instance.frostExplosionPool.Spawn(ListConnect[i].transform.position, true);

            ListConnect[i].countFrozen--;

            for (int j = 0; j < ListConnect[i].logicVisual.listForzen.Count; j++)
            {
                if (!ListConnect[i].logicVisual.listForzen[j].activeSelf) continue;

                ListConnect[i].logicVisual.listForzen[j].gameObject.SetActive(false);
                break;
            }

            if (ListConnect[i].countFrozen == 0)
            {
                ListConnect[i].status = Status.None;
            }
        }
    }

    public void UnlockedLockCoin(int currenPoint)
    {
        if (currenPoint >= pointToUnLock && isLocked)
        {

            StartCoroutine(UnlockPlate());
            //ParticleSystem unlock = LogicGame.Instance.unlockParticlePool.Spawn(this.transform.position, true);

            //txtPointUnlock.gameObject.SetActive(false);
            //logicVisual.SetVisualAfterUnlock(status);
            //pointToUnLock = 0;
            //isLocked = false;
            //if (status == Status.LockCoin)
            //    status = Status.None;
        }
    }

    IEnumerator UnlockPlate()
    {
        txtPointUnlock.gameObject.SetActive(false);
        pointToUnLock = 0;
        isLocked = false;
        var currentStatus = status;

        if (status == Status.LockCoin)
            status = Status.None;

        //if (logicVisual.animLockCoin != null)
        //    logicVisual.animLockCoin.Play("Unlock");

        yield return new WaitForSeconds(0.5f);

        ParticleSystem unlock = LogicGame.Instance.unlockParticlePool.Spawn(this.transform.position, true);

        txtPointUnlock.gameObject.SetActive(false);
        logicVisual.SetVisualAfterUnlock(currentStatus);
    }

}

