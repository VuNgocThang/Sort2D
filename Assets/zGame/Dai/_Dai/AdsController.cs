using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsController : MonoBehaviour
{
    //public AdmobAdsController admob;
    public MaxAdsController admob;
    //public IronsourceAdsController admob;

    public static bool isCallShowBanner = true;
    public static bool isCallShowBannerCollap = true;

    public static float TIME_DELAY_RESET_COLLAPBANNER = 12f;

    public static int VALUE_SHOW_INTER_PAUSE_GAME = 1;

    public static int VALUE_CONFIG_LEVEL_SHOW_RATE = 0;

    public static int VALUE_CONFIG_INTER_LEVEL_SHOW = 0;

    public static double CAPPING_TIME_INTER_BY_INTER_NOW = 10;
    public static double CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW = 10;

    public static bool AOA_FIRST_OPEN_ACTIVE = true;
    public static bool AOA_RESUME_ACTIVE = true;
    public static double TIME_AOA_SHOWINTERTITIAL = 1f;

    //----helper----
    public static bool interAdShowing = false;
    public static bool rewardAdShowing = false;
    //------------


    public static long timeLastShowReward = 0;

    public static AdsController instance;

    public static bool SCREEN_SHOW_BANNER = true;

    public static bool isUseInter = true;

    public static DateTime m_TimeShowInterstitial = DateTime.Now;

    public PopupGDPR popupGDPR;
    public GameObject SplashObj;

    public GameObject objNotification; //De deactive khi keo vao

    public void AddCallendars()
    {
        new System.Globalization.GregorianCalendar();
        new System.Globalization.PersianCalendar();
        new System.Globalization.UmAlQuraCalendar();
        new System.Globalization.ThaiBuddhistCalendar();
    }

    private void Awake()
    {
        AddCallendars();

        instance = this;
        //admob = new AdmobAdsController();
        admob = new MaxAdsController();
        //admob = new IronsourceAdsController();

        DontDestroyOnLoad(this);

        //if (Screen.height < 1000)
        //{
        //    SCREEN_SHOW_BANNER = false;
        //}
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer && PlayerPrefs.GetInt("ATTShowed", 0) == 0 && UnityATTPlugin.Instance.IsIOS14AndAbove())
        {
            //AnalysticManager.Instance.ATTShow();
            UnityATTPlugin.Instance.ShowATTRequest((action) =>
            {
                //if (action == ATTStatus.Authorized)
                    //AnalysticManager.Instance.ATTSuccess();
            });
            PlayerPrefs.SetInt("ATTShowed", 1);
        }
#endif

        if (popupGDPR != null)
        {
            if (PlayerPrefs.GetInt("showGDPR", 0) == 1)
            {
                InitStart();
            }
            else
            {
                PlayerPrefs.SetInt("showGDPR", 1);
                popupGDPR.gameObject.SetActive(true);
                popupGDPR.SetUp();
            }
        }
        else
        {
            InitStart();
        }
        // InitStart();
    }

    public void InitStart()
    {
        if (SplashObj != null)
        {
            SplashObj.SetActive(true);
        }

        admob.Setup();
        if (waitFetchFirebase != null)
        {
            StopCoroutine(waitFetchFirebase);
            waitFetchFirebase = null;
        }

        waitFetchFirebase = StartCoroutine(StartWaitFirebaseFetch());
        if (Config.ACTIVE_TEST)
        {
            Config.SetUnlockAll();
        }

        m_TimeShowInterstitial = DateTime.Now;

        StartCoroutine(WaitFirstLoading());
        StartCoroutine(WaitNotificationActive());
    }

    Coroutine waitFetchFirebase;
    bool startActiveLoadAOA = false;

    public void ActiveFetchFirebaseDone()
    {
        Config.AddLogShowDebug("FetchDone");
        if (!startActiveLoadAOA)
        {
            startActiveLoadAOA = true;
            Config.FirstCheckTypeUser();
            admob.StartLoadAOA();
        }

        if (waitFetchFirebase != null)
        {
            StopCoroutine(waitFetchFirebase);
            waitFetchFirebase = null;
        }
    }

    public void ActiveFetchFailed()
    {
        Config.AddLogShowDebug("FetchFailed");
        ActiveFetchFirebaseDone();
        if (waitFetchFirebase != null)
        {
            StopCoroutine(waitFetchFirebase);
            waitFetchFirebase = null;
        }
    }

    IEnumerator StartWaitFirebaseFetch()
    {
        //--them vao vi` doi fetch firebase---
        AdsController.instance.admob.FirstLoadAdsMax_WhenWaitFetch();
        //------------
        yield return new WaitForSeconds(6f);
        Config.AddLogShowDebug("FetchOut");
        ActiveFetchFirebaseDone();
    }

    IEnumerator WaitNotificationActive()
    {
        yield return null;
        yield return null;
        if (objNotification != null)
        {
            objNotification.SetActive(true);
        }
    }

    IEnumerator WaitFirstLoading()
    {
        yield return new WaitForSeconds(Config.TIME_WAIT_LOADING);
        AppOpenAdManager.Instance.showFirstOpen = true;
        FirstLoadAOA_Done();
        //if (!Config.FIRST_LOAD_ADS_DONE)
        //{
        //    Config.FIRST_LOAD_ADS_DONE = true;
        //    admob.LoadAgainWhenAOAFail();
        //}
    }

    public void FirstLoadAOA_Done()
    {
        admob.FirstLoadAOA_Done();
    }

    private void OnDestroy()
    {
        admob.OnDestroy();
    }

    //------------
    public void CheckActiveChangeTypeUser()
    {
        admob.ActiveCheckFirst_LoadAD();
        admob.CheckLoadPreAOAWhenChangeTypeUser();
    }
    //------------
    //public void DelayLoadReward()
    //{
    //    StartCoroutine(LoadRewardDelayActive());
    //}
    //IEnumerator LoadRewardDelayActive()
    //{
    //    yield return new WaitForSecondsRealtime(5);
    //    UnityMainThreadDispatcher.Instance().Enqueue(() =>
    //    {
    //        admob.ActiveStartReward();
    //    });
    //}
    //----
    //public void DelayLoadInter()
    //{
    //    StartCoroutine(LoadInterDelayActive());
    //}
    //IEnumerator LoadInterDelayActive()
    //{
    //    yield return new WaitForSecondsRealtime(1);
    //    //MediationTestSuite.Show();//test

    //    UnityMainThreadDispatcher.Instance().Enqueue(() =>
    //    {
    //        admob.ActiveStartInitInter();
    //    });
    //}
    public void ReloadVideoReward(float time)
    {
        Invoke(nameof(LoadRewardedAd), time);
    }

    void LoadRewardedAd()
    {
        admob.LoadRewardedAd();
    }

    public bool IsRewardedVideoAvailable(string where = null)
    {
        bool check = false;
        if (!Config.isActiveVideoReward)
        {
            return false;
        }

        check = admob.IsRewardedVideoAvailable(where);
        return check;
    }

    public void ShowRewardedVideo(Action<bool> pOnCompleted, Action pOnClose, string pWhere = "unknown")
    {
        if (!Config.isActiveVideoReward)
        {
            return;
        }
#if CHECK_NETWORK_CONNECT
        if( !Config.GetRemoveAd()&&Config.isCheckConnetNetwork){
            NetworkController.instance.CheckNetwork();
            //return;
        }
#endif
        bool check = admob.ShowRewardedVideo(pOnCompleted, pWhere);
        if (check)
        {
            FirebaseManager.instance.LogShowReward(pWhere, Config.currLevel);
        }

        FirebaseManager.instance.LogCallShowVR();
    }

    public void RewardVideoComplete(string pWhere)
    {
        //timeLastShowReward = Config.GetTimeStamp();
        FirebaseManager.instance.LogRewarded(pWhere, Config.currLevel);
    }

    //---------
    public void ReloadInter(float time)
    {
        Invoke(nameof(LoadInterstitial), time);
    }

    void LoadInterstitial()
    {
        admob.LoadInterstitial();
    }

    public bool IsInterAdAvailable(string where = null)
    {
        if (Config.GetRemoveAd() || Config.currLevel < AdsController.VALUE_CONFIG_INTER_LEVEL_SHOW ||
            !Config.ACTIVE_INTER_ADS) return false;

        if (!Config.isActiveInter)
        {
            return false;
        }

        if (IsInterCheckTimeShow())
        {
            //bool check = false;
            //check = admob.IsInterAvailable();
            //return check;
            return true;
        }

        return false;
    }

    public bool IsInterCheckTimeShow()
    {
        DateTime now = DateTime.Now;
        if (DateTime.Compare(now, m_TimeShowInterstitial) > 0)
        {
            return true;
        }

        return false;
    }

    public bool IsInterAdAvailableNotTime(string where = "unknown")
    {
        if (Config.GetRemoveAd()) return false;

        if (!Config.isActiveInter)
        {
            return false;
        }

        bool check = false;
        check = admob.IsInterAvailable();
        return check;
    }

    public void ShowInterAd(Action<bool> pOnCompleted, string pWhere = "unknown", bool isPopupBack = false)
    {
        if (!Config.isActiveInter)
        {
            return;
        }
#if CHECK_NETWORK_CONNECT
        if( !Config.GetRemoveAd()&&Config.isCheckConnetNetwork){
            NetworkController.instance.CheckNetwork();
            //return;
        }
#endif
// #if UNITY_EDITOR
//         Debug.LogError("Show inter");
// #endif
        if (!Config.ENABLE_INTER_BACK_POPUP && isPopupBack)
        {
            return;
        }

        if (!IsInterAdAvailable())
        {
            //----check xem 
            return;
        }

        FirebaseManager.instance.LogCallShowInter();
        if (!admob.IsInterAvailable())
        {
            return;
        }

        bool check = admob.ShowInter(pOnCompleted, pWhere);
        if (check)
        {
            //timeLastShowReward = Config.GetTimeStamp();
            m_TimeShowInterstitial = DateTime.Now;
            m_TimeShowInterstitial = m_TimeShowInterstitial.AddSeconds(AdsController.CAPPING_TIME_INTER_BY_INTER_NOW);
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("ShowInterAds");
            }

            FirebaseManager.instance.LogShowInter(pWhere, Config.currLevel);
        }
    }

    public void CloseInterShow()
    {
        m_TimeShowInterstitial = DateTime.Now;
        m_TimeShowInterstitial = m_TimeShowInterstitial.AddSeconds(AdsController.CAPPING_TIME_INTER_BY_INTER_NOW);
    }

    public void CloseAOAShow()
    {
        DateTime presentTime = DateTime.Now.AddSeconds(AdsController.TIME_AOA_SHOWINTERTITIAL);
        if (presentTime.CompareTo(m_TimeShowInterstitial) > 0)
        {
            m_TimeShowInterstitial = presentTime;
            //m_TimeShowInterstitial = DateTime.Now;
            //m_TimeShowInterstitial = m_TimeShowInterstitial.AddSeconds(AdsController.TIME_AOA_SHOWINTERTITIAL);
        }
    }

    public void CloseVideoRewarded()
    {
        DateTime presentTime = DateTime.Now.AddSeconds(AdsController.CAPPING_TIME_INTER_BY_REWARD_VIDEO_NOW);
        if (presentTime.CompareTo(m_TimeShowInterstitial) > 0)
        {
            m_TimeShowInterstitial = presentTime;
        }
    }

    //-----banner----
    public bool isInit = false;

    public void EndInitAds()
    {
        isInit = true;
        //LoadBannerAds();
    }

    void CheckLoadBannerAds()
    {
        if (Config.isActiveBanner)
        {
            LoadBannerAds();
        }
    }

    //-------load banner collap----
    public bool canResetCollapBanner = true;

    public void ActiveCountTimeResetCollapBanner()
    {
        canResetCollapBanner = false;
        CancelInvoke(nameof(LoadTimeResetCollapBannerDone));
        Invoke(nameof(LoadTimeResetCollapBannerDone), AdsController.TIME_DELAY_RESET_COLLAPBANNER);
    }

    public void LoadTimeResetCollapBannerDone()
    {
        canResetCollapBanner = true;
    }

    public void CancelResetActiveLoadBannerCollap()
    {
        CancelInvoke(nameof(ResetActiveLoadBannerCollapActive));
    }

    //ko goi gam` nay` -> chi phuc. vu cho sdk
    public void ResetActiveLoadBannerCollap(float time)
    {
        CancelReloadBannerCollapOnFailLoad();
        Invoke(nameof(ResetActiveLoadBannerCollapActive), time);
    }

    void ResetActiveLoadBannerCollapActive()
    {
        AppOpenAdManager.ResetActiveLoadBannerCollap();
    }

    //-----------
    public void CancelReloadBannerCollapOnFailLoad()
    {
        CancelInvoke(nameof(ReloadBannerCollapFail));
    }

    public void ReloadBannerCollapOnFailLoad(float time)
    {
        CancelReloadBannerCollapOnFailLoad();
        Invoke(nameof(ReloadBannerCollapFail), time);
    }

    void ReloadBannerCollapFail()
    {
        AppOpenAdManager.LoadBannerAd();
    }

    //----------
    public void CancelDelayLoadAgainBanner()
    {
        CancelInvoke(nameof(DelayLoadAgainBanner));
    }

    public void DelayLoadAgainBanner()
    {
        CancelDelayLoadAgainBanner();
        Invoke(nameof(LoadBannerAds), 3f);
    }

    void LoadBannerAds()
    {
        if (!Config.GetRemoveAd())
        {
            //if (string.IsNullOrEmpty(ConfigIdsAds._adUnitId))
            if (!string.IsNullOrEmpty(ConfigIdsAds.BannerAdUnitId))
            {
                admob.Request_Banner2();
            }

            //admob.Request_Banner2();
        }
    }

    public void HideBannerAd()
    {
        isCallShowBanner = false;
        admob.HideBannerAd();
    }

    public void HideBannerAdCollap()
    {
        //isCallShowBanner = false;
        isCallShowBannerCollap = false;
        AppOpenAdManager.HideBannerCollap();
        if (backFillBannerCollap)
        {
            backFillBannerCollap = false;
            if (isCallShowBanner)
            {
                HideBannerAd();
            }
        }
    }

    public void ShowBannerAd()
    {
        if (!Config.isActiveBanner)
        {
            return;
        }

        isCallShowBanner = true;
        if (Config.ACTIVE_TEST || !SCREEN_SHOW_BANNER)
        {
            admob.HideBannerAd();
            return;
        }

        if (!Config.GetRemoveAd())
        {
            admob.ShowBannerAd();
        }
    }

    public bool backFillBannerCollap = false;

    public void ShowBannerAdCollap()
    {
        if (!Config.isActiveBanner)
        {
            return;
        }

        //isCallShowBanner = true;
        isCallShowBannerCollap = true;
        if (Config.ACTIVE_TEST || !SCREEN_SHOW_BANNER)
        {
            AppOpenAdManager.HideBannerCollap();
            return;
        }

        if (!Config.GetRemoveAd())
        {
            if (AppOpenAdManager.isBannerCollapLoaded)
            {
                AppOpenAdManager.ShowBannerCollap();
            }
            else
            {
                ShowBannerAd();
                backFillBannerCollap = true;
            }
        }
    }

    public void ActiveResetBannerCollap()
    {
        if (!Config.isActiveBanner)
        {
            return;
        }

        if (!AppOpenAdManager.isBannerCollapLoaded)
        {
            ShowBannerAd();
            backFillBannerCollap = true;
        }
        else
        {
            if (canResetCollapBanner)
            {
                AppOpenAdManager.ResetShowBannerCollap();
                AdsController.instance.ActiveCountTimeResetCollapBanner();
            }
            else
            {
                ShowBannerAdCollap();
            }
        }
    }

    //---------MRec Ad-------
    public void ShowMrecAd()
    {
        if (Config.ACTIVE_TEST)
        {
            return;
        }

        if (!Config.GetRemoveAd())
        {
            admob.ShowMRec();
        }
    }

    public void HideMrecAd()
    {
        if (!Config.GetRemoveAd())
        {
            admob.HideMRec();
        }
    }
    //-------support ingame------
    //public static event Action OnChangeTimeWaitCol = delegate () { };

    //float timeWaitCar = 180;
    //public float countTimeWaitCar = 0;

    //float timeWaitCol = 180;
    //public float countTimeWaitCol = 0;

    //float timeWaitTheme = 180;
    //public float countTimeWaitTheme = 0;

    //private void Update()
    //{
    //    float deltaTimeUnscale = Time.unscaledDeltaTime;

    //    if (countTimeWaitCar > 0)
    //    {
    //        countTimeWaitCar -= deltaTimeUnscale;
    //    }
    //    if (countTimeWaitCol > 0)
    //    {
    //        countTimeWaitCol -= deltaTimeUnscale;
    //    }
    //    if (countTimeWaitTheme > 0)
    //    {
    //        countTimeWaitTheme -= deltaTimeUnscale;
    //    }
    //}
    //public void UnlockCarDone()
    //{
    //    countTimeWaitCar = timeWaitCar;
    //    OnChangeTimeWaitCol();
    //}
    //public void UnlockColDone()
    //{
    //    countTimeWaitCol = timeWaitCol;
    //    OnChangeTimeWaitCol();
    //}
    //public void UnlockThemeDone()
    //{
    //    countTimeWaitTheme = timeWaitTheme;
    //    OnChangeTimeWaitCol();
    //}
    //--------AOA MAX-------
    private void OnApplicationPause(bool pause)
    {
        //if (!pause)
        //{
        //    if (VALUE_SHOW_INTER_PAUSE_GAME == 1
        //        && Config.showInterOnPause
        //        //&& Config.GetCurrLevelComplete_Save_Now() > 2//FIXME
        //        //&& (UnityEngine.Random.Range(0, 10) < 4)
        //        )
        //    {
        admob.OnApplicationPause(pause);
        //        //UnityMainThreadDispatcher.Instance().Enqueue(ShowInterComeBack);
        //    }
        //    else
        //    {
        //        Config.showInterOnPause = true;
        //    }
        //}
    }
    //void ShowInterComeBack()
    //{
    //    admob.OnApplicationPause(pause);
    //}
    //-----------NOT MAX---
    //private void OnApplicationPause(bool pause)
    //{
    //    //Debug.Log(pause + "    ");
    //    if (!pause)
    //    {
    //        if (VALUE_SHOW_INTER_PAUSE_GAME == 1
    //            && Config.showInterOnPause
    //            && Config.GetCurrLevelComplete_Save_Now() > 2//FIXME
    //            && (UnityEngine.Random.Range(0, 10) < 4))
    //        {
    //            UnityMainThreadDispatcher.Instance().Enqueue(ShowInterComeBack);
    //            //ShowInterAd(null);
    //        }
    //        else
    //        {
    //            Config.showInterOnPause = true;
    //        }
    //    }
    //}
    //void ShowInterComeBack()
    //{
    //    if (IsInterAdAvailable())
    //    {
    //        //FIXME
    //        //if (SortPlayManager.instance != null)
    //        //{
    //        //    SortPlayManager.instance.ObjLoadingComeback.SetActive(true);
    //        //}

    //        CancelInvoke("ActiveShowInterComeback");
    //        Invoke("ActiveShowInterComeback", 0.5f);
    //    }
    //}

    //Coroutine waitHideLoadingCombeBack;
    //void ActiveShowInterComeback()
    //{
    //    if (waitHideLoadingCombeBack != null)
    //    {
    //        StopCoroutine(waitHideLoadingCombeBack);
    //    }
    //    waitHideLoadingCombeBack = StartCoroutine(HideLoadingComeback());
    //    if (IsInterAdAvailable())
    //    //if (IsInterAdAvailableNotTime())
    //    {
    //        ShowInterAd(null,"comeback");
    //    }
    //}
    //IEnumerator HideLoadingComeback()
    //{
    //    yield return new WaitForSecondsRealtime(1f);
    //    //FIXME
    //    //if (SortPlayManager.instance != null)
    //    //{
    //    //    SortPlayManager.instance.ObjLoadingComeback.SetActive(false);
    //    //}
    //}
    //---------------
    const int timeMenuUpdate = 5;
    WaitForSeconds timeCountMenu = new WaitForSeconds(timeMenuUpdate);
    Coroutine corCountMenu = null;

    public void ActiveCountTimeMenu()
    {
        if (corCountMenu != null)
        {
            StopCoroutine(corCountMenu);
            corCountMenu = null;
        }

        corCountMenu = StartCoroutine(CountTimeMenuAction());
    }

    IEnumerator CountTimeMenuAction()
    {
        while (true)
        {
            yield return timeCountMenu;
            Config.UpdateTimeMenu(timeMenuUpdate);
        }
    }

    public void EndCountTimeMenu()
    {
        if (corCountMenu != null)
        {
            StopCoroutine(corCountMenu);
            corCountMenu = null;
        }
    }
}