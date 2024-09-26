using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThangVN
{
    public class PopupOutOfHeart : Popup
    {
        [SerializeField] TextMeshProUGUI txtCountdownHeart, txtHeart;
        [SerializeField] EasyButton btnHome, btnBuy;
        [SerializeField] float countdownTimer;

        private void Awake()
        {
            btnBuy.OnClick(BuyHeart);
        }
        public static async void Show()
        {
            PopupOutOfHeart pop = await ManagerPopup.ShowPopup<PopupOutOfHeart>();
            pop.Init();
        }

        public override void Init()
        {
            base.Init();
            InitHeart();
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

        void BuyHeart()
        {
            if (SaveGame.Coin >= 100)
            {
                SaveGame.Coin -= 100;
                SaveGame.Heart += 1;
                ManagerEvent.ClearEvent();
                SceneManager.LoadScene("SceneGame");
            }
            else
            {
                EasyUI.Toast.Toast.Show("Not enough money!", 1f);
                Debug.Log("Not enough coin");
            }
        }

        public override void Hide()
        {
            SaveGame.CountDownTimer = countdownTimer;


            transform.localScale = Vector3.one;

            transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                SceneManager.LoadScene("SceneHome");
                gameObject.SetActive(false);
                ManagerEvent.RaiseEvent(EventCMD.EVENT_POPUP_CLOSE, this);
            });
        }

        private void InitHeart()
        {
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
}