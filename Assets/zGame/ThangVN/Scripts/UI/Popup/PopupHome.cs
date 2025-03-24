using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using BaseGame;
using ThangVN;
using System;

public class PopupHome : MonoBehaviour
{
    public static PopupHome Instance;

    [SerializeField] Camera cam;
    public EasyButton btnSetting, btnCloseItem/*, btnOpenTool*/;

    public TextMeshProUGUI txtPoint,
        txtLevel,
        txtCount,
        txtCurrentScore,
        txtBestScore,
        txtTargetPigment,
        txtLevelInTarget;

    public Image imgFill;
    public GameObject itemObj, nLevel, nBar, nScoreChallenges, nTargetPigment, nChallenges;
    [SerializeField] Animator animBtnSwitch;
    [SerializeField] GameObject imgSpecial, top, nSpawn;

    public Image iconItem, imgTextName;
    [SerializeField] TextMeshProUGUI txtNameBooster, txtExplain;
    [SerializeField] HandDrag handDrag;
    public ButtonBoosterHammer btnHammer;
    public ButtonBoosterRefresh btnRefresh;
    public ButtonBoosterSwap btnSwap;
    [SerializeField] BoosterData boosterData;
    [SerializeField] Animator animBar, animPigment, animChallenges;
    [SerializeField] RectTransform rectTransformTarget, rectTransformChallenges;
    [SerializeField] Transform iconFake, iconTargetPigment, txtFake, txtChallengesObj;
    public GameObject imgDanger;
    public GameObject UiEffect;
    public GameObject UiEffect2;
    //public GameObject tool;

    [SerializeField] Transform nParentBtnMiniGame, nParentNormal, nManagerMission, nButtons, nTopBar;

    private void Awake()
    {
        Instance = this;

        btnSetting.OnClick(() => { PopupSetting.Show(); });
        btnCloseItem.OnClick(ExitUsingItem);
        //btnOpenTool.OnClick(() => tool.SetActive(true));

        btnHammer.Init();
        btnSwap.Init();
        btnRefresh.Init();

        btnRefresh.button.OnClick(() =>
        {
            TutorialCamera.Instance.CloseTutorialBooster();
            if (SaveGame.Level >= btnRefresh.indexLevelUnlock)
            {
                if (SaveGame.Refresh > 0)
                {
                    SaveGame.Refresh--;
                    Debug.Log("Use Booster At Level: " + SaveGame.Level);
                    FirebaseCustom.LogUseBoosterAtLevel(SaveGame.Level);

                    if (DailyTaskManager.Instance != null)
                        DailyTaskManager.Instance.ExecuteDailyTask(TaskType.UseBoosterRefresh, 1);

                    if (DailyTaskManager.Instance != null)
                        DailyTaskManager.Instance.ExecuteDailyTask(TaskType.UseBoosters, 1);
                    ShuffleRandomColorSpawn();
                }
                else PopupShopInGame.Show((int)BoosterEnum.BoosterRefresh);
            }
        });

        btnHammer.button.OnClick(() =>
        {
            TutorialCamera.Instance.CloseTutorialBooster();
            if (SaveGame.Level == GameConfig.LEVEL_HAMMER && !SaveGame.IsDoneTutHammer)
                TutorialCamera.Instance.TutorialHammer();

            if (SaveGame.Level >= btnHammer.indexLevelUnlock)
            {
                if (SaveGame.Hammer > 0)
                {
                    UsingItemHammer();
                }
                else PopupShopInGame.Show((int)BoosterEnum.BoosterHammer);
            }
        });

        btnSwap.button.OnClick(() =>
        {
            TutorialCamera.Instance.CloseTutorialBooster();
            if (SaveGame.Level == GameConfig.LEVEL_SWAP && !SaveGame.IsDoneTutSwap)
                TutorialCamera.Instance.TutorialSwap();

            if (SaveGame.Level >= btnSwap.indexLevelUnlock)
            {
                if (SaveGame.Swap > 0)
                {
                    UsingItemSwap();
                }
                else PopupShopInGame.Show((int)BoosterEnum.BoosterSwap);
            }
        });

        ManagerEvent.RegEvent(EventCMD.EVENT_POINT, UpdatePoint);
        ManagerEvent.RegEvent(EventCMD.EVENT_WIN, ShowPopupWin);
        ManagerEvent.RegEvent(EventCMD.EVENT_LOSE, ShowPopupLose);
        ManagerEvent.RegEvent(EventCMD.EVENT_COUNT, UpdateCount);
        ManagerEvent.RegEvent(EventCMD.EVENT_CHALLENGES, ShowPopupEndChallenges);
    }

