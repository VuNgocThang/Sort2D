using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.SceneManagement;
using BaseGame;
using TMPro;
using System;
using UnityEngine.UI;
using EasyUI.Helpers;

public class HomeUI : MonoBehaviour
{
    public static HomeUI Instance;

    public EasyButton btnSetting,
        btnPlusCoin,
        btnPlusColorPlate,
        btnFreeCoin,
        btnChallenges,
        btnDecor,
        btnPlay,
        btnDailyTask,
        btnShop,
        btnCloseShop,
        btnNoAdsBundle;

    public TextMeshProUGUI txtCoin, txtHeart, txtCountdownHeart, txtColor, txtLevel, txtProgressTask;
    [SerializeField] int heart;
    [SerializeField] float countdownTimer, totalParts, currentParts;

    public GameObject nTop,
        nBot,
        iconNotice,
        nParent,
        nPanelShop,
        nNoticeFreecoin,
        nNoticeDailyTask,
        nNoticeTask,
        imgGrayDecor;

    public Animator animator;
    public List<Sprite> listSprite;
    public Image bg, imgProgressTask;
    [SerializeField] DataConfigDecor bookDataConfig;
    [SerializeField] ListBookDecorated listBook;
    [SerializeField] DataClaimedFreecoin dataFreeCoinClaimed;

