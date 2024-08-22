using BaseGame;
using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using ThangVN;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupLose : Popup
{
    public TextMeshProUGUI txtHeart, txtCountdownHeart;
    public EasyButton btnRevive, btnRetry;
    [SerializeField] float countdownTimer;

    public static async void Show()
    {
        PopupLose pop = await ManagerPopup.ShowPopup<PopupLose>();
        pop.Init();
    }

    private void Awake()
    {
        btnRevive.OnClick(() =>
        {
            //if (SaveGame.Heart > 0)
            //{
            //    PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            //    SaveGame.Heart--;
            LogicGame.Instance.ReviveGame();
            Hide();
            LogicGame.Instance.isPauseGame = false;
            LogicGame.Instance.isLose = false;
            //LoadScene("SceneGame");
            //}
            //else
            //{
            //    LoadScene("SceneHome");
            //}
        });

        btnRetry.OnClick(() =>
        {
            PopupRestart.Show();
        });
    }
    private void Update()
    {
        txtHeart.text = SaveGame.Heart.ToString();

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

    public override void Init()
    {
        base.Init();
        ManagerAudio.PlaySound(ManagerAudio.Data.soundPopupLose);
        InitHeart();
        Debug.Log("init popup lose");
    }

    public override void Hide()
    {
        base.Hide();
    }

    void LoadScene(string strScene)
    {
        transform.localScale = Vector3.one;

        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
            ManagerEvent.ClearEvent();
            SceneManager.LoadScene(strScene);
        });
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
}
