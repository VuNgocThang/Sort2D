using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Config
{
    //---pick for true---
    public static bool ACTIVE_INTER_ADS = true;
    public static bool ENABLE_GMA_FLAG_GDPR = false;
    public static bool isCheckConnetNetwork = true;

    public const int MEDIATION_MAX = 1;
    public const int MEDIATION_IRON = 2;

    public const int PUB_G_RK = 1;
    public const int PUB_G_AB = 2;
    public const int PUB_G_FACOL = 3;
    public const int PUB_G_HOPEE = 6;
    //-------------
    public const bool ACTIVE_TEST = false;//false
#if UNITY_EDITOR
    public static bool ACTIVE_DEBUG_LOG = true;//false
#else
    public static bool ACTIVE_DEBUG_LOG = false;
#endif
    public static bool ENABLE_INTER_BACK_POPUP = false;


    public const float TIME_WAIT_LOADING = 10f;
    public static bool FIRST_LOAD_ADS_DONE = false;

    public static float TIME_INTER_AUTO_INGAME = 29f;

    public const int LEVEL_DONE = 300;
    public const int MAX_LEVEL_TEST = 1000;

    public static bool showInterOnPause = true;

    public static int countRestart = 0;

    public static int currLevel = 0;//moi khi player choi 1 man` choi thi` set bang` level cua map choi //FIXME
    //---------
    static float START_POINT_CHECK_TIME = 0;
    //----------------
    public static bool isActiveBanner = true;
    public static bool isActiveInter = true;
    public static bool isActiveAOA = true;
    public static bool isActiveAOA_Switch = true;
    public static bool isActiveVideoReward = true;
    //------
    public const int TYPE_USER_PURCHASE_1 = 1;
    public const int TYPE_USER_PURCHASE_2 = 2;
    public const int TYPE_USER_ADS_1 = 3;
    public const int TYPE_USER_ADS_2 = 4;
    public const int TYPE_USER_FREE = 5;
    public const int TYPE_USER_FREE_2 = 6;
    public const int TYPE_USER_PURCHASE_1_FREE = 7;

    //----*********** remote config **********---
    public static bool ACTIVE_CHECK_TYPE_USER = true;//false

    public static bool ACTIVE_BANNER_ALWAY_TYPE_USER = true;//false

    public static double CAPPING_FREE_USER_TIME_INTER_BY_INTER = 10;
    public static double CAPPING_FREE_USER_TIME_INTER_BY_REWARD = 10;

    public static double CAPPING_PURCHASE_USER_TIME_INTER_BY_INTER = 10;
    public static double CAPPING_PURCHASE_USER_TIME_INTER_BY_REWARD = 10;

    public static double CAPPING_ADS_USER_TIME_INTER_BY_INTER = 10;
    public static double CAPPING_ADS_USER_TIME_INTER_BY_REWARD = 10;

    public static double CAPPING_FREE_2_USER_TIME_INTER_BY_INTER = 10;
    public static double CAPPING_FREE_2_USER_TIME_INTER_BY_REWARD = 10;
    //------main value---
    public static int COUNT_PURCHASE_GET_PURCHASE_1 = 1;
    public static int COUNT_PURCHASE_GET_PURCHASE_2 = 2;
    public static int COUNT_VIEW_GET_ADS_1 = 2;
    public static int COUNT_VIEW_GET_ADS_2 = 4;

    public static int SECOND_BUY_PURCHASE_ADD = 30;//45m 2700
    public static int SECOND_VIEW_REWARD_ADD = 10;//2m 120

    public static int LEVEL_FIRST_CHECK_TYPE_USER = 0;
    public static int TIME_FIRST_CHECK_TYPE_USER = 10;//5m 300

    public static int MAX_COUNT_CHECK_WAIT = 1;//check wwait bao nhieu lan la max

    public static float TILE_WIN_RATE_E = 0.6f;
    public static int MAX_COUNT_LEVEL_CHECK_RATE_WIN_E = 1;

    public static int MAX_COUNT_LEVEL_CHECK_RATE_WIN_SUM = 2;

    public static int TIME_ADD_WAIT_CHECK = 10;//2m 120

    public static int MAX_TIME_START_TO_END_LEVEL = 10;//3m 180

    public static int TIME_ADD_TO_TYPE_USER_DROP = 10;//2m  120
    //---time menu---
    public static int TIME_MENU_CHECK_USER_WHAT_DOING = 10;//thoi gian user o ngaoi menu qua' lau
    public static int TIME_MAX_MENU_CHECK_TO_LEVEL_START = 10;//thoi gian toi da o? ngoai` menu den luc start level

    //-----End remote config-------------

    public static int COUNT_TIME_INGAME = 0;
    //----------------
#region check user
    public const int MAX_VALUE_RATE_WIN = int.MaxValue / 3;

    public static bool isFirstCheckUserDone = false;

    public static int countLevelStart_Sum = 0;
    public static int countLevelLose_Sum = 0;
    public static int countLevelWin_Sum = 0;
    public static int countLevelQuit_Sum = 0;

    public static int countLevelStart_E = 0;
    public static int countLevelLose_E = 0;
    public static int countLevelWin_E = 0;
    public static int countLevelQuit_E = 0;

    //public static bool isActiveWait = false;
    //public static int countCheckWait = 0;

    public static int TYPE_USER_NOW = TYPE_USER_FREE;
    public static int countRewardedAds = 0;
    public static int countPurchase = 0;

    const string key_first_setup_type_user = "dai_first_setup_type_user";
    const string key_first_detect_type_user = "dai_first_detect_type_user";
    const string key_type_user = "dai_type_user";
    const string key_count_time_type_user = "dai_count_time_type_user";
    public static string GetLogStringDebugTypeUser()
    {
        StringBuilder strBuilderResulf = new StringBuilder();
        strBuilderResulf.Append("f_setup: " + Get_FirstSetup());
        strBuilderResulf.Append(",  f_TypeUser: " + Get_FirstDetectTypeUser());
        strBuilderResulf.Append("\n, TypeUser: " + GetTypeUser_Debug());
        strBuilderResulf.Append(",  ctime_TypeU: " + Get_CountTimeTypeUser());
        strBuilderResulf.Append(",  c_rv_ads: " + Get_CountRewardedAds());
        strBuilderResulf.Append(",  c_perchase: " + Get_CountBuyPurchase());
        strBuilderResulf.Append("\n, c_lvS_s: " + Get_CountLevelStartSum());
        strBuilderResulf.Append(",  c_lvL_s: " + Get_CountLevelLoseSum());
        strBuilderResulf.Append(",  c_lvW_s: " + Get_CountLevelWinSum());
        strBuilderResulf.Append(",  c_lvQ_s: " + Get_CountLevelQuitSum());
        strBuilderResulf.Append("\n, c_lvS_E: " + Get_CountLevelStartE());
        strBuilderResulf.Append(",  c_lvL_E: " + Get_CountLevelLoseE());
        strBuilderResulf.Append(",  c_lvW_E: " + Get_CountLevelWinE());
        strBuilderResulf.Append(",  c_lvQ_E: " + Get_CountLevelQuitE());
        strBuilderResulf.Append("\n, c_cW: " + Get_CountCheckWait());
        strBuilderResulf.Append(",  c_cW_S: " + Get_CountCheckWaitSum());
        if (strLogLoad == null)
        {
            return strBuilderResulf.ToString();
        }
        else
        {
            return strBuilderResulf.ToString() + "\n" + strLogLoad.ToString();
        }
    }
    //----------
    public static StringBuilder strLogLoad = null;
    public static void AddLogShowDebug(string nameLog)
    {
        if (ConfigIdsAds.TEST_TYPE_USER)
        {
            if (strLogLoad == null)
            {
                strLogLoad = new StringBuilder();
            }
            strLogLoad.Append(Time.time + ": " + nameLog + " -> ");
        }
    }
    //---------------
    public static bool InitedDetectUser = false;
    public static void FirstCheckTypeUser()
    {
        if (InitedDetectUser)
        {
            return;
        }
        InitedDetectUser = true;
        bool firstSetup = Get_FirstSetup();
        isFirstCheckUserDone = Get_FirstDetectTypeUser();

        countRewardedAds = Get_CountRewardedAds();
        countPurchase = Get_CountBuyPurchase();

        countLevelStart_Sum = Get_CountLevelStartSum();
        countLevelLose_Sum = Get_CountLevelLoseSum();
        countLevelWin_Sum = Get_CountLevelWinSum();
        countLevelQuit_Sum = Get_CountLevelQuitSum();

        countLevelStart_E = Get_CountLevelStartE();
        countLevelLose_E = Get_CountLevelLoseE();
        countLevelWin_E = Get_CountLevelWinE();
        countLevelQuit_E = Get_CountLevelQuitE();


        if (!firstSetup)
        {
            TYPE_USER_NOW = TYPE_USER_FREE;
            COUNT_TIME_INGAME = TIME_FIRST_CHECK_TYPE_USER;
            Set_TypeUser(TYPE_USER_NOW);
            Set_CountTimeTypeUser(COUNT_TIME_INGAME);
            Active_FirstSetup();
            PlayerPrefs.Save();
        }
        else
        {
            TYPE_USER_NOW = Get_TypeUser();
            COUNT_TIME_INGAME = Get_CountTimeTypeUser();
        }
        CheckSetActiveAds();
    }
    //--------
    static void CheckSetActiveAds()
    {
        if (!Config.ACTIVE_CHECK_TYPE_USER)
        {
            isActiveBanner = true;
            isActiveInter = true;
            isActiveAOA = true;
            isActiveAOA_Switch = true;
            isActiveVideoReward = true;
            AdsController.instance.CheckActiveChangeTypeUser();
            return;
        }
        if (isFirstCheckUserDone)
        {

            switch (TYPE_USER_NOW)
            {
                case TYPE_USER_PURCHASE_1:
                    isActiveBanner = false;
                    isActiveInter = false;
                    isActiveAOA = true;
                    isActiveAOA_Switch = false;
                    isActiveVideoReward = true;
                    //if (ACTIVE_CHECK_TYPE_USER)
                    {
                        AdsController.CAPPING_TIME_INTER_BY_INTER_NOW = CAPPING_PURCHASE_USER_TIME_INTER_BY_INTER;
                        AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = CAPPING_PURCHASE_USER_TIME_INTER_BY_REWARD;
                    }
                    break;
                case TYPE_USER_PURCHASE_2:
                    isActiveBanner = false;
                    isActiveInter = false;
                    isActiveAOA = false;
                    isActiveAOA_Switch = false;
                    isActiveVideoReward = true;
                    //if (ACTIVE_CHECK_TYPE_USER)
                    {
                        AdsController.CAPPING_TIME_INTER_BY_INTER_NOW = CAPPING_PURCHASE_USER_TIME_INTER_BY_INTER;
                        AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = CAPPING_PURCHASE_USER_TIME_INTER_BY_REWARD;
                    }
                    break;
                case TYPE_USER_ADS_1:
                    isActiveBanner = true;
                    isActiveInter = false;
                    isActiveAOA = true;
                    isActiveAOA_Switch = true;
                    isActiveVideoReward = true;
                    //if (ACTIVE_CHECK_TYPE_USER)
                    {
                        AdsController.CAPPING_TIME_INTER_BY_INTER_NOW = CAPPING_ADS_USER_TIME_INTER_BY_INTER;
                        AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = CAPPING_ADS_USER_TIME_INTER_BY_REWARD;
                    }
                    break;
                case TYPE_USER_ADS_2:
                    isActiveBanner = true;
                    isActiveInter = false;
                    isActiveAOA = true;
                    isActiveAOA_Switch = true;
                    isActiveVideoReward = true;
                    //if (ACTIVE_CHECK_TYPE_USER)
                    {
                        AdsController.CAPPING_TIME_INTER_BY_INTER_NOW = CAPPING_ADS_USER_TIME_INTER_BY_INTER;
                        AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = CAPPING_ADS_USER_TIME_INTER_BY_REWARD;
                    }
                    break;
                case TYPE_USER_FREE:
                    isActiveBanner = true;
                    isActiveInter = true;
                    isActiveAOA = true;
                    isActiveAOA_Switch = true;
                    isActiveVideoReward = true;
                    //if (ACTIVE_CHECK_TYPE_USER)
                    {
                        AdsController.CAPPING_TIME_INTER_BY_INTER_NOW = CAPPING_FREE_USER_TIME_INTER_BY_INTER;
                        AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = CAPPING_FREE_USER_TIME_INTER_BY_REWARD;
                    }
                    break;
                case TYPE_USER_FREE_2:
                    isActiveBanner = true;
                    isActiveInter = true;
                    isActiveAOA = true;
                    isActiveAOA_Switch = true;
                    isActiveVideoReward = true;
                    //if (ACTIVE_CHECK_TYPE_USER)
                    {
                        AdsController.CAPPING_TIME_INTER_BY_INTER_NOW = CAPPING_FREE_2_USER_TIME_INTER_BY_INTER;
                        AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = CAPPING_FREE_2_USER_TIME_INTER_BY_REWARD;
                    }
                    break;
                case TYPE_USER_PURCHASE_1_FREE:
                    isActiveBanner = true;
                    isActiveInter = true;
                    isActiveAOA = true;
                    isActiveAOA_Switch = true;
                    isActiveVideoReward = true;
                    //if (ACTIVE_CHECK_TYPE_USER)
                    {
                        AdsController.CAPPING_TIME_INTER_BY_INTER_NOW = CAPPING_PURCHASE_USER_TIME_INTER_BY_INTER;
                        AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = CAPPING_PURCHASE_USER_TIME_INTER_BY_REWARD;
                    }
                    break;
                default:
                    isActiveBanner = true;
                    isActiveInter = true;
                    isActiveAOA = true;
                    isActiveAOA_Switch = true;
                    isActiveVideoReward = true;
                    //if (ACTIVE_CHECK_TYPE_USER)
                    {
                        AdsController.CAPPING_TIME_INTER_BY_INTER_NOW = CAPPING_FREE_USER_TIME_INTER_BY_INTER;
                        AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = CAPPING_FREE_USER_TIME_INTER_BY_REWARD;
                    }
                    break;
            }
        }
        else
        {
            isActiveBanner = false;
            isActiveInter = false;
            isActiveAOA = false;
            isActiveAOA_Switch = false;
            isActiveVideoReward = true;
            //if (ACTIVE_CHECK_TYPE_USER)
            {
                AdsController.CAPPING_TIME_INTER_BY_INTER_NOW = CAPPING_FREE_USER_TIME_INTER_BY_INTER;
                AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = CAPPING_FREE_USER_TIME_INTER_BY_REWARD;
            }
        }
        if (Config.ACTIVE_BANNER_ALWAY_TYPE_USER)
        {
            isActiveBanner = true;
        }
        AdsController.instance.CheckActiveChangeTypeUser();
    }
    //-------
    const int MAX_VALUE_TIME_MENU = 9999999;
    static bool isActiveTimeMenu = false;
    static int countTimeMenu = 0;
    static void ActiveTimeMenu()
    {
        //if (!isActiveTimeMenu)
        {
            isActiveTimeMenu = true;
            countTimeMenu = 0;
            AdsController.instance.ActiveCountTimeMenu();
        }
    }
    static void CheckEndTimeMenu()
    {
        isActiveTimeMenu = false;
        countTimeMenu = 0;
        AdsController.instance.EndCountTimeMenu();
        if (countTimeMenu >= TIME_MAX_MENU_CHECK_TO_LEVEL_START)
        {
            //--user vao` game play ----

        }
    }
    public static void UpdateTimeMenu(int timeAdd)
    {
        if (isActiveTimeMenu)
        {
            countTimeMenu += timeAdd;
            if (countTimeMenu > MAX_VALUE_TIME_MENU)
            {
                countTimeMenu = MAX_VALUE_TIME_MENU;
            }
            if (countTimeMenu >= TIME_MENU_CHECK_USER_WHAT_DOING)
            {
                //--user o ngoai menu 1 thoi gian---

            }
        }
    }
    static string GetTypeUser_Debug()
    {
        string str = "ERROR";
        switch (TYPE_USER_NOW)
        {
            case TYPE_USER_PURCHASE_1:
                str = "TYPE_USER_PURCHASE_1";
                break;
            case TYPE_USER_PURCHASE_2:
                str = "TYPE_USER_PURCHASE_2";
                break;
            case TYPE_USER_ADS_1:
                str = "TYPE_USER_ADS_1";
                break;
            case TYPE_USER_ADS_2:
                str = "TYPE_USER_ADS_2";
                break;
            case TYPE_USER_FREE:
                str = "TYPE_USER_FREE";
                break;
            case TYPE_USER_FREE_2:
                str = "TYPE_USER_FREE_2";
                break;
            case TYPE_USER_PURCHASE_1_FREE:
                str = "TYPE_USER_PURCHASE_1_FREE";
                break;
            default:
                break;
        }
        return str;
    }
    //--------------
#region func save
    static void Active_FirstSetup()
    {
        PlayerPrefs.SetInt(key_first_setup_type_user, 1);
    }
    static bool Get_FirstSetup()
    {
        return (PlayerPrefs.GetInt(key_first_setup_type_user, 0) == 1);
    }
    static void Active_FirstDetectTypeUser()
    {
        PlayerPrefs.SetInt(key_first_detect_type_user, 1);
    }
    static bool Get_FirstDetectTypeUser()
    {
        return (PlayerPrefs.GetInt(key_first_detect_type_user, 0) == 1);
    }
    static void Set_TypeUser(int value)
    {
        PlayerPrefs.SetInt(key_type_user, value);
    }
    static int Get_TypeUser()
    {
        return PlayerPrefs.GetInt(key_type_user, TYPE_USER_FREE);
    }
    static void Set_CountTimeTypeUser(int value)
    {
        PlayerPrefs.SetInt(key_count_time_type_user, value);
    }
    static int Get_CountTimeTypeUser()
    {
        return PlayerPrefs.GetInt(key_count_time_type_user, 0);
    }
    //----------------
    static void Set_CountRewardedAds(int value)
    {
        PlayerPrefs.SetInt(key_count_rewarded_ads, value);
    }
    static int Get_CountRewardedAds()
    {
        return PlayerPrefs.GetInt(key_count_rewarded_ads, 0);
    }
    static void Set_CountBuyPurchase(int value)
    {
        PlayerPrefs.SetInt(key_count_purchase, value);
    }
    static int Get_CountBuyPurchase()
    {
        return PlayerPrefs.GetInt(key_count_purchase, 0);
    }
    //---
    static void Set_CountLevelStartSum(int value)
    {
        PlayerPrefs.SetInt(key_count_level_start_sum, value);
    }
    static int Get_CountLevelStartSum()
    {
        return PlayerPrefs.GetInt(key_count_level_start_sum, 0);
    }
    static void Set_CountLevelLoseSum(int value)
    {
        PlayerPrefs.SetInt(key_count_level_lose_sum, value);
    }
    static int Get_CountLevelLoseSum()
    {
        return PlayerPrefs.GetInt(key_count_level_lose_sum, 0);
    }
    static void Set_CountLevelWinSum(int value)
    {
        PlayerPrefs.SetInt(key_count_level_win_sum, value);
    }
    static int Get_CountLevelWinSum()
    {
        return PlayerPrefs.GetInt(key_count_level_win_sum, 0);
    }
    static void Set_CountLevelQuitSum(int value)
    {
        PlayerPrefs.SetInt(key_count_level_quit_sum, value);
    }
    static int Get_CountLevelQuitSum()
    {
        return PlayerPrefs.GetInt(key_count_level_quit_sum, 0);
    }
    static void Set_CountLevelStartE(int value)
    {
        PlayerPrefs.SetInt(key_count_level_start_e, value);
    }
    static int Get_CountLevelStartE()
    {
        return PlayerPrefs.GetInt(key_count_level_start_e, 0);
    }
    static void Set_CountLevelLoseE(int value)
    {
        PlayerPrefs.SetInt(key_count_level_lose_e, value);
    }
    static int Get_CountLevelLoseE()
    {
        return PlayerPrefs.GetInt(key_count_level_lose_e, 0);
    }
    static void Set_CountLevelWinE(int value)
    {
        PlayerPrefs.SetInt(key_count_level_win_e, value);
    }
    static int Get_CountLevelWinE()
    {
        return PlayerPrefs.GetInt(key_count_level_win_e, 0);
    }
    static void Set_CountLevelQuitE(int value)
    {
        PlayerPrefs.SetInt(key_count_level_quit_e, value);
    }
    static int Get_CountLevelQuitE()
    {
        return PlayerPrefs.GetInt(key_count_level_quit_e, 0);
    }

    static void Set_CountCheckWait(int value)
    {
        PlayerPrefs.SetInt(key_count_check_wait, value);
    }
    static int Get_CountCheckWait()
    {
        return PlayerPrefs.GetInt(key_count_check_wait, 0);
    }
    static void Set_CountCheckWaitSum(int value)
    {
        PlayerPrefs.SetInt(key_count_check_wait_sum, value);
    }
    static int Get_CountCheckWaitSum()
    {
        return PlayerPrefs.GetInt(key_count_check_wait_sum, 0);
    }
#endregion
    //----------------
    public static void Add_TimeIngameCheck(int timeAdd)
    {
        COUNT_TIME_INGAME += timeAdd;
        Set_CountTimeTypeUser(COUNT_TIME_INGAME);
        PlayerPrefs.Save();
    }
    static void SetFirstDetectTypeUser(bool value)
    {
        isFirstCheckUserDone = Get_FirstDetectTypeUser();
        if (!isFirstCheckUserDone)
        {
            Active_FirstDetectTypeUser();
        }
        isFirstCheckUserDone = Get_FirstDetectTypeUser();
    }

    const string key_count_rewarded_ads = "dai_count_rewarded_ads";
    public static void AddCountRewardedAds()
    {
        Add_TimeIngameCheck(SECOND_VIEW_REWARD_ADD);
        Set_CountCheckWait(0);

        Set_CountLevelStartE(0);
        Set_CountLevelLoseE(0);
        Set_CountLevelWinE(0);
        Set_CountLevelQuitE(0);

        int countRewardedAdsCheck = Get_CountRewardedAds();
        if (countRewardedAdsCheck == MAX_VALUE_RATE_WIN)
        {
            return;
        }
        countRewardedAdsCheck += 1;
        countRewardedAds += 1;
        Set_CountRewardedAds(countRewardedAdsCheck);
        CheckActionBuyView(false, countRewardedAdsCheck);
        PlayerPrefs.Save();
    }

    const string key_count_purchase = "dai_count_purchase";
    public static void AddCountPurchase()
    {
        Add_TimeIngameCheck(SECOND_BUY_PURCHASE_ADD);
        Set_CountCheckWait(0);

        Set_CountLevelStartE(0);
        Set_CountLevelLoseE(0);
        Set_CountLevelWinE(0);
        Set_CountLevelQuitE(0);

        int countPurchaseCheck = Get_CountBuyPurchase();
        if (countPurchaseCheck == MAX_VALUE_RATE_WIN)
        {
            return;
        }
        countPurchaseCheck += 1;
        countPurchase += 1;
        Set_CountBuyPurchase(countPurchaseCheck);
        CheckActionBuyView(true, countPurchaseCheck);
        PlayerPrefs.Save();
    }
    //-----
    const string key_count_level_start_sum = "dai_count_level_start_sum";
    public static void AddCountLevelStartSum()
    {
        START_POINT_CHECK_TIME = Time.time;

        int countLevelStartSumCheck = Get_CountLevelStartSum();
        if (countLevelStartSumCheck >= MAX_VALUE_RATE_WIN)
        {
            return;
        }
        countLevelStartSumCheck += 1;
        countLevelStart_Sum += 1;
        Set_CountLevelStartSum(countLevelStartSumCheck);
        //----
        AddCountLevelStartE();
        //------
        PlayerPrefs.Save();
        //----------
        CheckEndTimeMenu();
    }
    const string key_count_level_lose_sum = "dai_count_level_lose_sum";
    public static void AddCountLevelLoseSum()
    {
        int countLevelLoseSumCheck = Get_CountLevelLoseSum();
        if (countLevelLoseSumCheck >= MAX_VALUE_RATE_WIN)
        {
            return;
        }
        countLevelLoseSumCheck += 1;
        countLevelLose_Sum += 1;
        Set_CountLevelLoseSum(countLevelLoseSumCheck);
        //----
        AddCountLevelLoseE();
        //------
        CheckEndLevelSum();
        PlayerPrefs.Save();
    }
    const string key_count_level_win_sum = "dai_count_level_win_sum";
    public static void AddCountLevelWinSum()
    {
        int countLevelWinSumCheck = Get_CountLevelWinSum();
        if (countLevelWinSumCheck >= MAX_VALUE_RATE_WIN)
        {
            return;
        }
        countLevelWinSumCheck += 1;
        countLevelWin_Sum += 1;
        Set_CountLevelWinSum(countLevelWinSumCheck);
        //----
        AddCountLevelWinE();
        //------
        CheckEndLevelSum();
        PlayerPrefs.Save();
    }
    const string key_count_level_quit_sum = "dai_count_level_quit_sum";
    public static void AddCountLevelQuitSum()
    {
        int countLevelQuitSumCheck = Get_CountLevelQuitSum();
        if (countLevelQuitSumCheck >= MAX_VALUE_RATE_WIN)
        {
            return;
        }
        countLevelQuitSumCheck += 1;
        countLevelQuit_Sum += 1;
        Set_CountLevelQuitSum(countLevelQuitSumCheck);
        //----
        AddCountLevelQuitE();
        //------
        CheckEndLevelSum();
        PlayerPrefs.Save();
    }
    //-----
    static void CheckEndLevelSum()
    {
        float timeEnd = Time.time;
        float deltaTime = timeEnd - START_POINT_CHECK_TIME;
        if (deltaTime <= 0)
        {
            //--chac la bi vuot qua'-- reset cho xong--
            //START_POINT_CHECK_TIME = timeEnd;
        }
        else
        {
            if (deltaTime > MAX_TIME_START_TO_END_LEVEL)
            {
                deltaTime = MAX_TIME_START_TO_END_LEVEL;
            }
            COUNT_TIME_INGAME -= (int)deltaTime;
            if (COUNT_TIME_INGAME <= 0)
            {
                COUNT_TIME_INGAME = 0;
                CheckOutTimeBuyView();
            }
            Set_CountTimeTypeUser(COUNT_TIME_INGAME);
        }
        START_POINT_CHECK_TIME = timeEnd;
        //---------
        ActiveTimeMenu();
    }
    //-----
    const string key_count_level_start_e = "dai_count_level_start_e";
    static void AddCountLevelStartE()
    {
        int countLevelStartECheck = Get_CountLevelStartE();
        if (countLevelStartECheck >= MAX_VALUE_RATE_WIN)
        {
            return;
        }
        countLevelStartECheck += 1;
        countLevelStart_E += 1;
        Set_CountLevelStartE(countLevelStartECheck);
        PlayerPrefs.Save();
    }
    const string key_count_level_lose_e = "dai_count_level_lose_e";
    static void AddCountLevelLoseE()
    {
        int countLevelLoseECheck = Get_CountLevelLoseE();
        if (countLevelLoseECheck >= MAX_VALUE_RATE_WIN)
        {
            return;
        }
        countLevelLoseECheck += 1;
        countLevelLose_E += 1;
        Set_CountLevelLoseE(countLevelLoseECheck);
        PlayerPrefs.Save();
    }
    const string key_count_level_win_e = "dai_count_level_win_e";
    static void AddCountLevelWinE()
    {
        int countLevelWinECheck = Get_CountLevelWinE();
        if (countLevelWinECheck >= MAX_VALUE_RATE_WIN)
        {
            return;
        }
        countLevelWinECheck += 1;
        countLevelWin_E += 1;
        Set_CountLevelWinE(countLevelWinECheck);
        PlayerPrefs.Save();
    }
    const string key_count_level_quit_e = "dai_count_level_quit_e";
    static void AddCountLevelQuitE()
    {
        int countLevelQuitECheck = Get_CountLevelQuitE();
        if (countLevelQuitECheck >= MAX_VALUE_RATE_WIN)
        {
            return;
        }
        countLevelQuitECheck += 1;
        countLevelQuit_E += 1;
        Set_CountLevelQuitE(countLevelQuitECheck);
        PlayerPrefs.Save();
    }
    public static void ResetCountLevelE()
    {
        Set_CountLevelStartE(0);
        Set_CountLevelLoseE(0);
        Set_CountLevelWinE(0);
        Set_CountLevelQuitE(0);
        PlayerPrefs.Save();
    }
    //-----
    const string key_count_check_wait = "dai_count_check_wait";

    const string key_count_check_wait_sum = "dai_count_check_wait_sum";

    public static bool AddCountCheckWait()
    {
        //Debug.Log("---AddCountCheckWait");
        int countCheckWaitCheckSum = Get_CountCheckWaitSum();
        if (countCheckWaitCheckSum >= MAX_COUNT_LEVEL_CHECK_RATE_WIN_SUM)
        {
            return false;
        }
        int countCheckWaitCheck = Get_CountCheckWait();
        if (countCheckWaitCheck < MAX_COUNT_CHECK_WAIT)
        {
            if (countCheckWaitCheck >= int.MaxValue)
            {
                return false;
            }
            //Debug.Log("---AddCountCheckWait 111111111");
            countCheckWaitCheck += 1;
            Set_CountCheckWait(countCheckWaitCheck);
            if (countCheckWaitCheckSum < int.MaxValue)
            {
                Set_CountCheckWaitSum(countCheckWaitCheckSum + 1);
            }
            //---set wait ----> reset data---
            ResetCountLevelE();
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }
    static void ResetCountCheckWait()
    {
        //Debug.Log("---ResetCountCheckWait");
        Set_CountCheckWait(0);
        PlayerPrefs.Save();
    }
    //--------
    static float CheckTileWin_E(int countWinE, int countLoseE, int countQuitE)
    {
        float resulf = 0;

        //int countSumE = countWinE + countLoseE + countQuitE;
        //if (countSumE >= MAX_COUNT_LEVEL_CHECK_RATE_WIN_E)
        //{
        if (countWinE == 0)
        {
            if (countLoseE == 0 && countQuitE == 0)
            {
                //--chua choi lan nao ???
                resulf = 0;
            }
            else
            {
                //ko win dc lan nao ...
                resulf = 0;
            }
        }
        else
        {
            //co win
            resulf = (float)countWinE / (countWinE + countQuitE + countLoseE);
        }
        //}
        //else
        //{
        //    //---chua du lan choi de doan---
        //    resulf = 1;
        //    //--------
        //}
        return resulf;
    }
    static float ActiveCheckLevelWinRate()
    {
        float rate = 1;
        int countLoseE = Get_CountLevelLoseE();
        int countWinE = Get_CountLevelWinE();
        int countQuitE = Get_CountLevelQuitE();

        int countSumE = countWinE + countLoseE + countQuitE;
        if (countSumE >= MAX_COUNT_LEVEL_CHECK_RATE_WIN_E)
        {
            rate = CheckTileWin_E(countWinE, countLoseE, countQuitE);
        }
        else
        {
            //---chua du lan choi de doan---=> check sum
            countLoseE = Get_CountLevelLoseSum();
            countWinE = Get_CountLevelWinSum();
            countQuitE = Get_CountLevelQuitSum();

            rate = CheckTileWin_E(countWinE, countLoseE, countQuitE);

        }
        //Debug.Log("---ActiveCheckLevelWinRate: " + rate);
        return rate;
    }
    //---------
    public static void CheckActionBuyView(bool isPurchase, int sumValue)
    {
        //---Khi mua purchase hoac view rv---
        int oldValue = TYPE_USER_NOW;
        switch (TYPE_USER_NOW)
        {
            //case TYPE_USER_WAIT_CHECK:
            //    if (isPurchase)
            //    {
            //        if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_2)
            //        {
            //            TYPE_USER_NOW = TYPE_USER_PURCHASE_2;
            //        }
            //        else if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_1)
            //        {
            //            TYPE_USER_NOW = TYPE_USER_PURCHASE_1;
            //        }
            //        else
            //        {
            //            TYPE_USER_NOW = TYPE_USER_WAIT_CHECK;
            //        }
            //    }
            //    else
            //    {
            //        if (sumValue >= TYPE_USER_ADS_2)
            //        {
            //            TYPE_USER_NOW = TYPE_USER_ADS_2;
            //        }
            //        else if (sumValue >= TYPE_USER_ADS_1)
            //        {
            //            TYPE_USER_NOW = TYPE_USER_ADS_1;
            //        }
            //    }
            //    break;
            case TYPE_USER_PURCHASE_1_FREE:
                if (isPurchase)
                {
                    if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_2)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_2;
                    }
                    else if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_1)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_1;
                    }
                }
                break;
            case TYPE_USER_PURCHASE_1:
                if (isPurchase)
                {
                    if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_2)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_2;
                    }
                }
                break;
            case TYPE_USER_PURCHASE_2:
                break;
            case TYPE_USER_ADS_1:
                if (isPurchase)
                {
                    if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_2)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_2;
                    }
                    else if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_1)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_1;
                    }
                    else
                    {
                        //SetActiveCheckWait(true);
                        //TYPE_USER_NOW = TYPE_USER_WAIT_CHECK;
                    }
                }
                else
                {
                    if (sumValue >= COUNT_VIEW_GET_ADS_2)
                    {
                        TYPE_USER_NOW = TYPE_USER_ADS_2;
                    }
                }
                break;
            case TYPE_USER_ADS_2:
                if (isPurchase)
                {
                    if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_2)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_2;
                    }
                    else if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_1)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_1;
                    }
                    else
                    {
                        //SetActiveCheckWait(true);
                        //TYPE_USER_NOW = TYPE_USER_WAIT_CHECK;
                    }
                }
                break;
            case TYPE_USER_FREE:
                if (isPurchase)
                {
                    if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_2)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_2;
                    }
                    else if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_1)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_1;
                    }
                    else
                    {
                        //SetActiveCheckWait(true);
                        //TYPE_USER_NOW = TYPE_USER_WAIT_CHECK;
                    }
                }
                else
                {
                    if (sumValue >= COUNT_VIEW_GET_ADS_2)
                    {
                        TYPE_USER_NOW = TYPE_USER_ADS_2;
                    }
                    else if (sumValue >= COUNT_VIEW_GET_ADS_1)
                    {
                        TYPE_USER_NOW = TYPE_USER_ADS_1;
                    }
                }
                break;
            case TYPE_USER_FREE_2:
                if (isPurchase)
                {
                    if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_2)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_2;
                    }
                    else if (sumValue >= COUNT_PURCHASE_GET_PURCHASE_1)
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_1;
                    }
                    else
                    {
                        //SetActiveCheckWait(true);
                        TYPE_USER_NOW = TYPE_USER_FREE;
                    }
                }
                else
                {
                    if (sumValue >= COUNT_VIEW_GET_ADS_2)
                    {
                        TYPE_USER_NOW = TYPE_USER_ADS_2;
                    }
                    else if (sumValue >= COUNT_VIEW_GET_ADS_1)
                    {
                        TYPE_USER_NOW = TYPE_USER_ADS_1;
                    }
                    else
                    {
                        TYPE_USER_NOW = TYPE_USER_FREE;
                    }
                }
                break;
            default:
                Debug.LogError("ERROR  : " + TYPE_USER_NOW);
                break;
        }
        if (oldValue != TYPE_USER_NOW)
        {
            //SetActiveCheckWait(false);
            //ResetCountLevelE();
            Set_TypeUser(TYPE_USER_NOW);
        }
        if (!isFirstCheckUserDone)
        {
            SetFirstDetectTypeUser(true);
        }
        CheckSetActiveAds();
        //Debug.Log("---CheckActionBuyView: " + TYPE_USER_NOW);
    }

    public static void CheckOutTimeBuyView()
    {
        //---check khi het time cho type user do---
        float rate = ActiveCheckLevelWinRate();
        int oldValue = TYPE_USER_NOW;
        switch (TYPE_USER_NOW)
        {
            case TYPE_USER_PURCHASE_1_FREE:
                TYPE_USER_NOW = TYPE_USER_PURCHASE_1_FREE;
                break;
            case TYPE_USER_PURCHASE_1:
                if (rate <= TILE_WIN_RATE_E)
                {
                    TYPE_USER_NOW = TYPE_USER_PURCHASE_1_FREE;
                }
                else
                {
                    //---doi 1 turn nua---
                    if (AddCountCheckWait())
                    {
                        Add_TimeIngameCheck(TIME_ADD_WAIT_CHECK);
                    }
                    else
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_1_FREE;
                    }
                }
                break;
            case TYPE_USER_PURCHASE_2:
                //TYPE_USER_NOW = TYPE_USER_PURCHASE_1;
                if (rate <= TILE_WIN_RATE_E)
                {
                    TYPE_USER_NOW = TYPE_USER_PURCHASE_1;
                }
                else
                {
                    //---doi 1 turn nua---
                    if (AddCountCheckWait())
                    {
                        Add_TimeIngameCheck(TIME_ADD_WAIT_CHECK);
                    }
                    else
                    {
                        TYPE_USER_NOW = TYPE_USER_PURCHASE_1;
                    }
                }
                break;
            case TYPE_USER_ADS_1:
                //TYPE_USER_NOW = TYPE_USER_FREE;
                if (rate <= TILE_WIN_RATE_E)
                {
                    TYPE_USER_NOW = TYPE_USER_FREE;
                }
                else
                {
                    //---doi 1 turn nua---
                    if (AddCountCheckWait())
                    {
                        Add_TimeIngameCheck(TIME_ADD_WAIT_CHECK);
                    }
                    else
                    {
                        TYPE_USER_NOW = TYPE_USER_FREE;
                    }
                }
                break;
            case TYPE_USER_ADS_2:
                //TYPE_USER_NOW = TYPE_USER_ADS_1;
                if (rate <= TILE_WIN_RATE_E)
                {
                    TYPE_USER_NOW = TYPE_USER_ADS_1;
                }
                else
                {
                    //---doi 1 turn nua---
                    if (AddCountCheckWait())
                    {
                        Add_TimeIngameCheck(TIME_ADD_WAIT_CHECK);
                    }
                    else
                    {
                        TYPE_USER_NOW = TYPE_USER_ADS_1;
                    }
                }
                break;
            case TYPE_USER_FREE:
                //TYPE_USER_NOW = TYPE_USER_FREE_2;
                if (rate <= TILE_WIN_RATE_E)
                {
                    TYPE_USER_NOW = TYPE_USER_FREE_2;
                }
                else
                {
                    //---doi 1 turn nua---
                    if (AddCountCheckWait())
                    {
                        Add_TimeIngameCheck(TIME_ADD_WAIT_CHECK);
                    }
                    else
                    {
                        TYPE_USER_NOW = TYPE_USER_FREE_2;
                    }
                }
                break;
            case TYPE_USER_FREE_2:
                TYPE_USER_NOW = TYPE_USER_FREE_2;
                break;
            default:
                Debug.LogError("ERROR  : " + TYPE_USER_NOW);
                break;
        }
        if (oldValue != TYPE_USER_NOW)
        {
            //SetActiveCheckWait(false);
            ResetCountLevelE();
            ResetCountCheckWait();
            Set_TypeUser(TYPE_USER_NOW);
            if (oldValue == TYPE_USER_PURCHASE_2
                || oldValue == TYPE_USER_ADS_1
                || oldValue == TYPE_USER_ADS_2)
            {
                //---khi xuong -> them Time---
                Add_TimeIngameCheck(TIME_ADD_TO_TYPE_USER_DROP);
            }
        }
        if (!isFirstCheckUserDone)
        {
            SetFirstDetectTypeUser(true);
        }
        CheckSetActiveAds();
        //Debug.Log("---CheckOutTimeBuyView: " + TYPE_USER_NOW);
    }
