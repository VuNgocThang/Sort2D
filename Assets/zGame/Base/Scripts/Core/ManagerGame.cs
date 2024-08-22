using System;
using System.Collections.Generic;

// by nt.Dev93
namespace ntDev
{
    public static class ManagerGame
    {
        public static float TIME_SCALE = 1;

        static List<Action> listAction;
        public static List<Action> ListAction
        {
            get
            {
                if (listAction == null) listAction = new List<Action>();
                return listAction;
            }
        }

        static List<RaiseEventObject> listEvent;
        public static List<RaiseEventObject> ListEvent
        {
            get
            {
                if (listEvent == null) listEvent = new List<RaiseEventObject>();
                return listEvent;
            }
        }

        static List<ManagerTimer> listTimer;
        public static List<ManagerTimer> ListTimer
        {
            get
            {
                if (listTimer == null) listTimer = new List<ManagerTimer>();
                return listTimer;
            }
        }
        public static void AddTimer(ManagerTimer timer)
        {
            if (ListTimer.Contains(timer)) return;
            ListTimer.Add(timer);
        }
        public static void RemoveTimer(ManagerTimer timer)
        {
            ListTimer.Remove(timer);
        }

        static List<ManagerSpine> listSpine;
        public static List<ManagerSpine> ListSpine
        {
            get
            {
                if (listSpine == null) listSpine = new List<ManagerSpine>();
                return listSpine;
            }
        }
        public static void AddSpine(ManagerSpine anim)
        {
            if (ListSpine == null) return;
            if (ListSpine.Contains(anim)) return;
            ListSpine.Add(anim);
        }
        public static void RemoveSpine(ManagerSpine anim)
        {
            if (ListSpine == null) return;
            ListSpine.Remove(anim);
        }

        public static void Clear()
        {
            TIME_SCALE = 1;
            ManagerEvent.ClearEvent();
            ListAction.Clear();
            ListEvent.Clear();
            ListTimer.Clear();
            ListSpine.Clear();
        }

        static bool IsInitialized = false;
        public static bool FirebaseReady = false;
        public static void InitSDK()
        {
            if (IsInitialized) return;
            // InitAdsIAP();
            // FB.Init(() => { FB.Mobile.SetAdvertiserTrackingEnabled(true); });
            // FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            // {
            //     ManagerGame.FirebaseReady = task.Result == DependencyStatus.Available;
            //     if (ManagerGame.FirebaseReady) CoreGame.RunOnMainThread(ManagerStorage.Init);
            //     else
            //     {
            //         ManagerStorage.Ready = true;
            //         ManagerScene.StartGameOnMainThread();
            //     }
            // });

            // #if UNITY_ANDROID
            //             try
            //             {
            //                 PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            //                 PlayGamesPlatform.InitializeInstance(config);
            //                 PlayGamesPlatform.Activate();
            //             }
            //             catch (Exception) { }
            // #endif
        }

        public static void InitAdsIAP()
        {
            // managerAds.Init();
            // managerIAP.Init();
            IsInitialized = true;
        }

        public static void LogInServices()
        {
            //             if (SaveGame.PlayServicesID == "" || SaveGame.PlayServicesName == "")
            //             {
            // #if UNITY_EDITOR
            //                 CreateAccountPvP();
            // #elif UNITY_ANDROID
            //                 Ez.Log("Login Google Play Services");
            //                 try
            //                 {
            //                     PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
            //                     {
            //                         Ez.Log("Login Google Play Services Result");
            //                         Ez.Log(result);
            //                         if (result == SignInStatus.Success)
            //                         {
            //                             SaveGame.PlayServicesID = Social.localUser.id;
            //                             SaveGame.PlayServicesName = Social.localUser.userName;
            //                             if (ManagerGame.FirebaseReady) CoreGame.RunOnMainThread(ManagerStorage.Init);
            //                             CreateAccountPvP();
            //                         }
            //                         else
            //                         {
            //                             ManagerStorage.Ready = true;
            //                             ManagerScene.StartGameOnMainThread();
            //                         }
            //                     });
            //                 }
            //                 catch (Exception) { }
            // #elif UNITY_IOS
            //                 Ez.Log("Login Game Center");
            //                 Social.localUser.Authenticate(result =>
            //                 {
            //                     Ez.Log("Login Game Center Result");
            //                     Ez.Log(result);
            //                     if (result)
            //                     {
            //                         if (!Social.localUser.id.Contains("Identif"))
            //                             SaveGame.PlayServicesID = Social.localUser.id;
            //                         SaveGame.PlayServicesName = Social.localUser.userName;
            //                         if (ManagerGame.FirebaseReady) CoreGame.RunOnMainThread(ManagerStorage.Init);
            //                         CreateAccountPvP();
            //                     }
            //                     else 
            //                     {
            //                         ManagerStorage.Ready = true;
            //                         ManagerScene.StartGameOnMainThread();
            //                     }
            //                 });
            // #endif
            //             }
            //             else CreateAccountPvP();
        }

        // public static void LogEvent(string name)
        // {
        //     if (FirebaseReady) FirebaseAnalytics.LogEvent(name);
        // }

        // public static void LogEvent(string name, string param = "", int value = 0)
        // {
        //     if (FirebaseReady)
        //     {
        //         Firebase.Analytics.Parameter[] para = new Firebase.Analytics.Parameter[1];
        //         para[0] = new Firebase.Analytics.Parameter(param, value.ToString(5));
        //         FirebaseAnalytics.LogEvent(name, para);
        //     }
        // }

        // public static void LogEvent(string name, string param = "", string value = "")
        // {
        //     if (FirebaseReady)
        //     {
        //         Firebase.Analytics.Parameter[] para = new Firebase.Analytics.Parameter[1];
        //         para[0] = new Firebase.Analytics.Parameter(param, value == "" ? "null" : value);
        //         FirebaseAnalytics.LogEvent(name, para);
        //     }
        // }

        // public static void LogEvent(string name, Parameter[] arrPara)
        // {
        //     if (FirebaseReady) FirebaseAnalytics.LogEvent(name, arrPara);
        // }
    }
}