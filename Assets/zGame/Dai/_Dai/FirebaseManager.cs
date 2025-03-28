#if ACTIVE_ADJUST
using com.adjust.sdk;
#endif
#if ACTIVE_APPSFLYER
using AppsFlyerSDK;
#endif
#if ACTIVE_FIREBASE_ANALYTIC
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#if ACTIVE_FACEBOOK
using Facebook.Unity;
#endif

public class FirebaseManager : MonoBehaviour
{
    const int MAX_LEVEL_LOG= 300;

    public static FirebaseManager instance;
    public bool firebaseInitialized = false;

    public static FirebaseRemoteConfig remoteConfig { get { return FirebaseRemoteConfig.Instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
#if ACTIVE_FACEBOOK
            FB.Init();
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        //    Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        //});
        //DelayLoad();

#if ACTIVE_FIREBASE_ANALYTIC
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {

            if (task.Result == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                //---
                //Config.CheckNewDay();
                //Config.CheckRetent();

                AdsController.instance.ActiveWaitFetchFirebase();//NEW
                Debug.LogError(
                 "Error Could not resolve all Firebase dependencies: " + task.Result);
            }

        });
#endif

#if ACTIVE_APPSFLYER && FLYER_ADREVENUE
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                AppsFlyerAdRevenue.start();
                break;
            default:
                break;
        }
#endif

    }
    //public void DelayLoad()
    //{
    //    StartCoroutine(LoadDelayActive());
    //}
    //IEnumerator LoadDelayActive()
    //{
    //    yield return new WaitForSeconds(3);
    //    UnityMainThreadDispatcher.Instance().Enqueue(() =>
    //    {
    //        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
    //        {
    //            if (task.Result == DependencyStatus.Available)
    //            {
    //                InitializeFirebase();
    //            }
    //            else
    //            {
    //                Debug.LogError(
    //                 "Error Could not resolve all Firebase dependencies: " + task.Result);
    //            }
    //        });
    //    });
    //}
    void InitializeFirebase()
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Enabling data collection.");
        }
        //FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        firebaseInitialized = true;

        //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        //Debug.Log("Firebase Messaging Initialized");


        //// This will display the prompt to request permission to receive
        //// notifications if the prompt has not already been displayed before. (If
        //// the user already responded to the prompt, thier decision is cached by
        //// the OS and can be changed in the OS settings).
        //Firebase.Messaging.FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(
        //  task => {
        //      LogTaskCompletion(task, "RequestPermissionAsync");
        //  }
        //);

        remoteConfig.InitializeFirebase();

        //FIXME
        //if (LoadSceneManager.instance.isActiveAndEnabled)
        //{
        //    LoadSceneManager.instance.LoadMenuScene();
        //}

        //---
        Config.CheckNewDay();
        Config.CheckRetent();
    }
    //------UserProperty---- 
    public void LogUserPropertyLevelReach(int levelComplete)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyLevelReach :  " + levelComplete.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                FirebaseAnalytics.SetUserProperty("level_reach", levelComplete.ToString());
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                FirebaseAnalytics.SetUserProperty("level", levelComplete.ToString());
                break;
            default:
                FirebaseAnalytics.SetUserProperty("level_reach", levelComplete.ToString());
                break;
        }
#endif
    }
    public void LogUserPropertyDaysPlaying(int countDay)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyDaysPlaying :  " + countDay.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                FirebaseAnalytics.SetUserProperty("days_playing", countDay.ToString());
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                break;
            default:
                FirebaseAnalytics.SetUserProperty("days_playing", countDay.ToString());
                break;
        }
#endif
    }
    public void LogUserPropertyTotalSpent(int totalSpent, int valueNow, string where)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyTotalSpent :  " + totalSpent.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                FirebaseAnalytics.SetUserProperty("total_spent", totalSpent.ToString());
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                Firebase.Analytics.FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSpendVirtualCurrency, new Parameter[] { new Parameter(FirebaseAnalytics.ParameterCurrency, "gold"), new Parameter(FirebaseAnalytics.ParameterValue, (long)valueNow)
                , new Parameter(FirebaseAnalytics.ParameterItemName, where)});
                break;
            default:
                FirebaseAnalytics.SetUserProperty("total_spent", totalSpent.ToString());
                break;
        }
