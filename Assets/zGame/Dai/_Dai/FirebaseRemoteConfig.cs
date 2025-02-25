
#if ACTIVE_FIREBASE_ANALYTIC
using Firebase.Extensions;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseRemoteConfig
{
    //---------Config---------
    //-----------------------

    const string KEY_REMOTE_ACTIVE_INTER_ADS = "cf_active_inter_ad";

    const string KEY_REMOTE_GMA_FLAG_GDPR = "cf_gma_flag_gdpr";

    const string KEY_REMOTE_LEVEL_SHOW_RATE = "cf_level_show_rate";

    const string KEY_REMOTE_INTER_LEVEL_SHOW = "cf_inter_level_show";
    const string KEY_REMOTE_INTER_TIME_DELAY_INTER = "cf_inter_time_delay_inter";
    const string KEY_REMOTE_TIME_RESET_COLLAPBANNER = "cf_time_reset_collapbanner";

    const string KEY_REMOTE_REWARD_VIDEO_TIME_DELAY_INTER = "cf_reward_video_time_delay_inter";
    const string KEY_REMOTE_AOA_TIME_DELAY_INTER = "cf_aoa_time_delay_inter";
    const string KEY_REMOTE_INTER_AUTO_INGAME = "cf_inter_time_auto_ingame";
    const string KEY_REMOTE_ENABLE_INTER_BACK_POPUP = "cf_enable_inter_back_popup";
    const string KEY_REMOTE_AOA_FIRST_OPEN_ACTIVE = "cf_enable_aoa_first_open_active";
    const string KEY_REMOTE_AOA_RESUME_ACTIVE = "cf_enable_aoa_resume_active";

    const string KEY_REMOTE_CHECK_CONNECTNETWORK_ACTIVE = "cf_enable_connectnetwork_active";

    const string KEY_REMOTE_VALUE_ADS_PAUSEGAME = "cf_value_ads_pausegame";
    //------check user------
    const string KEY_REMOTE_ACTIVE_CHECK_TYPE_USER = "cf_active_check_type_user";
    const string KEY_REMOTE_ACTIVE_BANNER_ALWAY_TYPE_USER = "cf_active_banner_alway_type_user";


    const string KEY_REMOTE_CAPPING_FREE_USER_TIME_INTER_BY_INTER = "cf_capping_free_user_time_inter_by_inter";
    const string KEY_REMOTE_CAPPING_FREE_USER_TIME_INTER_BY_REWARD = "cf_capping_free_user_time_inter_by_reward";

    const string KEY_REMOTE_CAPPING_PURCHASE_USER_TIME_INTER_BY_INTER = "cf_capping_purchase_user_time_inter_by_inter";
    const string KEY_REMOTE_CAPPING_PURCHASE_USER_TIME_INTER_BY_REWARD = "cf_capping_purchase_user_time_inter_by_reward";

    const string KEY_REMOTE_CAPPING_ADS_USER_TIME_INTER_BY_INTER = "cf_capping_ads_user_time_inter_by_inter";
    const string KEY_REMOTE_CAPPING_ADS_USER_TIME_INTER_BY_REWARD = "cf_capping_ads_user_time_inter_by_reward";

    const string KEY_REMOTE_CAPPING_FREE_2_USER_TIME_INTER_BY_INTER = "cf_capping_free_2_user_time_inter_by_inter";
    const string KEY_REMOTE_CAPPING_FREE_2_USER_TIME_INTER_BY_REWARD = "cf_capping_free_2_user_time_inter_by_reward";

    const string KEY_REMOTE_COUNT_PURCHASE_GET_PURCHASE_1 = "cf_count_purchase_get_purchase_1";
    const string KEY_REMOTE_COUNT_PURCHASE_GET_PURCHASE_2 = "cf_count_purchase_get_purchase_2";
    const string KEY_REMOTE_COUNT_VIEW_GET_ADS_1 = "cf_count_view_get_ads_1";
    const string KEY_REMOTE_COUNT_VIEW_GET_ADS_2 = "cf_count_view_get_ads_2";

    const string KEY_REMOTE_SECOND_BUY_PURCHASE_ADD = "cf_second_buy_purchase_add";
    const string KEY_REMOTE_SECOND_VIEW_REWARD_ADD = "cf_second_view_reward_add";

    const string KEY_REMOTE_LEVEL_FIRST_CHECK_TYPE_USER = "cf_level_first_check_type_user";
    const string KEY_REMOTE_TIME_FIRST_CHECK_TYPE_USER = "cf_time_first_check_type_user";

    const string KEY_REMOTE_MAX_COUNT_CHECK_WAIT = "cf_max_count_check_wait";

    const string KEY_REMOTE_TILE_WIN_RATE_E = "cf_tile_win_rate_e";
    const string KEY_REMOTE_MAX_COUNT_LEVEL_CHECK_RATE_WIN_E = "cf_max_count_level_check_rate_win_e";

    const string KEY_REMOTE_MAX_COUNT_LEVEL_CHECK_RATE_WIN_SUM = "cf_max_count_level_check_rate_win_sum";

    const string KEY_REMOTE_TIME_ADD_WAIT_CHECK = "cf_time_add_wait_check";

    const string KEY_REMOTE_MAX_TIME_START_TO_END_LEVEL = "cf_max_time_start_to_end_level";

    const string KEY_REMOTE_TIME_ADD_TO_TYPE_USER_DROP = "cf_time_add_to_type_user_drop";

    const string KEY_REMOTE_TIME_MENU_CHECK_USER_WHAT_DOING = "cf_time_menu_check_user_what_doing";
    const string KEY_REMOTE_TIME_MAX_MENU_CHECK_TO_LEVEL_START = "cf_time_max_menu_check_to_level_start";

    //------Debug log------
    const string KEY_REMOTE_ACTIVE_DEBUG_LOG = "cf_active_debug_log";
    //------------------
    public const string strSaveRemoteLvShow = "inter_level_show";
    public const string strSaveRemoteTimeDelayInter = "inter_time_delay_inter";

    public const string strValueAdsPauseGame = "value_ads_pausegame";

    //----
    protected bool isFirebaseInitialized = false;
    private static FirebaseRemoteConfig mInstance;

    public static FirebaseRemoteConfig Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new FirebaseRemoteConfig();
            return mInstance;
        }
    }
    //Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    //protected virtual void Start()
    //{
    //    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
    //    {
    //        dependencyStatus = task.Result;
    //        if (dependencyStatus == Firebase.DependencyStatus.Available)
    //        {
    //            InitializeFirebase();
    //        }
    //        else
    //        {
    //            Debug.LogError(
    //              "Could not resolve all Firebase dependencies: " + dependencyStatus);
    //        }
    //    });
    //}

    // Initialize remote config, and set the default values.
    public void InitializeFirebase()
    {
        // [START set_defaults]
        System.Collections.Generic.Dictionary<string, object> defaults =
          new System.Collections.Generic.Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
        defaults.Add(KEY_REMOTE_ACTIVE_INTER_ADS, true);
        defaults.Add(KEY_REMOTE_GMA_FLAG_GDPR, true);//false
        defaults.Add(KEY_REMOTE_LEVEL_SHOW_RATE, 0);
        defaults.Add(KEY_REMOTE_INTER_LEVEL_SHOW, 0);
        defaults.Add(KEY_REMOTE_INTER_TIME_DELAY_INTER, 10);
        defaults.Add(KEY_REMOTE_TIME_RESET_COLLAPBANNER, 12);
        defaults.Add(KEY_REMOTE_REWARD_VIDEO_TIME_DELAY_INTER, 1);
        defaults.Add(KEY_REMOTE_AOA_TIME_DELAY_INTER, 1);
        defaults.Add(KEY_REMOTE_INTER_AUTO_INGAME, 29);
        defaults.Add(KEY_REMOTE_ENABLE_INTER_BACK_POPUP, false);
        defaults.Add(KEY_REMOTE_AOA_FIRST_OPEN_ACTIVE, true);
        defaults.Add(KEY_REMOTE_AOA_RESUME_ACTIVE, true);
        defaults.Add(KEY_REMOTE_CHECK_CONNECTNETWORK_ACTIVE, true);
        //---------------type user---------------------------------------
        defaults.Add(KEY_REMOTE_ACTIVE_CHECK_TYPE_USER, false);
        defaults.Add(KEY_REMOTE_ACTIVE_BANNER_ALWAY_TYPE_USER, true);

        defaults.Add(KEY_REMOTE_CAPPING_FREE_USER_TIME_INTER_BY_INTER, 10);
        defaults.Add(KEY_REMOTE_CAPPING_FREE_USER_TIME_INTER_BY_REWARD, 10);

        defaults.Add(KEY_REMOTE_CAPPING_PURCHASE_USER_TIME_INTER_BY_INTER, 10);
        defaults.Add(KEY_REMOTE_CAPPING_PURCHASE_USER_TIME_INTER_BY_REWARD, 10);

        defaults.Add(KEY_REMOTE_CAPPING_ADS_USER_TIME_INTER_BY_INTER, 10);
        defaults.Add(KEY_REMOTE_CAPPING_ADS_USER_TIME_INTER_BY_REWARD, 10);

        defaults.Add(KEY_REMOTE_CAPPING_FREE_2_USER_TIME_INTER_BY_INTER, 10);
        defaults.Add(KEY_REMOTE_CAPPING_FREE_2_USER_TIME_INTER_BY_REWARD, 10);

        defaults.Add(KEY_REMOTE_COUNT_PURCHASE_GET_PURCHASE_1, 1);
        defaults.Add(KEY_REMOTE_COUNT_PURCHASE_GET_PURCHASE_2, 2);
        defaults.Add(KEY_REMOTE_COUNT_VIEW_GET_ADS_1, 2);
        defaults.Add(KEY_REMOTE_COUNT_VIEW_GET_ADS_2, 4);

        defaults.Add(KEY_REMOTE_SECOND_BUY_PURCHASE_ADD, 30);
        defaults.Add(KEY_REMOTE_SECOND_VIEW_REWARD_ADD, 10);

        defaults.Add(KEY_REMOTE_LEVEL_FIRST_CHECK_TYPE_USER, 0);
        defaults.Add(KEY_REMOTE_TIME_FIRST_CHECK_TYPE_USER, 10);

        defaults.Add(KEY_REMOTE_MAX_COUNT_CHECK_WAIT, 1);

        defaults.Add(KEY_REMOTE_TILE_WIN_RATE_E, 0.6f);
        defaults.Add(KEY_REMOTE_MAX_COUNT_LEVEL_CHECK_RATE_WIN_E, 1);

        defaults.Add(KEY_REMOTE_MAX_COUNT_LEVEL_CHECK_RATE_WIN_SUM, 2);

        defaults.Add(KEY_REMOTE_TIME_ADD_WAIT_CHECK, 10);

        defaults.Add(KEY_REMOTE_MAX_TIME_START_TO_END_LEVEL, 10);

        defaults.Add(KEY_REMOTE_TIME_ADD_TO_TYPE_USER_DROP, 10);

        defaults.Add(KEY_REMOTE_TIME_MENU_CHECK_USER_WHAT_DOING, 10);
        defaults.Add(KEY_REMOTE_TIME_MAX_MENU_CHECK_TO_LEVEL_START, 10);

        //---------------debug log---------------------------------------
#if UNITY_EDITOR
        defaults.Add(KEY_REMOTE_ACTIVE_DEBUG_LOG, true);
#else
        defaults.Add(KEY_REMOTE_ACTIVE_DEBUG_LOG, false);
#endif
        //------------------------------------------------------
#if ACTIVE_FIREBASE_ANALYTIC
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
          .ContinueWithOnMainThread(task =>
          {
              // [END set_defaults]
              DebugLog("RemoteConfig configured and ready!");
              isFirebaseInitialized = true;

              //FIXME
              FetchDataAsync();
          });
#endif
    }

