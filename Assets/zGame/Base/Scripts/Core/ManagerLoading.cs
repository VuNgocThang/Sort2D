using System;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class ManagerLoading : MonoBehaviour
    {
        public static ManagerLoading Instance;
        public bool IsShow => Instance.canvasLoadScene.blocksRaycasts;
        public bool IsShowDone => Instance.canvasLoadScene.alpha == 1;

        [SerializeField] CanvasGroup canvasLoadScene;
        [SerializeField] GameObject imgLoadScene;
        public GameObject LoadConnect;

        ManagerTimer managerTimer;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                managerTimer = new ManagerTimer(true);

                canvasLoadScene.alpha = 0;
                canvasLoadScene.blocksRaycasts = false;
                imgLoadScene.SetActive(false);

                LoadConnect.SetActive(false);

                gameObject.SetActive(true);
            }
            else gameObject.SetActive(false);
        }

        public static void ShowLoadScene(Action act = null, float time = 1f)
        {
            if (Instance == null) act?.Invoke();
            else Instance.ShowLoading(act, time);
        }

        public static void HideLoadScene(float time = 1f)
        {
            Instance.HideLoading(time);
        }

        void ShowLoading(Action act, float time)
        {
            canvasLoadScene.blocksRaycasts = true;
            imgLoadScene.SetActive(false);

            managerTimer.Set("LOADING", time, () =>
            {
                imgLoadScene.SetActive(true);
                managerTimer.Set("LOADING", time, act, (remain, total) =>
                {
                    float d = total - remain;
                    canvasLoadScene.alpha = remain * 1.4f / total;
                });
            }, (remain, total) =>
            {
                float d = total - remain;
                canvasLoadScene.alpha = (total - remain) * 1.4f / total;
            });
        }

        void HideLoading(float time)
        {
            imgLoadScene.SetActive(true);
            canvasLoadScene.blocksRaycasts = true;

            managerTimer.Set("LOADING", time, () =>
            {
                imgLoadScene.SetActive(false);
                managerTimer.Set("LOADING", time, () =>
                {
                    canvasLoadScene.alpha = 0;
                    canvasLoadScene.blocksRaycasts = false;
                }, (remain, total) =>
                {
                    float d = total - remain;
                    canvasLoadScene.alpha = remain * 1.4f / total;
                });
            }, (remain, total) =>
            {
                float d = total - remain;
                canvasLoadScene.alpha = (total - remain) * 1.4f / total;
            });
        }
    }
}