#endif
    }
    public void LogUserPropertyTotalEarn(int totalEarn, int valueNow, string where)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyTotalEarn :  " + totalEarn.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                FirebaseAnalytics.SetUserProperty("total_earn", totalEarn.ToString());
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                Firebase.Analytics.FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency, new Parameter[] { new Parameter(FirebaseAnalytics.ParameterCurrency, "gold"), new Parameter(FirebaseAnalytics.ParameterValue, (long)valueNow)
                , new Parameter(FirebaseAnalytics.ParameterSource, where)});
                break;
            default:
                FirebaseAnalytics.SetUserProperty("total_earn", totalEarn.ToString());
                break;
        }
#endif
    }
    public void LogUserPropertyTotalSpentGem(int totalSpent, int valueNow, string where)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyTotalSpent :  " + totalSpent.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                FirebaseAnalytics.SetUserProperty("total_spent_gem", totalSpent.ToString());
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                Firebase.Analytics.FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSpendVirtualCurrency, new Parameter[] { new Parameter(FirebaseAnalytics.ParameterCurrency, "gem"), new Parameter(FirebaseAnalytics.ParameterValue, (long)valueNow)
                , new Parameter(FirebaseAnalytics.ParameterItemName, where)});
                break;
            default:
                FirebaseAnalytics.SetUserProperty("total_spent_gem", totalSpent.ToString());
                break;
        }
#endif
    }
    public void LogUserPropertyTotalEarnGem(int totalEarn, int valueNow, string where)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyTotalEarn :  " + totalEarn.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                FirebaseAnalytics.SetUserProperty("total_earn_gem", totalEarn.ToString());
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                Firebase.Analytics.FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency, new Parameter[] { new Parameter(FirebaseAnalytics.ParameterCurrency, "gem"), new Parameter(FirebaseAnalytics.ParameterValue, (long)valueNow)
                , new Parameter(FirebaseAnalytics.ParameterSource, where)});
                break;
            default:
                FirebaseAnalytics.SetUserProperty("total_earn_gem", totalEarn.ToString());
                break;
        }
#endif
    }
    //---
    public void LogUserPropertyTotalSpentHint(int totalSpent, int valueNow, int pLevel)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyTotalSpent :  " + totalSpent.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        Firebase.Analytics.FirebaseAnalytics.LogEvent("use_hint",
            new Parameter[]
            {
                new Parameter("level", pLevel.ToString()),
                new Parameter("level_str", "level_" + pLevel.ToString())
            });
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                FirebaseAnalytics.SetUserProperty("total_spent_hint", totalSpent.ToString());
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                Firebase.Analytics.FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSpendVirtualCurrency,
                    new Parameter[]
                    {
                        new Parameter(FirebaseAnalytics.ParameterCurrency, "hint"),
                        new Parameter(FirebaseAnalytics.ParameterValue, (long)valueNow),
                        new Parameter(FirebaseAnalytics.ParameterItemName, "level_"+pLevel)
                    });
                break;
            default:
                FirebaseAnalytics.SetUserProperty("total_spent_hint", totalSpent.ToString());
                break;
        }
#endif
    }

    public void LogUserPropertyTotalEarnHint(int totalEarn, int valueNow, string where)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyTotalEarn :  " + totalEarn.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                FirebaseAnalytics.SetUserProperty("total_earn_hint", totalEarn.ToString());
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                Firebase.Analytics.FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency,
                    new Parameter[]
                    {
                        new Parameter(FirebaseAnalytics.ParameterCurrency, "hint"),
                        new Parameter(FirebaseAnalytics.ParameterValue, (long)valueNow),
                        new Parameter(FirebaseAnalytics.ParameterSource, where)
                    });
                break;
            default:
                FirebaseAnalytics.SetUserProperty("total_earn_hint", totalEarn.ToString());
                break;
        }
#endif
    }
    public void LogUserPropertyRetent(int retent)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyRetent :  " + retent.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                FirebaseAnalytics.SetUserProperty("retent_type", retent.ToString());
                break;
            default:
                break;
        }
#endif
    }
    public void LogUserPropertyCountPlayGame(int countDayPlaying)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyRetent :  " + countDayPlaying.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                FirebaseAnalytics.SetUserProperty("days_played", countDayPlaying.ToString());
                break;
            default:
                break;
        }
