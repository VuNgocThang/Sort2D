using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.SceneManagement;
using BaseGame;
using TMPro;
using System;

public class HomeUI : MonoBehaviour
{
    public static HomeUI Instance;
    public EasyButton btnSetting, btnPlusCoin, btnPlusColorPlate, btnFreeCoin, btnChallenges, btnDecor, btnPlay;
    public TextMeshProUGUI txtCoin, txtHeart, txtCountdownHeart, txtColor;
    [SerializeField] int heart;
    [SerializeField] float countdownTimer;
    public GameObject nTop, nBot, iconNotice;
    public Animator animator;
    public Animator animUsePigment;

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
            if (SaveGame.Level >= 3) SaveGame.FirstDecor = false;

            PopupDecor.Show();
        });

        btnPlay.OnClick(() =>
        {
            SaveGame.Challenges = false;
            ManagerEvent.ClearEvent();
            StartCoroutine(LoadScene("SceneGame"));
        });
    }

    private void Start()
    {
        SaveGame.Challenges = false;
        animator.Play("Show");
        if (SaveGame.Music) ManagerAudio.PlayMusic(ManagerAudio.Data.musicBG);
        else ManagerAudio.PauseMusic();

        InitHeart();

        InitFirstDecor();
    }

    private void Update()
    {
        txtCoin.text = SaveGame.Coin.ToString();
        txtHeart.text = SaveGame.Heart.ToString();
        txtColor.text = SaveGame.Pigment.ToString();

        if (Input.GetKeyDown(KeyCode.M))
        {
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