#endregion

    //void FuncTest()
    //{
    //    //1
    //    Config.currLevel =??; //Set giá trị level hiện tại "tính từ 1", khi vào menu (home) và vào gameplay
    //    //2
    //    FirebaseManager.instance.LogLevelStart(level); //Gọi khi vào Start Gameplay truyền lv màn chơi vào
    //    //3
    //    FirebaseManager.instance.LogLevelLose(level, timeSecond); //Gọi khi thua trong Gameplay, truyền lv màn chơi và thời gian tính theo giây chơi vào
    //    //4
    //    FirebaseManager.instance.LogLevelWin(level, timeSecond); //Gọi khi thắng trong Gameplay, truyền lv màn chơi và thời gian tính theo giây chơi vào
    //    //5
    //    FirebaseManager.instance.LogLevelReplay(level, timeSecond); //Gọi khi ấn nút retry (replay) trong Gameplay khi đang trong game, truyền lv màn chơi và thời gian tính theo giây chơi vào
    //    //6
    //    Config.AddGoldSpent(valueGold, where);// Gọi khi chi tiêu gold , truyền vào số lượng và chỗ chi tiêu ở đâu
    //    //7
    //    Config.AddGoldEarn(valueGold, where);// Gọi khi chi nhận đc gold , truyền vào số lượng và chỗ nhận đc gold ở đâu
    //    //8
    //    Config.AddGemSpent(valueGold, where);// Gọi khi chi tiêu Gem (hoặc đá quý) , truyền vào số lượng và chỗ chi tiêu ở đâu
    //    //9
    //    Config.AddGemEarn(valueGold, where);// Gọi khi chi nhận đc Gem (hoặc đá quý) , truyền vào số lượng và chỗ nhận đc Gem ở đâu
    //    //10
    //    Config.SetRate();//Gọi khi user ấn vào nút Yes (nút rate) trong popup Rate game
    //    //11
    //    bool isRate = Config.GetRate();//Gọi khi cần kiểm tra User đã ấn Rate chưa
    //    //12
    //    Config.ActiveShowPopupRate();//Gọi khi Player ấn vào nút Yes trong popup rate để rate (bắt buộc , nếu không sẽ bị hiển thị ad khi quay lại)
    //    //13
    //    Config.SetRemoveAd();//Gọi khi user mua thành công gói remoteAd
    //    //14
    //    bool isRemoveAd = Config.GetRemoveAd();//Gọi khi muốn kiểm tra xem user đã mua removeAd chưa để còn tắt hoặc lock nút mua gói RemoveAd lại
    //    //-------
    //    //15
    //    FirebaseManager.instance.LogCheckPoint(idCheckPoint);//Call khi cần check log các vị trí đặc biệt đc yêu cầu
    //}

    //void ActiveShowInterSpinBack(){
    //    //Những chỗ show inter back lại từ các popup hay màn hình tính năng hoặc mini game => thêm "true" vào thuộc tính thứ 3
    //    AdsController.instance.ShowInterAd(null, "SpinBack", true);
    //}
    //void ActiveShowInterNextInPopupWin() {
    //    //Show trong gameplay khi Replay (Retry), thoát về home, ấn next , retry , home ở popup win, lose, setting trong game => ko cần điền thuộc tính thứ 3
    //    AdsController.instance.ShowInterAd(null, "NextByPopupWin");
    //}