#endif
    }
    public void LogUserPropertyPayingType(int countPayingType)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogUserPropertyPayingType :  " + countPayingType.ToString());
        }
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                FirebaseAnalytics.SetUserProperty("paying_type", countPayingType.ToString());
                break;
            default:
                break;
        }
#endif
    }
    //----Event------------------------
    public void LogLevelStart(int level)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogLevelStart :  " + level);
        }
        Config.AddCountLevelStartSum();
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            if ((level >= 0 && level <= MAX_LEVEL_LOG)
                || (level >= 10000 && level <= 10100)
                || (level >= 20000 && level <= 20100)
                || (level >= 30000 && level <= 30100)
                )
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("checkpoint_level_" + level.ToString());
            }
            switch (ConfigIdsAds.TYPE_PUB_G)
            {
                case Config.PUB_G_RK:
                    //Firebase.Analytics.FirebaseAnalytics.LogEvent("level_start", "level", level.ToString());
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_start", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
                case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                    //Firebase.Analytics.FirebaseAnalytics.LogEvent("level_start", "level", level.ToString());
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_start", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
                default:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_start", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
            }
#endif
        }
#if ACTIVE_ADJUST
        AdjustEvent adjustEvent = new AdjustEvent(str);
        Adjust.trackEvent(adjustEvent);
#endif
    }
    public void LogLevelLose(int level, int timeSecond)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogLevelLose :  " + level + "  timeSecond: " + timeSecond);
        }
        Config.AddCountLevelLoseSum();
        Config.SetLevelFailCount(level);
        //string str = "Lv_" + level + "_F";
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            if ((level >= 0 && level <= MAX_LEVEL_LOG)
               || (level >= 10000 && level <= 10100)
               || (level >= 20000 && level <= 20100)
               || (level >= 30000 && level <= 30100)
               )
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("check_level_Lose_" + level.ToString(), new Parameter[] { new Parameter("time", timeSecond), new Parameter("failcount", Config.GetLevelFailCount(level)) });
            }
            switch (ConfigIdsAds.TYPE_PUB_G)
            {
                case Config.PUB_G_RK:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_lose", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("time", timeSecond), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
                case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_fail", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("failcount", Config.GetLevelFailCount(level)), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
                default:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_lose", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("time", timeSecond), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
            }
#endif
        }
#if ACTIVE_ADJUST
        AdjustEvent adjustEvent = new AdjustEvent(str);
        Adjust.trackEvent(adjustEvent);
#endif
    }
    public void LogLevelWin(int level, int timeSecond)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogLevelWin :  " + level + "  timeSecond: " + timeSecond);
        }
        Config.AddCountLevelWinSum();
        Config.SetCurrLevelComplete(level);
        //string str = "Lv_" + level + "_W";
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC

            switch (ConfigIdsAds.TYPE_PUB_G)
            {
                case Config.PUB_G_RK:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_win", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("time", timeSecond), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
                case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_complete", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("timeplayed", timeSecond), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
                default:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_win", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("time", timeSecond), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
            }
#endif
        }
#if ACTIVE_ADJUST
        AdjustEvent adjustEvent = new AdjustEvent(str);
        Adjust.trackEvent(adjustEvent);
#endif
#if ACTIVE_APPSFLYER
        if (Config.GetLevelBest() < level)
        {
            Config.SetLevelBest(level);
            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add("level", level.ToString());
            switch (ConfigIdsAds.TYPE_PUB_G)
            {
                case Config.PUB_G_RK:
                    break;
                case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                    AppsFlyer.sendEvent("af_level_achieved", eventValues);
                    break;
                default:
                    break;
            }
        }
#endif
    }
    public void LogLevelQuit(int level, int timeSecond)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogLevelQuit :  " + level + "  timeSecond: " + timeSecond);
        }
        Config.AddCountLevelQuitSum();
        Config.SetLevelFailCount(level);
        //string str = "Lv_" + level + "_F";
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC

            if (level <= MAX_LEVEL_LOG)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("check_level_Lose_" + level.ToString(), new Parameter[] { new Parameter("time", timeSecond), new Parameter("failcount", Config.GetLevelFailCount(level)) });
            }
            switch (ConfigIdsAds.TYPE_PUB_G)
            {
                case Config.PUB_G_RK:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_lose",
                        new Parameter[] {
                            new Parameter("level", level.ToString()),
                            new Parameter("time", timeSecond),
                            new Parameter("level_str", "level_" + level.ToString())
                        });
                    break;
                case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_fail", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("failcount", Config.GetLevelFailCount(level)), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
                default:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_lose", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("time", timeSecond), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
            }
