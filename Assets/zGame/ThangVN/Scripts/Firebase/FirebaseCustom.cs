using System.Collections;
using System.Collections.Generic;
#if ACTIVE_APPSFLYER
using AppsFlyerSDK;
#endif
using UnityEngine;

public class FirebaseCustom
{
    public static void LogCountUseHammer()
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("use_hammer");
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);
#endif
        }
    }

    public static void LogCountUseSuperHammer()
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("use_hammer");
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);
#endif
        }
    }

    public static void LogCountUseDrag()
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("use_drag");
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);
#endif
        }
    }

    public static void LogCountUseRefresh()
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("use_refresh");
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);
#endif
        }
    }

    public static void LogLosePoint(int currentPoint)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("lose_point", "point", currentPoint);
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);
#endif
        }
    }

    public static void LogTotalCake(int totalCake)
    {
        Config.AddCountLevelWinSum();
        Config.AddCountLevelStartSum();
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("total_unlock_cake", "total_cake", "index_" + totalCake);
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);
#endif
        }
    }

    public static void LogBackHome()
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("back_home");
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);
#endif
        }
    }

    public static void LogOpenDecor()
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("open_decor");
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);
#endif
        }
    }

    public static void LogLoseAtProcessCake(int indexProcessCake)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("lose_at_process_cake_" + indexProcessCake);
#endif
        }
    }

    public static void LogDayGetProcessCake(int indexProcessCake)
    {
        int countDayActiveNew = PlayerPrefs.GetInt(Config.COUNT_DAY_ACTIVE, 0);
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            // int countDayActiveNew = PlayerPrefs.GetInt(Config.COUNT_DAY_ACTIVE, 0);E    
            Firebase.Analytics.FirebaseAnalytics.LogEvent("get_process_cake_" + indexProcessCake,
                new Firebase.Analytics.Parameter[]
                    { new Firebase.Analytics.Parameter("day", "day_" + (countDayActiveNew - 1)) });
#endif
        }
#if ACTIVE_APPSFLYER
            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add("day", "day_" + (countDayActiveNew - 1));
            switch (Config.TYPE_PUB_G)
            {
                case Config.PUB_G_RK:
                    break;
                case Config.PUB_G_AB:
                    AppsFlyer.sendEvent("get_process_cake_" + indexProcessCake, eventValues);
                    break;
                default:
                    break;
            }
#endif
    }

    public static void LogDay3AtProcessCake(int indexProcessCake)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            bool activeLog = PlayerPrefs.GetInt("log_day_3_get_cake", 0) == 0;
            if (activeLog)
            {
                int countDayActiveNew = PlayerPrefs.GetInt(Config.COUNT_DAY_ACTIVE, 0);
                if (countDayActiveNew >= 3)
                {
                    Firebase.Analytics.FirebaseAnalytics.LogEvent("day3_at_process_cake_" + indexProcessCake);
                    PlayerPrefs.SetInt("log_day_3_get_cake", 1);
                }
            }
#endif
        }
    }

    public static void LogInterShowContinueEndGame(int indexProcessCake)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("inter_continue_end_game", "placement",
                "index_" + indexProcessCake);
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);
#endif
        }
    }
}