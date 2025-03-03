using System.Collections;
using System.Collections.Generic;
#if ACTIVE_APPSFLYER
using AppsFlyerSDK;
#endif
using UnityEngine;

public class FirebaseCustom
{
    #region NgocThang

    public static void LogLevelRevive(int indexLevel)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("revive_level_" + indexLevel);
#endif
        }
    }

    public static void LogCountBuyLivesUseGold()
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("buy_lives_");
            //Debug.Log("logPaymentItem : "+"iap_payment_"+pIdIAP);
#endif
        }
    }

    public static void LogUseBoosterAtLevel(int indexLevel)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("use_booster_level_" + indexLevel);
#endif
        }
    }

    public static void LogBuyBooster(int indexBooster)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("buy_booster_" + indexBooster);
#endif
        }
    }

    public static void LogRedecorateBook(int indexBook)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("redecorate_book_" + indexBook);
#endif
        }
    }

    public static void LogScoreBookDecorated(int score, int indexBook)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            int indexScore = 0;

            if (score >= 0 && score <= 50) indexScore = 1;
            else if (score >= 51 && score <= 99) indexScore = 2;
            else indexScore = 3;
            Debug.Log($"score_{indexScore}_book_" + indexBook);
            Firebase.Analytics.FirebaseAnalytics.LogEvent($"score_{indexScore}_book_" + indexBook);
#endif
        }
    }

    public static void LogDailyRewardFreeCoin(int indexFreeCoin)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Debug.Log("daily_reward_" + indexFreeCoin);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("daily_reward_" + indexFreeCoin);
#endif
        }
    }

    public static void LogCountPlayChallenges()
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Debug.Log("play_challenge");
            Firebase.Analytics.FirebaseAnalytics.LogEvent("play_challenge");
#endif
        }
    }

    public static void LogBonusPlay(int indexLevel)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("play_bonus_" + indexLevel);
#endif
        }
    }

    public static void LogBonusWin(int indexLevel)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("win_bonus_" + indexLevel);
#endif
        }
    }

    public static void LogBonusLoseTime(int indexLevel)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("lose_time_level_" + indexLevel);
#endif
        }
    }

    public static void LogBonusLoseSlot(int indexLevel)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("lose_slot_level_" + indexLevel);
#endif
        }
    }

    public static void LogDailyTaskCompleted(int indexTask)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Debug.Log("complete_task_" + indexTask);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("complete_task_" + indexTask);
#endif
        }
    }

    public static void LogDailyTaskRewardClaimed(int indexReward)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            Debug.Log("reward_task_" + indexReward);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("reward_task_" + indexReward);
#endif
        }
    }

    #endregion


    public static void LogScoreChallenges(int bestScore)
    {
        if (FirebaseManager.instance.firebaseInitialized)
        {
#if ACTIVE_FIREBASE_ANALYTIC
            int indexScore = 0;

            if (bestScore >= 0 && bestScore <= 150) indexScore = 1;
            else if (bestScore >= 151 && bestScore <= 500) indexScore = 2;
            else if (bestScore >= 501 && bestScore <= 1000) indexScore = 3;
            else indexScore = 4;
            Debug.Log($"best_score_{indexScore}_challenge");
            Firebase.Analytics.FirebaseAnalytics.LogEvent($"best_score_{indexScore}_challenge");
#endif
        }
    }
}