#endif
        }
#if ACTIVE_ADJUST
        AdjustEvent adjustEvent = new AdjustEvent(str);
        Adjust.trackEvent(adjustEvent);
#endif
    }
    public void LogLevelReplay(int level, int timeSecond)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogLevelReplay :  " + level + "  timeSecond: " + timeSecond);
        }
        Config.AddCountLevelQuitSum();
        Config.SetLevelFailCount(level);
        //string str = "Lv_" + level + "_F";
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC

            if (level <= MAX_LEVEL_LOG)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("check_level_Lose_" + level.ToString(), new Parameter[] { new Parameter("time", timeSecond), new Parameter("failcount", Config.GetLevelFailCount(level)) });
            }
            switch (ConfigIdsAds.TYPE_PUB_G)
            {
                case Config.PUB_G_RK:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_lose",
                        new Parameter[] {
                            new Parameter("level", level.ToString()),
                            new Parameter("time", timeSecond),
                            new Parameter("level_str", "level_" + level.ToString())
                        });
                    break;
                case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_fail", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("failcount", Config.GetLevelFailCount(level)), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
                default:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("level_lose", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("time", timeSecond), new Parameter("level_str", "level_" + level.ToString()) });
                    break;
            }
#endif
        }
#if ACTIVE_ADJUST
        AdjustEvent adjustEvent = new AdjustEvent(str);
        Adjust.trackEvent(adjustEvent);
#endif
    }
    public void LogShowInter(string pWhere, int curentLevel)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogShowInter :  " + pWhere + "  curentLevel: " + curentLevel);
        }
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            switch (ConfigIdsAds.TYPE_PUB_G)
            {
                case Config.PUB_G_RK:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("interstitial_show", new Parameter[] { new Parameter("level", curentLevel.ToString()), new Parameter("placement", pWhere), new Parameter("level_str", "level_" + curentLevel.ToString()) });
                    break;
                case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("interstitial_show", new Parameter[] { new Parameter("level", curentLevel.ToString()), new Parameter("placement", pWhere), new Parameter("level_str", "level_" + curentLevel.ToString()) });
                    //Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_abi", new Parameter[] { new Parameter("level", curentLevel.ToString()), new Parameter("placement", pWhere), new Parameter("level_str", "level_" + curentLevel.ToString()) });
                    break;
                case Config.PUB_G_FACOL:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("interstitial_show", new Parameter[] { new Parameter("level", curentLevel.ToString()), new Parameter("placement", pWhere), new Parameter("level_str", "level_" + curentLevel.ToString()) });
                    break;
                default:
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("interstitial_show", new Parameter[] { new Parameter("level", curentLevel.ToString()), new Parameter("placement", pWhere), new Parameter("level_str", "level_" + curentLevel.ToString()) });
                    break;
            }
#endif
        }
#if ACTIVE_ADJUST
        AdjustEvent adjustEvent = new AdjustEvent("Ads_Inter");
        Adjust.trackEvent(adjustEvent);