#region Sound, Music
    public const string SOUND = "sound";
    public static bool isSound = true;
    public static void SetSound(bool _isSound)
    {
        isSound = _isSound;
        if (_isSound)
        {
            PlayerPrefs.SetInt(SOUND, 1);
        }
        else
        {
            PlayerPrefs.SetInt(SOUND, 0);
        }
        PlayerPrefs.Save();
    }

    public static void GetSound()
    {
        int soundInt = PlayerPrefs.GetInt(SOUND, 1);
        if (soundInt == 1)
        {
            isSound = true;
        }
        else
        {
            isSound = false;
        }
    }


    public const string MUSIC = "music";
    public static bool isMusic = true;
    public static void SetMusic(bool _isMusic)
    {
        isMusic = _isMusic;
        if (_isMusic)
        {
            PlayerPrefs.SetInt(MUSIC, 1);
        }
        else
        {
            PlayerPrefs.SetInt(MUSIC, 0);
        }
        PlayerPrefs.Save();
    }

    public static void GetMusic()
    {
        int musicInt = PlayerPrefs.GetInt(MUSIC, 0);
        if (musicInt == 1)
        {
            isMusic = true;
        }
        else
        {
            isMusic = false;
        }
    }

#endregion

#region ADS_INTERSTITIAL
    public const int interstitialAd_levelShowAd = 2;
    public const int interstitialAd_SHOW_WIN_INTERVAL = 2;
    public const int interstitialAd_SHOW_LOSE_INTERVAL = 2;
    public static int interstitialAd_countWin = 0;
    public static int interstitialAd_countLose = 0;
    public static int interstitialAd_countRestart = 0;
    public const int interstitialAd_SHOW_PAUSE_INTERVAL = 1;
    public static int interstitialAd_countPause = 0;

    public static int LEVEL_ACTIVE_INTER_COMEBACK = 2;
