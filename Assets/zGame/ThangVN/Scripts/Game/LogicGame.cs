using BaseGame;
using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Common;
using Color = UnityEngine.Color;

public enum GameMode
{
    Edit,
    Play,
    EditGame
}

public class LogicGame : MonoBehaviour
{
    public static LogicGame Instance;
    public GameMode gameMode;

    public const int RULE_COMPLETE = 10;
    [SerializeField] Camera cam;
    [SerializeField] Transform holder;
    [SerializeField] Transform nParentArrow;
    [SerializeField] public Transform targetUIPosition;

    [SerializeField] public ColorPlateData colorPlateData;
    [SerializeField] ColorPlate colorPLatePrefab;
    [SerializeField] ColorPlate arrowPlatePrefab;

    [SerializeField] public List<ColorPlate> listNextPlate;
    [SerializeField] public List<ColorPlate> listSpawnNew;
    [SerializeField] public List<ColorPlate> ListColorPlate;
    [SerializeField] public List<ColorPlate> ListArrowPlate;
    [SerializeField] public List<ColorPlate> ListCheckPlate;
    /*[HideInInspector]*/
    public int rows;
    /*[HideInInspector]*/
    public int cols;
    [SerializeField] public PopupHome homeInGame;

    [SerializeField] LayerMask layerArrow;
    [SerializeField] LayerMask layerPlateSpawn;
    [SerializeField] LayerMask layerUsingItem;
    [SerializeField] LayerMask layerPlate;
    [SerializeField] List<LogicColor> listColors;
    [SerializeField] private ColorPlate colorRoot;

    [SerializeField] private ParticleSystem clickParticle;
    [SerializeField] private ParticleSystem eatParticle;
    [SerializeField] private ParticleSystem unlockParticle;
    [SerializeField] private ParticleSystem specialParticle;
    [SerializeField] private ParticleSystem chargingParticle;
    [SerializeField] private ParticleSystem changeColorParticle;
    [SerializeField] private ParticleSystem frostExplosion;

    public CustomPool<ParticleSystem> clickParticlePool;
    public CustomPool<ParticleSystem> eatParticlePool;
    public CustomPool<ParticleSystem> unlockParticlePool;
    public CustomPool<ParticleSystem> specialParticlePool;
    public CustomPool<ParticleSystem> chargingParticlePool;
    public CustomPool<ParticleSystem> changeColorParticlePool;
    public CustomPool<ParticleSystem> frostExplosionPool;

    public bool isMergeing;
    Tweener tweenerMove;

    public bool isLose = false;
    public bool isWin = false;
    public bool isPauseGame = false;
    public static bool isContiuneMerge = false;
    public int point;
    public int maxPoint;
    public int gold;
    public int pigment;
    public int countRevive;

    int pointSpawnSpecial = 100;
    [SerializeField] RectTransform slot_5;
    [SerializeField] RectTransform slot_6;

    public AnimationCurve curveMove;
    [SerializeField] SetMapManager setMapManager;
    //[HideInInspector] public int countMove;
    public int countDiff;
    public int countDiffMax;
    public bool isUsingHammer;
    public bool isUsingHand;
    public Hammer hammer;
    public HammerSpineEvent hammerSpine;
    [SerializeField] public Canvas canvasTutorial;
    public Tutorial tutorial;

    int countSpawnSpecial = 0;
    [SerializeField] bool isHadSpawnSpecial = false;
    [SerializeField] SpineSelectionChange spineSelection;
    [SerializeField] ControllerAnimState ControllerAnimState;
    [SerializeField] TimerConfigData timerConfigData;
    [SerializeField] SpawnBookTest spawnBook;

    LogicColor GetColorNew()
    {
        return listColors.GetClone();
    }
    private void Awake()
    {
        Instance = this;
        ManagerEvent.RegEvent(EventCMD.EVENT_SWITCH, SwitchNextPlate);
        //ManagerEvent.RegEvent(EventCMD.EVENT_SPAWN_PLATE, InitPlateSpawn);

    }

    void Start()
    {
        Application.targetFrameRate = 60;
        Refresh();
        //InitPlateSpawn(false);
        LoadSaveData();
        LoadData();
        InitListCheckPlate();
        InitNextPlate();
        RecursiveMerge();
    }
    //void Test(int count)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (i != 0 && i != count - 1)
    //        {
    //            Debug.Log("__" + i + "__");
    //        }
    //    }
    //}

    private void Refresh()
    {
        DOTween.KillAll();
        isWin = false;
        isLose = false;
        isMergeing = false;
        isPauseGame = false;
        listColors.Refresh();
        countDiff = 2;
        countRevive = 1;
        //countMove = 2;
        point = 0;
        //ManagerEvent.RaiseEvent(EventCMD.EVENT_COUNT, countMove);
        clickParticlePool = new CustomPool<ParticleSystem>(clickParticle, 5, transform, false);
        eatParticlePool = new CustomPool<ParticleSystem>(eatParticle, 5, transform, false);
        unlockParticlePool = new CustomPool<ParticleSystem>(unlockParticle, 5, transform, false);
        specialParticlePool = new CustomPool<ParticleSystem>(specialParticle, 2, transform, false);
        chargingParticlePool = new CustomPool<ParticleSystem>(chargingParticle, 2, transform, false);
        changeColorParticlePool = new CustomPool<ParticleSystem>(changeColorParticle, 2, transform, false);
        frostExplosionPool = new CustomPool<ParticleSystem>(frostExplosion, 2, transform, false);


        //ResetPosSpawn();

    }

    public void InitTutorial()
    {
        canvasTutorial.enabled = true;
        //tutorial.Init(slot_2, cam);
    }
    void ResetPosSpawn()
    {
        Vector3 screenPos5 = RectTransformUtility.WorldToScreenPoint(cam, slot_5.position);
        Vector3 screenPos6 = RectTransformUtility.WorldToScreenPoint(cam, slot_6.position);

        Vector3 worldPos5;
        Vector3 worldPos6;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(slot_5, screenPos5, cam, out worldPos5);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(slot_6, screenPos6, cam, out worldPos6);

        listNextPlate[0].transform.position = worldPos5;
        listNextPlate[1].transform.position = worldPos6;
    }
    void LoadData()
    {
        setMapManager.LoadData();
        colorPlateData = setMapManager.colorPlateData;

        rows = colorPlateData.rows;
        cols = colorPlateData.cols;
        ResetNDesk();
        setMapManager.Init(rows, cols, holder, ListColorPlate, colorPLatePrefab);

        if (SaveGame.Challenges)
        {
            if (saveGameChallenges == null) LoadLevelChallenges();
            else LoadSaveChallengesData();
        }
        else
        {
            maxPoint = colorPlateData.goalScore;
            gold = colorPlateData.gold;
            pigment = colorPlateData.pigment;

            if (saveGameNormal == null) LoadLevelNormal();
            else LoadSaveNormalData();
        }

        setMapManager.InitArrowPlates(rows, cols, ListColorPlate, nParentArrow, arrowPlatePrefab, ListArrowPlate);
        DataLevel dataLevel = DataLevel.GetData(SaveGame.Level + 1);
        countDiffMax = dataLevel.CountDiff;
    }