#endif
//#if ACTIVE_APPSFLYER
//        switch (ConfigIdsAds.TYPE_PUB_G)
//        {
//            case Config.PUB_G_FACOL:
//                Dictionary<string, string> eventValues = new Dictionary<string, string>();
//                AppsFlyer.sendEvent("af_inters_displayed", eventValues);
//                break;
//            default:
//                break;
//        }
//#endif
    }


    public void LogValueAds(string ad_platform, string ad_source, string ad_unit_name, string ad_format, double value, string currency, string placement,string countryCode,string networkPlacement,string creativeIdentifier)
    {
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            switch (ConfigIdsAds.TYPE_PUB_G)
            {
                case Config.PUB_G_RK:
                    // Firebase.Analytics.FirebaseAnalytics.LogEvent("interstitial_show", new Parameter[] { new Parameter("level", curentLevel.ToString()), new Parameter("placement", pWhere), new Parameter("level_str", "level_" + curentLevel.ToString()) });

                    var impressionParametersRK = new[] {
                      new Firebase.Analytics.Parameter("ad_platform", ad_platform),
                      new Firebase.Analytics.Parameter("ad_source", ad_source),
                      new Firebase.Analytics.Parameter("ad_unit_name", ad_unit_name),
                      new Firebase.Analytics.Parameter("ad_format", ad_format),
                      new Firebase.Analytics.Parameter("value", value),
                      new Firebase.Analytics.Parameter("currency", currency), // All AppLovin revenue is sent in USD
                      new Firebase.Analytics.Parameter("placement",placement),
                      new Firebase.Analytics.Parameter("country_code",countryCode)
                    };
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParametersRK);
                    //-----
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_rocket_max", impressionParametersRK);
                    break;
                case Config.PUB_G_AB:

                    var impressionParameters = new[] {
                      new Firebase.Analytics.Parameter("ad_platform", ad_platform),
                      new Firebase.Analytics.Parameter("ad_source", ad_source),
                      new Firebase.Analytics.Parameter("ad_unit_name", ad_unit_name),
                      new Firebase.Analytics.Parameter("ad_format", ad_format),
                      new Firebase.Analytics.Parameter("value", value),
                      new Firebase.Analytics.Parameter("currency", currency), // All AppLovin revenue is sent in USD
                    };
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
                    //-----
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_abi", impressionParameters);
                    break;
                case Config.PUB_G_HOPEE:

                    var impressionParametersHopee = new[] {
                      new Firebase.Analytics.Parameter("ad_platform", ad_platform),
                      new Firebase.Analytics.Parameter("ad_source", ad_source),
                      new Firebase.Analytics.Parameter("ad_unit_name", ad_unit_name),
                      new Firebase.Analytics.Parameter("ad_format", ad_format),
                      new Firebase.Analytics.Parameter("value", value),
                      new Firebase.Analytics.Parameter("currency", currency), // All AppLovin revenue is sent in USD
                    };
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParametersHopee);
                    //-----
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_hopee", impressionParametersHopee);
                    break;
                default:
                    break;
            }
#endif
        }

#if ACTIVE_APPSFLYER && FLYER_ADREVENUE
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                //Dictionary<string, string> dic = new Dictionary<string, string>();
                //dic.Add("ad_platform", ad_platform);
                //dic.Add("ad_source", ad_source);
                //dic.Add("ad_unit_name", ad_unit_name);
                //dic.Add("ad_format", ad_format);
                //AppsFlyerAdRevenue.logAdRevenue(ad_source, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, value, currency, dic);
                if (ConfigIdsAds.TYPE_MEDIATION_AD == Config.MEDIATION_MAX)
                {
                    AppsFlyerAdRevenue.logAdRevenue(ad_source, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, value, currency, null);
                }
                else if (ConfigIdsAds.TYPE_MEDIATION_AD == Config.MEDIATION_IRON)
                {
                    AppsFlyerAdRevenue.logAdRevenue(ad_source, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource, value, currency, null);
                }
                break;
            default:
                break;
        }
#endif


#if ACTIVE_AIRBRIDGE
        AirbridgeEvent @event = new AirbridgeEvent("airbridge.adImpression");

        var appLovin = new Dictionary<string, object>();
        appLovin["revenue"] = value;
        appLovin["country_code"] = countryCode;
        appLovin["network_name"] = ad_source;
        appLovin["network_placement"] = networkPlacement;
        appLovin["adunit_identifier"] = ad_unit_name;
        appLovin["creative_identifier"] = creativeIdentifier;
        appLovin["placement"] = placement;

        var adPartners = new Dictionary<string, object>();
        string strNameMediation = "appLovin";
        switch (ConfigIdsAds.TYPE_MEDIATION_AD)
        {
            case Config.MEDIATION_MAX:
                strNameMediation = "appLovin";
                break;
            default:
                if (Config.ACTIVE_DEBUG_LOG)
                {
                    Debug.LogError("Error ACTIVE_AIRBRIDGE");
                }
                break;
        }
        //adPartners["appLovin"] = appLovin;
        adPartners[strNameMediation] = appLovin;

        //@event.SetAction(ad_source);
        //@event.SetLabel(networkPlacement);

        @event.SetAction(ad_format);
        @event.SetLabel(ad_source);
        @event.SetValue(value);
        @event.AddSemanticAttribute("adPartners", adPartners);
        // AppLovin MAX has a default currency of USD
        @event.AddSemanticAttribute("currency", "USD");

        AirbridgeUnity.TrackEvent(@event);