    const int bigSize = 60;
    const int minSize = 40;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);

        btnSetting.OnClick(() => PopupSettingHome.Show());

        btnPlusCoin.OnClick(() => PopupFreeCoin.Show());

        btnPlusColorPlate.OnClick(() => PopupGoToLevel.Show());

        btnFreeCoin.OnClick(() => PopupFreeCoin.Show());

        btnChallenges.OnClick(() => PopupEndless.Show());

        btnDecor.OnClick(() =>
        {
            if (SaveGame.Level < 2)
            {
                return;
                EasyUI.Toast.Toast.Show("Unlock at level 3", 1f);
            }
            else
            {
                if (SaveGame.Level >= 2)
                {
                    SaveGame.FirstDecor = false;
                    btnDecor.transform.SetParent(nBot.transform);
                }

                PopupDecor.Show();
            }
        });

        btnPlay.OnClick(() =>
        {
            if (SaveGame.Heart <= 0)
            {
                PopupOutOfHeartHome.Show();
                //EasyUI.Toast.Toast.Show("Not enough heart!", 1f);
            }
            else
            {
                SaveGame.Challenges = false;
                if (SaveGame.PlayBonus)
                    PopupBonusLevel.Show();
                else
                {
                    ManagerEvent.ClearEvent();
                    StartCoroutine(LoadScene("SceneGame"));
                }
            }
        });

        btnDailyTask.OnClick(() =>
        {
            PopupDailyTask.Show();
            Debug.Log("show popup daily task");
        });

        btnShop.OnClick(() =>
        {
            nParent.SetActive(false);
            nPanelShop.SetActive(true);
        });

        btnCloseShop.OnClick(() =>
        {
            nParent.SetActive(true);
            nPanelShop.SetActive(false);
        });

        btnNoAdsBundle.OnClick(() => { PopupNoAdsBundle.Show(); });
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        // DailyTaskManager.Instance.Init();
        int randomBG = UnityEngine.Random.Range(0, 2);
        bg.sprite = listSprite[randomBG];

        SaveGame.Challenges = false;
        animator.Play("Show");
        if (SaveGame.Music)
        {
            //Debug.Log("Play music");
            ManagerAudio.PlayMusic(ManagerAudio.Data.musicBG);
        }
        else ManagerAudio.PauseMusic();

        InitGrayDecor();

        InitHeart();

        InitFirstDecor();

        InitButtonInHome();

        InitDataClaimedFreecoin();
        // Debug.Log("BonusLevel: " + SaveGame.LevelBonus);
    }

    private void Update()
    {
        txtCoin.text = SaveGame.Coin.ToString();
        txtHeart.text = SaveGame.Heart.ToString();
        txtColor.text = SaveGame.Pigment.ToString();
        txtLevel.text = $"Level {SaveGame.Level + 1}";

        CalculateTask();

        CalculateHeart();

        int count = CheckNoticeFreecoin();
        if (count == 6) nNoticeFreecoin.SetActive(false);
        else nNoticeFreecoin.SetActive(true);

        bool checkDailyTask = CheckNoticeDailyTask();
        if (checkDailyTask) nNoticeDailyTask.SetActive(true);
        else nNoticeDailyTask.SetActive(false);

        if (CheckTaskDecor()) nNoticeTask.SetActive(true);
        else nNoticeTask.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PopupReward1.Show();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            PopupReward2.Show();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PopupReward3.Show();
        }
    }

    private void CalculateTask()
    {
        totalParts = bookDataConfig.listDataBooks[bookDataConfig.listDataBooks.Count - 1].totalParts;

        listBook = SaveGame.ListBookDecorated;

        int count = 0;
        for (int i = 0;
             i < listBook.listBookDecorated[listBook.listBookDecorated.Count - 1].listItemDecorated.Count;
             i++)
        {
            if (listBook.listBookDecorated[listBook.listBookDecorated.Count - 1].listItemDecorated[i]
                .isPainted) count++;
        }

        currentParts = count;
        //if (!listBook.listBookDecorated[listBook.listBookDecorated.Count - 1].isPainted) currentParts++;

        imgProgressTask.fillAmount = currentParts / totalParts;
        txtProgressTask.text = $"{currentParts}/{totalParts}";
    }

    private void CalculateHeart()
    {
        if (SaveGame.Heart >= GameConfig.MAX_HEART)
        {
            txtCountdownHeart.text = "FULL";
            txtHeart.fontSize = bigSize;
        }
        else
        {
            if (countdownTimer > 0)
            {
                countdownTimer -= Time.deltaTime;

                float minutes = Mathf.Floor(countdownTimer / 60);
                float seconds = Mathf.RoundToInt(countdownTimer % 60);

                txtCountdownHeart.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            }

            if (countdownTimer <= 0 && SaveGame.Heart < GameConfig.MAX_HEART)
            {
                SaveGame.Heart++;
                SaveGame.Heart = Mathf.Min(SaveGame.Heart, GameConfig.MAX_HEART);
                countdownTimer = GameConfig.TIME_COUNT_DOWN;
                PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            }

            txtHeart.fontSize = minSize;
        }
    }

    async void InitButtonInHome()
    {
        if (SaveGame.Level < 5)
        {
            btnChallenges.gameObject.SetActive(false);
        }
        else
        {
            btnChallenges.gameObject.SetActive(true);

            if (!SaveGame.IsTutChallenges && SaveGame.Level >= GameConfig.LEVEL_CHALLENGES)
            {
                Debug.Log("b");
                bool b = await PopupEndless.Show();
            }
        }

        if (SaveGame.Level < GameConfig.LEVEL_DAILY_TASK)
        {
            btnDailyTask.gameObject.SetActive(false);
        }
        else
        {
            btnDailyTask.gameObject.SetActive(true);

            if (!SaveGame.IsTutDailyTask)
            {
                Debug.Log("c");
                bool b = await PopupDailyTask.Show();
            }
        }

        if (SaveGame.Level < GameConfig.LEVEL_FREE_COIN)
        {
            btnFreeCoin.gameObject.SetActive(false);
        }
        else
        {
            btnFreeCoin.gameObject.SetActive(true);

            if (!SaveGame.IsTutFreeCoin || !SaveGame.ShowFreeCoin)
            {
                Debug.Log("a");
                bool b = await PopupFreeCoin.Show();
            }
        }
    }

    void InitGrayDecor()
    {
        if (SaveGame.Level < 2)
            imgGrayDecor.SetActive(true);
        else
            imgGrayDecor.SetActive(false);
    }

    private void InitHeart()
    {
        //if (SaveGame.CountDownTimer > 0)
        //{
        //Debug.Log(SaveGame.CountDownTimer + " countDownTimer");

        if (PlayerPrefs.HasKey(GameConfig.LAST_HEART_LOSS))
        {
            float timeSinceLastLoss =
                (float)(DateTime.Now - DateTime.Parse(PlayerPrefs.GetString(GameConfig.LAST_HEART_LOSS))).TotalSeconds;

            int increaseHeart = (int)(timeSinceLastLoss / GameConfig.TIME_COUNT_DOWN);

            float timeSub = timeSinceLastLoss % GameConfig.TIME_COUNT_DOWN;


            if (GameConfig.MAX_HEART >= SaveGame.Heart)
            {
                // Debug.Log("Heart_Before:" + SaveGame.Heart);
                SaveGame.Heart += increaseHeart;
                // Debug.Log("Heart_AfterAdd:" + SaveGame.Heart);
                SaveGame.Heart = Mathf.Min(SaveGame.Heart, GameConfig.MAX_HEART);
                // Debug.Log("Heart_After:" + SaveGame.Heart);
            }

            //Debug.Log("timeSinceLastLoss: " + timeSinceLastLoss);
            //Debug.Log("timeSub: " + timeSub);
            //Debug.Log("SaveGame: " + SaveGame.CountDownTimer);
            countdownTimer = SaveGame.CountDownTimer - timeSub;
            countdownTimer = Mathf.Max(countdownTimer, 0);

            if (SaveGame.Heart >= GameConfig.MAX_HEART)
            {
                countdownTimer = GameConfig.TIME_COUNT_DOWN;
            }
        }
        else
        {
            countdownTimer = GameConfig.TIME_COUNT_DOWN;
        }
        //}
    }

    IEnumerator LoadScene(string str)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(str);
    }

    void InitFirstDecor()
    {
        if (SaveGame.Level >= 2 && SaveGame.FirstDecor)
        {
            //TutorialDecor.Instance.InitTut();
            TutorialDecor.Instance.SetParent(btnDecor.transform);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame.CountDownTimer = countdownTimer;

        if (SaveGame.Heart <= GameConfig.MAX_HEART)
        {
            PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            PlayerPrefs.Save();
        }
    }

    private void OnDisable()
    {
        if (SaveGame.Heart <= GameConfig.MAX_HEART)
        {
            SaveGame.CountDownTimer = countdownTimer;
            PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            PlayerPrefs.Save();
        }
    }

    //public void DisableObject()
    //{
    //    if (!SaveGame.FirstDecor && SaveGame.Level >= 2)
    //    {
    //        iconNotice.SetActive(false);
    //    }
    //}

    #region Notice Freecoins

    void InitDataClaimedFreecoin()
    {
        dataFreeCoinClaimed = SaveGame.DataFreeCoin;
    }

    int CheckNoticeFreecoin()
    {
        int count = 0;
        if (dataFreeCoinClaimed.isClaimed50) count++;

        for (int i = 0; i < dataFreeCoinClaimed.listDataFreeCoin.Count; i++)
        {
            if (dataFreeCoinClaimed.listDataFreeCoin[i].isClaimed) count++;
        }

        return count;
    }

    #endregion

    #region Notice DailyTask

    bool CheckNoticeDailyTask()
    {
        return DailyTaskManager.Instance.CheckNotice();
    }

    #endregion

    #region Notice Task Decore

    bool CheckTaskDecor()
    {
        bool canDecor = false;

        int currentBookIndex = SaveGame.MaxCurrentBook;

        int minCost = 10000;

        for (int i = 0; i < bookDataConfig.listDataBooks[currentBookIndex].listDataItemDecor.Count; i++)
        {
            DataItemDecor dataItemDecor = bookDataConfig.listDataBooks[currentBookIndex].listDataItemDecor[i];

            if (listBook.listBookDecorated[currentBookIndex].listItemDecorated.Count > 0)
            {
                for (int j = 0; j < listBook.listBookDecorated[currentBookIndex].listItemDecorated.Count; j++)
                {
                    ItemDecorated itemDecorated = listBook.listBookDecorated[currentBookIndex].listItemDecorated[j];

                    if (itemDecorated.idItemDecorated == dataItemDecor.idItemDecor) continue;

                    if (dataItemDecor.cost <= minCost)
                        minCost = dataItemDecor.cost;
                }
            }
            else
            {
                if (dataItemDecor.cost <= minCost)
                    minCost = dataItemDecor.cost;
            }
        }

        if (SaveGame.Pigment >= minCost) canDecor = true;

        return canDecor;
    }

    bool CheckListDecor()
    {
        bool canShow = false;

        return canShow;
    }

    #endregion


    public bool IsComingSoon()
    {
        bool isComingSoon = false;
        int indexLastBook = bookDataConfig.listDataBooks.Count - 1;

        int count = 0;
        for (int i = 0; i < listBook.listBookDecorated[indexLastBook].listItemDecorated.Count; i++)
        {
            if (listBook.listBookDecorated[indexLastBook].listItemDecorated[i].isPainted) count++;
        }

        if (listBook.listBookDecorated[indexLastBook].colorPainted != GameConfig.DEFAULT_COLOR) count++;

        if (count == bookDataConfig.listDataBooks[indexLastBook].totalParts) return true;

        return isComingSoon;
    }
}