    void InitListCheckPlate()
    {
        for (int i = 0; i < ListArrowPlate.Count; i++)
        {
            if (!ListCheckPlate.Contains(ListArrowPlate[i].ListConnect[0]))
            {
                ListCheckPlate.Add(ListArrowPlate[i].ListConnect[0]);
            }
        }
    }
    [SerializeField] GameObject testStack;
    void ResetNDesk()
    {

        if (cols >= rows)
        {
            float y = 0.3f * (6 - cols);
            testStack.transform.position = new Vector3(0, 1.8f + y, 0);

            float scale = 6f / cols;
            holder.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            float y = 0.3f * (6 - rows);
            testStack.transform.position = new Vector3(0, 1.8f + y, 0);

            float scale = 6f / rows;
            holder.localScale = new Vector3(scale, scale, scale);
        }

    }
    void LoadLevelChallenges()
    {
        for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
        {
            int index = colorPlateData.listSpecialData[i].Row * cols + colorPlateData.listSpecialData[i].Col;

            ListColorPlate[index].status = (Status)colorPlateData.listSpecialData[i].type;
            ListColorPlate[index].logicVisual.SetSpecialSquare(ListColorPlate[index].status, colorPlateData.listSpecialData[i].Row);
        }

        for (int i = 0; i < colorPlateData.listArrowData.Count; i++)
        {
            int index = colorPlateData.listArrowData[i].Row * cols + colorPlateData.listArrowData[i].Col;

            ListArrowPlate.Add(ListColorPlate[index]);

            if (ListColorPlate[index].isLocked)
            {
                ListColorPlate[index].anim.enabled = false;
            }
            else
            {
                ListColorPlate[index].anim.enabled = true;
            }

            ListColorPlate[index].status = (Status)colorPlateData.listArrowData[i].type;
            ListColorPlate[index].gameObject.layer = 6;
            ListColorPlate[index].logicVisual.SetDirectionArrow(ListColorPlate[index].status, ListColorPlate[index].isLocked);
        }

        for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
        {
            int index = colorPlateData.listEmptyData[i].Row * cols + colorPlateData.listEmptyData[i].Col;
            ListColorPlate[index].logicVisual.DeletePlate();
        }
    }
    private void LoadLevelNormal()
    {
        for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
        {
            int index = colorPlateData.listSpecialData[i].Row * cols + colorPlateData.listSpecialData[i].Col;

            ListColorPlate[index].status = (Status)colorPlateData.listSpecialData[i].type;

            ListColorPlate[index].logicVisual.SetSpecialSquare(ListColorPlate[index].status, colorPlateData.listSpecialData[i].Row);
            if (ListColorPlate[index].status == Status.Frozen)
            {
                ListColorPlate[index].countFrozen = 3;
                ListColorPlate[index].Init(GetColorNew);
                ListColorPlate[index].InitColor();
            }

            if (ListColorPlate[index].status == Status.LockCoin)
            {
                ListColorPlate[index].isLocked = true;
                ListColorPlate[index].txtPointUnlock.gameObject.SetActive(true);
                ListColorPlate[index].pointToUnLock = colorPlateData.listSpecialData[i].pointUnlock;
                ListColorPlate[index].txtPointUnlock.text = ListColorPlate[index].pointToUnLock.ToString();
            }

            if (ListColorPlate[index].status == Status.Ads)
            {
                ListColorPlate[index].logicVisual.SetAds();
            }
        }

        for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
        {
            int index = colorPlateData.listExistedData[i].Row * cols + colorPlateData.listExistedData[i].Col;
            ListColorPlate[index].Init(GetColorNew);
            ListColorPlate[index].InitColor();

        }

        for (int i = 0; i < colorPlateData.listArrowData.Count; i++)
        {
            int index = colorPlateData.listArrowData[i].Row * cols + colorPlateData.listArrowData[i].Col;

            ListArrowPlate.Add(ListColorPlate[index]);

            if (ListColorPlate[index].isLocked)
            {
                ListColorPlate[index].anim.enabled = false;
            }
            else
            {
                ListColorPlate[index].anim.enabled = true;
            }

            ListColorPlate[index].status = (Status)colorPlateData.listArrowData[i].type;
            ListColorPlate[index].gameObject.layer = 6;
            ListColorPlate[index].logicVisual.SetDirectionArrow(ListColorPlate[index].status, ListColorPlate[index].isLocked);
        }

        for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
        {
            int index = colorPlateData.listEmptyData[i].Row * cols + colorPlateData.listEmptyData[i].Col;

            ListColorPlate[index].status = (Status)colorPlateData.listEmptyData[i].type;
            ListColorPlate[index].logicVisual.DeletePlate();
        }


    }