#endregion

    //khi click vao` ok show rate
    public static void ActiveShowPopupRate()
    {
        Config.showInterOnPause = false;
    }

    public static bool CheckWideScreen()
    {
        if (Screen.width * 1f / Screen.height > 1242f / 2208f)
        {
            return true;
        }
        return false;
    }

#region RATE
    public const string RATE = "rate";
    public static void SetRate()
    {
        PlayerPrefs.SetInt(RATE, 1);
        PlayerPrefs.Save();
    }

    public static bool GetRate()
    {
        if (PlayerPrefs.GetInt(RATE, 0) == 1) return true;
        return false;
    }
#endregion
#region Level Best
    public const string LEVEL_BEST = "level_best";
    public static void SetLevelBest(int level)
    {
        PlayerPrefs.SetInt(LEVEL_BEST, level);
        PlayerPrefs.Save();
    }

    public static int GetLevelBest()
    {
        return PlayerPrefs.GetInt(LEVEL_BEST, 0);
    }
#endregion
#region Level Fail Count
    public const string LEVEL_FAIL_COUNT = "level_fail_count_";
    public static void SetLevelFailCount(int level)
    {
        PlayerPrefs.SetInt(LEVEL_FAIL_COUNT + level.ToString(), GetLevelFailCount(level) + 1);
        PlayerPrefs.Save();
    }

    public static int GetLevelFailCount(int level)
    {
        return PlayerPrefs.GetInt(LEVEL_FAIL_COUNT + level.ToString(), 0);
    }
