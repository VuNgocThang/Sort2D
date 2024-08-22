using System;
using System.Collections;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class CoreGame : MonoBehaviour
    {
        public static CoreGame Instance;
        public static bool DEV = true;

        public bool initSDK;
        public bool dev;

        int systemStartFrom;

        void Awake()
        {
            if (Instance != null)
            {
                gameObject.SetActive(false);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(Instance);

            // Application.targetFrameRate = 120;
            DEV = dev;

            systemStartFrom = ManagerTime.SystemCount;
            last = systemStartFrom;

            // if (DEV) SRDebug.Init();
            if (initSDK) ManagerGame.InitSDK();
            else ManagerGame.InitAdsIAP();
        }

        public static void RunOnMainThread(Action act)
        {
            ManagerGame.ListAction.Add(act);
        }

        void Update()
        {
            if (ManagerGame.ListAction.Count > 0)
            {
                for (int i = 0; i < ManagerGame.ListAction.Count; ++i)
                    ManagerGame.ListAction[i]?.Invoke();
                ManagerGame.ListAction.Clear();
            }
            if (ManagerGame.ListEvent.Count > 0)
            {
                for (int i = 0; i < ManagerGame.ListEvent.Count; ++i)
                    ManagerEvent.RaiseEvent(ManagerGame.ListEvent[i].cmd, ManagerGame.ListEvent[i].obj);
                ManagerGame.ListEvent.Clear();
            }
            for (int i = 0; i < ManagerGame.ListTimer.Count; ++i)
                ManagerGame.ListTimer[i].Update();

            for (int i = 0; i < ManagerGame.ListSpine.Count; ++i)
                ManagerGame.ListSpine[i].TimeScale = ManagerGame.TIME_SCALE;
        }

        int last;
        void OnApplicationPause(bool pause)
        {
            if (pause) last = ManagerTime.SystemCount;
            else
            {
                if (last == systemStartFrom) return;
                for (int i = 0; i < ManagerGame.ListTimer.Count; ++i)
                    ManagerGame.ListTimer[i].Pass((ManagerTime.SystemCount - last) / 1000);
            }
            Save();
        }

        void OnApplicationQuit()
        {
            Save();
        }

        void Save()
        {

        }
    }
}