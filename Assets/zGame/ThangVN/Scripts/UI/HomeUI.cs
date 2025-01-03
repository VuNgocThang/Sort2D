using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.SceneManagement;
using BaseGame;
using TMPro;
using System;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    public static HomeUI Instance;
    public EasyButton btnSetting, btnPlusCoin, btnPlusColorPlate, btnFreeCoin, btnChallenges, btnDecor, btnPlay, btnDailyTask;
    public TextMeshProUGUI txtCoin, txtHeart, txtCountdownHeart, txtColor, txtLevel, txtProgressTask;
    [SerializeField] int heart;
    [SerializeField] float countdownTimer, totalParts, currentParts;
    public GameObject nTop, nBot, iconNotice;
    public Animator animator;
    public List<Sprite> listSprite;
    public Image bg, imgProgressTask;
    [SerializeField] DataConfigDecor bookDataConfig;
    [SerializeField] ListBookDecorated listBook;

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
            //if (SaveGame.Level >= 3) SaveGame.FirstDecor = false;

            PopupDecor.Show();
        });

        btnPlay.OnClick(() =>
        {
            SaveGame.Challenges = false;
            if (SaveGame.PlayBonus)
                PopupBonusLevel.Show();
            else
            {
                ManagerEvent.ClearEvent();
                StartCoroutine(LoadScene("SceneGame"));
            }
        });

        btnDailyTask.OnClick(() =>
        {
            PopupDailyTask.Show();
            Debug.Log("show popup daily task");
        });
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

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

        InitHeart();

        InitFirstDecor();
    }

    private void Update()
    {
        txtCoin.text = SaveGame.Coin.ToString();
        txtHeart.text = SaveGame.Heart.ToString();
        txtColor.text = SaveGame.Pigment.ToString();
        txtLevel.text = $"Level {SaveGame.Level + 1}";

        totalParts = bookDataConfig.listDataBooks[bookDataConfig.listDataBooks.Count - 1].totalParts;

        listBook = SaveGame.ListBookDecorated;

        int count = 0;
        for (int i = 0; i < listBook.listBookDecorated[listBook.listBookDecorated.Count - 1].listItemDecorated.Count; i++)
        {
            if (listBook.listBookDecorated[listBook.listBookDecorated.Count - 1].listItemDecorated[i].isPainted) count++;
        }
        currentParts = count;
        //if (!listBook.listBookDecorated[listBook.listBookDecorated.Count - 1].isPainted) currentParts++;

        imgProgressTask.fillAmount = currentParts / totalParts;
        txtProgressTask.text = $"{currentParts}/{totalParts}";
        if (Input.GetKeyDown(KeyCode.M))
        {
            DateTime timer = DateTime.Now + TimeSpan.FromSeconds(90f);
            Debug.Log("Test DataTime.Now: " + DateTime.Now);
            Debug.Log("timer: " + timer);

            if (SaveGame.Heart > 0)
            {
                SaveGame.Heart--;
                PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            }
        }

        if (SaveGame.Heart >= GameConfig.MAX_HEART)
        {
            txtCountdownHeart.text = "FULL";
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
                countdownTimer = GameConfig.TIME_COUNT_DOWN;
                PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            }
        }
    }

    private void InitHeart()
    {
        //if (SaveGame.CountDownTimer > 0)
        //{
        //Debug.Log(SaveGame.CountDownTimer + " countDownTimer");

        if (PlayerPrefs.HasKey(GameConfig.LAST_HEART_LOSS))
        {
            float timeSinceLastLoss = (float)(DateTime.Now - DateTime.Parse(PlayerPrefs.GetString(GameConfig.LAST_HEART_LOSS))).TotalSeconds;

            int increaseHeart = (int)(timeSinceLastLoss / GameConfig.TIME_COUNT_DOWN);

            float timeSub = timeSinceLastLoss % GameConfig.TIME_COUNT_DOWN;

            if (GameConfig.MAX_HEART >= SaveGame.Heart)
            {
                SaveGame.Heart += increaseHeart;
                SaveGame.Heart = Mathf.Min(SaveGame.Heart, GameConfig.MAX_HEART);
            }
            countdownTimer = SaveGame.CountDownTimer - timeSub;
            countdownTimer = Mathf.Max(countdownTimer, 0);

            if (SaveGame.Heart >= GameConfig.MAX_HEART)
            {
                countdownTimer = GameConfig.TIME_COUNT_DOWN;
            }

            Debug.Log(timeSinceLastLoss);
        }
        else
        {
            countdownTimer = GameConfig.TIME_COUNT_DOWN;
        }
        //}

        txtHeart.text = SaveGame.Heart.ToString();
    }

    IEnumerator LoadScene(string str)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(str);

    }

    void InitFirstDecor()
    {
        if (SaveGame.Pigment >= 300 && SaveGame.FirstDecor)
        {
            iconNotice.SetActive(true);
            SaveGame.FirstDecor = false;
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

    public void DisableObject()
    {
        if (!SaveGame.FirstDecor && SaveGame.Level >= 2)
        {
            iconNotice.SetActive(false);
        }
    }
}
