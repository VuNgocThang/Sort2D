using BaseGame;
using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThangVN
{
    public class PopupRestart : Popup
    {
        public EasyButton btnRestart;
        public static async void Show()
        {
            PopupRestart pop = await ManagerPopup.ShowPopup<PopupRestart>();
            pop.Init();
        }

        private void Awake()
        {
            btnRestart.OnClick(() =>
            {
                if (SaveGame.Heart > 0)
                {
                    PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
                    SaveGame.Heart--;
                    Debug.Log("heart -- " + SaveGame.Heart);
                    LoadScene("SceneGame");
                }
                else LoadScene("SceneHome");
            });
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Hide()
        {
            base.Hide();
            Debug.Log("Hide popup lose");
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