    void Start()
    {
        btnSetting.gameObject.SetActive(SaveGame.Level > 0);

        if (SaveGame.Music) ManagerAudio.PlayMusic(ManagerAudio.Data.musicInGame);
        else ManagerAudio.PauseMusic();

        txtLevel.text = $"Level {SaveGame.Level + 1}";

        btnHammer.Init();
        btnSwap.Init();
        btnRefresh.Init();

        if (GameManager.IsNormalGame())
        {
            nLevel.SetActive(true);
            nScoreChallenges.SetActive(false);
            SetParentNButton(nParentNormal);
            StartCoroutine(ShowTarget());
        }
        else if (GameManager.IsChallengesGame())
        {
            nLevel.SetActive(false);
            nBar.SetActive(false);
            SetParentNButton(nParentNormal);
            //nScoreChallenges.SetActive(true);
            StartCoroutine(ShowText());
        }
        else if (GameManager.IsBonusGame())
        {
            nLevel.SetActive(false);
            nBar.SetActive(false);
            SetParentNButton(nParentBtnMiniGame);
            nManagerMission.gameObject.SetActive(true);
        }

        iconFake.DOMove(rectTransformTarget.position, 1f);
        // Reach Level Show Popup UnlockBooster
        //PopupUnlockBooster.Show(index);
    }