    #region InitNextPlate
    public void ShufflePlateSpawn()
    {
        //for (int i = 0; i < listSpawnNew.Count; i++)
        //{
        //    listSpawnNew[i].Init(GetColorNew);
        //}

        //Sequence sequence = DOTween.Sequence();

        //for (int i = 0; i < listSpawnNew.Count; i++)
        //{
        //    int index = i;

        //    sequence.AppendCallback(() =>
        //    {
        //        listSpawnNew[index].Init(GetColorNew);
        //        listSpawnNew[index].InitColor();

        //        foreach (LogicColor c in listSpawnNew[index].ListColor)
        //        {
        //            c.transform.localPosition = new Vector3(5f, c.transform.localPosition.y, c.transform.localPosition.z);
        //        }

        //        foreach (LogicColor c in listSpawnNew[index].ListColor)
        //        {
        //            c.transform.DOLocalMoveX(0, 0.3f);
        //        }
        //    });
        //    sequence.AppendInterval(0.2f);
        //}

        for (int i = 0; i < listNextPlate.Count; i++)
        {
            listNextPlate[i].Init(GetColorNew);
        }

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < listNextPlate.Count; i++)
        {
            int index = i;

            sequence.AppendCallback(() =>
            {
                listNextPlate[index].Init(GetColorNew);
                listNextPlate[index].InitColor();

                foreach (LogicColor c in listNextPlate[index].ListColor)
                {
                    c.transform.localPosition = new Vector3(5f, c.transform.localPosition.y, c.transform.localPosition.z);
                }

                foreach (LogicColor c in listNextPlate[index].ListColor)
                {
                    float randomX = UnityEngine.Random.Range(-0.5f, 0.5f);
                    c.transform.DOLocalMoveX(0, 0.3f);
                }
            });
            sequence.AppendInterval(0.2f);
        }
    }

    //public void InitPlateSpawn(object e)
    //{
    //    Sequence sequenceSpawn = DOTween.Sequence();

    //    for (int i = 0; i < listSpawnNew.Count; i++)
    //    {
    //        int index = i;

    //        if (listSpawnNew[index].ListValue.Count == 0)
    //        {
    //            sequenceSpawn.AppendCallback(() =>
    //            {
    //                //ManagerAudio.PlaySound(ManagerAudio.Data.soundSwitch);
    //                listSpawnNew[index].Init(GetColorNew);
    //                listSpawnNew[index].InitColor();

    //                foreach (LogicColor c in listSpawnNew[index].ListColor)
    //                {
    //                    c.transform.localPosition = new Vector3(5f, c.transform.localPosition.y, c.transform.localPosition.z);
    //                }

    //                foreach (LogicColor c in listSpawnNew[index].ListColor)
    //                {
    //                    c.transform.DOLocalMoveX(0, 0.3f);
    //                }
    //            });

    //            sequenceSpawn.AppendInterval(0.2f);
    //        }
    //    }
    //}

    public void InitNextPlate()
    {
        for (int i = 0; i < listNextPlate.Count; i++)
        {
            if (listNextPlate[i].ListValue.Count == 0)
            {
                listNextPlate[i].Init(GetColorNew);
                if (i == 0) listNextPlate[i].InitColor();
                else listNextPlate[i].InitColor();
            }
        }
    }
    #endregion

    void Swap(List<ColorPlate> a)
    {
        ColorPlate temp = a[0];
        a[0] = a[1];
        a[1] = temp;
    }

    void SwitchNextPlate(object e)
    {
        //listNextPlate[0].transform.DOLocalMove(listNextPlate[1].transform.localPosition, 0.2f).SetEase(Ease.OutCirc);
        //listNextPlate[1].transform.DOLocalMove(listNextPlate[0].transform.localPosition, 0.2f).SetEase(Ease.InCirc);
        //foreach (LogicColor c in listNextPlate[0].ListColor)
        //{
        //    c.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.2f);
        //}

        //foreach (LogicColor c in listNextPlate[1].ListColor)
        //{
        //    c.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        //}

        //Swap(listNextPlate);
    }
    float timeClick = -1;

    RaycastHit raycastHit;
    [SerializeField] float timerRun = -1;
    void Update()
    {
        if (timeClick >= 0)
        {
            timeClick -= Ez.TimeMod;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                timeClick = .8f;
                Vector3 spawnPosition = GetMouseWorldPosition();
                clickParticlePool.Spawn(spawnPosition, true);

                if (gameMode == GameMode.Play)
                {
                    if (isLose || isWin) return;

                    if (Physics.Raycast(ray, out var hit, 100f, layerArrow) && !isPauseGame)
                    {
                        //if (countMove == 0)
                        //{
                        ColorPlate arrowPlate = hit.collider.GetComponent<ColorPlate>();

                        if (arrowPlate.isLocked || arrowPlate.ListValue.Count > 0) return;

                        ICheckStatus checkStatusHolder = new CheckGetHolderStatus();
                        ColorPlate holder = checkStatusHolder.CheckHolder(arrowPlate);

                        //ICheckListMove checkListMove = new CheckGetListMove();
                        //List<ColorPlate> listMove = checkListMove.GetListSlotVisual(arrowPlate);


                        if (holder != null)
                        {
                            spineSelection.ActionToIdle();
                            ControllerAnimState.ActionToIdle();
                            arrowPlate.PlayAnimOnClick();
                            ManagerAudio.PlaySound(ManagerAudio.Data.soundArrowButton);
                            //Debug.Log(arrowPlate.name + " __ " + holder.name);
                            SetColor(arrowPlate, holder);

                            if (!SaveGame.IsDoneTutorial) canvasTutorial.enabled = false;
                        }

                        if (isHadSpawnSpecial)
                        {
                            //countMove = 0;
                            //ManagerEvent.RaiseEvent(EventCMD.EVENT_COUNT, countMove);
                            Debug.Log("Play Effect Has Special");
                            listNextPlate[0].SpawnSpecialColor(GetColorNew);
                            Vector3 spawnPos = listNextPlate[0].transform.position;
                            specialParticlePool.Spawn(spawnPos, true);
                            isHadSpawnSpecial = false;
                        }
                        else
                        {
                            //countMove = UnityEngine.Random.Range(2, 4);
                            //ManagerEvent.RaiseEvent(EventCMD.EVENT_COUNT, countMove);
                        }
                        //}
                        //else
                        //{
                        //    ManagerAudio.PlaySound(ManagerAudio.Data.soundCannotClick);
                        //    EasyUI.Toast.Toast.Show("Not enough quantity", 1f);
                        //}
                    }

                    // click from spawn to start
                    if (Physics.Raycast(ray, out var hitPlate, 100f, layerPlateSpawn) && !isPauseGame)
                    {
                        ColorPlate plateSpawn = hitPlate.collider.GetComponent<ColorPlate>();
                        if (plateSpawn.ListValue.Count == 0) return;

                        //if (countMove > 0)
                        //{
                        //    ManagerAudio.PlaySound(ManagerAudio.Data.soundEasyButton);
                        //    SetColorIntoStartPlate(plateSpawn, listNextPlate[0]);
                        //}
                    }

                    // using Item Hammer
                    if (isUsingHammer)
                    {
                        if (Physics.Raycast(ray, out var plate, 100f, layerUsingItem))
                        {
                            ColorPlate plateSelect = plate.collider.GetComponent<ColorPlate>();

                            Debug.Log(plateSelect.name);

                            if (plateSelect.ListValue.Count == 0 || plateSelect.status == Status.Frozen) return;

                            //hammer.gameObject.SetActive(true);
                            //hammer.transform.position = plateSelect.transform.position + GameConfig.OFFSET_HAMMER;
                            //hammer.hitColorPlate = plateSelect.transform.position;
                            //hammer.colorPlateDestroy = plateSelect;

                            hammerSpine.gameObject.SetActive(true);
                            hammerSpine.anim.transform.position = plateSelect.transform.position;
                            hammerSpine.PlayAnim();
                            hammerSpine.colorPlateDestroy = plateSelect;

                            SaveGame.Hammer--;
                            isUsingHammer = false;
                        }
                    }

                    if (Physics.Raycast(ray, out var hitPlateAds, 100f, layerPlate) && !isPauseGame && !isUsingHammer)
                    {
                        ColorPlate adsPlate = hitPlateAds.collider.GetComponent<ColorPlate>();

                        if (adsPlate.status != Status.Ads) return;

                        Debug.Log(" Watch Ads to Unlock AdsPlate");
                        adsPlate.status = Status.None;
                        adsPlate.logicVisual.Refresh();
                    }
                }
            }
        }

        if (timerRun >= 0)
        {
            timerRun -= Ez.TimeMod;
            if (timerRun < 0)
            {
                RecursiveMerge();
            }
        }

        if (point >= maxPoint && !isWin && !isContiuneMerge && !SaveGame.Challenges)
        {
            CheckWin();
            StartCoroutine(RaiseEventWin());
        };

        for (int i = 0; i < ListArrowPlate.Count; i++)
        {
            ListArrowPlate[i].PlayAnimArrow();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            saveGameNormal = null;
            PlayerPrefs.DeleteKey(GameConfig.GAMESAVENORMAL);

            SaveGame.Level++;
            ManagerEvent.ClearEvent();

            SceneManager.LoadScene("SceneGame");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            saveGameNormal = null;
            PlayerPrefs.DeleteKey(GameConfig.GAMESAVENORMAL);

            SaveGame.Level--;
            ManagerEvent.ClearEvent();

            SceneManager.LoadScene("SceneGame");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            isLose = true;
            StartCoroutine(RaiseEventLose());
        }

        //if (!isMergeing)
        //{
        //    CheckClear();
        //}
    }


    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        return cam.ScreenToWorldPoint(mousePos);
    }

    public void RecursiveMerge()
    {
        if (listSteps.Count > 0)
        {
            isContiuneMerge = true;

            Merge(listSteps[listSteps.Count - 1].nearByColorPlate, listSteps[listSteps.Count - 1].rootColorPlate);
        }
        else
        {
            isContiuneMerge = false;

            //CheckClear();

            if (isWin) return;

            ProcessRemainingPlates();

            if (listSteps.Count > 0)
            {
                RecursiveMerge();
            }
            else
            {
                colorRoot = null;
                isMergeing = false;
                CheckClear();
            }

        }

        if (!isLose)
        {
            CheckLose();
        }
    }

    void SetColor(ColorPlate startColorPlate, ColorPlate endColorPlate)
    {
        if (startColorPlate.ListValue.Count == 0)
        {
            if (listNextPlate[0].ListValue.Count == 0) return;

            foreach (LogicColor renderer in listNextPlate[0].ListColor)
            {
                Vector3 localPos = renderer.transform.localPosition;
                renderer.transform.SetParent(startColorPlate.transform);
                float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);

                renderer.transform.localPosition = new Vector3(randomX, localPos.y, localPos.z);
                renderer.transform.localRotation = Quaternion.identity;
                renderer.transform.localScale = Vector3.one;
            }

            startColorPlate.ListValue.AddRange(listNextPlate[0].ListValue);
            startColorPlate.ListColor.AddRange(listNextPlate[0].ListColor);
            startColorPlate.listTypes.AddRange(listNextPlate[0].listTypes);

            startColorPlate.InitValue(startColorPlate.transform);
            listNextPlate[0].ListValue.Clear();
            listNextPlate[0].ListColor.Clear();
            listNextPlate[0].listTypes.Clear();

            //return;

            //Tween t = null;

            foreach (LogicColor renderer in listNextPlate[1].ListColor)
            {
                Vector3 localPos = renderer.transform.localPosition;
                renderer.transform.SetParent(listNextPlate[0].transform);
                float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);

                /*t = */
                //renderer.transform.DOLocalMove(new Vector3(randomX, localPos.y, localPos.z), 0.3f);
                renderer.transform.localPosition = new Vector3(randomX, localPos.y, localPos.z);
                renderer.transform.localRotation = Quaternion.identity;
                renderer.transform.localScale = Vector3.one;
            }

            listNextPlate[0].ListValue.AddRange(listNextPlate[1].ListValue);
            listNextPlate[0].ListColor.AddRange(listNextPlate[1].ListColor);
            listNextPlate[0].listTypes.AddRange(listNextPlate[1].listTypes);
            listNextPlate[0].InitValue(listNextPlate[0].transform);

            listNextPlate[1].ListValue.Clear();
            listNextPlate[1].ListColor.Clear();
            listNextPlate[1].listTypes.Clear();

            spawnBook.PlayAnimSpawn();
            //Tweener t = null;
            //t = listNextPlate[0].transform.DOMove(listNextPlate[1].transform.position, 0.3f).SetEase(Ease.OutCirc);
            //listNextPlate[1].transform.DOMove(listNextPlate[0].transform.position, 0.3f).SetEase(Ease.InCirc);
            //t = listNextPlate[0].transform.DOLocalMove(listNextPlate[1].transform.localPosition, 0.3f).SetEase(Ease.OutCirc);
            //listNextPlate[1].transform.DOLocalMove(listNextPlate[0].transform.localPosition, 0.3f).SetEase(Ease.InCirc);
            //foreach (LogicColor c in listNextPlate[0].ListColor)
            //{
            //    c.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.2f);
            //}

            //foreach (LogicColor c in listNextPlate[1].ListColor)
            //{
            //    c.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            //}
            //Swap(listNextPlate);
            //t.OnComplete(() =>
            //{
            //    listNextPlate[1].InitColor();
            //});

            float delay = 0f;

            //Debug.Log($"=----------------End Position {endColorPlate.transform.position}");

            Sequence sq = DOTween.Sequence();
            foreach (LogicColor renderer in startColorPlate.ListColor)
            {
                Vector3 localPos = renderer.transform.localPosition;
                renderer.transform.SetParent(endColorPlate.transform);

                Transform transformCache = renderer.transform;
                float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);

                sq.Insert(delay, transformCache.DOLocalMove(new Vector3(randomX, localPos.y, localPos.z), 0.4f)
                                                .SetEase(curveMove)
                    //.OnComplete(() =>
                    //{
                    //    transformCache.localPosition = new Vector3(randomX, localPos.y, localPos.z);
                    //})
                    );
                //transformCache.DOLocalMove(new Vector3(0, localPos.y, localPos.z), 0.4f);

                renderer.transform.localRotation = Quaternion.identity;
                renderer.transform.localScale = Vector3.one;
                delay += 0.01f;
            }

            List<ColorEnum> listValueMid = new List<ColorEnum>();
            List<LogicColor> ListColorMid = new List<LogicColor>();
            List<GroupEnum> listTypes = new List<GroupEnum>();

            listValueMid.AddRange(startColorPlate.ListValue);
            ListColorMid.AddRange(startColorPlate.ListColor);
            listTypes.AddRange(startColorPlate.listTypes);


            startColorPlate.ListColor.Clear();
            startColorPlate.listTypes.Clear();
            startColorPlate.ListValue.Clear();

            endColorPlate.ListColor.AddRange(ListColorMid);
            endColorPlate.listTypes.AddRange(listTypes);
            endColorPlate.ListValue.AddRange(listValueMid);

            sq.OnComplete(() =>
            {
                if ((int)endColorPlate.TopValue != (int)ColorEnum.Random)
                {
                    if (!isMergeing)
                    {
                        List<ColorPlate> listDataConnect = new List<ColorPlate>();
                        CheckNearByRecursive(listDataConnect, endColorPlate);

                        if (listDataConnect.Count <= 1)
                        {
                            if (!isLose) CheckLose();
                        }
                        else
                        {
                            //listSteps.Clear();

                            FindTarget findTarget = new FindTarget();
                            if (colorRoot == null) colorRoot = findTarget.FindTargetRoot(listDataConnect);
                            Debug.Log("listDataConnect.Count: " + listDataConnect.Count);
                            Debug.Log(" Color Root:" + colorRoot.name);
                            HashSet<ColorPlate> processedNearBy = new HashSet<ColorPlate>();
                            HashSet<ColorPlate> processedRoot = new HashSet<ColorPlate>();
                            AddStepRecursivelyOtherRoot(colorRoot, listDataConnect, processedRoot, processedNearBy);
                            RecursiveMerge();
                        }
                    }
                }
                else
                {
                    FindColorEnum findColorEnum = new FindColorEnum();
                    ColorEnum cEnum = findColorEnum.FindTargetColorEnum(endColorPlate.ListConnect);

                    Sequence sequenceSpecial = DOTween.Sequence();

                    sequenceSpecial.AppendCallback(() =>
                        chargingParticlePool.Spawn(endColorPlate.transform.position, true)
                    );

                    sequenceSpecial.AppendInterval(0.5f);

                    sequenceSpecial.AppendCallback(() =>
                        {
                            endColorPlate.Init(GetColorNew);
                            endColorPlate.ChangeSpecialColorPLate(cEnum);
                        }
                    );

                    sequenceSpecial.AppendInterval(0.1f);

                    sequenceSpecial.AppendCallback(() =>
                        {
                            changeColorParticlePool.Spawn(endColorPlate.transform.position, true);
                            RecursiveMerge();

                        }
                    );

                    //chargingParticlePool.Spawn(endColorPlate.transform.position + new Vector3(0, 1.2f, 0), true);
                    //changeColorParticlePool.Spawn(endColorPlate.transform.position + new Vector3(0, 1.2f, 0), true);

                    //endColorPlate.Init(GetColorNew);
                    //endColorPlate.ChangeSpecialColorPLate(cEnum);

                }

                //if (!SaveGame.IsDoneTutorial)
                //{
                //    canvasTutorial.enabled = true;
                //    tutorial.PlayProgressTut(2);
                //    isPauseGame = true;
                //}
            });
        }
    }


    public void SetColorUsingSwapItem(ColorPlate startColorPlate, ColorPlate endColorPlate)
    {
        if (endColorPlate.ListValue.Count == 0)
        {
            for (int i = 0; i < startColorPlate.ListColor.Count; i++)
            {
                LogicColor c = startColorPlate.ListColor[i];
                c.transform.SetParent(endColorPlate.transform);
                c.transform.localPosition = new Vector3(0, i * GameConfig.OFFSET_PLATE, 0);
                c.transform.localRotation = Quaternion.identity;
                c.transform.localScale = Vector3.one;
            }

            List<ColorEnum> listValueMid = new List<ColorEnum>();
            List<LogicColor> ListColorMid = new List<LogicColor>();
            List<GroupEnum> listTypes = new List<GroupEnum>();

            listValueMid.AddRange(startColorPlate.ListValue);
            ListColorMid.AddRange(startColorPlate.ListColor);
            listTypes.AddRange(startColorPlate.listTypes);


            startColorPlate.ListValue.Clear();
            startColorPlate.ListColor.Clear();
            startColorPlate.listTypes.Clear();


            endColorPlate.ListValue.AddRange(listValueMid);
            endColorPlate.ListColor.AddRange(ListColorMid);
            endColorPlate.listTypes.AddRange(listTypes);
        }
        else
        {
            for (int i = 0; i < startColorPlate.ListColor.Count; i++)
            {
                LogicColor c = startColorPlate.ListColor[i];
                c.transform.SetParent(endColorPlate.transform);
                c.transform.localPosition = new Vector3(0, i * GameConfig.OFFSET_PLATE, 0);
                c.transform.localRotation = Quaternion.identity;
                c.transform.localScale = Vector3.one;
            }

            for (int i = 0; i < endColorPlate.ListColor.Count; i++)
            {
                LogicColor c = endColorPlate.ListColor[i];
                c.transform.SetParent(startColorPlate.transform);
                c.transform.localPosition = new Vector3(0, i * GameConfig.OFFSET_PLATE, 0);
                c.transform.localRotation = Quaternion.identity;
                c.transform.localScale = Vector3.one;
            }

            List<ColorEnum> listValueMid = new List<ColorEnum>();
            List<LogicColor> ListColorMid = new List<LogicColor>();
            List<GroupEnum> listTypes = new List<GroupEnum>();

            listValueMid.AddRange(startColorPlate.ListValue);
            ListColorMid.AddRange(startColorPlate.ListColor);
            listTypes.AddRange(startColorPlate.listTypes);

            startColorPlate.ListValue.Clear();
            startColorPlate.ListColor.Clear();
            startColorPlate.listTypes.Clear();

            startColorPlate.ListValue.AddRange(endColorPlate.ListValue);
            startColorPlate.ListColor.AddRange(endColorPlate.ListColor);
            startColorPlate.listTypes.AddRange(endColorPlate.listTypes);

            endColorPlate.ListValue.Clear();
            endColorPlate.ListColor.Clear();
            endColorPlate.listTypes.Clear();

            endColorPlate.ListValue.AddRange(listValueMid);
            endColorPlate.ListColor.AddRange(ListColorMid);
            endColorPlate.listTypes.AddRange(listTypes);
        }
    }

    void ProcessRemainingPlates()
    {
        colorRoot = null;
        isMergeing = false;
        Debug.Log(" _________________________________________ ");
        foreach (ColorPlate c in ListColorPlate)
        {
            if (c.ListValue.Count == 0 || c.status == Status.Empty || c.countFrozen != 0) continue;
            List<ColorPlate> listDataConnect = new List<ColorPlate>();
            CheckNearByRecursive(listDataConnect, c);
            if (listDataConnect.Count <= 1) continue;

            FindTarget findTarget = new FindTarget();
            if (colorRoot == null) colorRoot = findTarget.FindTargetRoot(listDataConnect);
            Debug.Log("Root : " + colorRoot.name);

            HashSet<ColorPlate> processedNearBy = new HashSet<ColorPlate>();
            HashSet<ColorPlate> processedRoot = new HashSet<ColorPlate>();
            AddStepRecursivelyOtherRoot(colorRoot, listDataConnect, processedRoot, processedNearBy);
            break;
        }
    }

    public List<Step> listSteps = new List<Step>();
    public void AddStepRecursively(ColorPlate colorRoot, List<ColorPlate> listDataConnect, HashSet<ColorPlate> processedNearBy)
    {
        ColorPlate colorNearBy = new Step().ColorNearByColorRoot(colorRoot, listDataConnect, processedNearBy);
        if (colorNearBy == null || processedNearBy.Contains(colorNearBy))
        {
            return;
        }

        processedNearBy.Add(colorNearBy);
        processedNearBy.Add(colorRoot);

        listSteps.Add(new Step
        {
            rootColorPlate = colorRoot,
            nearByColorPlate = colorNearBy
        });

        AddStepRecursively(colorRoot, listDataConnect, processedNearBy);
    }
    public void AddStepRecursivelyOtherRoot(ColorPlate colorRoot, List<ColorPlate> listDataConnect, HashSet<ColorPlate> processedRoot, HashSet<ColorPlate> processedNearBy)
    {
        if (processedRoot.Contains(colorRoot))
        {
            return;
        }
        processedRoot.Add(colorRoot);

        foreach (var p in processedRoot.ToList())
        {
            AddStepRecursively(p, listDataConnect, processedNearBy);
        }

        foreach (var p in processedNearBy.ToList())
        {
            AddStepRecursivelyOtherRoot(p, listDataConnect, processedRoot, processedNearBy);
        }
    }

    public List<ColorPlate> listNearByCanConnect = new List<ColorPlate>();
    public void CheckNearByRecursive(List<ColorPlate> listDataConnect, ColorPlate colorPlate)
    {
        List<ColorPlate> listNearBySame = colorPlate.CheckNearByCanConnect(/*colorPlate*/);

        foreach (ColorPlate c in listNearBySame)
        {
            if (c.ListValue.Count == 0 || c.countFrozen != 0) continue;

            if (c.TopValue == colorPlate.TopValue)
            {
                if (!listDataConnect.Contains(colorPlate))
                {
                    listDataConnect.Add(colorPlate);

                }
                if (!listDataConnect.Contains(c))
                {
                    listDataConnect.Add(c);
                    CheckNearByRecursive(listDataConnect, c);
                }
            }
        }
    }
    public void CheckClear()
    {
        foreach (ColorPlate colorPlate in ListColorPlate)
        {
            if (colorPlate.listTypes.Count <= 0 || colorPlate.status == Status.Empty) continue;
            int count = colorPlate.listTypes[colorPlate.listTypes.Count - 1].listPlates.Count;
            if (count < RULE_COMPLETE) continue;

            if (count >= RULE_COMPLETE)
            {
                //if (!SaveGame.IsDoneTutorial)
                //{
                //    canvasTutorial.enabled = true;
                //    tutorial.PlayProgressTut(2);
                //    isPauseGame = true;
                //}

                //if (!isPauseGame)
                //{

                ControllerAnimState.ActionToBonus();
                timerRun += timerConfigData.timeRun;

                colorPlate.InitClear(true);
                colorPlate.DecreaseCountFrozenNearBy();
                colorPlate.InitValue();
                //}
            }
        }
        //Debug.Log("point: " + point);
        //IncreaseCountDiff();
    }

    public void IncreaseCountDiff()
    {
        if (point >= 20) countDiff = 3;
        if (point >= 50) countDiff = 4;
        if (point >= 100) countDiff = 5;
        if (point >= 150) countDiff = 6;
        if (point >= 180) countDiff = 7;

        if (countDiff > countDiffMax) countDiff = countDiffMax;
    }

    public void SpawnSpecialColor()
    {
        if (SaveGame.Level < 12) return;

        if (point >= pointSpawnSpecial)
        {
            Debug.Log("spawn special color");
            isHadSpawnSpecial = true;
            pointSpawnSpecial += 50;
        }
    }

    public void ExecuteLockCoin(int point)
    {
        foreach (ColorPlate c in ListColorPlate)
        {
            if (c.status == Status.Empty) continue;
            c.UnlockedLockCoin(point);
        }
    }

    //for (int i = listSteps.Count - 1; i >= 0; i--)
    //{
    //    Debug.Log(listSteps[i].nearByColorPlate + " to " + listSteps[i].rootColorPlate);
    //}
    void Merge(ColorPlate startColorPlate, ColorPlate endColorPlate)
    {
        timerRun = 0;
        isMergeing = true;
        int count = startColorPlate.listTypes[startColorPlate.listTypes.Count - 1].listPlates.Count;
        Sequence sequence = DOTween.Sequence();

        for (int i = count - 1; i >= 0; i--)
        {
            //float delay = 0.08f * (count - 1 - i);

            sequence.AppendCallback(() =>
            {
                //    DOVirtual.DelayedCall(delay, () =>
                //{
                if (startColorPlate.TopValue == endColorPlate.TopValue)
                {
                    startColorPlate.TopColor.transform.SetParent(endColorPlate.transform);

                    ManagerAudio.PlaySound(ManagerAudio.Data.soundMerge);

                    endColorPlate.listTypes[endColorPlate.listTypes.Count - 1].listPlates.Add(startColorPlate.TopValue);
                    startColorPlate.listTypes[startColorPlate.listTypes.Count - 1].listPlates.Remove(startColorPlate.TopValue);

                    startColorPlate.ClearLastType();

                    endColorPlate.ListValue.Add(startColorPlate.TopValue);
                    endColorPlate.ListColor.Add(startColorPlate.TopColor);


                    startColorPlate.ListValue.RemoveAt(startColorPlate.ListValue.Count - 1);
                    startColorPlate.ListColor.RemoveAt(startColorPlate.ListColor.Count - 1);

                    if (startColorPlate.Col == endColorPlate.Col)
                    {
                        endColorPlate.InitValue(endColorPlate.transform, true, 1);

                    }
                    else if (startColorPlate.Row == endColorPlate.Row)
                    {

                        endColorPlate.InitValue(endColorPlate.transform, true, 0);
                    }
                }
            });

            //timerRun += 0.13f;

            sequence.AppendInterval(timerConfigData.timeMerge);
            timerRun += timerConfigData.timeRun;
        }
        //timerRun += 0.2f;
        //timerRun += 0.1f * count + 0.5f;
        sequence.Play();
        if (listSteps.Count > 0) listSteps.RemoveAt(listSteps.Count - 1);
    }

    void CheckLose()
    {
        //bool allPlaced = true;
        int countZeroListValues = 0;


        for (int i = 0; i < ListCheckPlate.Count; i++)
        {
            if (ListCheckPlate[i].ListValue.Count == 0)
            {
                if (!CheckColorPlateValue(ListCheckPlate[i]))
                {
                    //Debug.Log(ListCheckPlate[i].name);
                    countZeroListValues++;
                }
            }

        }
        //Debug.Log("countZeroListValues: " + countZeroListValues);
        if (countZeroListValues > 2)
        {
            homeInGame.imgDanger.SetActive(false);
            //Debug.LogWarning("More than 2 items with ListValue count equal to 0.");

        }
        else
        {
            homeInGame.imgDanger.SetActive(true);
            //Debug.LogWarning(" Show Danger");
        }

        if (countZeroListValues == 0 && !isMergeing)
        {
            isLose = true;
            saveGameNormal = null;
            PlayerPrefs.DeleteKey(GameConfig.GAMESAVENORMAL);

            if (SaveGame.Challenges)
            {
                saveGameChallenges = null;
                PlayerPrefs.DeleteKey(GameConfig.GAMESAVECHALLENGES);
            }

            Debug.Log("You lose");
            StartCoroutine(RaiseEventLose());
        }
    }

    bool CheckColorPlateValue(ColorPlate colorPlateCheck)
    {

        if (colorPlateCheck.isLocked || colorPlateCheck.status == Status.CannotPlace || colorPlateCheck.countFrozen != 0 || colorPlateCheck.status == Status.Ads)
        {
            return true;
        }

        return false;
    }

    IEnumerator RaiseEventLose()
    {
        yield return new WaitForSeconds(1f);
        if (!SaveGame.Challenges)
            ManagerEvent.RaiseEvent(EventCMD.EVENT_LOSE);
        else ManagerEvent.RaiseEvent(EventCMD.EVENT_CHALLENGES);
    }
    void CheckWin()
    {
        isWin = true;

        SaveGame.IsShowBook = false;
        Debug.Log(point + " __ " + gold + " __ " + pigment);
        Debug.Log("check win");
        if (SaveGame.Level < 19)
            SaveGame.Level++;

        saveGameNormal = null;
        PlayerPrefs.DeleteKey(GameConfig.GAMESAVENORMAL);

        foreach (ColorPlate c in ListColorPlate)
        {
            if (c.ListValue.Count == 0 || c.status == Status.Empty) continue;

            c.ClearAll();
        }
    }
    IEnumerator RaiseEventWin()
    {
        yield return new WaitForSeconds(1.5f);
        ManagerEvent.RaiseEvent(EventCMD.EVENT_WIN);
    }
    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }

    public void ReviveGame()
    {
        countRevive--;
        StartCoroutine(ClearSomeArrows());
    }

    IEnumerator ClearSomeArrows()
    {
        yield return new WaitForSeconds(0.5f);

        if (ListCheckPlate.Count >= 4)
        {
            List<ColorPlate> newListRevive = new List<ColorPlate>();
            while (newListRevive.Count < 3)
            {
                int randomIndex = UnityEngine.Random.Range(0, ListCheckPlate.Count);

                if (!newListRevive.Contains(ListCheckPlate[randomIndex]))
                {
                    Debug.Log("randomIndex: " + randomIndex);
                    newListRevive.Add(ListCheckPlate[randomIndex]);
                }
            }

            for (int i = 0; i < newListRevive.Count; i++)
            {
                newListRevive[i].ClearAll();
            }

            homeInGame.imgDanger.SetActive(false);
        }
        else
        {
            for (int i = 0; i < ListArrowPlate.Count; i++)
            {
                ListArrowPlate[i].ClearAll();
            }

            homeInGame.imgDanger.SetActive(false);
        }
    }

    #region SaveDataProgress
    public SaveCurrentDataGame saveGameNormal = new SaveCurrentDataGame();
    public SaveCurrentChallenges saveGameChallenges = new SaveCurrentChallenges();

    private void OnApplicationQuit()
    {
        SaveDataGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("Páue");
            SaveDataGame();
        }
        else
        {
            Debug.Log("UnPause");
        }
    }

    public void SaveDataGame()
    {
        if (!SaveGame.Challenges)
        {
            SaveGameNormal();
        }
        else
        {
            SaveGameChallenges();
        }
    }

    void SaveGameNormal()
    {
        SaveCurrentDataGame currentData = new SaveCurrentDataGame();

        currentData.currentPoint = point;

        List<ColorPlateInTable> listColorPlateInTable = new List<ColorPlateInTable>();

        for (int i = 0; i < ListColorPlate.Count; i++)
        {
            List<CurrentEnum> listCurrentEnum = new List<CurrentEnum>();

            for (int j = 0; j < ListColorPlate[i].listTypes.Count; j++)
            {
                CurrentEnum currentEnum = new CurrentEnum();

                currentEnum.indexEnum = (int)ListColorPlate[i].listTypes[j].type;
                currentEnum.countEnum = ListColorPlate[i].listTypes[j].listPlates.Count;
                listCurrentEnum.Add(currentEnum);
            }

            ColorPlateInTable colorPlateInTable = new ColorPlateInTable()
            {
                typeColorPlate = (int)ListColorPlate[i].status,
                countFrozen = ListColorPlate[i].countFrozen,
                pointToUnlock = ListColorPlate[i].pointToUnLock,
                listEnum = listCurrentEnum,
            };

            listColorPlateInTable.Add(colorPlateInTable);
        }

        currentData.ListColorPlate = listColorPlateInTable;
        saveGameNormal = currentData;
        string gameSaveData = JsonUtility.ToJson(saveGameNormal);
        PlayerPrefs.SetString(GameConfig.GAMESAVENORMAL, gameSaveData);
        PlayerPrefs.Save();
    }

    void SaveGameChallenges()
    {
        SaveCurrentChallenges currentData = new SaveCurrentChallenges();

        currentData.currentPoint = point;

        List<ColorPlateInTable> listColorPlateInTable = new List<ColorPlateInTable>();

        for (int i = 0; i < ListColorPlate.Count; i++)
        {
            List<CurrentEnum> listCurrentEnum = new List<CurrentEnum>();

            for (int j = 0; j < ListColorPlate[i].listTypes.Count; j++)
            {
                CurrentEnum currentEnum = new CurrentEnum();

                currentEnum.indexEnum = (int)ListColorPlate[i].listTypes[j].type;
                currentEnum.countEnum = ListColorPlate[i].listTypes[j].listPlates.Count;
                listCurrentEnum.Add(currentEnum);
            }

            ColorPlateInTable colorPlateInTable = new ColorPlateInTable()
            {
                typeColorPlate = (int)ListColorPlate[i].status,
                countFrozen = ListColorPlate[i].countFrozen,
                pointToUnlock = ListColorPlate[i].pointToUnLock,
                listEnum = listCurrentEnum,
            };

            listColorPlateInTable.Add(colorPlateInTable);
        }

        currentData.ListColorPlate = listColorPlateInTable;
        saveGameChallenges = currentData;

        string gameSaveData = JsonUtility.ToJson(saveGameChallenges);
        PlayerPrefs.SetString(GameConfig.GAMESAVECHALLENGES, gameSaveData);
        PlayerPrefs.Save();
    }

    void LoadSaveData()
    {
        if (!SaveGame.Challenges)
        {
            string gameSaveData = PlayerPrefs.GetString(GameConfig.GAMESAVENORMAL, "");
            if (string.IsNullOrEmpty(gameSaveData))
            {
                Debug.Log("nullll");
                saveGameNormal = null;
                return;
            }

            saveGameNormal = JsonUtility.FromJson<SaveCurrentDataGame>(gameSaveData);
        }
        else
        {
            string gameSaveData = PlayerPrefs.GetString(GameConfig.GAMESAVECHALLENGES, "");
            if (string.IsNullOrEmpty(gameSaveData))
            {
                Debug.Log("nullll");
                saveGameChallenges = null;
                return;
            }

            saveGameChallenges = JsonUtility.FromJson<SaveCurrentChallenges>(gameSaveData);
        }
    }

    void LoadSaveNormalData()
    {
        Debug.Log("saveGameNormal.currentPoint: " + saveGameNormal.currentPoint);
        point = saveGameNormal.currentPoint;

        for (int i = 0; i < saveGameNormal.ListColorPlate.Count; i++)
        {
            //Debug.Log(saveGameNormal.ListColorPlate[i].typeColorPlate + " ___ " + saveGameNormal.ListColorPlate[i].listEnum.Count);

            ListColorPlate[i].status = (Status)saveGameNormal.ListColorPlate[i].typeColorPlate;

            ListColorPlate[i].logicVisual.SetSpecialSquareExisted(ListColorPlate[i].status, saveGameNormal.ListColorPlate[i].countFrozen, ListColorPlate[i].Row);

            if (ListColorPlate[i].status == Status.Frozen)
            {
                ListColorPlate[i].countFrozen = saveGameNormal.ListColorPlate[i].countFrozen;
            }

            if (ListColorPlate[i].status == Status.LockCoin)
            {
                ListColorPlate[i].isLocked = true;
                ListColorPlate[i].pointToUnLock = saveGameNormal.ListColorPlate[i].pointToUnlock;
                ListColorPlate[i].txtPointUnlock.text = saveGameNormal.ListColorPlate[i].pointToUnlock.ToString();
                ListColorPlate[i].txtPointUnlock.gameObject.SetActive(true);
            }

            if (ListColorPlate[i].status == Status.Empty)
            {
                ListColorPlate[i].logicVisual.DeletePlate();
            }

            if (ListColorPlate[i].status == Status.None)
            {
                ListColorPlate[i].logicVisual.Refresh();
            }

            ListColorPlate[i].Init(GetColorNew);

            ListColorPlate[i].InitColorExisted(saveGameNormal.ListColorPlate[i].listEnum);

        }
    }


    void LoadSaveChallengesData()
    {
        //Debug.Log("saveGameChallenges.currentPoint: " + saveGameChallenges.currentPoint);
        point = saveGameChallenges.currentPoint;

        for (int i = 0; i < saveGameChallenges.ListColorPlate.Count; i++)
        {
            //Debug.Log(saveGameChallenges.ListColorPlate[i].typeColorPlate + " ___ " + saveGameChallenges.ListColorPlate[i].listEnum.Count);

            ListColorPlate[i].status = (Status)saveGameChallenges.ListColorPlate[i].typeColorPlate;

            ListColorPlate[i].logicVisual.SetSpecialSquareExisted(ListColorPlate[i].status, saveGameChallenges.ListColorPlate[i].countFrozen, ListColorPlate[i].Row);

            if (ListColorPlate[i].status == Status.Frozen)
            {
                ListColorPlate[i].countFrozen = saveGameChallenges.ListColorPlate[i].countFrozen;
            }

            if (ListColorPlate[i].status == Status.LockCoin)
            {
                ListColorPlate[i].isLocked = true;
                ListColorPlate[i].pointToUnLock = saveGameChallenges.ListColorPlate[i].pointToUnlock;
                ListColorPlate[i].txtPointUnlock.text = saveGameChallenges.ListColorPlate[i].pointToUnlock.ToString();
                ListColorPlate[i].txtPointUnlock.gameObject.SetActive(true);
            }

            if (ListColorPlate[i].status == Status.Empty)
            {
                ListColorPlate[i].logicVisual.DeletePlate();
            }

            if (ListColorPlate[i].status == Status.None)
            {
                ListColorPlate[i].logicVisual.Refresh();
            }

            ListColorPlate[i].Init(GetColorNew);

            ListColorPlate[i].InitColorExisted(saveGameChallenges.ListColorPlate[i].listEnum);

        }
    }

    #endregion


}