#endregion
#region REMOVE_AD
    public const string REMOVE_AD = "remove_Ad";
    public static void SetRemoveAd()
    {

        PlayerPrefs.SetInt(REMOVE_AD, 1);
        PlayerPrefs.Save();
    }

    public static bool GetRemoveAd()
    {
        if (PlayerPrefs.GetInt(REMOVE_AD, 0) == 1) return true;
        return false;
    }
#endregion
#region UnlockAll
    public const string UNLOCK_ALL = "unlock_all";
    public static void SetUnlockAll()
    {
        PlayerPrefs.SetInt(UNLOCK_ALL, 1);
        PlayerPrefs.Save();
    }

    public static bool GetUnlockAll()
    {
        if (PlayerPrefs.GetInt(UNLOCK_ALL, 0) == 1) return true;
        return false;
    }
#endregion

#region retent day

    public const string TIME_FIRST_OPEN_GAME = "time_first_open_game";
    public const string CACHE_RETENT_GAME = "cache_retent_game";

    public const string COUNT_DAY_PLAY_GAME = "count_day_play_game";

    public static void CheckRetent()
    {
        int dayRetent = 0;
        try
        {
            string strTimeOpenGame = PlayerPrefs.GetString(TIME_FIRST_OPEN_GAME, "-1");
            long timeOpenGame = long.Parse(strTimeOpenGame);
            if (timeOpenGame < 0)
            {
                timeOpenGame = DateTime.Today.ToFileTime();
                PlayerPrefs.SetString(TIME_FIRST_OPEN_GAME, timeOpenGame.ToString());
                PlayerPrefs.Save();
                dayRetent = 0;
                //if (FirebaseManager.instance.firebaseInitialized)
                {
                    FirebaseManager.instance.LogUserPropertyLevelReach(1);//lan dau cai game la` 1
                }
            }
            else
            {
                DateTime dateFirstOpen = DateTime.FromFileTime(timeOpenGame);
                dayRetent = int.Parse((DateTime.Today - dateFirstOpen).TotalDays.ToString());
            }
        }
        catch (Exception)
        {
            Debug.LogError("error parse fire");
        }
        int cacheDayRetent = PlayerPrefs.GetInt(CACHE_RETENT_GAME, -1);
        if (dayRetent > cacheDayRetent)
        {
            cacheDayRetent = dayRetent;
            PlayerPrefs.SetInt(CACHE_RETENT_GAME, dayRetent);
            PlayerPrefs.Save();
            //if (FirebaseManager.instance.firebaseInitialized)
            {
                FirebaseManager.instance.LogUserPropertyRetent(dayRetent);
            }
            //----------count day playing------
            int countDayPlayGame = PlayerPrefs.GetInt(COUNT_DAY_PLAY_GAME, 0);
            countDayPlayGame += 1;
            PlayerPrefs.SetInt(COUNT_DAY_PLAY_GAME, countDayPlayGame);
            PlayerPrefs.Save();
            //if (FirebaseManager.instance.firebaseInitialized)
            {
                FirebaseManager.instance.LogUserPropertyCountPlayGame(countDayPlayGame);
            }
        }

    }