    void SetParentNButton(Transform nParent)
    {
        nButtons.SetParent(nParent);
        nButtons.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void Update()
    {
        if (!LogicGame.Instance.IsDataLoaded) return;

        if (GameManager.IsNormalGame())
        {
            imgFill.fillAmount = (float)LogicGame.Instance.point / (float)LogicGame.Instance.maxPoint;
            txtPoint.text =
                $"<color=#E3382F>{LogicGame.Instance.point}</color><color=#3A2B74>/{LogicGame.Instance.maxPoint} </color>";
        }
        else if (GameManager.IsChallengesGame())
        {
            txtCurrentScore.text = $"{LogicGame.Instance.point}";

            if (LogicGame.Instance.point >= SaveGame.BestScore) SaveGame.BestScore = LogicGame.Instance.point;
            txtBestScore.text = $"Best score: {SaveGame.BestScore}";
        }


        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    SaveGame.Level++;
        //}

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    ShuffleRandomColorSpawn();
        //}
    }

    public void ResetNBar()
    {
        nBar.transform.SetParent(nTopBar);
    }

    void UpdatePoint(object e)
    {
        if (animBar != null && nBar.activeSelf)
            animBar.Play("AnimBar", 0, 0);

        LogicGame.Instance.point += (int)e;

        if (GameManager.IsNormalGame())
        {
            if (LogicGame.Instance.point >= LogicGame.Instance.maxPoint)
                LogicGame.Instance.point = LogicGame.Instance.maxPoint;

            if (imgFill != null)
                imgFill.fillAmount = (float)LogicGame.Instance.point / (float)LogicGame.Instance.maxPoint;
            txtPoint.text =
                $"<color=#E3382F>{LogicGame.Instance.point} </color>/ <color=#3A2B74>{LogicGame.Instance.maxPoint} </color>";
        }
        else if (GameManager.IsChallengesGame())
        {
            txtCurrentScore.text = $"{LogicGame.Instance.point}";

            if (LogicGame.Instance.point >= SaveGame.BestScore) SaveGame.BestScore = LogicGame.Instance.point;
            txtBestScore.text = $"Best score: {SaveGame.BestScore}";
        }
    }

    void UpdateCount(object e)
    {
        txtCount.text = $"{e}";
    }

    void ShowPopupWin(object e)
    {
        PopupWin.Show();
    }

    void ShowPopupLose(object e)
    {
        PopupLose.Show();
    }

    void ShowPopupEndChallenges(object e)
    {
        PopupEndChallenges.Show();
    }

    public IEnumerator PlayEffectSpecial()
    {
        imgSpecial.SetActive(true);
        yield return new WaitForSeconds(1f);
        imgSpecial.SetActive(false);
    }

    void ShuffleRandomColorSpawn()
    {
        //LogicGame.Instance.InitPlateSpawn(true);
        LogicGame.Instance.ShufflePlateSpawn();
    }

    void UsingItemHammer()
    {
        itemObj.SetActive(true);
        top.SetActive(false);
        //bot.SetActive(false);
        nSpawn.SetActive(false);
        LogicGame.Instance.isPauseGame = true;

        CameraChange.Ins.ChangeCameraUsingItem();

        LogicGame.Instance.isUsingHammer = true;
        SetupImgItem((int)BoosterEnum.BoosterHammer);
        Debug.Log("use hammer");
    }


    void UsingItemSwap()
    {
        itemObj.SetActive(true);
        top.SetActive(false);
        //bot.SetActive(false);
        nSpawn.SetActive(false);
        LogicGame.Instance.isPauseGame = true;

        CameraChange.Ins.ChangeCameraUsingItem();

        LogicGame.Instance.isUsingHand = true;
        SetupImgItem((int)BoosterEnum.BoosterSwap);

        Debug.Log("use swap");
    }

    public void ExitUsingItem()
    {
        if (handDrag.isDrag) return;

        if (SaveGame.Level == GameConfig.LEVEL_HAMMER && !SaveGame.IsDoneTutHammer) return;
        if (SaveGame.Level == GameConfig.LEVEL_SWAP && !SaveGame.IsDoneTutSwap) return;

        ResetPositionAfterTutorial();
        handDrag.selectingPlate = null;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        CameraChange.Ins.ExitUsingItemCamera();
        LogicGame.Instance.isPauseGame = false;
        LogicGame.Instance.isUsingHammer = false;
        LogicGame.Instance.isUsingHand = false;

        itemObj.SetActive(false);
        top.SetActive(true);
        //bot.SetActive(true);
        nSpawn.SetActive(true);
        LogicGame.Instance.RefreshLayer();
        LogicGame.Instance.RecursiveMerge();
    }

    void SetupImgItem(int index)
    {
        for (int i = 0; i < boosterData.listBooster.Count; i++)
        {
            if (index == (int)boosterData.listBooster[i].boosterEnum)
            {
                imgTextName.sprite = boosterData.listBooster[i].spriteText;
                iconItem.sprite = boosterData.listBooster[i].spriteIcon;
                txtNameBooster.text = boosterData.listBooster[i].nameBooster;
                txtExplain.text = boosterData.listBooster[i].textExplain;
            }
        }
    }

    IEnumerator ShowTarget()
    {
        LogicGame.Instance.isPauseGame = true;
        yield return null;

        while (!LogicGame.Instance.IsDataLoaded)
        {
            yield return null;
        }

        nTargetPigment.SetActive(true);
        txtLevelInTarget.text = $"Level {SaveGame.Level + 1}";
        txtTargetPigment.text = LogicGame.Instance.pigment.ToString();
        animPigment.Play("Show");

        yield return new WaitForSeconds(1f);

        Vector3 targetWorldPosition = cam.ScreenToWorldPoint(rectTransformTarget.position);
        targetWorldPosition.z = 0;

        iconFake.gameObject.SetActive(true);
        iconFake.position = iconTargetPigment.position;
        iconTargetPigment.gameObject.SetActive(false);

        iconFake.transform.DOMove(rectTransformTarget.position, 0.3f)
            .OnComplete(() =>
            {
                iconFake.gameObject.SetActive(false);
                nBar.SetActive(true);
                nTargetPigment.SetActive(false);
                TutorialManager.ShowPopup(SaveGame.Level);
            });
    }


    IEnumerator ShowText()
    {
        LogicGame.Instance.isPauseGame = true;
        yield return null;

        while (!LogicGame.Instance.IsDataLoaded)
        {
            yield return null;
        }

        nChallenges.SetActive(true);
        if (nChallenges.activeSelf)
            animChallenges.Play("Show");

        yield return new WaitForSeconds(1f);

        Vector3 targetWorldPosition = cam.ScreenToWorldPoint(rectTransformChallenges.position);
        targetWorldPosition.z = 0;

        txtFake.gameObject.SetActive(true);
        txtFake.position = txtChallengesObj.position;
        txtChallengesObj.gameObject.SetActive(false);

        txtFake.transform.DOMove(rectTransformChallenges.position, 0.3f)
            .OnComplete(() =>
            {
                txtFake.gameObject.SetActive(false);
                nScoreChallenges.SetActive(true);
                nChallenges.SetActive(false);
                LogicGame.Instance.isPauseGame = false;
                //ManagerEvent.RaiseEvent(EventCMD.EVENT_SPAWN_PLATE);
            });
    }

    public void ResetPositionAfterTutorial()
    {
        btnSwap.transform.SetParent(nButtons);
        btnHammer.transform.SetParent(nButtons);
        btnRefresh.transform.SetParent(nButtons);
    }

    private void OnApplicationQuit()
    {
        if (SaveGame.Heart <= GameConfig.MAX_HEART)
        {
            PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            PlayerPrefs.Save();
        }
    }
}