#endif

    }
    public void LogCallShowInter()
    {
#if ACTIVE_APPSFLYER
        Dictionary<string, string> eventValues = new Dictionary<string, string>();
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                AppsFlyer.sendEvent("af_inters_logicgame", eventValues);
                break;
            default:
                break;
        }
#endif
    }
    public void LogInterSuccLoaded()
    {
#if ACTIVE_APPSFLYER
        Dictionary<string, string> eventValues = new Dictionary<string, string>();
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                AppsFlyer.sendEvent("af_inters_successfullyloaded", eventValues);
                break;
            default:
                break;
        }
#endif
    }
    public void LogInterDisplayed()
    {
#if ACTIVE_APPSFLYER
        Dictionary<string, string> eventValues = new Dictionary<string, string>();

        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_FACOL:
                case Config.PUB_G_HOPEE:
                AppsFlyer.sendEvent("af_inters_displayed", eventValues);
                break;
            default:
                break;
        }
#endif
    }

    public void LogShowReward(string pWhere, int curentLevel)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogShowReward :  " + pWhere + "  curentLevel: " + curentLevel);
        }
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("rv_show", new Parameter[] { new Parameter("level", curentLevel.ToString()), new Parameter("placement", pWhere), new Parameter("level_str", "level_" + curentLevel.ToString()) });
#endif
        }
#if ACTIVE_ADJUST
        AdjustEvent adjustEvent = new AdjustEvent("Ads_Reward");
        Adjust.trackEvent(adjustEvent);
#endif
        //#if ACTIVE_APPSFLYER
        //        AppsFlyer.AFLog("didFinishValidateReceipt", result);
        //#endif
    }
    public void LogRewarded(string pWhere, int curentLevel)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("---------------- LogRewarded :  " + pWhere + "  curentLevel: " + curentLevel);
        }
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("rewarded_video_show", new Parameter[] { new Parameter("level", curentLevel.ToString()), new Parameter("placement", pWhere), new Parameter("level_str", "level_" + curentLevel.ToString()) });
#endif
        }
#if ACTIVE_ADJUST
        AdjustEvent adjustEvent = new AdjustEvent("Ads_Rewarded");
        Adjust.trackEvent(adjustEvent);
#endif
#if ACTIVE_APPSFLYER
        //Dictionary<string, string> eventValues = new Dictionary<string, string>();
        //AppsFlyer.sendEvent("rewarded_video_show", eventValues);
#endif
    }
    public void LogCallShowVR()
    {
#if ACTIVE_APPSFLYER
        Dictionary<string, string> eventValues = new Dictionary<string, string>();
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                AppsFlyer.sendEvent("af_rewarded_logicgame", eventValues);
                break;
            default:
                break;
        }
#endif
    }
    public void LogVRSuccLoaded()
    {
#if ACTIVE_APPSFLYER
        Dictionary<string, string> eventValues = new Dictionary<string, string>();
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                AppsFlyer.sendEvent("af_rewarded_successfullyloaded", eventValues);
                break;
            default:
                break;
        }
#endif
    }
    public void LogVRDisplayed()
    {
#if ACTIVE_APPSFLYER
        Dictionary<string, string> eventValues = new Dictionary<string, string>();
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
                case Config.PUB_G_HOPEE:
                AppsFlyer.sendEvent("af_rewarded_displayed", eventValues);
                break;
            default:
                break;
        }
#endif
    }

    //-------------------
    public void LogCheckPoint(int idCheckPoint)
    {
        //log check poin tung` buoc de tao funnel
#if ACTIVE_FIREBASE_ANALYTIC
        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                if (idCheckPoint < 10)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("checkpoint_0" + idCheckPoint.ToString());
                }
                else
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("checkpoint_" + idCheckPoint.ToString());
                }
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_HOPEE:
                if (idCheckPoint < 10)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("checkpoint_0" + idCheckPoint.ToString());
                }
                else
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("checkpoint_" + idCheckPoint.ToString());
                }
                break;
            default:
                if (idCheckPoint < 10)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("checkpoint_0" + idCheckPoint.ToString());
                }
                else
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("checkpoint_" + idCheckPoint.ToString());
                }
                break;
        }