#endregion

#region New day
    public const string DAY_CHECK_ACTIVE = "day_check_active";
    public const string COUNT_DAY_ACTIVE = "count_day_active";
    public static bool CheckNewDay()
    {
        int day = DateTime.Now.DayOfYear;
        int dayCache = PlayerPrefs.GetInt(DAY_CHECK_ACTIVE, -1);
        if (dayCache != day)
        {
            PlayerPrefs.SetInt(DAY_CHECK_ACTIVE, day);

            int countDayActiveNew = PlayerPrefs.GetInt(COUNT_DAY_ACTIVE, 0) + 1;
            PlayerPrefs.SetInt(COUNT_DAY_ACTIVE, countDayActiveNew);

            PlayerPrefs.Save();
            //if (FirebaseManager.instance.firebaseInitialized)
            {
                FirebaseManager.instance.LogUserPropertyDaysPlaying(countDayActiveNew);
            }
            if (dayCache == -1)
            {
                //first active
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
#endregion
#region level complete
    public const string CURR_LEVEL_COMPLETE = "curr_level_complete";

    //public static int currLevelComplete = 0;//level ma player da hoan` thanh`

    public static void SetCurrLevelComplete(int _LevelComplete)
    {
        if (_LevelComplete <= 0)
        {
            Debug.LogError("SetCurrentLevel fail : " + _LevelComplete);
        }

        if (_LevelComplete > GetCurrLevelComplete())
        {
            PlayerPrefs.SetInt(CURR_LEVEL_COMPLETE, _LevelComplete);
            PlayerPrefs.Save();

            FirebaseManager.instance.LogUserPropertyLevelReach(currLevel + 1);//mac dinh la 1 , lay moc level comp + 1
        }
    }

    public static int GetCurrLevelComplete()
    {
        return PlayerPrefs.GetInt(CURR_LEVEL_COMPLETE, 0);
    }
    public static int GetCurrLevelComplete_Save_Now()
    {
        return PlayerPrefs.GetInt(CURR_LEVEL_COMPLETE, 0);
    }
#endregion
#region Gold total Spent , Earn
    public const string GOLD_SPENT = "gold_spent";//Gold tieu thu.
    public const string GOLD_EARN = "gold_earn";//Gold kiem dc.

    public static void AddGoldSpent(int pGold, string pWhere)//truyen vao` so' + gold tieu thu.
    {
        int goldSpentNow = PlayerPrefs.GetInt(GOLD_SPENT, 0) + pGold;
        PlayerPrefs.SetInt(GOLD_SPENT, goldSpentNow);
        PlayerPrefs.Save();

        FirebaseManager.instance.LogUserPropertyTotalSpent(goldSpentNow, pGold, pWhere);
    }

    public static void AddGoldEarn(int pGold, string pWhere)//truyen vao` so' + gold nhan. dc.
    {
        int goldEarnNow = PlayerPrefs.GetInt(GOLD_EARN, 0) + pGold;
        PlayerPrefs.SetInt(GOLD_EARN, goldEarnNow);
        PlayerPrefs.Save();

        FirebaseManager.instance.LogUserPropertyTotalEarn(goldEarnNow, pGold, pWhere);
    }
    public const string GEM_SPENT = "gem_spent";//Gold tieu thu.
    public const string GEM_EARN = "gem_earn";//Gold kiem dc.

    public static void AddGemSpent(int pGem, string pWhere)//truyen vao` so' + gold tieu thu.
    {
        int gemSpentNow = PlayerPrefs.GetInt(GEM_SPENT, 0) + pGem;
        PlayerPrefs.SetInt(GEM_SPENT, gemSpentNow);
        PlayerPrefs.Save();

        FirebaseManager.instance.LogUserPropertyTotalSpentGem(gemSpentNow, pGem, pWhere);
    }

    public static void AddGemEarn(int pGem, string pWhere)//truyen vao` so' + gold nhan. dc.
    {
        int gemEarnNow = PlayerPrefs.GetInt(GEM_EARN, 0) + pGem;
        PlayerPrefs.SetInt(GEM_EARN, gemEarnNow);
        PlayerPrefs.Save();

        FirebaseManager.instance.LogUserPropertyTotalEarnGem(gemEarnNow, pGem, pWhere);
    }

    //----
    public const string HINT_SPENT = "hint_spent";//Gold tieu thu.
    public const string HINT_EARN = "hint_earn";//Gold kiem dc.

    public static void AddHintSpent(int pHint, int pLevel)//truyen vao` so' + gold tieu thu.
    {
        int hintSpentNow = PlayerPrefs.GetInt(HINT_SPENT, 0) + pHint;
        PlayerPrefs.SetInt(HINT_SPENT, hintSpentNow);
        PlayerPrefs.Save();

        FirebaseManager.instance.LogUserPropertyTotalSpentHint(hintSpentNow, pHint, pLevel);
    }

    public static void AddHintEarn(int pHint, string pWhere)//truyen vao` so' + gold nhan. dc.
    {
        int hintEarnNow = PlayerPrefs.GetInt(HINT_EARN, 0) + pHint;
        PlayerPrefs.SetInt(HINT_EARN, hintEarnNow);
        PlayerPrefs.Save();

        FirebaseManager.instance.LogUserPropertyTotalEarnHint(hintEarnNow, pHint, pWhere);
    }
#endregion
}



