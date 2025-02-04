using System;
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
    public GameObject circleZZZ;
    public TextMeshPro txtPointUnlock;
    public bool isLocked;
    public int pointToUnLock;
    public int countFrozen;
    public int countMaxDiff;
    public AnimationCurve curve;
    public AnimationCurve curveMoveUp;
    public TimerConfigData timerConfigData;
    public ParticleSystem magicRune;
    public bool isMoving;
    public bool isMerging;
    public PathType pathType;

    private void Start()
    {
        if (LogicGame.Instance != null)
            targetUIPosition = LogicGame.Instance.targetUIPosition;
    }

    private void Update()
    {
        if (ListColor.Count <= 0) return;

        if (isMerging || countFrozen > 0)
        {
            for (int i = 0; i < ListColor.Count; i++)
            {
                ListColor[i].nBoxText.gameObject.SetActive(false);
                ListColor[i].txtCount.gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < ListColor.Count; i++)
            {
                ListColor[i].nBoxText.gameObject.SetActive(false);
                ListColor[i].txtCount.gameObject.SetActive(false);
            }

            TopColor.nBoxText.gameObject.SetActive(true);
            TopColor.txtCount.gameObject.SetActive(true);
            TopColor.txtCount.color = new Color(0.2156863f, 0.1098039f, 0.4313726f, 1f);
            TopColor.txtCount.text = this.listTypes[this.listTypes.Count - 1].listPlates.Count.ToString();
        }
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


    public void InitColor(bool isSpecial = false, bool isTutorial = false)
    {
        DataLevel levelData = LogicGame.Instance.dataLevel;

        listTypes.Clear();

        int randomCountInStacks = 0;
        List<int> listDiff = new List<int>();

        if (!isTutorial)
        {
            listDiff.Add(0);

            GroupEnum group = new GroupEnum { type = (ColorEnum)0 };
            listTypes.Add(group);

            for (int i = 0; i < 5; i++)
            {
                group.listPlates.Add(group.type);
                ListValue.Add(group.type);
            }
        }
        else
        {
            // tính số lượng màu khác nhau trong 1 stacks
            randomCountInStacks = CalculateCountInStacks(levelData);

            //Debug.Log("randomCountInStacks: " + randomCountInStacks);

            // chọn màu trong stacks 
            listDiff = CalculateListDiff(levelData, randomCountInStacks);

            //listDiff.Reverse();

            //Debug.Log(listDiff.Count);
            foreach (int type in listDiff)
            {
                //Debug.Log("type : " + type);
            }

            int maxReach = 0;
            if (isSpecial) maxReach = 6;
            else maxReach = 10;
            // Spawn Count Same Type
            foreach (int type in listDiff)
            {
                //Debug.Log("type: " + type + " __ " + (ColorEnum)type);
                GroupEnum group = new GroupEnum { type = (ColorEnum)type };
                listTypes.Add(group);

                int remainingCount = maxReach - ListValue.Count;
                int maxPossibleAdditions = listDiff.Count > 3 ? remainingCount / listDiff.Count : 3;
                int randomCount = UnityEngine.Random.Range(2, Mathf.Min(4, maxPossibleAdditions + 1));
                //int randomCount = UnityEngine.Random.Range(5, Mathf.Min(5, maxPossibleAdditions + 1));

                for (int j = 0; j < randomCount; j++)
                {
                    if (ListValue.Count >= maxReach)
                    {
                        break;
                    }

                    group.listPlates.Add(group.type);
                    ListValue.Add(group.type);
                }
            }
        }



        InitValue(this.transform);
    }

    int CalculateCountInStacks(DataLevel levelData)
    {
        int randomCountInStacks = -1;
        int rdRatioCountInStacks = UnityEngine.Random.Range(0, 100);
        //Debug.Log("rdRatioCountInStacks: "+ rdRatioCountInStacks);
        for (int i = 0; i < levelData.RatioInStacks.Length; i++)
        {
            if (levelData.RatioInStacks[i] > rdRatioCountInStacks)
            {
                randomCountInStacks = i + 1;
                break;
            }

            randomCountInStacks = levelData.RatioInStacks.Length + 1;
        }

        randomCountInStacks = randomCountInStacks > LogicGame.Instance.countDiff ? LogicGame.Instance.countDiff : randomCountInStacks;

        return randomCountInStacks;
    }

    //int CalculateRandomColor(DataLevel levelData)
    //{
    //    int randomColor = -1;

    //    int rdRatioType = UnityEngine.Random.Range(0, levelData.Ratio[LogicGame.Instance.countDiff - 1]);

    //    for (int i = 0; i < levelData.Ratio.Length; i++)
    //    {
    //        if (levelData.Ratio[i] > rdRatioType)
    //        {
    //            randomColor = i;
    //            break;
    //        }

    //        randomColor = levelData.Ratio.Length;
    //    }

    //    randomColor = randomColor > LogicGame.Instance.countDiff - 1 ? LogicGame.Instance.countDiff - 1 : randomColor;

    //    return randomColor;
    //}

    List<int> CalculateListDiff(DataLevel levelData, int randomCountInStacks)
    {
        List<int> listResult = new List<int>();
        List<int> listValue = new List<int>();
        List<float> listRatio = new List<float>();

        List<float> listRatioChange = GameManager.ChangeToList(levelData.Ratio);

        listValue.AddRange(LogicGame.Instance.CalculateCountColorInDesk());

        //Debug.Log("listvalue: " + listValue.Count);

        for (int i = 0; i < LogicGame.Instance.countDiff; i++)
        {
            listRatio.Add(listRatioChange[i]);
        }

        //Debug.Log(listRatio[0] + " ___ " + listRatio[1]);

        while (listResult.Count < randomCountInStacks)
        {
            int a = GameManager.GetRandomWithRatio(listRatio);
            //Debug.Log(a + " ___ " + listValue[a]);

            listResult.Add(listValue[a]);
            listValue.RemoveAt(a);
            listRatio.RemoveAt(a);
        }

        return listResult;
    }

    public void SpawnSpecialColor(GetColorNew getColorNew)
    {
        Init(getColorNew);
        GroupEnum group = new GroupEnum { type = ColorEnum.Random };
        listTypes.Add(group);
        group.listPlates.Add(group.type);
        ListValue.Add(group.type);
        InitValue(this.transform);
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

        InitValue(this.transform);

    }

    public AnimationCurve customCurve;

    public void InitValue(Transform transform = null, int index = -1, int _Row = -1)
    {
        if (isMoving) return;

        for (int i = 0; i < ListValue.Count; ++i)
        {
            if (i >= ListColor.Count)
            {
                CreateNewColor(transform, _Row, i);
            }
            else
            {
                UpdateExistingColor(index, _Row, i);
            }
        }
    }

    private void CreateNewColor(Transform transform, int _Row, int i)
    {
        LogicColor color = GetColorNew();
        int layer = CalculateLayer(_Row);

        color.Init((int)ListValue[i], layer);
        color.transform.SetParent(transform);
        color.transform.localRotation = Quaternion.identity;

        color.transform.localScale = Vector3.one;
        float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);
        color.transform.localPosition = new Vector3(randomX, i * GameConfig.OFFSET_PLATE, -i * GameConfig.OFFSET_PLATE);
        ListColor.Add(color);
    }

    private void UpdateExistingColor(int index, int ROW, int i)
    {
        LogicColor colorZ = ListColor[i];
        float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);

        if (Math.Abs(colorZ.transform.localPosition.x) > 1)
        {
            if (index != 0) return;

            Vector3 midPoint = new Vector3(colorZ.transform.localPosition.x / 2, -GameConfig.MID_POINT, -i * GameConfig.OFFSET_PLATE);
            CreatePathAnimation(colorZ, randomX, ROW, i, midPoint);
        }
        else if (Math.Abs(colorZ.transform.localPosition.y + colorZ.transform.localPosition.z) > 1)
        {
            if (index != 1 && index != 2) return;

            Vector3 midPoint = new Vector3(-GameConfig.MID_POINT, colorZ.transform.localPosition.y / 2, -i * GameConfig.OFFSET_PLATE);
            CreatePathAnimation(colorZ, randomX, ROW, i, midPoint);
        }
        else
        {
            int layer = CalculateLayer(ROW);
            colorZ.spriteRender.sortingOrder = layer;
        }
    }

    private int CalculateLayer(int ROW)
    {
        int layer = 0;
        if (ROW != -1)
            layer = (GameConfig.OFFSET_LAYER - ROW) > 1 ? GameConfig.OFFSET_LAYER - ROW : 1;
        else
            layer = (GameConfig.OFFSET_LAYER - this.Row) > 1 ? GameConfig.OFFSET_LAYER - this.Row : 1;
        return layer;
    }

    private void CreatePathAnimation(LogicColor color, float randomX, int ROW, int index, Vector3 midPoint)
    {
        Vector3 from = color.transform.localPosition;
        Vector3 to = new Vector3(randomX, index * GameConfig.OFFSET_PLATE, -index * GameConfig.OFFSET_PLATE);

        //color.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        //color.transform.DOScale(Vector3.one, GameConfig.TIME_MOVE)
        //    .OnComplete(() =>
        //    {
        //        color.transform.localPosition = Vector3.one;
        //    });

        color.transform.DOLocalPath(new Vector3[] { from, midPoint, to }, GameConfig.TIME_MOVE, PathType.CatmullRom)
            .OnStart(() =>
            {
                color.spriteRender.sortingOrder = 15;
            })
            .OnComplete(() =>
            {
                int layer = CalculateLayer(ROW);
                color.spriteRender.sortingOrder = layer;
            });
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
            //Debug.Log(transform.name + " plate clear listLastType");
            listTypes.RemoveAt(listTypes.Count - 1);
        }
    }

    public bool NearLastType()
    {
        bool IsNearLast = false;

        if (listTypes[listTypes.Count - 1].listPlates.Count == 1)
        {
            return true;
        }

        return IsNearLast;
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
                    //if (color.trail != null) color.trail.enabled = true;
                    if (color.trail != null) color.trail.SetActive(true);

                    // Camera overlay
                    Vector3 viewportPos = new Vector3(targetUIPosition.position.x / Screen.width, targetUIPosition.position.y / Screen.height, Camera.main.nearClipPlane);
                    Vector3 targetPos = Camera.main.ViewportToWorldPoint(viewportPos);

                    //Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, targetUIPosition.position);
                    //Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, Camera.main.nearClipPlane));

                    color.transform.DOMove(targetPos, GameConfig.TIME_FLY).OnComplete(() =>
                    {
                        //if (color.trail != null) color.trail.enabled = false;
                        if (color.trail != null) color.trail.SetActive(false);

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

            ParticleSystem frostPart = LogicGame.Instance.frostExplosionPool.Spawn();
            frostPart.transform.SetParent(ListConnect[i].transform);
            frostPart.transform.localPosition = Vector3.zero;
            frostPart.transform.localScale = Vector3.one;
            frostPart.Play();

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

    public float GetDistanceToCenter(int totalRows, int totalCols)
    {
        float centerCol = totalCols / 2;
        float centerRow = totalRows;

        return Mathf.Sqrt(Mathf.Pow(Row - centerRow, 2) + Mathf.Pow(Col - centerCol, 2));
    }


}