#endif
    }
    
    public void LogPaymentItem(string pIdIAP,float defauntUSD)
    {
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("iap_payment_" + pIdIAP);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("iap_payment_buy_successfully");
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);

            var purchaseParameters = new[] {
                      new Firebase.Analytics.Parameter("value", defauntUSD),
                      new Firebase.Analytics.Parameter("currency", "USD")
                    };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("iap_succ", purchaseParameters);
#endif
        }
    }
    //    public void LogShowNative(int curentLevel)
    //    {
    //        if (firebaseInitialized)
    //        {
    //#if ACTIVE_FIREBASE_ANALYTIC
    //            Firebase.Analytics.FirebaseAnalytics.LogEvent("Ads_Native", "CurentLevel", curentLevel);
    //#endif
    //        }
    //#if ACTIVE_ADJUST
    //        AdjustEvent adjustEvent = new AdjustEvent("Ads_Native");
    //        Adjust.trackEvent(adjustEvent);
    //#endif
    //    }

    //    public const string POWERUP_UNDO = "Undo";
    //    public const string POWERUP_SUGGEST = "Suggest";
    //    public const string POWERUP_HINT = "Hint";
    //    public const string POWERUP_SHUFFLE = "Shuffle";
    //    public const string POWERUP_REMOVEBOMB = "RemoveBomb";
    //    public const string POWERUP_ADDCOL = "AddCol";
    //    public void LogUsePowerUp(string type, int curentLevel)
    //    {
    //        string str = "PowerUp_" + type;
    //        if (firebaseInitialized)
    //        {
    //#if ACTIVE_FIREBASE_ANALYTIC
    //            Firebase.Analytics.FirebaseAnalytics.LogEvent(str, "CurentLevel", curentLevel);
    //#endif
    //        }
    //#if ACTIVE_ADJUST
    //        AdjustEvent adjustEvent = new AdjustEvent(str);
    //        Adjust.trackEvent(adjustEvent);
    //#endif
    //    }

    public void LogUseVacuum(int pLevel)
    {
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("use_vacuum", new Parameter[] { new Parameter("level", "lv_" + (pLevel)) });
#endif
        }
    }

    public void LogUsePunch(int pLevel)
    {
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("use_punch", new Parameter[] { new Parameter("level", "lv_" + (pLevel)) });
#endif
        }
    }

    public void LogUseBomb(int pLevel)
    {
        if (firebaseInitialized)
        {
            {
#if ACTIVE_FIREBASE_ANALYTIC
                Firebase.Analytics.FirebaseAnalytics.LogEvent("use_bomb", new Parameter[] { new Parameter("level", "lv_" + (pLevel)) });
#endif
            }
        }
    }
    public void LogUseSandGlass(int pLevel)
    {
        if (firebaseInitialized)
        {
            {
#if ACTIVE_FIREBASE_ANALYTIC
                Firebase.Analytics.FirebaseAnalytics.LogEvent("use_sandglass", new Parameter[] { new Parameter("level", "lv_" + (pLevel)) });
#endif
            }
        }
    }
    public void LogUseFirework(int pLevel)
    {
        if (firebaseInitialized)
        {
            {
#if ACTIVE_FIREBASE_ANALYTIC
                Firebase.Analytics.FirebaseAnalytics.LogEvent("use_firework", new Parameter[] { new Parameter("level", "lv_" + (pLevel)) });
#endif
            }
        }
    }
    public void LogUseAddTime(int pLevel)
    {
        if (firebaseInitialized)
        {
            {
#if ACTIVE_FIREBASE_ANALYTIC
                Firebase.Analytics.FirebaseAnalytics.LogEvent("use_addtime", new Parameter[] { new Parameter("level", "lv_" + (pLevel)) });
#endif
            }
        }
    }

    public void LogClaimDaily()
    {
        if (firebaseInitialized)
        {
            int countDayActiveNew = PlayerPrefs.GetInt(Config.COUNT_DAY_ACTIVE, 0);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("claim_daily_rw", new Parameter[] { new Parameter("day", "day_" + (countDayActiveNew - 1)) });
        }
    }

    public void LogLevelKeyChallenge(int pLevel)
    {
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("key_challenge_"+pLevel);
#endif
        }
    }

    public void LogPlayPinBall()
    {
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("play_pin_ball");
#endif
        }
    }

    public void LogCheckPointPinBall(int pLevel)
    {
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("stage_pinball_"+pLevel);
#endif
        }
    }

    public void LogWinStreak(int streak, int pLevel)
    {
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("win_streak_"+streak, new Parameter[] { new Parameter("level", "lv_" + pLevel) });
#endif
        }
    }

    public void LogLevelFactoryPass(int pLevel)
    {
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("factory_pass_level_"+pLevel );
#endif
        }
        
    }

    public void LogSpentCoins(string where)
    {
        if (firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("spent_coin" ,new Parameter("placement", where));
#endif
        }
    }
    public void LogBannerDisplayed()
    {
#if ACTIVE_APPSFLYER
        Dictionary<string, string> eventValues = new Dictionary<string, string>();

        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_FACOL:
                case Config.PUB_G_HOPEE:
                AppsFlyer.sendEvent("af_banner_displayed", eventValues);
                break;
            default:
                break;
        }
