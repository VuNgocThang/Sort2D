using BaseGame;
using DG.Tweening;
using ntDev;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThangVN
{
    public class PopupRestart : Popup
    {
        public EasyButton btnRestart, btnHome;
        public static async void Show()
        {
            PopupRestart pop = await ManagerPopup.ShowPopup<PopupRestart>();
            pop.Init();
        }

        private void Awake()
        {
            btnRestart.OnClick(() =>
            {
                InitHeart();

                if (SaveGame.Heart > 0)
                {
                    ManagerEvent.ClearEvent();
                    LoadScene("SceneGame");
                }
                else
                {
                    // Logic Heart == 0, Show Popup OutOfHeart
                    PopupOutOfHeart.Show();
                    //LoadScene("SceneHome");
                }
            });

            btnHome.OnClick(() =>
            {
                ManagerEvent.ClearEvent();
                SceneManager.LoadScene("SceneHome");
            });
        }

        public override void Init()
        {
            base.Init();
        }

        void InitHeart()
        {
            if (PlayerPrefs.HasKey(GameConfig.LAST_HEART_LOSS))
            {
                float timeSinceLastLoss = (float)(DateTime.Now - DateTime.Parse(PlayerPrefs.GetString(GameConfig.LAST_HEART_LOSS))).TotalSeconds;

                int increaseHeart = (int)(timeSinceLastLoss / GameConfig.TIME_COUNT_DOWN);

                if (GameConfig.MAX_HEART >= SaveGame.Heart)
                {
                    SaveGame.Heart += increaseHeart;
                    SaveGame.Heart = Mathf.Min(SaveGame.Heart, GameConfig.MAX_HEART);
                }

                DateTime timer = DateTime.Now + TimeSpan.FromSeconds(increaseHeart * GameConfig.TIME_COUNT_DOWN);

                //PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, timer.ToString());
            }
        }

        public override void Hide()
        {
            base.Hide();
            Debug.Log("Hide popup Restart");
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

    }
}
