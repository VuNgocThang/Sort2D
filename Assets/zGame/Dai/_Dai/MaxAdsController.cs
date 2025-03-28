//using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

using GoogleMobileAds.Ump.Api;

public class MaxAdsController
{
    private int interstitialRetryAttempt;
    private int rewardedRetryAttempt;

    bool isMRecAdActive = true;

    bool isInitGoogleDone = false;

    float timescaleCache = 1f;
    public void StartLoadAOA()
    {
        Config.AddLogShowDebug("StartLoadAOA");
        //if (!Config.GetRemoveAd())
        {
            if (Config.ENABLE_GMA_FLAG_GDPR)
            {
                ConsentRequestParameters request = new ConsentRequestParameters
                {
                    TagForUnderAgeOfConsent = false,
                };
                timescaleCache = Time.timeScale;
                Time.timeScale = 0;
                // Check the current consent information status.
                ConsentInformation.Update(request, OnConsentInfoUpdated);
            }
            else
            {
                MobileAds.Initialize(status =>
                {
                    //if (Config.isActiveAOA)
                    {
                        SetInitGoogleDone();
                    }
                });
            }
        }
    }
    //-----------------
    void SetInitGoogleDone()
    {
        if (isInitGoogleDone)
        {
            return;
        }
        AppOpenAdManager.Instance.InitActive();
        if (Config.isActiveAOA
            || Config.isActiveAOA_Switch)
        {
            if (!Config.isActiveAOA)
            {
                //bo doan show aoa dau` di
                AdsController.instance.FirstLoadAOA_Done();
                AppOpenAdManager.Instance.showFirstOpen = true;
            }
            Active_FistLoadAOA();
        }
        else {
            Config.FIRST_LOAD_ADS_DONE = true;
        }
        isInitGoogleDone = true;
    }
    public void CheckLoadPreAOAWhenChangeTypeUser() {
        if (!isInitGoogleDone) {
            return;
        }

        if (Config.isActiveAOA_Switch) 
        {
            Active_FistLoadAOA();
        }
    }
    bool isActiveLoadAOA_Real = false;
    void Active_FistLoadAOA()
    {
        if (isActiveLoadAOA_Real)
        {
            return;
        }
        isActiveLoadAOA_Real = true;

        Config.AddLogShowDebug("FirstLoadAOA");
        if (string.IsNullOrEmpty(ConfigIdsAds.MAX_AOA_AdUnitId))
        {
            AppOpenAdManager.Instance.LoadAd();
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }
        else
        {
            InitAOA_MAX();
        }
    }
    bool isActiveFirstLoadAOA = false;
    public void FirstLoadAOA_Done() {
        if (isActiveFirstLoadAOA) {
            return;
        }
        isActiveFirstLoadAOA = true;
        if (!Config.FIRST_LOAD_ADS_DONE)
        {
            Config.FIRST_LOAD_ADS_DONE = true;
        }
        AppOpenAdManager.Instance.CheckWaitLoadAd();
        ActiveCheckFirst_LoadAD();
    }
    //----
    public void ActiveCheckFirst_LoadAD()
    {
        if (Config.isActiveVideoReward)
        {
            Active_FirstLoadRewardedAd();
        }
        if (Config.isActiveInter)
        {
            Active_FirstLoadInterAd();
        }
        if (Config.isActiveBanner)
        {
            Active_FirstLoadBannerAd();
        }
    }
    bool isActiveLoadRewardAd = false;
    void Active_FirstLoadRewardedAd()
    {
        if (isActiveLoadRewardAd)
        {
            return;
        }
        isActiveLoadRewardAd = true;
        Config.AddLogShowDebug("FirstLoadRewardAd");

        FirstLoadRewardedAd_MAX();
        AppOpenAdManager.Instance.LoadRewardPre();
    }
    bool isActiveLoadInterAd = false;
    void Active_FirstLoadInterAd()
    {
        if (isActiveLoadInterAd)
        {
            return;
        }
        isActiveLoadInterAd = true;
        Config.AddLogShowDebug("FirstLoadInter");

        FirstLoadInter_MAX();
        AppOpenAdManager.Instance.LoadInterPre();
    }
    bool isActiveLoadBannerAd = false;
    void Active_FirstLoadBannerAd()
    {
        if (isActiveLoadBannerAd)
        {
            return;
        }
        isActiveLoadBannerAd = true;
        Config.AddLogShowDebug("FirstLoadBanner");

        FirstLoadBanner_MAX();
        AppOpenAdManager.Instance.LoadBannerPre();
    }
    //--------------
    bool isInitMax = false;
    public void InitActiveMAX()
    {
        InitRewardedAds();
        Active_FirstLoadRewardedAd();//load luon
        if (isWaitActiveRewardAd)
        {
            LoadRewardedAd();
        }
        InitBannerAds();
        if (isWaitActiveBannerAd)
        {
            LoadBanner();
        }
        InitInterstitialAds();
        if (isWaitActiveInterAd)
        {
            LoadInterstitial();
        }
    }
    public void Setup()
    {
        Config.AddLogShowDebug("StartSetup");
        if (string.IsNullOrEmpty(ConfigIdsAds.MRecAdUnitId))
        {
            isMRecAdActive = false;
        }
        else
        {
            isMRecAdActive = true;
        }

        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            isInitMax = true;
            // AppLovin SDK is initialized, configure and start loading ads.
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("MAX SDK Initialized");
            }
            EndInitAds();
            if (isMRecAdActive)
            {
                InitializeMRecAds();
                LoadMRecAds();
            }
            //-----------
            InitActiveMAX();
            //// Show Mediation Debugger
            ////MaxSdk.ShowMediationDebugger();
        };

        ApsActive();

        MaxSdk.SetSdkKey(ConfigIdsAds.MaxSdkKey);
        MaxSdk.InitializeSdk();
    }

    void InitAOA_MAX()
    {

        if (!string.IsNullOrEmpty(ConfigIdsAds.MAX_AOA_AdUnitId))
        {
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
            //ShowAdIfReady();

            MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAppOpenAdLoadedEvent;

            MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAppOpenAdLoadFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAppOpenAdDisplayFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnAppOpenAdDisplayedEvent;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            
            //ad.OnAdFullScreenContentClosed += HandleAdDidDismissFullScreenContent;
            //ad.OnAdFullScreenContentFailed += HandleAdFailedToPresentFullScreenContent;
            //ad.OnAdFullScreenContentOpened += HandleAdDidPresentFullScreenContent;
            //ad.OnAdPaid += HandlePaidEvent;

            MaxSdk.LoadAppOpenAd(ConfigIdsAds.MAX_AOA_AdUnitId);
        }
    }
    //------AOA--------

    private bool isShowingAd = false;
    public void ShowAdIfReady()
    {
        if (string.IsNullOrEmpty(ConfigIdsAds.MAX_AOA_AdUnitId))
        {
            return;
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            //-----DAI FIX----
            if (Config.ACTIVE_TEST) return;
            if (Config.GetRemoveAd())
            {
                return;
            }
            //----------------
            if (isShowingAd)
            {
                return;
            }
            Config.showInterOnPause = false;

            if (MaxSdk.IsAppOpenAdReady(ConfigIdsAds.MAX_AOA_AdUnitId))
            {
                MaxSdk.ShowAppOpenAd(ConfigIdsAds.MAX_AOA_AdUnitId);
            }
            else
            {
                MaxSdk.LoadAppOpenAd(ConfigIdsAds.MAX_AOA_AdUnitId);
            }
        });
    }
    public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        MaxSdk.LoadAppOpenAd(ConfigIdsAds.MAX_AOA_AdUnitId);
        isShowingAd = false;
        //LoadAOA();
        AdsController.instance.CloseAOAShow();
    }
    public void OnAppOpenAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        isShowingAd = true;
    }

    public void OnAppOpenAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AdsController.instance.FirstLoadAOA_Done();
        if (!AppOpenAdManager.Instance.showFirstOpen && AppOpenAdManager.ConfigOpenApp && AdsController.AOA_FIRST_OPEN_ACTIVE)
        {
            AppOpenAdManager.Instance.showFirstOpen = true;
            if (!Config.isActiveAOA)
            {
                return;
            }
            ShowAdIfReady();
        }
    }
    public void OnAppOpenAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo adInfo)
    {
        MaxSdk.LoadAppOpenAd(ConfigIdsAds.MAX_AOA_AdUnitId);
    }
    public void OnAppOpenAdDisplayFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo adError, MaxSdkBase.AdInfo adInfo)
    {
        MaxSdk.LoadAppOpenAd(ConfigIdsAds.MAX_AOA_AdUnitId);
    }
    //--------
    void OnConsentInfoUpdated(FormError consentError)
    {
        if (consentError != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(consentError);

            Time.timeScale = timescaleCache;
            return;
        }

        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
        ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
        {
            if (formError != null)
            {
                // Consent gathering failed.
                UnityEngine.Debug.LogError(consentError);
                return;
            }

            Time.timeScale = timescaleCache;
            // Consent has been gathered.
            if (ConsentInformation.CanRequestAds())
            {
                MobileAds.Initialize(status =>
            {
                //if (Config.isActiveAOA)
                {
                    SetInitGoogleDone();
                }
            });
            }
        });
    }
    //--------AOA GoogleAds---------
    public void FirstLoadAdsMax_WhenWaitFetch()
    {
        if (isFirstLoad_WhenWaitFetch)
        {
            isFirstLoad_WhenWaitFetch = false;
        }
    }
    private void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        //Debug.Log("App State is " + state);
        if (state == AppState.Foreground)
        {
            if (!Config.isActiveAOA_Switch)
            {
                return;
            }
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if (Config.showInterOnPause && !AdsController.interAdShowing && !AdsController.rewardAdShowing)
                {
                    //Debug.Log("App State is 11111 : showwww");
                    if (string.IsNullOrEmpty(ConfigIdsAds.MAX_AOA_AdUnitId))
                    {
                        AppOpenAdManager.Instance.ShowAdIfAvailable();
                    }
                    else
                    {
                        //Debug.Log("App State is 22222222 : showwww");
                        ShowAdIfReady();
                    }
                }
                else
                {
                    Config.showInterOnPause = true;
                }
            });
        }
    }
    //---------------
    public void LoadAgainWhenAOAFail()
    {

    }
    private void EndInitAds()
    {
        AdsController.instance.EndInitAds();
    }
    //----------
    #region reward video

    private void InitRewardedAds()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        //MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    }

    bool isFirstLoadRewardMax = false;
    public void FirstLoadRewardedAd_MAX()
    {
        if (isFirstLoadRewardMax)
        {
            return;
        }
        isFirstLoadRewardMax = true;
        Config.AddLogShowDebug("FirstLoadRewardAd_MAX");
        LoadRewardedAd();
    }
    bool isWaitActiveRewardAd = false;
    public void LoadRewardedAd()
    {
        if (!isInitMax)
        {
            isWaitActiveRewardAd = true;
            return;
        }
        isWaitActiveRewardAd = false;
        if (string.IsNullOrEmpty(ConfigIdsAds.ApsAmazonAppId))
        {
            MaxSdk.LoadRewardedAd(ConfigIdsAds.RewardedAdUnitId);
        }
        else
        {
#if UNITY_EDITOR
            MaxSdk.LoadRewardedAd(ConfigIdsAds.RewardedAdUnitId);
#else
            ApsRewardVideoLoadAd();
#endif
        }
        //MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
        //rewardedStatusText.text = "Loaded";
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Rewarded ad loaded");
        }

        // Reset retry attempt
        rewardedRetryAttempt = 0;

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            FirebaseManager.instance.LogVRSuccLoaded();
        });
    }

    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        //var retryDelay = 2 * Math.Min(6, rewardedRetryAttempt);

        //rewardedStatusText.text = "Load failed: " + errorInfo.Code + "\nRetrying in " + retryDelay + "s...";
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Rewarded ad failed to load with error code: " + errorInfo.Code);
        }
        //Invoke("LoadRewardedAd", (float)retryDelay);
        AdsController.instance.ReloadVideoReward((float)retryDelay);

    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
        }
        LoadRewardedAd();
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            AdsController.rewardAdShowing = false;
        });
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Rewarded ad displayed");
        }
        AdsController.rewardAdShowing = true;
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            FirebaseManager.instance.LogVRDisplayed();
        });
    }

    //private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    //{
    //    Debug.Log("Rewarded ad clicked");
    //}

    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Rewarded ad dismissed");
        }
        LoadRewardedAd();
        ClickCloseVideo();

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            AdsController.rewardAdShowing = false;
        });
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad was displayed and user should receive the reward
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Rewarded ad received reward");
        }
        VideoRewardComplete();

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            AdsController.rewardAdShowing = false;
        });
    }
    public bool IsRewardedVideoAvailable(string where = null)
    {
        if (Config.ACTIVE_TEST)
        {
            return true;
        }
        //#if UNITY_EDITOR
        //        return true;
        //#endif
        bool check = MaxSdk.IsRewardedAdReady(ConfigIdsAds.RewardedAdUnitId);
        if (!check)
        {
            check = AppOpenAdManager.Instance.IsRewardedVideoAvailableGoogle();
        }
        return check;
    }
    //    public bool IsRewardedVideoAvailable(string where = null)
    //    {
    ////#if UNITY_EDITOR
    ////        return true;
    ////#endif
    //        return (!ConfigGP.isActiveShow && this.rewardedAd != null && this.rewardedAd.IsLoaded());
    //    }
    public Action<bool> mOnRewardedAdCompleted;
    private string strWhere = "unknown";
    //public Action mOnClose;

    //public bool ShowRewardedVideo(Action<bool> pOnCompleted, Action pOnClose)
    public bool ShowRewardedVideo(Action<bool> pOnCompleted, string pWhere = null)
    {
        if (pWhere != null)
        {
            this.strWhere = pWhere;
        }
        else
        {
            this.strWhere = "unknown";
        }
        if (Config.ACTIVE_TEST)
        {
            pOnCompleted(true);
            return true;
        }
        //#if UNITY_EDITOR
        //        pOnCompleted(true);
        //        return true;
        //#endif
        bool isShow = false;
        if (MaxSdk.IsRewardedAdReady(ConfigIdsAds.RewardedAdUnitId))
        {

            //rewardedStatusText.text = "Showing";
            Config.showInterOnPause = false;
            isShow = true;
            MaxSdk.ShowRewardedAd(ConfigIdsAds.RewardedAdUnitId, pWhere);
        }
        else
        {
            //rewardedStatusText.text = "Ad not ready";
        }
        if (isShow)
        {
            //isActiveShow = true;
            mOnRewardedAdCompleted = pOnCompleted;
            //mOnClose = pOnClose;
        }
        else
        {
            //---backfill---
            isShow = AppOpenAdManager.Instance.ShowRewardedAd(pOnCompleted, pWhere);

            //----backfill----
            //pOnCompleted(false);
        }
        return isShow;
    }

    public void VideoRewardComplete()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {

            if (mOnRewardedAdCompleted != null)
            {
                mOnRewardedAdCompleted(true);
                mOnRewardedAdCompleted = null;
            }
            AdsController.instance.RewardVideoComplete(strWhere);
        });
        //isActiveShow = false;
        //typeAdsEnd = 1;
    }
    public void ClickCloseVideo()
    {
        //if (mOnClose != null)
        //{
        //    mOnClose();
        //    mOnClose = null;
        //}
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            AdsController.instance.CloseVideoRewarded();
            if (mOnRewardedAdCompleted != null)
            {
                mOnRewardedAdCompleted(false);
                mOnRewardedAdCompleted = null;
            }
        });
    }
    #endregion

    #region Inter

    Action<bool> mOnInterAdCompleted;
    private void InitInterstitialAds()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent; //Successful show ads
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent; //
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        //MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    }


    bool isFirstLoadInterMax = false;
    public void FirstLoadInter_MAX()
    {
        if (isFirstLoadInterMax)
        {
            return;
        }
        isFirstLoadInterMax = true;
        Config.AddLogShowDebug("FirstLoadInter_MAX");
        LoadInterstitial();
    }
    bool isWaitActiveInterAd = false;
    public void LoadInterstitial()
    {
        if (!isInitMax)
        {
            isWaitActiveInterAd = true;
            return;
        }
        isWaitActiveInterAd = false;

        if (string.IsNullOrEmpty(ConfigIdsAds.ApsAmazonAppId))
        {
            MaxSdk.LoadInterstitial(ConfigIdsAds.InterstitialAdUnitId);
        }
        else
        {
#if UNITY_EDITOR
            MaxSdk.LoadInterstitial(ConfigIdsAds.InterstitialAdUnitId);
#else
            ApsInterLoadAd();
#endif
            // ApsInterLoadAd();
        }
        //MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    }

    public bool IsInterAvailable()
    {
        if (Config.ACTIVE_TEST)
        {
            return true;
        }
        //#if UNITY_EDITOR
        //        return true;
        //#endif
        bool check = MaxSdk.IsInterstitialReady(ConfigIdsAds.InterstitialAdUnitId);
        if (!check)
        {
            check = AppOpenAdManager.Instance.IsInterAvailableGoogle();
        }

        return check;
    }
    public bool ShowInter(Action<bool> pOnCompleted, string placement)
    {
        if (placement == null)
        {
            placement = "unknown";
        }
        if (Config.ACTIVE_TEST)
        {
            if (pOnCompleted != null)
            {
                pOnCompleted(true);
            }
            return true;
        }
        //#if UNITY_EDITOR
        //        pOnCompleted(true);
        //        return true;
        //#endif
        bool isShow = false;
        if (MaxSdk.IsInterstitialReady(ConfigIdsAds.InterstitialAdUnitId))
        {
            Config.showInterOnPause = false;
            //interstitialStatusText.text = "Showing";

            MaxSdk.ShowInterstitial(ConfigIdsAds.InterstitialAdUnitId, placement);
            isShow = true;
        }
        else
        {
            //interstitialStatusText.text = "Ad not ready";
        }
        //-----backfill-----
        if (!isShow)
        {
            isShow = AppOpenAdManager.Instance.ShowInterstitialAd();
        }
        //-----------
        if (isShow)
        {
            //mOnInterAdCompleted = pOnCompleted;
            if (pOnCompleted != null)
            {
                pOnCompleted(true);
            }
        }
        else
        {
            if (pOnCompleted != null)
            {
                pOnCompleted(false);
            }
        }
        return isShow;
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
        //interstitialStatusText.text = "Loaded";
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Interstitial loaded");
        }
        // Reset retry attempt
        interstitialRetryAttempt = 0;
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            FirebaseManager.instance.LogInterSuccLoaded();
        });
    }

    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        //var retryDelay = 2 * Math.Min(6, rewardedRetryAttempt);

        //interstitialStatusText.text = "Load failed: " + errorInfo.Code + "\nRetrying in " + retryDelay + "s...";
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Interstitial failed to load with error code: " + errorInfo.Code);
        }
        //Invoke("LoadInterstitial", (float)retryDelay);
        AdsController.instance.ReloadInter((float)retryDelay);
    }
    private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        LoadInterstitial();
        RefreshInterClose();
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            AdsController.interAdShowing = false;
        });
    }
    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Interstitial displayed");
        }
        AdsController.interAdShowing = true;
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            FirebaseManager.instance.LogInterDisplayed();
        });
    }

    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Interstitial dismissed");
        }
        LoadInterstitial();
        RefreshInterClose();
        AdsController.instance.CloseInterShow();
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            AdsController.interAdShowing = false;
        });
    }
    public void RefreshInterClose()
    {
        if (mOnInterAdCompleted != null)
        {
            mOnInterAdCompleted(true);
            mOnInterAdCompleted = null;
        }
    }
    #endregion

    #region banner
    private void InitBannerAds()
    {
        if (string.IsNullOrEmpty(ConfigIdsAds.BannerAdUnitId))
        {
            return;
        }
        if (!string.IsNullOrEmpty(ConfigIdsAds._adUnitId))
        {
            AdsController.instance.HideBannerAd();
        }
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        //MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
        //MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        //MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent_Banner;

        //// Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
        //// You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
        //MaxSdk.CreateBanner(ConfigIdsAds.BannerAdUnitId, ConfigIdsAds.bannerPosition);
        //MaxSdk.SetBannerExtraParameter(ConfigIdsAds.BannerAdUnitId, "adaptive_banner", "true");
        //MaxSdk.SetBannerPlacement(ConfigIdsAds.BannerAdUnitId, "Banner");
        //// Set background or background color for banners to be fully functional.
        //MaxSdk.SetBannerBackgroundColor(ConfigIdsAds.BannerAdUnitId, Color.clear);

        //ShowBanner();
    }


    bool isFirstLoadBannerMax = false;
    public void FirstLoadBanner_MAX()
    {
        if (isFirstLoadBannerMax)
        {
            return;
        }
        isFirstLoadBannerMax = true;
        Config.AddLogShowDebug("FirstLoadBanner_MAX");
        LoadBanner();
    }
    bool isWaitActiveBannerAd = false;
    public void LoadBanner()
    {
        if (!isInitMax)
        {
            isWaitActiveBannerAd = true;
            return;
        }
        isWaitActiveBannerAd = false;
        if (string.IsNullOrEmpty(ConfigIdsAds.ApsAmazonAppId))
        {
            if (string.IsNullOrEmpty(ConfigIdsAds.BannerAdUnitId))
            {
                return;
            }

            MaxSdk.CreateBanner(ConfigIdsAds.BannerAdUnitId, ConfigIdsAds.bannerPosition);
            MaxSdk.SetBannerExtraParameter(ConfigIdsAds.BannerAdUnitId, "adaptive_banner", "true");
            MaxSdk.SetBannerPlacement(ConfigIdsAds.BannerAdUnitId, "Banner");
            MaxSdk.SetBannerBackgroundColor(ConfigIdsAds.BannerAdUnitId, Color.clear);

        }
        else
        {
#if UNITY_EDITOR

            if (string.IsNullOrEmpty(ConfigIdsAds.BannerAdUnitId))
            {
                return;
            }

            MaxSdk.CreateBanner(ConfigIdsAds.BannerAdUnitId, ConfigIdsAds.bannerPosition);
            MaxSdk.SetBannerExtraParameter(ConfigIdsAds.BannerAdUnitId, "adaptive_banner", "true");
            MaxSdk.SetBannerPlacement(ConfigIdsAds.BannerAdUnitId, "Banner");
            MaxSdk.SetBannerBackgroundColor(ConfigIdsAds.BannerAdUnitId, Color.clear);
#else
            ApsBannerLoadAd();
#endif
        }
        //// Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
        //// You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
        //MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        //MaxSdk.SetBannerExtraParameter(BannerAdUnitId, "adaptive_banner", "true");
        //MaxSdk.SetBannerPlacement(BannerAdUnitId, "Banner");
        //// Set background or background color for banners to be fully functional.
        //MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, Color.clear);
        ////ShowBanner();
    }
    bool isFirstLoadBanner=false;
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad is ready to be shown.
        // If you have already called MaxSdk.ShowBanner(BannerAdUnitId) it will automatically be shown on the next ad refresh.
        //Debug.Log("Banner ad loaded");
        isFirstLoadBanner = true;
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            //if (HomeUI.instance == null)
            if (AdsController.isCallShowBanner)
            {
                if (!isActiveMRec)
                {
                    MaxSdk.ShowBanner(ConfigIdsAds.BannerAdUnitId);
                }
            }
            else
            {
                MaxSdk.HideBanner(ConfigIdsAds.BannerAdUnitId);
            }
        });

        MaxSdkCallbacks.Banner.OnAdLoadedEvent -= OnBannerAdLoadedEvent;
    }

    //private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    //{
    //    // Banner ad failed to load. MAX will automatically try loading a new ad internally.
    //    Debug.Log("Banner ad failed to load with error code: " + errorInfo.Code);
    //}

    //private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    //{
    //    Debug.Log("Banner ad clicked");
    //}

    //BannerView banner2View;
    public void Request_Banner2()
    {
        if (!AdsController.SCREEN_SHOW_BANNER)
        {
            return;
        }
        // InitBannerAds();
        LoadBanner();
    }
    public void ShowBannerAd()
    {
        //if (banner2View != null)
        //{
        //    banner2View.Show();
        //}
       
        if (string.IsNullOrEmpty(ConfigIdsAds.BannerAdUnitId))
        {
            return;
        }
        MaxSdk.ShowBanner(ConfigIdsAds.BannerAdUnitId);
    }

    public void HideBannerAd()
    {
        if (string.IsNullOrEmpty(ConfigIdsAds.BannerAdUnitId))
        {
            return;
        }
        MaxSdk.HideBanner(ConfigIdsAds.BannerAdUnitId);
        //if (banner2View != null)
        //{
        //    banner2View.Hide();
        //}
    }
    #endregion
    #region MREC Ad Methods

    private void InitializeMRecAds()
    {
        if (string.IsNullOrEmpty(ConfigIdsAds.MRecAdUnitId))
        {
            return;
        }
        // Attach Callbacks
        //MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
        //MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdFailedEvent;
        //MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
        //MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaidEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent_Mrec;

    }
    private void LoadMRecAds()
    {
        if (string.IsNullOrEmpty(ConfigIdsAds.ApsAmazonAppId))
        {
            if (string.IsNullOrEmpty(ConfigIdsAds.MRecAdUnitId))
            {
                return;
            }
            MaxSdk.CreateMRec(ConfigIdsAds.MRecAdUnitId, ConfigIdsAds.adViewPositionMrec);
        }
        else
        {
            ApsMrecLoadAd();
        }
        // MRECs are automatically sized to 300x250.
        //MaxSdk.CreateMRec(MRecAdUnitId, MaxSdkBase.AdViewPosition.TopCenter);
    }
    bool isActiveMRec = false;
    public void ShowMRec()
    {
        if (string.IsNullOrEmpty(ConfigIdsAds.MRecAdUnitId))
        {
            return;
        }
        MaxSdk.ShowMRec(ConfigIdsAds.MRecAdUnitId);
        isActiveMRec = true;
    }
    public void HideMRec()
    {
        if (string.IsNullOrEmpty(ConfigIdsAds.MRecAdUnitId))
        {
            return;
        }
        MaxSdk.HideMRec(ConfigIdsAds.MRecAdUnitId);
        isActiveMRec = false;
    }

    private void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // MRec ad is ready to be shown.
        // If you have already called MaxSdk.ShowMRec(MRecAdUnitId) it will automatically be shown on the next MRec refresh.
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("MRec ad loaded");
        }
    }

    private void OnMRecAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // MRec ad failed to load. MAX will automatically try loading a new ad internally.
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("MRec ad failed to load with error code: " + errorInfo.Code);
        }
    }

    private void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("MRec ad clicked");
        }
    }

    private void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // MRec ad revenue paid. Use this callback to track user revenue.
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("MRec ad revenue paid");
        }
        // Ad revenue
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
    }

    #endregion
    #region AOA Ad
    private bool isFirstLoad_WhenWaitFetch = true;
    public void OnApplicationPause(bool pauseStatus)
    {
        //if (!pauseStatus)
        //{
        //    if (string.IsNullOrEmpty(ConfigIdsAds.MAX_AOA_AdUnitId))
        //    {
        //        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        //        {
        //            if (Config.showInterOnPause && !AdsController.interAdShowing && !AdsController.rewardAdShowing)
        //            {
        //            //Debug.Log("App State is 11111 : showwww");
        //            if (string.IsNullOrEmpty(ConfigIdsAds.MAX_AOA_AdUnitId))
        //                {
        //                    AppOpenAdManager.Instance.ShowAdIfAvailable();
        //                }
        //            }
        //            else
        //            {
        //                Config.showInterOnPause = true;
        //            }
        //        });
        //    }
        //}


        //OnPauseAOAGoogleAds(pauseStatus);

        //if (!pauseStatus)
        //{
        //    ShowAdIfReady();
        //}
    }

    //private void InitAOA()
    //{
    //    MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
    //    MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAppOpenLoadedEvent;
    //    MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAppOpenLoadFailedEvent;
    //    MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAppOpenFailedToDisplayEvent;

    //    MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
    //    //ShowAdIfReady();
    //}
    //public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    //{
    //    if (Config.ACTIVE_DEBUG_LOG)
    //    {
    //        Debug.Log("Aoa OnAppOpenDismissedEvent, ID: " + AppOpenAdUnitId);
    //    }
    //    MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
    //    AdsController.instance.CloseAOAShow();
    //}
    //public void OnAppOpenLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    //{
    //    if (Config.ACTIVE_DEBUG_LOG)
    //    {
    //        Debug.Log("Aoa loaded successfully, ID: " + AppOpenAdUnitId);
    //    }
    //    if (isFirstLoad)
    //    {
    //        isFirstLoad = false;
    //        ShowAdIfReady();
    //        if (!Config.FIRST_LOAD_ADS_DONE)
    //        {
    //            Config.FIRST_LOAD_ADS_DONE = true;
    //        }
    //        //#if !UNITY_EDITOR
    //        LoadInterstitial();
    //        LoadRewardedAd();
    //        //#endif
    //    }
    //}

    //private void OnAppOpenLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    //{
    //    if (isFirstLoad)
    //    {
    //        isFirstLoad = false;
    //        //#if !UNITY_EDITOR
    //        LoadInterstitial();
    //        LoadRewardedAd();
    //        //#endif
    //    }
    //    if (Config.ACTIVE_DEBUG_LOG)
    //    {
    //        Debug.Log(errorInfo.Code + "  Load AOA Failed. : " + errorInfo.ToString());
    //    }
    //}
    //private void OnAppOpenFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    //{
    //    // Rewarded ad failed to display. We recommend loading the next ad
    //    if (Config.ACTIVE_DEBUG_LOG)
    //    {
    //        Debug.Log(errorInfo.Code + "  AOA ad failed to display with error code: " + errorInfo.ToString());
    //    }
    //    MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
    //}

    //public void ShowAdIfReady()
    //{
    //    if (Config.ACTIVE_DEBUG_LOG)
    //    {
    //        Debug.Log("AOA ShowAdIfReady : " + Config.GetRemoveAd() + "   --   " + MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId));
    //    }
    //    if (Config.GetRemoveAd()) {
    //        return;
    //    }

    //    if (MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId))
    //    {
    //        if (Config.ACTIVE_DEBUG_LOG)
    //        {
    //            Debug.Log("Showwwwww App Open Ad");
    //        }
    //        MaxSdk.ShowAppOpenAd(AppOpenAdUnitId);
    //    }
    //    else
    //    {
    //        if (Config.ACTIVE_DEBUG_LOG)
    //        {
    //            Debug.Log("Load App Open Ad");
    //        }
    //        MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
    //    }
    //}
    //public void OnApplicationPause(bool pauseStatus)
    //{
    //    if (!pauseStatus)
    //    {
    //        ShowAdIfReady();
    //    }
    //}
    #endregion
    public void OnDestroy()
    {
        //if (string.IsNullOrEmpty(ConfigIdsAds.MAX_AOA_AdUnitId))
        if (Config.isActiveAOA
            || Config.isActiveAOA_Switch)
        {
            AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
        }
        AppOpenAdManager.Instance.OnDestroy();

    }
    const int MAX_IMP_BANNER = 30;
    static int countImpBanner = 0;
    double revenueImpBannerCache = 0;
    private void OnAdRevenuePaidEvent_Banner(string adUnitId, MaxSdkBase.AdInfo impressionData)
    {
        double revenue = impressionData.Revenue;
        revenueImpBannerCache += revenue;
        countImpBanner += 1;
        if (countImpBanner >= MAX_IMP_BANNER)
        {
            OnAdRevenuePaidEvent_Final(adUnitId, impressionData, revenueImpBannerCache);
            revenueImpBannerCache = 0;
            countImpBanner = 0;
        }
    }
    const int MAX_IMP_MREC = 30;
    static int countImpMrec = 0;
    double revenueImpMrecCache = 0;
    private void OnAdRevenuePaidEvent_Mrec(string adUnitId, MaxSdkBase.AdInfo impressionData)
    {
        double revenue = impressionData.Revenue;
        revenueImpMrecCache += revenue;
        countImpMrec += 1;
        if (countImpMrec >= MAX_IMP_MREC)
        {
            OnAdRevenuePaidEvent_Final(adUnitId, impressionData, revenueImpMrecCache);
            revenueImpMrecCache = 0;
            countImpMrec = 0;
        }
    }
    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
    {
        double revenue = impressionData.Revenue;
        //        var impressionParameters = new[] {
        //  new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
        //  new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
        //  new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
        //  new Firebase.Analytics.Parameter("ad_format", impressionData.Placement),
        //  new Firebase.Analytics.Parameter("value", revenue),
        //  new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
        //};
        //        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
        //FirebaseManager.instance.LogValueAds("AppLovin", impressionData.NetworkName, impressionData.AdUnitIdentifier, impressionData.Placement, revenue, "USD");
        //string countryCode = MaxSdk.GetSdkConfiguration().CountryCode;
        //FirebaseManager.instance.LogValueAds("AppLovin", impressionData.NetworkName, impressionData.AdUnitIdentifier, impressionData.AdFormat, revenue, "USD", impressionData.Placement, countryCode, impressionData.NetworkPlacement, impressionData.CreativeIdentifier);
        OnAdRevenuePaidEvent_Final(adUnitId, impressionData, revenue);
    }
    private void OnAdRevenuePaidEvent_Final(string adUnitId, MaxSdkBase.AdInfo impressionData, double revenue)
    {
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode;
        FirebaseManager.instance.LogValueAds("AppLovin", impressionData.NetworkName, impressionData.AdUnitIdentifier, impressionData.AdFormat, revenue, "USD", impressionData.Placement, countryCode, impressionData.NetworkPlacement, impressionData.CreativeIdentifier);
    }
    #region APS Adapter
    //-----------APS adapter----------
    public void ApsActive()
    {
#if APS_NETWORK
#if UNITY_EDITOR
        return;
#endif
        if (!string.IsNullOrEmpty(ConfigIdsAds.ApsAmazonAppId))
        {
            Amazon.Initialize(ConfigIdsAds.ApsAmazonAppId);
            Amazon.SetAdNetworkInfo(new AdNetworkInfo(DTBAdNetwork.MAX));
        }
#endif
    }
    //---banner-----
    private void ApsBannerLoadAd()
    {
#if APS_NETWORK
        int width;
        int height;
        string slotId;
        if (MaxSdkUtils.IsTablet())
        {
            width = 728;
            height = 90;
            slotId = ConfigIdsAds.ApsBannerIdTablet;
        }
        else
        {
            width = 320;
            height = 50;
            slotId = ConfigIdsAds.ApsBannerIdPhone;
        }

        var apsBanner = new APSBannerAdRequest(width, height, slotId);
        apsBanner.onSuccess += (adResponse) =>
        {
            if (string.IsNullOrEmpty(ConfigIdsAds.BannerAdUnitId))
            {
                return;
            }
            MaxSdk.SetBannerLocalExtraParameter(ConfigIdsAds.BannerAdUnitId, "amazon_ad_response", adResponse.GetResponse());
            CreateMaxBannerAd();
        };
        apsBanner.onFailedWithError += (adError) =>
        {
            if (string.IsNullOrEmpty(ConfigIdsAds.BannerAdUnitId))
            {
                return;
            }
            MaxSdk.SetBannerLocalExtraParameter(ConfigIdsAds.BannerAdUnitId, "amazon_ad_error", adError.GetAdError());
            CreateMaxBannerAd();
        };

        apsBanner.LoadAd();
#endif
    }

    private void CreateMaxBannerAd()
    {
#if APS_NETWORK
        if (string.IsNullOrEmpty(ConfigIdsAds.BannerAdUnitId))
        {
            return;
        }
        MaxSdk.CreateBanner(ConfigIdsAds.BannerAdUnitId, ConfigIdsAds.bannerPosition);
        MaxSdk.SetBannerPlacement(ConfigIdsAds.BannerAdUnitId, "banner-placement");

        MaxSdk.SetBannerExtraParameter(ConfigIdsAds.BannerAdUnitId, "adaptive_banner", "true");
        MaxSdk.SetBannerBackgroundColor(ConfigIdsAds.BannerAdUnitId, Color.clear);
#endif
    }
    //----MRec----
    private void ApsMrecLoadAd()
    {
#if APS_NETWORK
        var apsMRec = new APSBannerAdRequest(300, 250, ConfigIdsAds.ApsMrecId);
        apsMRec.onSuccess += (adResponse) =>
        {

            if (string.IsNullOrEmpty(ConfigIdsAds.MRecAdUnitId))
            {
                return;
            }
            MaxSdk.SetMRecLocalExtraParameter(ConfigIdsAds.MRecAdUnitId, "amazon_ad_response", adResponse.GetResponse());
            CreateMaxMRecAd();
        };
        apsMRec.onFailedWithError += (adError) =>
        {

            if (string.IsNullOrEmpty(ConfigIdsAds.MRecAdUnitId))
            {
                return;
            }
            MaxSdk.SetMRecLocalExtraParameter(ConfigIdsAds.MRecAdUnitId, "amazon_ad_error", adError.GetAdError());
            CreateMaxMRecAd();
        };

        apsMRec.LoadAd();
#endif
    }

    private void CreateMaxMRecAd()
    {
#if APS_NETWORK
        MaxSdk.CreateMRec(ConfigIdsAds.MRecAdUnitId, ConfigIdsAds.adViewPositionMrec);
        MaxSdk.SetMRecPlacement(ConfigIdsAds.MRecAdUnitId, "placement");
#endif
    }
    //------Inter-----

    private bool IsFirstLoadInter = true;

    private void ApsInterLoadAd()
    {
#if APS_NETWORK
        if (IsFirstLoadInter)
        {
            IsFirstLoadInter = false;

            var interstitialAd = new APSInterstitialAdRequest(ConfigIdsAds.ApsInterId);
            interstitialAd.onSuccess += (adResponse) =>
            {
                MaxSdk.SetInterstitialLocalExtraParameter(ConfigIdsAds.InterstitialAdUnitId, "amazon_ad_response", adResponse.GetResponse());
                MaxSdk.LoadInterstitial(ConfigIdsAds.InterstitialAdUnitId);
            };
            interstitialAd.onFailedWithError += (adError) =>
            {
                MaxSdk.SetInterstitialLocalExtraParameter(ConfigIdsAds.InterstitialAdUnitId, "amazon_ad_error", adError.GetAdError());
                MaxSdk.LoadInterstitial(ConfigIdsAds.InterstitialAdUnitId);
            };

            interstitialAd.LoadAd();
        }
        else
        {
            MaxSdk.LoadInterstitial(ConfigIdsAds.InterstitialAdUnitId);
        }
#endif
    }
    //----reward video -----
    private bool IsFirstLoadRewardVideo = true;
    private void ApsRewardVideoLoadAd()
    {
#if APS_NETWORK
        if (IsFirstLoadRewardVideo)
        {
            IsFirstLoadRewardVideo = false;

            var rewardedVideoAd = new APSVideoAdRequest(320, 480, ConfigIdsAds.ApsRewardVideoId);
            rewardedVideoAd.onSuccess += (adResponse) =>
            {
                MaxSdk.SetRewardedAdLocalExtraParameter(ConfigIdsAds.RewardedAdUnitId, "amazon_ad_response", adResponse.GetResponse());
                MaxSdk.LoadRewardedAd(ConfigIdsAds.RewardedAdUnitId);
            };
            rewardedVideoAd.onFailedWithError += (adError) =>
            {
                MaxSdk.SetRewardedAdLocalExtraParameter(ConfigIdsAds.RewardedAdUnitId, "amazon_ad_error", adError.GetAdError());
                MaxSdk.LoadRewardedAd(ConfigIdsAds.RewardedAdUnitId);
            };

            rewardedVideoAd.LoadAd();
        }
        else
        {
            MaxSdk.LoadRewardedAd(ConfigIdsAds.RewardedAdUnitId);
        }
#endif
    }
    #endregion
}