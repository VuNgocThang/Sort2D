using BaseGame;
using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.Common;

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
    public Transform targetUIPosition;

    [SerializeField] Camera cam;
    [SerializeField] Transform holder;
    [SerializeField] Transform nParentArrow;

    [SerializeField]
    Transform nParentNextCubeNormal,
        nParentNextCubeMini,
        nParentSpawnBookNormal,
        nParentSpawnBookMini,
        nBgNormal,
        nBgMini;

    [SerializeField] GameObject testStack;
    [SerializeField] Transform nNextCube1, nStand;

    [SerializeField] ColorPlate colorPLatePrefab;
    [SerializeField] ColorPlate arrowPlatePrefab;

    public List<ColorPlate> listNextPlate;
    public List<ColorPlate> listSpawnNew;
    public PopupHome homeInGame;

    /*[HideInInspector]*/
    public List<ColorPlate> ListArrowPlate;

    /*[HideInInspector]*/
    public List<ColorPlate> ListCheckPlate;

    /*[HideInInspector]*/
    public List<ColorPlate> ListColorPlate;

    /*[HideInInspector]*/
    public int rows;

    /*[HideInInspector]*/
    public int cols;

    /*[HideInInspector]*/
    public bool isMergeing;

    /*[HideInInspector]*/
    public bool isLose = false;

    /*[HideInInspector]*/
    public bool isWin = false;

    /*[HideInInspector]*/
    public bool isPauseGame = false;

    /*[HideInInspector]*/
    public static bool isContiuneMerge = false;

    /*[HideInInspector]*/
    public int point;

    /*[HideInInspector]*/
    public int maxPoint;

    /*[HideInInspector]*/
    public int gold;

    /*[HideInInspector]*/
    public int pigment;

    /*[HideInInspector]*/
    public int countRevive;

    /*[HideInInspector]*/
    public int countDiff;

    /*[HideInInspector]*/
    public int countDiffMax;

    /*[HideInInspector]*/
    public bool isUsingHammer;

    /*[HideInInspector]*/
    public bool isUsingHand;

    /*[HideInInspector]*/
    public List<int> listIntColor;

    [SerializeField] LayerMask layerArrow;
    [SerializeField] LayerMask layerPlateSpawn;
    [SerializeField] LayerMask layerUsingItem;
    [SerializeField] LayerMask layerPlate;
    [SerializeField] List<LogicColor> listColors;
    private ColorPlate colorRoot;

    [SerializeField] private ParticleSystem clickParticle;
    [SerializeField] private ParticleSystem arrowClickParticle;
    [SerializeField] private ParticleSystem eatParticle;
    [SerializeField] private ParticleSystem unlockParticle;
    [SerializeField] private ParticleSystem unlockAdsParticle;
    [SerializeField] private ParticleSystem specialParticle;
    [SerializeField] private ParticleSystem chargingParticle;
    [SerializeField] private ParticleSystem changeColorParticle;
    [SerializeField] private ParticleSystem frostExplosion;
    [SerializeField] private ParticleSystem currentSpecialParticle;

    public CustomPool<ParticleSystem> clickParticlePool;
    public CustomPool<ParticleSystem> arrowClickParticlePool;
    public CustomPool<ParticleSystem> eatParticlePool;
    public CustomPool<ParticleSystem> unlockParticlePool;
    public CustomPool<ParticleSystem> unlockAdsParticlePool;
    public CustomPool<ParticleSystem> specialParticlePool;
    public CustomPool<ParticleSystem> chargingParticlePool;
    public CustomPool<ParticleSystem> changeColorParticlePool;
    public CustomPool<ParticleSystem> frostExplosionPool;

    Tweener tweenerMove;

    public AnimationCurve curveMove;
    [SerializeField] SetMapManager setMapManager;
    [SerializeField] public Canvas canvasTutorial;
    public HammerSpineEvent hammerSpine;
    public Tutorial tutorial;

    int pointSpawnSpecial = 100;
    int countSpawnSpecial = 0;
    bool isHadSpawnSpecial = false;
    float timeClick = -1;
    float timerRun = -1;

    //[SerializeField] SpineSelectionChange spineSelection;
    [SerializeField] ControllerAnimState ControllerAnimState;
    [SerializeField] TimerConfigData timerConfigData;
    [SerializeField] SpawnBookTest spawnBook;

    public ColorPlateData colorPlateData;
    public DataLevel dataLevel = new DataLevel();
    public bool IsDataLoaded { get; private set; } = false;
    private int timePlayed;
    private DateTime timeStart;
    private DateTime timeEnd;
    public bool IsInitDone;

    LogicColor GetColorNew()
    {
        //LogicColor logicColor = listColors.GetClone();
        //logicColor.RefreshColor();
        return listColors.GetClone();
        //return logicColor;
    }

    private void Awake()
    {
        Instance = this;
        ManagerEvent.RegEvent(EventCMD.EVENT_SWITCH, SwitchNextPlate);
        ManagerEvent.RegEvent(EventCMD.EVENT_INTER_ADS, ShowInterAds);

        //ManagerEvent.RegEvent(EventCMD.EVENT_SPAWN_PLATE, InitPlateSpawn);
    }

    void ShowInterAds(object e)
    {
        string pWhere = e as string;
        AdsController.instance.ShowInterAd((check) => { SaveGame.CountWatchInter++; }, pWhere);
    }

    async void Start()
    {
        // Application.targetFrameRate = 60;
        //enabled = false;
        await Refresh();
        LoadSaveData();
        await LoadData();
        InitListCheckPlate();
        spawnBook.gameObject.SetActive(true);
        spawnBook.PlayAnimSpawn();
        // GameManager.ShowInterAds("Replay");

        //InitNextPlate();
        RecursiveMerge();
        //enabled = true;
    }

    private async Task Refresh()
    {
        DOTween.KillAll();

        countDiff = 3;

        if (GameManager.IsNormalGame())
        {
            int indexLevelNormal = 0;
            if (SaveGame.Level > GameConfig.MAX_LEVEL)
            {
                indexLevelNormal = (SaveGame.Level % GameConfig.MAX_LEVEL) + GameConfig.LOOP_START_LEVEL;
            }
            else
            {
                indexLevelNormal = SaveGame.Level;
            }

            Config.currLevel = indexLevelNormal;
            dataLevel = await DataLevel.GetData(indexLevelNormal);

            FirebaseManager.instance.LogLevelStart(SaveGame.Level);
        }
        else if (GameManager.IsBonusGame())
        {
            dataLevel = await DataLevel.GetData(SaveGame.LevelBonus);
            Config.currLevel = SaveGame.LevelBonus;

            FirebaseManager.instance.LogLevelStart(SaveGame.LevelBonus);
        }
        else if (GameManager.IsChallengesGame())
        {
            dataLevel = await DataLevel.GetData(SaveGame.LevelChallenges);
            Config.currLevel = SaveGame.LevelChallenges;
        }

        countDiffMax = dataLevel.CountDiff;
        listIntColor = dataLevel.Colors.ToList();

        IsInitDone = false;
        isWin = false;
        isLose = false;
        isMergeing = false;
        isPauseGame = false;
        listColors.Refresh();
        countRevive = 1;
        point = 0;
        clickParticlePool = new CustomPool<ParticleSystem>(clickParticle, 5, transform, false);
        arrowClickParticlePool = new CustomPool<ParticleSystem>(arrowClickParticle, 5, transform, false);
        eatParticlePool = new CustomPool<ParticleSystem>(eatParticle, 5, transform, false);
        unlockParticlePool = new CustomPool<ParticleSystem>(unlockParticle, 5, transform, false);
        unlockAdsParticlePool = new CustomPool<ParticleSystem>(unlockAdsParticle, 5, transform, false);
        specialParticlePool = new CustomPool<ParticleSystem>(specialParticle, 2, transform, false);
        chargingParticlePool = new CustomPool<ParticleSystem>(chargingParticle, 2, transform, false);
        changeColorParticlePool = new CustomPool<ParticleSystem>(changeColorParticle, 2, transform, false);
        frostExplosionPool = new CustomPool<ParticleSystem>(frostExplosion, 2, transform, false);

        if (GameManager.IsBonusGame())
        {
            nNextCube1.transform.SetParent(nParentNextCubeMini);
            nNextCube1.transform.localPosition = Vector3.zero;

            spawnBook.SetParent(nParentSpawnBookMini);
            spawnBook.transform.localPosition = Vector3.zero;

            nBgMini.gameObject.SetActive(true);
            nBgNormal.gameObject.SetActive(false);
            nStand.gameObject.SetActive(true);
            countDiff = countDiffMax;
        }
        else
        {
            nNextCube1.transform.SetParent(nParentNextCubeNormal);
            nNextCube1.transform.localPosition = Vector3.zero;

            spawnBook.SetParent(nParentSpawnBookNormal);
            spawnBook.transform.localPosition = Vector3.zero;

            nBgMini.gameObject.SetActive(false);
            nBgNormal.gameObject.SetActive(true);
            nStand.gameObject.SetActive(false);
        }

        if (SaveGame.Level == 0 && !SaveGame.IsDoneTutorial)
        {
            SaveGame.TutorialFirst = true;
            nParentNextCubeNormal.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
    }

    public void InitTutorial()
    {
        //canvasTutorial.enabled = true;
        TutorialCamera.Instance.MoveHand(1, 0);
    }


    private async Task LoadData()
    {
        await LoadDataFromAsset();

        rows = colorPlateData.rows;
        cols = colorPlateData.cols;
        ResetNDesk();
        setMapManager.Init(rows, cols, holder, ListColorPlate, colorPLatePrefab);

        if (GameManager.IsChallengesGame())
        {
            if (saveGameChallenges == null) LoadLevelChallenges();
            else LoadSaveChallengesData();
        }
        else if (GameManager.IsNormalGame())
        {
            maxPoint = colorPlateData.goalScore;
            gold = colorPlateData.gold;
            pigment = colorPlateData.pigment;

            if (saveGameNormal == null)
            {
                LoadLevelNormal();
                timeStart = DateTime.Now;
                SaveGame.TimePlayed = 0;
                timePlayed = 0;
            }
            else
            {
                LoadSaveNormalData();
                timeStart = DateTime.Now;
                timePlayed = SaveGame.TimePlayed;
            }
        }

        setMapManager.InitArrowPlates(rows, cols, ListColorPlate, nParentArrow, arrowPlatePrefab, ListArrowPlate);

        Debug.Log("SaveGame.IsDoneTutorial: " + SaveGame.IsDoneTutorial);
        //Logic Tutorial Arrow
        if (SaveGame.Level == 0 && !SaveGame.IsDoneTutorial)
        {
            for (int i = 0; i < ListArrowPlate.Count; i++)
            {
                if (i != 1)
                {
                    //Debug.Log("i: " + i);
                    ListArrowPlate[i].canClick = false;
                }
            }
        }

        IsDataLoaded = true;
    }

    private async Task LoadDataFromAsset()
    {
        string filePath = "";
        if (GameManager.IsNormalGame())
        {
            //Debug.Log("Level: " + SaveGame.Level);
            //filePath = Resources.Load<TextAsset>($"LevelData/Level_{SaveGame.Level}").ToString();

            int indexLevelNormal = 0;
            if (SaveGame.Level > GameConfig.MAX_LEVEL)
            {
                indexLevelNormal = (SaveGame.Level % GameConfig.MAX_LEVEL) + GameConfig.LOOP_START_LEVEL;
            }
            else
            {
                indexLevelNormal = SaveGame.Level;
            }


            // var ta = await ManagerAsset.LoadAssetAsync<TextAsset>($"Level_{SaveGame.Level}");
            var ta = await ManagerAsset.LoadAssetAsync<TextAsset>($"Level_{indexLevelNormal}");
            filePath = ta.text;
        }
        else if (GameManager.IsChallengesGame())
        {
            //filePath = Resources.Load<TextAsset>($"LevelData/Level_{SaveGame.LevelChallenges}").ToString();

            var ta = await ManagerAsset.LoadAssetAsync<TextAsset>($"Level_{SaveGame.LevelChallenges}");
            filePath = ta.text;
        }
        else if (GameManager.IsBonusGame())
        {
            // change file Path => Bonus Level

            //filePath = Resources.Load<TextAsset>($"LevelData/Level_{SaveGame.LevelBonus}").ToString();
            //filePath = Resources.Load<TextAsset>($"LevelData/Level_{SaveGame.LevelBonus}").ToString();

            var ta = await ManagerAsset.LoadAssetAsync<TextAsset>($"Level_{SaveGame.LevelBonus}");
            filePath = ta.text;
        }

        colorPlateData = JsonUtility.FromJson<ColorPlateData>(filePath);
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

        StartCoroutine(ArrowController.instance.LightUpArrows(ListArrowPlate));
    }

    void ResetNDesk()
    {
        //float offset = 0f;
        float offset = 1.65f;
        if (GameManager.IsBonusGame()) offset = 1.6f;

        if (cols >= rows)
        {
            float y = 0.3f * (6 - cols);
            if (cols == 6 /*&& rows == 6*/)
            {
                y += 0.2f;
            }
            testStack.transform.position = new Vector3(0, 1.2f + y - offset, 0);

            float scale = 6f / cols;
            holder.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            float y = 0.3f * (6 - rows);
            if (/*cols == 5 &&*/ rows == 6)
            {
                y += 0.2f;
            }
            testStack.transform.position = new Vector3(0, 1.2f + y - offset, 0);

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
            ListColorPlate[index].logicVisual
                .SetSpecialSquare(ListColorPlate[index].status, colorPlateData.listSpecialData[i].Row);
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
            ListColorPlate[index].logicVisual
                .SetDirectionArrow(ListColorPlate[index].status, ListColorPlate[index].isLocked);
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

            ListColorPlate[index].logicVisual
                .SetSpecialSquare(ListColorPlate[index].status, colorPlateData.listSpecialData[i].Row);
            if (ListColorPlate[index].status == Status.Frozen)
            {
                ListColorPlate[index].countFrozen = 3;
                ListColorPlate[index].Init(GetColorNew);
                ListColorPlate[index].InitColor(true);
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
            ListColorPlate[index].logicVisual
                .SetDirectionArrow(ListColorPlate[index].status, ListColorPlate[index].isLocked);
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
                    c.transform.localPosition =
                        new Vector3(5f, c.transform.localPosition.y, c.transform.localPosition.z);
                }

                foreach (LogicColor c in listNextPlate[index].ListColor)
                {
                    float randomX = UnityEngine.Random.Range(-0.5f, 0.5f);
                    c.transform.DOLocalMoveX(0, 0.3f);
                }
            });
            sequence.AppendInterval(0.2f);
        }

        specialParticlePool.Release(currentSpecialParticle);
        ManagerAudio.PlaySound(ManagerAudio.Data.soundRefresh);
    }

    public List<LogicColor> InitTutorialColorPlate(int index)
    {
        List<LogicColor> listColor = new List<LogicColor>();

        ListArrowPlate[index].Init(GetColorNew);
        ListArrowPlate[index].InitColor(false, true);

        for (int i = 0; i < ListArrowPlate[index].ListColor.Count; i++)
        {
            LogicColor logicColor = ListArrowPlate[index].ListColor[i];

            logicColor.InitTutorial();

            listColor.Add(logicColor);
        }

        ListArrowPlate[index].ListValue.Clear();
        ListArrowPlate[index].ListColor.Clear();
        ListArrowPlate[index].listTypes.Clear();

        return listColor;
    }

    public void InitNextPlate(bool tutorialFirst)
    {
        for (int i = 0; i < listNextPlate.Count; i++)
        {
            if (listNextPlate[i].ListValue.Count == 0)
            {
                listNextPlate[i].Init(GetColorNew);
                listNextPlate[i].InitColor(false, tutorialFirst);
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
    }


    RaycastHit raycastHit;
    private const float timeCanClick = 0.8f;

    private void Update()
    {
        //Debug.Log("inter watched:  " + SaveGame.CountWatchInter);

        if (!LogicGame.Instance.IsDataLoaded) return;


        if (timeClick >= 0)
        {
            timeClick -= Ez.TimeMod;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                // timeClick = .8f;
                timeClick = timeCanClick;
                Vector3 spawnPosition = GetMouseWorldPosition();
                clickParticlePool.Spawn(spawnPosition, true);

                if (gameMode == GameMode.Play)
                {
                    if (isLose || isWin) return;

                    if (Physics.Raycast(ray, out var hit, 100f, layerArrow) && !isPauseGame)
                    {
                        ColorPlate arrowPlate = hit.collider.GetComponent<ColorPlate>();

                        if (arrowPlate.isLocked || arrowPlate.ListValue.Count > 0 || !arrowPlate.canClick)
                        {
                            Debug.Log("ohnocannotclick");
                            ManagerAudio.PlaySound(ManagerAudio.Data.soundCannotClick);
                            return;
                        }

                        if (!SaveGame.IsDoneTutorial)
                        {
                            TutorialCamera.Instance.HideHandTut();
                            arrowPlate.canClick = false;
                        }

                        arrowClickParticlePool.Spawn(arrowPlate.transform.position, true);

                        ICheckStatus checkStatusHolder = new CheckGetHolderStatus();
                        ColorPlate holder = checkStatusHolder.CheckHolder(arrowPlate);

                        if (holder != null)
                        {
                            if (ControllerAnimState.gameObject.activeSelf)
                                ControllerAnimState.ActionToIdle();

                            arrowPlate.PlayAnimOnClick();
                            ManagerAudio.PlaySound(ManagerAudio.Data.soundArrowButton);
                            holder.magicRune.Play();
                            SetColor(arrowPlate, holder);

                            ArrowController.instance.PlayAnim(ListArrowPlate);

                            if (!SaveGame.IsDoneTutorial) canvasTutorial.enabled = false;
                        }

                        if (isHadSpawnSpecial)
                        {
                            Vector3 spawnPos = listNextPlate[1].transform.position;
                            listNextPlate[1].SpawnSpecialColor(GetColorNew);
                            currentSpecialParticle = specialParticlePool.Spawn(spawnPos, true);
                            isHadSpawnSpecial = false;
                        }
                    }

                    // click from spawn to start
                    if (Physics.Raycast(ray, out var hitPlate, 100f, layerPlateSpawn) && !isPauseGame)
                    {
                        ColorPlate plateSpawn = hitPlate.collider.GetComponent<ColorPlate>();
                        if (plateSpawn.ListValue.Count == 0) return;
                    }

                    // using Item Hammer
                    if (isUsingHammer)
                    {
                        if (Physics.Raycast(ray, out var plate, 100f, layerUsingItem))
                        {
                            ColorPlate plateSelect = plate.collider.GetComponent<ColorPlate>();

                            //Debug.Log(plateSelect.name);

                            if (plateSelect.ListValue.Count == 0 || plateSelect.status == Status.Frozen) return;
                            if (!SaveGame.IsDoneTutHammer)
                            {
                                //Debug.Log("?");
                                isPauseGame = false;
                                SaveGame.IsDoneTutHammer = true;
                                TutorialCamera.Instance.EndTut();
                            }

                            hammerSpine.gameObject.SetActive(true);
                            //hammerSpine.anim.transform.position = plateSelect.transform.position;
                            hammerSpine.animPen.transform.position = plateSelect.transform.position;
                            hammerSpine.PlayAnim();
                            hammerSpine.colorPlateDestroy = plateSelect;

                            Debug.Log("Use Booster At Level: " + SaveGame.Level);
                            FirebaseCustom.LogUseBoosterAtLevel(SaveGame.Level);

                            SaveGame.Hammer--;
                            isUsingHammer = false;
                            if (DailyTaskManager.Instance != null)
                                DailyTaskManager.Instance.ExecuteDailyTask(TaskType.UseBoosterHammer, 1);

                            if (DailyTaskManager.Instance != null)
                                DailyTaskManager.Instance.ExecuteDailyTask(TaskType.UseBoosters, 1);
                        }
                    }

                    if (Physics.Raycast(ray, out var hitPlateAds, 100f, layerPlate) && !isPauseGame && !isUsingHammer)
                    {
                        ColorPlate adsPlate = hitPlateAds.collider.GetComponent<ColorPlate>();

                        if (adsPlate.status != Status.Ads) return;

                        if (!AdsController.instance.IsRewardedVideoAvailable())
                        {
                            EasyUI.Toast.Toast.Show("No Ads Now", 1f);
                        }
                        else
                        {
                            AdsController.instance.ShowRewardedVideo(successful =>
                            {
                                if (successful)
                                {
                                    ParticleSystem unlockPart = unlockAdsParticlePool.Spawn();
                                    unlockPart.transform.SetParent(adsPlate.transform);
                                    unlockPart.transform.localPosition = Vector3.zero;
                                    unlockPart.transform.localScale = Vector3.one;
                                    unlockPart.Play();

                                    Debug.Log(" Watch Ads to Unlock AdsPlate");
                                    adsPlate.status = Status.None;
                                    adsPlate.logicVisual.Refresh();
                                }
                            }, null, "Reward Ads Plate");
                        }
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

        if (point >= maxPoint && !isLose && !isWin && !isContiuneMerge && GameManager.IsNormalGame())
        {
            CheckWin();
            StartCoroutine(RaiseEventWin());
        }

        ;

        for (int i = 0; i < ListArrowPlate.Count; i++)
        {
            ListArrowPlate[i].PlayAnimArrow();
        }

        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    saveGameNormal = null;
        //    PlayerPrefs.DeleteKey(GameConfig.GAMESAVENORMAL);

        //    SaveGame.Level++;
        //    ManagerEvent.ClearEvent();

        //    SceneManager.LoadScene("SceneGame");
        //}

        if (Input.GetKeyDown(KeyCode.S))
        {
            isHadSpawnSpecial = true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            isLose = true;
            StartCoroutine(RaiseEventLose());
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            CheckWin();
            StartCoroutine(RaiseEventWin());
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveGame.TutorialFirst = true;
        }
    }


    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        return cam.ScreenToWorldPoint(mousePos);
    }

    public void RefreshLayer()
    {
        for (int i = 0; i < ListColorPlate.Count; i++)
        {
            if (ListColorPlate[i].ListColor.Count == 0) continue;
            for (int j = 0; j < ListColorPlate[i].ListColor.Count; j++)
            {
                ListColorPlate[i].ListColor[j].SetLayer(ListColorPlate[i].Row);
            }
        }
    }

    public void RecursiveMerge()
    {
        if (listSteps.Count > 0)
        {
            isContiuneMerge = true;

            Merge(listSteps[listSteps.Count - 1].nearByColorPlate, listSteps[listSteps.Count - 1].rootColorPlate);
            if (DailyTaskManager.Instance != null)
                DailyTaskManager.Instance.ExecuteDailyTask(TaskType.CountMerge, 1);
        }
        else
        {
            isContiuneMerge = false;
            ArrowController.instance.PlayAnim(ListArrowPlate);

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
                ArrowController.instance.PlayAnim(ListArrowPlate);
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

            if (!SaveGame.IsDoneTutorial || TutorialCamera.Instance.isDoneStep2)
            {
                TutorialCamera.Instance.tweenTutorial.Kill();
                TutorialCamera.Instance.RefreshListColorTutorial();
                nParentNextCubeNormal.localScale = Vector3.one;
            }

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


            startColorPlate.InitValue(startColorPlate.transform, -1, endColorPlate.Row);
            listNextPlate[0].ListValue.Clear();
            listNextPlate[0].ListColor.Clear();
            listNextPlate[0].listTypes.Clear();

            foreach (LogicColor renderer in listNextPlate[1].ListColor)
            {
                Vector3 worldPos = listNextPlate[0].transform.position;
                Vector3 localPos = renderer.transform.localPosition;
                Transform newParent = listNextPlate[0].transform;

                float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);
                Vector3 targetLocalPos = new Vector3(randomX, localPos.y, localPos.z);

                renderer.transform
                    .DOJump(worldPos, 1f, 1, 0.3f)
                    .OnComplete(() =>
                    {
                        renderer.transform.SetParent(newParent, true);
                        renderer.transform.localPosition = targetLocalPos;
                        renderer.transform.localRotation = Quaternion.identity;
                        renderer.transform.localScale = Vector3.one;
                    });
            }

            listNextPlate[0].ListValue.AddRange(listNextPlate[1].ListValue);
            listNextPlate[0].ListColor.AddRange(listNextPlate[1].ListColor);
            listNextPlate[0].listTypes.AddRange(listNextPlate[1].listTypes);
            listNextPlate[0].InitValue(listNextPlate[0].transform);

            listNextPlate[1].ListValue.Clear();
            listNextPlate[1].ListColor.Clear();
            listNextPlate[1].listTypes.Clear();


            spawnBook.PlayAnimSpawn();

            float delay = 0f;

            Sequence sq = DOTween.Sequence();

            endColorPlate.isMoving = true;


            foreach (LogicColor renderer in startColorPlate.ListColor)
            {
                Vector3 localPos = renderer.transform.localPosition;
                renderer.transform.SetParent(endColorPlate.transform);

                Transform transformCache = renderer.transform;
                float randomX = UnityEngine.Random.Range(-0.05f, 0.05f);

                sq.Insert(delay, transformCache.DOLocalMove(new Vector3(randomX, localPos.y, localPos.z), 0.4f)
                        .SetEase(curveMove)
                );

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
            //Debug.Log(endColorPlate.name);

            specialParticlePool.Release(currentSpecialParticle);

            sq.OnComplete(() =>
            {
                if (!SaveGame.IsDoneTutorial && !TutorialCamera.Instance.isDoneStep2)
                {
                    TutorialCamera.Instance.MoveHand(0, 1);
                }

                endColorPlate.isMoving = false;

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


    public void SetColorUsingSwapItem(ColorPlate startColorPlate, ColorPlate endColorPlate, int currentLayer)
    {
        int changeLayer;

        if (endColorPlate.ListValue.Count == 0)
        {
            changeLayer = (GameConfig.OFFSET_LAYER - endColorPlate.Row) > 1
                ? GameConfig.OFFSET_LAYER - endColorPlate.Row
                : 1;
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

            for (int i = 0; i < endColorPlate.ListColor.Count; i++)
            {
                endColorPlate.ListColor[i].spriteRender.sortingOrder = changeLayer;
            }
        }
        else
        {
            changeLayer = endColorPlate.ListColor[0].spriteRender.sortingOrder;

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

            for (int i = 0; i < endColorPlate.ListColor.Count; i++)
            {
                endColorPlate.ListColor[i].spriteRender.sortingOrder = changeLayer;
            }

            for (int i = 0; i < startColorPlate.ListColor.Count; i++)
            {
                startColorPlate.ListColor[i].spriteRender.sortingOrder = currentLayer;
            }
        }
    }

    void ProcessRemainingPlates()
    {
        colorRoot = null;
        isMergeing = false;
        //Debug.Log(" _________________________________________ ");
        foreach (ColorPlate c in ListColorPlate)
        {
            if (c.ListValue.Count == 0 || c.status == Status.Empty || c.countFrozen != 0 || c.isMoving) continue;
            List<ColorPlate> listDataConnect = new List<ColorPlate>();
            CheckNearByRecursive(listDataConnect, c);
            if (listDataConnect.Count <= 1) continue;

            FindTarget findTarget = new FindTarget();
            if (colorRoot == null) colorRoot = findTarget.FindTargetRoot(listDataConnect);
            //Debug.Log("Root : " + colorRoot.name);

            HashSet<ColorPlate> processedNearBy = new HashSet<ColorPlate>();
            HashSet<ColorPlate> processedRoot = new HashSet<ColorPlate>();
            AddStepRecursivelyOtherRoot(colorRoot, listDataConnect, processedRoot, processedNearBy);
            break;
        }
    }

    [HideInInspector] public List<Step> listSteps = new List<Step>();

    public void AddStepRecursively(ColorPlate colorRoot, List<ColorPlate> listDataConnect,
        HashSet<ColorPlate> processedNearBy)
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

    public void AddStepRecursivelyOtherRoot(ColorPlate colorRoot, List<ColorPlate> listDataConnect,
        HashSet<ColorPlate> processedRoot, HashSet<ColorPlate> processedNearBy)
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
        List<ColorPlate> listNearBySame = colorPlate.CheckNearByCanConnect( /*colorPlate*/);

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
        int countCanClear = 0;
        List<ColorPlate> listCanClear = new List<ColorPlate>();

        foreach (ColorPlate colorPlate in ListColorPlate)
        {
            if (colorPlate.listTypes.Count <= 0 || colorPlate.status == Status.Empty) continue;
            int count = colorPlate.listTypes[colorPlate.listTypes.Count - 1].listPlates.Count;
            if (count < RULE_COMPLETE) continue;

            if (count >= RULE_COMPLETE)
            {
                countCanClear++;
                listCanClear.Add(colorPlate);

                // if (!SaveGame.IsDoneTutorial)
                // {
                //     TutorialCamera.Instance.PlayTut3();
                //     isPauseGame = true;
                // }
                //
                // if (!isPauseGame)
                // {
                //     if (ControllerAnimState.gameObject.activeSelf)
                //     {
                //         ControllerAnimState.ActionToBonus();
                //     }
                //
                //     colorPlate.InitClear(true);
                //     colorPlate.DecreaseCountFrozenNearBy();
                //     colorPlate.InitValue();
                //
                //     StartCoroutine(DelayToCheckMerge());
                // }
            }
        }

        if (countCanClear <= 0) return;

        if (!SaveGame.IsDoneTutorial)
        {
            TutorialCamera.Instance.PlayTut3();
            isPauseGame = true;
        }

        if (isPauseGame) return;

        if (ControllerAnimState.gameObject.activeSelf)
        {
            ControllerAnimState.ActionToBonus();
        }

        for (int i = 0; i < listCanClear.Count; i++)
        {
            bool isFirst = (i == 0);
            listCanClear[i].InitClear(countCanClear, true, isFirst);
            listCanClear[i].DecreaseCountFrozenNearBy();
            listCanClear[i].InitValue();
        }
        ArrowController.instance.PlayAnim(ListArrowPlate);

        StartCoroutine(DelayToCheckMerge());
    }

    IEnumerator DelayToCheckMerge()
    {
        yield return new WaitForSeconds(0.6f);
        RecursiveMerge();
    }

    public void IncreaseCountDiff()
    {
        if (GameManager.IsBonusGame()) return;

        // point config
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

    void SoundMerge()
    {
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            ManagerAudio.PlaySound(ManagerAudio.Data.soundMerge);
        }
        else
        {
            ManagerAudio.PlaySound(ManagerAudio.Data.soundMerge);
        }
    }

    void Merge(ColorPlate startColorPlate, ColorPlate endColorPlate)
    {
        timerRun = 0;
        isMergeing = true;
        //Debug.Log(startColorPlate.isMoving + "  __  " + endColorPlate.isMoving);

        if (startColorPlate.isMoving || endColorPlate.isMoving) return;
        startColorPlate.isMerging = true;
        endColorPlate.isMerging = true;

        if (startColorPlate.listTypes.Count == 0) return;

        int count = startColorPlate.listTypes[startColorPlate.listTypes.Count - 1].listPlates.Count;
        Sequence sequence = DOTween.Sequence();

        SoundMerge();
        //Debug.Log("count " + count);


        for (int i = count - 1; i >= 0; i--)
        {
            sequence.AppendCallback(() =>
            {
                //Debug.Log("111111111");
                //if (startColorPlate.TopValue == null || endColorPlate.TopValue == null) return;

                if (startColorPlate.TopValue == endColorPlate.TopValue)
                {
                    //Debug.Log("22222222");
                    startColorPlate.TopColor.transform.SetParent(endColorPlate.transform);


                    endColorPlate.listTypes[endColorPlate.listTypes.Count - 1].listPlates.Add(startColorPlate.TopValue);
                    startColorPlate.listTypes[startColorPlate.listTypes.Count - 1].listPlates
                        .Remove(startColorPlate.TopValue);

                    //if (startColorPlate.NearLastType())
                    //{
                    //    Debug.Log("fuck gan last");
                    //}

                    startColorPlate.ClearLastType();

                    endColorPlate.ListValue.Add(startColorPlate.TopValue);
                    endColorPlate.ListColor.Add(startColorPlate.TopColor);


                    startColorPlate.ListValue.RemoveAt(startColorPlate.ListValue.Count - 1);
                    startColorPlate.ListColor.RemoveAt(startColorPlate.ListColor.Count - 1);

                    if (startColorPlate.Col == endColorPlate.Col)
                    {
                        endColorPlate.InitValue(endColorPlate.transform, 1, endColorPlate.Row);
                    }
                    else if (startColorPlate.Row == endColorPlate.Row)
                    {
                        endColorPlate.InitValue(endColorPlate.transform, 0, endColorPlate.Row);
                    }
                }
            });

            sequence.AppendInterval(timerConfigData.timeMerge);
            timerRun += timerConfigData.timeRun;

            //Debug.Log("timeRun:" + timerRun);
        }


        sequence.OnComplete(() =>
        {
            //Debug.Log("fuck done");
            startColorPlate.isMerging = false;
            endColorPlate.isMerging = false;

            if (listSteps.Count > 0) listSteps.RemoveAt(listSteps.Count - 1);
        });
    }

    private void CheckLose()
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

            DeleteSaveDataGame();
            Debug.Log("You lose");
            StartCoroutine(RaiseEventLose());
            CalculateLevelLose();
        }
    }

    private void CalculateLevelLose()
    {
        if (!GameManager.IsNormalGame()) return;

        timeEnd = DateTime.Now;
        double durationPlayed = (timeEnd - timeStart).TotalSeconds;

        timePlayed += (int)Math.Round(durationPlayed);
        int time = timePlayed;
        Debug.Log("Lose Level: " + SaveGame.Level + " in " + time);
        FirebaseManager.instance.LogLevelLose(SaveGame.Level, time);
    }

    bool CheckColorPlateValue(ColorPlate colorPlateCheck)
    {
        if (colorPlateCheck.isLocked || colorPlateCheck.status == Status.CannotPlace ||
            colorPlateCheck.countFrozen != 0 || colorPlateCheck.status == Status.Ads)
        {
            return true;
        }

        return false;
    }

    IEnumerator RaiseEventLose()
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.IsNormalGame())
            ManagerEvent.RaiseEvent(EventCMD.EVENT_LOSE);
        else if (GameManager.IsChallengesGame())
            ManagerEvent.RaiseEvent(EventCMD.EVENT_CHALLENGES);
        else if (GameManager.IsBonusGame())
        {
            FirebaseCustom.LogBonusLoseSlot(SaveGame.LevelBonus);
            PopupLoseMiniGame.Show();
            Debug.Log("Raise PopupLose BonusGame");
        }
    }

    private void CheckWin()
    {
        isWin = true;
        SaveGame.IsShowBook = false;

        if (GameManager.IsNormalGame())
        {
            CalculateLogLevelWin();

            if (DailyTaskManager.Instance != null)
                DailyTaskManager.Instance.ExecuteDailyTask(TaskType.CompleteLevel, 1);
            SaveGame.Level++;

            saveGameNormal = null;
            PlayerPrefs.DeleteKey(GameConfig.GAMESAVENORMAL);

            if (PlayerPrefs.HasKey(GameConfig.GAMESAVENORMAL))
            {
                Debug.Log("van con saveGameNormal");
            }
            else
            {
                Debug.Log("clear saveGameNormal");
            }
        }

        PlayerPrefs.Save();

        foreach (ColorPlate c in ListColorPlate)
        {
            if (c.ListValue.Count == 0 || c.status == Status.Empty) continue;

            c.ClearAll();
        }
    }

    private void CalculateLogLevelWin()
    {
        timeEnd = DateTime.Now;
        double durationPlayed = (timeEnd - timeStart).TotalSeconds;

        timePlayed += (int)Math.Round(durationPlayed);
        int time = timePlayed;
        Debug.Log("Win Level: " + SaveGame.Level + " in " + time);
        FirebaseManager.instance.LogLevelWin(SaveGame.Level, time);
    }

    IEnumerator RaiseEventWin()
    {
        yield return new WaitForSeconds(GameConfig.TIME_FLY + 1f);
        ManagerEvent.RaiseEvent(EventCMD.EVENT_WIN);
    }

    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }

    public void ReviveGame()
    {
        countRevive--;

        Debug.Log("Revive at " + SaveGame.Level);
        FirebaseCustom.LogLevelRevive(SaveGame.Level);

        StartCoroutine(ClearSomeArrows());
    }

    IEnumerator ClearSomeArrows()
    {
        yield return new WaitForSeconds(0.5f);

        if (ListCheckPlate.Count >= 4)
        {
            List<ColorPlate> newListRevive = new List<ColorPlate>();
            while (newListRevive.Count < 5)
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
        //SaveGame.PlayBonus = false;
        if (!isWin)
        {
            SaveDataGame();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("Pause");
            if (!isWin)
                SaveDataGame();
        }
        else
        {
            Debug.Log("UnPause");

            timePlayed = SaveGame.TimePlayed;
            timeStart = DateTime.Now;
        }
    }

    public void DeleteSaveDataGame()
    {
        saveGameNormal = null;
        PlayerPrefs.DeleteKey(GameConfig.GAMESAVENORMAL);

        if (GameManager.IsChallengesGame())
        {
            saveGameChallenges = null;
            PlayerPrefs.DeleteKey(GameConfig.GAMESAVECHALLENGES);
        }
    }

    public void SaveDataGame()
    {
        if (GameManager.IsNormalGame())
        {
            SaveGameNormal();

            double timeSecond = (DateTime.Now - timeStart).TotalSeconds;
            int time = (int)Math.Round(timeSecond);
            SaveGame.TimePlayed = time;
        }
        else if (GameManager.IsChallengesGame())
        {
            SaveGameChallenges();
        }
    }

    private void SaveGameNormal()
    {
        if (SaveGame.Level == 0 && !SaveGame.IsDoneTutorial) return;

        SaveCurrentDataGame currentData = new SaveCurrentDataGame();

        currentData.currentPoint = point;
        currentData.countDiff = countDiff;

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

    private void SaveGameChallenges()
    {
        SaveCurrentChallenges currentData = new SaveCurrentChallenges();

        currentData.currentPoint = point;
        currentData.countDiff = countDiff;

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

    private void LoadSaveData()
    {
        if (GameManager.IsNormalGame())
        {
            string gameSaveData = PlayerPrefs.GetString(GameConfig.GAMESAVENORMAL, "");
            if (string.IsNullOrEmpty(gameSaveData))
            {
                saveGameNormal = null;
                Debug.Log("savegameNormal = null");
            }
            else
            {
                saveGameNormal = JsonUtility.FromJson<SaveCurrentDataGame>(gameSaveData);
                Debug.Log("SaveGameNormal: " + saveGameNormal);
            }
        }
        else if (GameManager.IsChallengesGame())
        {
            string gameSaveData = PlayerPrefs.GetString(GameConfig.GAMESAVECHALLENGES, "");
            if (string.IsNullOrEmpty(gameSaveData))
            {
                Debug.Log("nullll");
                saveGameChallenges = null;
            }
            else
            {
                saveGameChallenges = JsonUtility.FromJson<SaveCurrentChallenges>(gameSaveData);
            }
        }
    }

    private void LoadSaveNormalData()
    {
        //Debug.Log("saveGameNormal.currentPoint: " + saveGameNormal.currentPoint);
        point = saveGameNormal.currentPoint;
        countDiff = saveGameNormal.countDiff;

        for (int i = 0; i < saveGameNormal.ListColorPlate.Count; i++)
        {
            //Debug.Log(saveGameNormal.ListColorPlate[i].typeColorPlate + " ___ " + saveGameNormal.ListColorPlate[i].listEnum.Count);

            ListColorPlate[i].status = (Status)saveGameNormal.ListColorPlate[i].typeColorPlate;

            ListColorPlate[i].logicVisual.SetSpecialSquareExisted(ListColorPlate[i].status,
                saveGameNormal.ListColorPlate[i].countFrozen, ListColorPlate[i].Row);

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

    private void LoadSaveChallengesData()
    {
        //Debug.Log("saveGameChallenges.currentPoint: " + saveGameChallenges.currentPoint);
        point = saveGameChallenges.currentPoint;
        countDiff = saveGameChallenges.countDiff;

        for (int i = 0; i < saveGameChallenges.ListColorPlate.Count; i++)
        {
            //Debug.Log(saveGameChallenges.ListColorPlate[i].typeColorPlate + " ___ " + saveGameChallenges.ListColorPlate[i].listEnum.Count);

            ListColorPlate[i].status = (Status)saveGameChallenges.ListColorPlate[i].typeColorPlate;

            ListColorPlate[i].logicVisual.SetSpecialSquareExisted(ListColorPlate[i].status,
                saveGameChallenges.ListColorPlate[i].countFrozen, ListColorPlate[i].Row);

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


    #region Sort List

    public List<int> ListIntCurrent = new List<int>();

    public List<int> CalculateCountColorInDesk()
    {
        CalculateListCurrent();

        List<int> ListResult = new List<int>();

        Dictionary<ColorEnum, int> countInDesk = new Dictionary<ColorEnum, int>();

        for (int i = 0; i < ListIntCurrent.Count; i++)
        {
            countInDesk.Add((ColorEnum)(ListIntCurrent[i] - 1), 0);
        }

        for (int i = 0; i < ListColorPlate.Count; i++)
        {
            if (ListColorPlate[i].ListValue.Count == 0) continue;

            for (int j = 0; j < ListIntCurrent.Count; j++)
            {
                if (ListColorPlate[i].TopValue == (ColorEnum)(ListIntCurrent[j] - 1))
                {
                    //Debug.Log(i + "___" + ListColorPlate[i].TopValue + " ___ " + ListIntCurrent[j]);
                    if (countInDesk.ContainsKey(ListColorPlate[i].TopValue))
                    {
                        countInDesk[ListColorPlate[i].TopValue]++;
                    }
                }
            }
        }

        var sortedCountInDesk = countInDesk.OrderBy(pair => pair.Value);

        foreach (var obj in sortedCountInDesk)
        {
            //Debug.Log(obj.Key);
            ListResult.Add((int)obj.Key);
        }

        //for (int i = 0; i < ListResult.Count; i++)
        //{
        //    Debug.Log((ColorEnum)ListResult[i]);
        //}

        return ListResult;
    }

    void CalculateListCurrent()
    {
        for (int i = 0; i < countDiff; i++)
        {
            if (!ListIntCurrent.Contains(listIntColor[i]))
                ListIntCurrent.Add(listIntColor[i]);
        }
    }

    #endregion
}