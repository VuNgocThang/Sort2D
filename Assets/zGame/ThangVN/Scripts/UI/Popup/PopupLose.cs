using BaseGame;
using DG.Tweening;
using ntDev;
using System;
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
                        RefreshButton(false);

                        if (DailyTaskManager.Instance != null)
                            DailyTaskManager.Instance.ExecuteDailyTask(TaskType.Revive, 1);

                        SaveGame.Heart++;
                        SaveGame.Heart = Mathf.Min(SaveGame.Heart, GameConfig.MAX_HEART);

                        LogicGame.Instance.ReviveGame();
                        Hide();
                        LogicGame.Instance.isPauseGame = false;
                        LogicGame.Instance.isLose = false;
                    }
                }, null, "Reward Revive");
            }
        });

        btnRetry.OnClick(() =>
        {
            RefreshButton(false);

            PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
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

                SaveGame.CountDownTimer = countdownTimer;
            }

            if (countdownTimer <= 0 && SaveGame.Heart < GameConfig.MAX_HEART)
            {
                SaveGame.Heart++;
                SaveGame.Heart = Mathf.Min(SaveGame.Heart, GameConfig.MAX_HEART);
                SaveGame.CountDownTimer = GameConfig.TIME_COUNT_DOWN;
                countdownTimer = GameConfig.TIME_COUNT_DOWN;
                PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            }
        }
    }

    public override void Init()
    {
        base.Init();
        RefreshButton(true);

        if (LogicGame.Instance.countRevive == 0) btnRevive.gameObject.SetActive(false);
        else btnRevive.gameObject.SetActive(true);

        ManagerAudio.PlaySound(ManagerAudio.Data.soundPopupLose);
        if (SaveGame.Heart > 0)
        {
            //PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            SaveGame.Heart--;
        }

        Debug.Log("Heart: " + SaveGame.Heart);

        InitHeart();

        //float additionalTime = GameConfig.TIME_COUNT_DOWN - countdownTimer;
        //DateTime timeWithAdditionalSeconds = DateTime.Now.AddSeconds(additionalTime);
        //PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, timeWithAdditionalSeconds.ToString());

        if (SaveGame.Heart == GameConfig.MAX_HEART)
        {
            Debug.Log("Fuck");
            PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
        }

        Debug.Log("init popup lose");
    }

    private void RefreshButton(bool enabled)
    {
        btnRetry.enabled = enabled;
        btnRevive.enabled = enabled;
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
            float timeSinceLastLoss =
                (float)(DateTime.Now - DateTime.Parse(PlayerPrefs.GetString(GameConfig.LAST_HEART_LOSS))).TotalSeconds;

            int increaseHeart = (int)(timeSinceLastLoss / GameConfig.TIME_COUNT_DOWN);

            float timeSub = timeSinceLastLoss % GameConfig.TIME_COUNT_DOWN;

            if (GameConfig.MAX_HEART >= SaveGame.Heart)
            {
                SaveGame.Heart += increaseHeart;
                SaveGame.Heart = Mathf.Min(SaveGame.Heart, GameConfig.MAX_HEART);
            }

            Debug.Log("timeSinceLastLossLOSE: " + timeSinceLastLoss);
            Debug.Log("timeSubLOSE: " + timeSub);
            Debug.Log("SaveGameLOSE: " + SaveGame.CountDownTimer);

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
            PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
        }
        //}

        txtHeart.text = SaveGame.Heart.ToString();
    }
}