#if ACTIVE_FIREBASE_ANALYTIC
    // [START fetch_async]
    // Start a fetch request.
    // FetchAsync only fetches new data if the current data is older than the provided
    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
    // By default the timespan is 12 hours, and for production apps, this is a good
    // number. For this example though, it's set to a timespan of zero, so that
    // changes in the console will always show up immediately.
    public Task FetchDataAsync()
    {
        DebugLog("Fetching data...");
        System.Threading.Tasks.Task fetchTask =
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }
    //[END fetch_async]

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            DebugLog("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            DebugLog("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            DebugLog("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                .ContinueWithOnMainThread(task =>
                {
                    DebugLog(String.Format("Remote data loaded and ready (last fetch time {0}).",
                               info.FetchTime));
                    ChangeConfigByFetch();
                });

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        DebugLog("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        DebugLog("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                FailedByFetch();
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                DebugLog("Latest Fetch call still pending.");
                break;
        }
    }
#endif
    public void DebugLog(string s)
    {
        //UnityEngine.Debug.Log(s);
    }
    //--------
    const int MAX_COUNT_FAIL_FETCH = 3;
    static int countFailedFetch = 0;
    void FailedByFetch()
    {
        countFailedFetch++;
        if (countFailedFetch < MAX_COUNT_FAIL_FETCH)
        {
            //fetch again
            FetchDataAsync();
        }
        else
        {
            AdsController.instance.ActiveFetchFailed();
        }
    }
    void ChangeConfigByFetch()
    {
#if ACTIVE_FIREBASE_ANALYTIC
        AdsController.VALUE_CONFIG_LEVEL_SHOW_RATE = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_LEVEL_SHOW_RATE).LongValue;


        AdsController.VALUE_CONFIG_INTER_LEVEL_SHOW = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_INTER_LEVEL_SHOW).LongValue;
        //PlayerPrefs.SetInt(strSaveRemoteLvShow, (int)AdsController.VALUE_CONFIG_INTER_LEVEL_SHOW);
        AdsController.CAPPING_TIME_INTER_BY_INTER_NOW = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_INTER_TIME_DELAY_INTER).DoubleValue;
        AdsController.TIME_DELAY_RESET_COLLAPBANNER = (float)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_TIME_RESET_COLLAPBANNER).DoubleValue;


        AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_REWARD_VIDEO_TIME_DELAY_INTER).DoubleValue;


        Config.ACTIVE_INTER_ADS = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
        .GetValue(KEY_REMOTE_ACTIVE_INTER_ADS).BooleanValue;

        Config.ENABLE_GMA_FLAG_GDPR = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
            .GetValue(KEY_REMOTE_GMA_FLAG_GDPR).BooleanValue;
        Config.ENABLE_INTER_BACK_POPUP = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
            .GetValue(KEY_REMOTE_ENABLE_INTER_BACK_POPUP).BooleanValue;
        Config.TIME_INTER_AUTO_INGAME = (float)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_INTER_AUTO_INGAME).DoubleValue;
        if (Config.TIME_INTER_AUTO_INGAME <= 0)
        {
            Config.TIME_INTER_AUTO_INGAME = 29;
        }

        AdsController.TIME_AOA_SHOWINTERTITIAL = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_AOA_TIME_DELAY_INTER).DoubleValue;

        //PlayerPrefs.SetFloat(strSaveRemoteTimeDelayInter, (float)AdsController.TIME_SHOWREWARD_NOT_SHOWINTERTITIAL);
        AdsController.AOA_FIRST_OPEN_ACTIVE = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_AOA_FIRST_OPEN_ACTIVE).BooleanValue;
        AdsController.AOA_RESUME_ACTIVE = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_AOA_RESUME_ACTIVE).BooleanValue;

        //AdsController.VALUE_SHOW_INTER_PAUSE_GAME = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_VALUE_ADS_PAUSEGAME).LongValue;
        //PlayerPrefs.SetInt(strValueAdsPauseGame, (int)AdsController.VALUE_SHOW_INTER_PAUSE_GAME);
        Config.isCheckConnetNetwork = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_CHECK_CONNECTNETWORK_ACTIVE).BooleanValue;
        //------type user------
        Config.ACTIVE_CHECK_TYPE_USER = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_ACTIVE_CHECK_TYPE_USER).BooleanValue;
        Config.ACTIVE_BANNER_ALWAY_TYPE_USER = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_ACTIVE_BANNER_ALWAY_TYPE_USER).BooleanValue;

        Config.CAPPING_FREE_USER_TIME_INTER_BY_INTER = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_CAPPING_FREE_USER_TIME_INTER_BY_INTER).LongValue;
        Config.CAPPING_FREE_USER_TIME_INTER_BY_REWARD = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_CAPPING_FREE_USER_TIME_INTER_BY_REWARD).LongValue;

        Config.CAPPING_PURCHASE_USER_TIME_INTER_BY_INTER = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_CAPPING_PURCHASE_USER_TIME_INTER_BY_INTER).LongValue;
        Config.CAPPING_PURCHASE_USER_TIME_INTER_BY_REWARD = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_CAPPING_PURCHASE_USER_TIME_INTER_BY_REWARD).LongValue;

        Config.CAPPING_ADS_USER_TIME_INTER_BY_INTER = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_CAPPING_ADS_USER_TIME_INTER_BY_INTER).LongValue;
        Config.CAPPING_ADS_USER_TIME_INTER_BY_REWARD = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_CAPPING_ADS_USER_TIME_INTER_BY_REWARD).LongValue;

        Config.CAPPING_FREE_2_USER_TIME_INTER_BY_INTER = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_CAPPING_FREE_2_USER_TIME_INTER_BY_INTER).LongValue;
        Config.CAPPING_FREE_2_USER_TIME_INTER_BY_REWARD = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_CAPPING_FREE_2_USER_TIME_INTER_BY_REWARD).LongValue;

        Config.COUNT_PURCHASE_GET_PURCHASE_1 = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_COUNT_PURCHASE_GET_PURCHASE_1).LongValue;
        Config.COUNT_PURCHASE_GET_PURCHASE_2 = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_COUNT_PURCHASE_GET_PURCHASE_2).LongValue;
        Config.COUNT_VIEW_GET_ADS_1 = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_COUNT_VIEW_GET_ADS_1).LongValue;
        Config.COUNT_VIEW_GET_ADS_2 = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_COUNT_VIEW_GET_ADS_2).LongValue;

        Config.SECOND_BUY_PURCHASE_ADD = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_SECOND_BUY_PURCHASE_ADD).LongValue;
        Config.SECOND_VIEW_REWARD_ADD = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_SECOND_VIEW_REWARD_ADD).LongValue;

        Config.LEVEL_FIRST_CHECK_TYPE_USER = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_LEVEL_FIRST_CHECK_TYPE_USER).LongValue;
        Config.TIME_FIRST_CHECK_TYPE_USER = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_TIME_FIRST_CHECK_TYPE_USER).LongValue;

        Config.MAX_COUNT_CHECK_WAIT = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_MAX_COUNT_CHECK_WAIT).LongValue;

        Config.TILE_WIN_RATE_E = (float)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_TILE_WIN_RATE_E).DoubleValue;
        Config.MAX_COUNT_LEVEL_CHECK_RATE_WIN_E = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_MAX_COUNT_LEVEL_CHECK_RATE_WIN_E).LongValue;

        Config.MAX_COUNT_LEVEL_CHECK_RATE_WIN_SUM = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_MAX_COUNT_LEVEL_CHECK_RATE_WIN_SUM).LongValue;

        Config.TIME_ADD_WAIT_CHECK = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_TIME_ADD_WAIT_CHECK).LongValue;

        Config.MAX_TIME_START_TO_END_LEVEL = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_MAX_TIME_START_TO_END_LEVEL).LongValue;

        Config.TIME_ADD_TO_TYPE_USER_DROP = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_TIME_ADD_TO_TYPE_USER_DROP).LongValue;

        Config.TIME_MENU_CHECK_USER_WHAT_DOING = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_TIME_MENU_CHECK_USER_WHAT_DOING).LongValue;
        Config.TIME_MAX_MENU_CHECK_TO_LEVEL_START = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_TIME_MAX_MENU_CHECK_TO_LEVEL_START).LongValue;
        //----------Debug Log-------------
#if UNITY_EDITOR
        Config.ACTIVE_DEBUG_LOG = true;
#else
        Config.ACTIVE_DEBUG_LOG = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_ACTIVE_DEBUG_LOG).BooleanValue;
#endif
        //------------------
        ConfigIdsAds.FetchFirebaseDone();

#endif
        AdsController.instance.ActiveFetchFirebaseDone();
    }
}