#endif
    }
    public void LogAppOpenDisplayed()
    {
#if ACTIVE_APPSFLYER
        Dictionary<string, string> eventValues = new Dictionary<string, string>();

        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_FACOL:
                case Config.PUB_G_HOPEE:
                AppsFlyer.sendEvent("af_appopen_displayed", eventValues);
                break;
            default:
                break;
        }
#endif
    }
    public void LogMRECDisplayed()
    {
#if ACTIVE_APPSFLYER
        Dictionary<string, string> eventValues = new Dictionary<string, string>();

        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_FACOL:
                case Config.PUB_G_HOPEE:
                AppsFlyer.sendEvent("af_mrec_displayed", eventValues);
                break;
            default:
                break;
        }
#endif
    }
    public void LogNativeDisplayed()
    {
#if ACTIVE_APPSFLYER
        Dictionary<string, string> eventValues = new Dictionary<string, string>();

        switch (ConfigIdsAds.TYPE_PUB_G)
        {
            case Config.PUB_G_RK:
                break;
            case Config.PUB_G_AB:
            case Config.PUB_G_FACOL:
                case Config.PUB_G_HOPEE:
                AppsFlyer.sendEvent("af_native_displayed", eventValues);
                break;
            default:
                break;
        }
#endif
    }

    #region FIREBASE MESSAGE
    //public virtual void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    //{
    //    Debug.Log("Received a new message");
    //    var notification = e.Message.Notification;
    //    if (notification != null)
    //    {
    //        Debug.Log("title: " + notification.Title);
    //        Debug.Log("body: " + notification.Body);
    //        var android = notification.Android;
    //        if (android != null)
    //        {
    //            Debug.Log("android channel_id: " + android.ChannelId);
    //        }
    //    }
    //    if (e.Message.From.Length > 0)
    //        Debug.Log("from: " + e.Message.From);
    //    if (e.Message.Link != null)
    //    {
    //        Debug.Log("link: " + e.Message.Link.ToString());
    //    }
    //    if (e.Message.Data.Count > 0)
    //    {
    //        Debug.Log("data:");
    //        foreach (System.Collections.Generic.KeyValuePair<string, string> iter in
    //                 e.Message.Data)
    //        {
    //            Debug.Log("  " + iter.Key + ": " + iter.Value);
    //        }
    //    }
    //}

    //public virtual void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    //{
    //    Debug.Log("Received Registration Token: " + token.Token);

    //}

    //// Log the result of the specified task, returning true if the task
    //// completed successfully, false otherwise.
    //protected bool LogTaskCompletion(Task task, string operation)
    //{
    //    bool complete = false;
    //    if (task.IsCanceled)
    //    {
    //        Debug.Log(operation + " canceled.");
    //    }
    //    else if (task.IsFaulted)
    //    {
    //        Debug.Log(operation + " encounted an error.");
    //        foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
    //        {
    //            string errorCode = "";
    //            Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
    //            if (firebaseEx != null)
    //            {
    //                errorCode = String.Format("Error.{0}: ",
    //                  ((Firebase.Messaging.Error)firebaseEx.ErrorCode).ToString());
    //            }
    //            Debug.Log(errorCode + exception.ToString());
    //        }
    //    }
    //    else if (task.IsCompleted)
    //    {
    //        Debug.Log(operation + " completed");
    //        complete = true;
    //    }
    //    return complete;
    //}
    #endregion

}
