////using GoogleMobileAds.Api;
//using System;
//using UnityEngine;

//using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;

//using GoogleMobileAds.Ump.Api;
//#if ACTIVE_ADJUST
//using com.adjust.sdk;
//#endif

//public class IronsourceAdsController
//{

////#if UNITY_ANDROID
////    private const string SdkKey = "1dab553b5";
////#elif UNITY_IOS
////    private const string SdkKey = "";
////#else
////    private const string SdkKey = "";
////#endif

//    //    private const string SdkKey = "18a2cb05d";
//    //private const string InterstitialAdUnitId = "14aae71a5f1e483e";
//    //private const string RewardedAdUnitId = "d1e5eee8c0613deb";
//    //private const string BannerAdUnitId = "7f22d822f842ed1c";

//    private int interstitialRetryAttempt;
//    private int rewardedRetryAttempt;

//    float timescaleCache = 1f;

//    public void StartLoadAOA() {
//        if (!Config.GetRemoveAd())
//        {
//            if (Config.ENABLE_GMA_FLAG_GDPR)
//            {
//                ConsentRequestParameters request = new ConsentRequestParameters
//                {
//                    TagForUnderAgeOfConsent = false,
//                };
//                timescaleCache = Time.timeScale;
//                Time.timeScale = 0;
//                // Check the current consent information status.
//                ConsentInformation.Update(request, OnConsentInfoUpdated);
//            }
//            else
//            {
//                MobileAds.Initialize(status =>
//                {
//                    AppOpenAdManager.Instance.InitActive();
//                    AppOpenAdManager.Instance.LoadAd();
//                    AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
//                    AppOpenAdManager.Instance.LoadRewardPre();
//                });
//            }
//        }
//        else
//        {
//            if (!Config.FIRST_LOAD_ADS_DONE)
//            {
//                Config.FIRST_LOAD_ADS_DONE = true;
//            }
//            //------
//            if (Config.ENABLE_GMA_FLAG_GDPR)
//            {
//                ConsentRequestParameters request = new ConsentRequestParameters
//                {
//                    TagForUnderAgeOfConsent = false,
//                };
//                timescaleCache = Time.timeScale;
//                Time.timeScale = 0;
//                // Check the current consent information status.
//                ConsentInformation.Update(request, OnConsentInfoUpdated);
//            }
//            else
//            {
//                MobileAds.Initialize(status =>
//                {
//                    AppOpenAdManager.Instance.InitActive();
//                    AppOpenAdManager.Instance.LoadRewardPre();
//                });
//            }
//        }
//    }
//    //--------
//    void OnConsentInfoUpdated(FormError consentError)
//    {
//        if (consentError != null)
//        {
//            // Handle the error.
//            UnityEngine.Debug.LogError(consentError);

//            Time.timeScale = timescaleCache;
//            return;
//        }

//        // If the error is null, the consent information state was updated.
//        // You are now ready to check if a form is available.
//        ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
//        {
//            if (formError != null)
//            {
//                // Consent gathering failed.
//                UnityEngine.Debug.LogError(consentError);
//                return;
//            }

//            Time.timeScale = timescaleCache;
//            // Consent has been gathered.
//            if (ConsentInformation.CanRequestAds())
//            {
//                IronSource.Agent.setConsent(true);
//                //MobileAds.Initialize((InitializationStatus initstatus) =>
//                //{
//                //    // TODO: Request an ad.
//                //});
//                MobileAds.Initialize(status =>
//                {
//                    AppOpenAdManager.Instance.InitActive();
//                    if (!Config.GetRemoveAd())
//                    {
//                        AppOpenAdManager.Instance.LoadAd();
//                        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
//                    }
//                    AppOpenAdManager.Instance.LoadRewardPre();
//                });
//            }
//        });
//    }


//    public void Setup()
//    {

//        if (!Config.GetRemoveAd())
//        {
//            MobileAds.Initialize(status =>
//            {
//                AppOpenAdManager.Instance.LoadAd();
//               // AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
//            });
//        }
//        else
//        {
//            if (!Config.FIRST_LOAD_ADS_DONE)
//            {
//                Config.FIRST_LOAD_ADS_DONE = true;
//            }
//        }
//       /* 
//        if (!Config.FIRST_LOAD_ADS_DONE)
//        {
//            Config.FIRST_LOAD_ADS_DONE = true;
//        }
//       */
//        //---------------------

//        //IronSourceEvents.onSdkInitializationCompletedEvent += () =>
//        //{
//        //};

//        InitBannerAds();
//        InitInterstitialAds();
//        InitRewardedAds();
//        EndInitAds();
//        //--------------


//        //Add ImpressionSuccess Event
//        IronSourceEvents.onImpressionSuccessEvent += ImpressionSuccessEvent;
//        IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;

//        //Debug.Log("unity-script: I got Setup, code: ");

//        IronSource.Agent.init(ConfigIdsAds.SdkKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);
//        //IronSource.Agent.init(ConfigIdsAds.SdkKey, IronSourceAdUnits.INTERSTITIAL);
//        //IronSource.Agent.init(ConfigIdsAds.SdkKey, IronSourceAdUnits.REWARDED_VIDEO);

//        IronSource.Agent.shouldTrackNetworkState(true);
//        IronSourceConfig.Instance.setClientSideCallbacks(true);
//        IronSource.Agent.setAdaptersDebug(false);
//        //RegisterBannerAdsCallback();
//        //RegisterInterstitialAdsCallback();
//        //RegisterRewardAdsCallback();
//        //----------
//        IronSource.Agent.validateIntegration();
//        //------------
//        //SetConsent();
//        // IronSourceAdQuality.Initialize(SdkKey);

//    }

//    public void SetConsent()
//    {
//        var consent = PlayerPrefs.GetInt("IronSource_Consent", 1) == 1;
//        IronSource.Agent.setConsent(consent);
//        // Debug.Log(consent);
//    }
//    //--------AOA GoogleAds---------
//    public void FirstLoadAdsMax()
//    {
//        if (isFirstLoad)
//        {
//            isFirstLoad = false;

//            LoadInterstitial();
//            LoadRewardedAd();

//        }
//    }
//    //    public void OnPauseAOAGoogleAds(bool pause)
//    //    {
//    //        if (!pause && AppOpenAdManager.ConfigResumeApp
//    //              && AdsController.AOA_RESUME_ACTIVE
//    //            //&& !AppOpenAdManager.ResumeFromAds
//    //          )
//    //        {
//    //            AppOpenAdManager.Instance.ShowAdIfAvailable();
//    //        }
//    //    }
//    private void OnAppStateChanged(AppState state)
//    {
//        // Display the app open ad when the app is foregrounded.
//        //Debug.Log("App State is " + state);
//        if (state == AppState.Foreground)
//        {
//            UnityMainThreadDispatcher.Instance().Enqueue(() =>
//            {
//                if (Config.showInterOnPause && !AdsController.interAdShowing && !AdsController.rewardAdShowing)
//                {
//                    // Debug.Log("App State is 11111 : showwww");
//                    AppOpenAdManager.Instance.ShowAdIfAvailable();
//                }
//                else
//                {
//                    Config.showInterOnPause = true;
//                }
//            });
//        }
//    }


//    ////------delay load ----
//    //void LoadInterDelay()
//    //{
//    //    AdsController.instance.DelayLoadInter();
//    //}
//    //void LoadRewardDelay()
//    //{
//    //    AdsController.instance.DelayLoadReward();
//    //}

//    //public void ActiveStartInitInter() {
//    //    StartInitInter();
//    //}
//    //public void ActiveStartReward(){
//    //    StartInitAds();
//    //}
//    //---------------
//    public void LoadAgainWhenAOAFail()
//    {
//        LoadInterstitial();
//        LoadRewardedAd();
//    }
//    private void EndInitAds()
//    {
//        AdsController.instance.EndInitAds();
//    }
//#region reward video

//    //public void StartInitAds()
//    //{
//    //    // Attach callbacks
//    //    MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
//    //    MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
//    //    MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
//    //    //MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
//    //    //MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
//    //    MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
//    //    MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
//    //    MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

//    //    // Load the first RewardedAd
//    //    LoadRewardedAd();
//    //}
//    private void InitRewardedAds()
//    {

//        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
//        //IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
//        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
//        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
//        //IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
//        //IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
//        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
//        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
//        IronSourceEvents.onRewardedVideoAdLoadFailedEvent += RewardedVideoAdLoadFailed;
//    }
//    private void RewardedVideoAdOpenedEvent()
//    {
//        AudioListener.pause = true;
//    }
//    public void LoadRewardedAd()
//    {
//        //rewardedStatusText.text = "Loading...";
//        IronSource.Agent.loadRewardedVideo();
//    }

//    void RewardedVideoAvailabilityChangedEvent(bool available)
//    {
//        //Change the in-app 'Traffic Driver' state according to availability.
//        bool rewardedVideoAvailability = available;
//        if (available)
//        {
//            if (Config.ACTIVE_DEBUG_LOG)
//            {
//                Debug.Log("Rewarded ad loaded");
//            }

//            // Reset retry attempt
//            rewardedRetryAttempt = 0;
//        }
//        else
//        {
//            //txtRewardedStatus.text = "Loading...";
//        }
//        //Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + rewardedVideoAvailability);
//    }
//    void RewardedVideoAdLoadFailed(IronSourceError error)
//    {
//        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
//        rewardedRetryAttempt++;
//        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
//        //var retryDelay = 2 * Math.Min(6, rewardedRetryAttempt);

//        //rewardedStatusText.text = "Load failed: " + errorInfo.Code + "\nRetrying in " + retryDelay + "s...";
//        if (Config.ACTIVE_DEBUG_LOG)
//        {
//            Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
//        }
//        //Invoke("LoadRewardedAd", (float)retryDelay);
//        AdsController.instance.ReloadVideoReward((float)retryDelay);
//    }
//    void RewardedVideoAdShowFailedEvent(IronSourceError error)
//    {
//        // Rewarded ad failed to display. We recommend loading the next ad
//        if (Config.ACTIVE_DEBUG_LOG)
//        {
//            Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent");
//        }
//        //if (!IronSource.Agent.isRewardedVideoAvailable())
//        {
//            LoadRewardedAd();
//        }
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//            AdsController.rewardAdShowing = false;
//        });
//    }

//    //private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//    //{
//    //    Debug.Log("Rewarded ad displayed");
//    //}

//    //private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//    //{
//    //    Debug.Log("Rewarded ad clicked");
//    //}
//    void RewardedVideoAdClosedEvent()
//    {
//        // Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
//        if (Config.ACTIVE_DEBUG_LOG)
//        {
//            Debug.Log("Rewarded ad dismissed");
//        }
//        LoadRewardedAd();
//        UnityMainThreadDispatcher.Instance().Enqueue(()=> {
//            ClickCloseVideo();
//            AdsController.rewardAdShowing = false;
//            FirebaseManager.instance.LogVRDisplayed();

//            AudioListener.pause = false;
//        });
//        //ClickCloseVideo();
//    }
//    void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
//    {
//        if (Config.ACTIVE_DEBUG_LOG)
//        {
//            Debug.Log("Rewarded ad received reward");
//        }
//        VideoRewardComplete();
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//            AdsController.rewardAdShowing = false;
//        });
//    }

//    public bool IsRewardedVideoAvailable(string where = null)
//    {
//        if (Config.ACTIVE_TEST)
//        {
//            return true;
//        }
//        //#if UNITY_EDITOR
//        //        return true;
//        //#endif
//        bool check = IronSource.Agent.isRewardedVideoAvailable();
//        if (!check)
//        {
//            check = AppOpenAdManager.Instance.IsRewardedVideoAvailableGoogle();
//        }
//        return check;
//    }
//    //    public bool IsRewardedVideoAvailable(string where = null)
//    //    {
//    ////#if UNITY_EDITOR
//    ////        return true;
//    ////#endif
//    //        return (!ConfigGP.isActiveShow && this.rewardedAd != null && this.rewardedAd.IsLoaded());
//    //    }
//    public Action<bool> mOnRewardedAdCompleted;
//    private string strWhere = "unknown";
//    //public Action mOnClose;

//    //public bool ShowRewardedVideo(Action<bool> pOnCompleted, Action pOnClose)
//    public bool ShowRewardedVideo(Action<bool> pOnCompleted, string pWhere = null)
//    {
//        if (pWhere != null)
//        {
//            this.strWhere = pWhere;
//        }
//        else
//        {
//            this.strWhere = "unknown";
//        }
//        if (Config.ACTIVE_TEST)
//        {
//            pOnCompleted(true);
//            return true;
//        }
//        //#if UNITY_EDITOR
//        //        pOnCompleted(true);
//        //        return true;
//        //#endif
//        bool isShow = false;
//        if (IronSource.Agent.isRewardedVideoAvailable())
//        {
//            //rewardedStatusText.text = "Showing";
//            Config.showInterOnPause = false;
//            isShow = true;
//            IronSource.Agent.showRewardedVideo(pWhere);
//        }
//        else
//        {
//            //rewardedStatusText.text = "Ad not ready";
//        }
//        if (isShow)
//        {
//            //isActiveShow = true;
//            mOnRewardedAdCompleted = pOnCompleted;
//            //mOnClose = pOnClose;
//        }
//        else
//        {
//            //---backfill---
//            isShow = AppOpenAdManager.Instance.ShowRewardedAd(pOnCompleted, pWhere);

//            //pOnCompleted(false);

//            //pOnClose();
//        }
//        return isShow;
//    }
//    public void VideoRewardComplete()
//    {
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//            if (mOnRewardedAdCompleted != null)
//            {
//                mOnRewardedAdCompleted(true);
//                mOnRewardedAdCompleted = null;
//            }
//            AdsController.instance.RewardVideoComplete(strWhere);
//        });
//    }
//    public void ClickCloseVideo()
//    {
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//            AdsController.instance.CloseVideoRewarded();
//            if (mOnRewardedAdCompleted != null)
//            {
//                mOnRewardedAdCompleted(false);
//                mOnRewardedAdCompleted = null;
//            }
//        });
//    }
//#endregion

//#region Inter

//    Action<bool> mOnInterAdCompleted;
//    //private void StartInitInter()
//    //{
//    //    // Attach callbacks
//    //    MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
//    //    MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
//    //    MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
//    //    MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
//    //    MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

//    //    // Load the first interstitial
//    //    LoadInterstitial();
//    //}
//    private void InitInterstitialAds()
//    {

//        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
//        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
//        //IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
//        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
//        //IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
//        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
//        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
//    }

//    private void InterstitialAdOpenedEvent()
//    {
//        AudioListener.pause = true;
//    }
//    public void LoadInterstitial()
//    {
//        //Debug.Log("unity-script: I got LoadInterstitial");
//        //interstitialStatusText.text = "Loading...";
//        IronSource.Agent.loadInterstitial();
//    }

//    public bool IsInterAvailable()
//    {
//        if (Config.ACTIVE_TEST)
//        {
//            return true;
//        }
//        //#if UNITY_EDITOR
//        //        return true;
//        //#endif
//        bool check = IronSource.Agent.isInterstitialReady();
//        if (!check)
//        {
//            check = AppOpenAdManager.Instance.IsInterAvailableGoogle();
//        }
//        return check;
//    }
//    public bool ShowInter(Action<bool> pOnCompleted, string placement)
//    {
//        if (placement == null)
//        {
//            placement = "unknown";
//        }
//        if (Config.ACTIVE_TEST)
//        {
//            if (pOnCompleted != null)
//            {
//                pOnCompleted(true);
//            }
//            return true;
//        }
//        //#if UNITY_EDITOR
//        //        pOnCompleted(true);
//        //        return true;
//        //#endif
//        bool isShow = false;
//        if (IronSource.Agent.isInterstitialReady())
//        {
//            Config.showInterOnPause = false;
//            //interstitialStatusText.text = "Showing";
//            IronSource.Agent.showInterstitial(placement);
//            isShow = true;
//        }
//        else
//        {
//            //interstitialStatusText.text = "Ad not ready";
//        }
//        //-----backfill-----
//        if (!isShow)
//        {
//            isShow = AppOpenAdManager.Instance.ShowInterstitialAd();
//        }
//        //-----------

//        if (isShow)
//        {
//            //mOnInterAdCompleted = pOnCompleted;
//            if (pOnCompleted != null)
//            {
//                pOnCompleted(true);
//            }
//        }
//        else
//        {
//            if (pOnCompleted != null)
//            {
//                pOnCompleted(false);
//            }
//        }
//        return isShow;
//    }
//    void InterstitialAdReadyEvent()
//    {
//        if (Config.ACTIVE_DEBUG_LOG)
//        {
//            Debug.Log("Interstitial loaded");
//        }
//        // Reset retry attempt
//        interstitialRetryAttempt = 0;
//    }
//    void InterstitialAdLoadFailedEvent(IronSourceError error)
//    {
//        interstitialRetryAttempt++;
//        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
//        //var retryDelay = 2 * Math.Min(6, rewardedRetryAttempt);

//        //interstitialStatusText.text = "Load failed: " + errorInfo.Code + "\nRetrying in " + retryDelay + "s...";
//        if (Config.ACTIVE_DEBUG_LOG)
//        {
//            Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
//        }
//        //Invoke("LoadInterstitial", (float)retryDelay);
//        AdsController.instance.ReloadInter((float)retryDelay);
//    }
//    void InterstitialAdShowFailedEvent(IronSourceError error)
//    {
//        //Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
//        // Interstitial ad failed to display. We recommend loading the next ad
//        LoadInterstitial();
//        RefreshInterClose();
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//            AdsController.interAdShowing = false;
//        });
//    }
//    void InterstitialAdClosedEvent()
//    {
//        if (Config.ACTIVE_DEBUG_LOG)
//        {
//            Debug.Log("unity-script: I got InterstitialAdClosedEvent");
//        }
//        LoadInterstitial();
//        RefreshInterClose();
//        AdsController.instance.CloseInterShow();
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//            AdsController.interAdShowing = false;
//            FirebaseManager.instance.LogInterDisplayed();

//            AudioListener.pause = false;
//        });
//    }

//    public void RefreshInterClose()
//    {
//        if (mOnInterAdCompleted != null)
//        {
//            mOnInterAdCompleted(true);
//            mOnInterAdCompleted = null;
//        }
//    }
//#endregion

//#region banner
//    private bool bannerAdsAvailable = false;
//    //private void InitializeBannerAds()
//    //{
//    //    // Attach Callbacks
//    //    MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
//    //    //MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
//    //    //MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
//    //    MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

//    //    // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
//    //    // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
//    //    MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

//    //    // Set background or background color for banners to be fully functional.
//    //    MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, Color.black);
//    //}
//    private void InitBannerAds()
//    {
//        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
//        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
//        //IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
//        //IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
//        //IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
//        //IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
//        //LoadBanner();
//    }

//    public void LoadBanner()
//    {
//        var size = IronSourceBannerSize.BANNER;
//        size.SetAdaptive(true);
//        IronSource.Agent.loadBanner(size, ConfigIdsAds.bannerPosition);
//        Debug.Log("Start load Banner Ad...");
//    }
//    void BannerAdLoadedEvent()
//    {
//        bannerAdsAvailable = true;
//        Debug.Log("unity-script: I got BannerAdLoadedEvent");
//        AdsController.instance.CancelDelayLoadAgainBanner();
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//            AdsController.instance.ShowBannerAd();
//        });
//    }
//    void BannerAdLoadFailedEvent(IronSourceError error)
//    {
//        bannerAdsAvailable = false;
//        RetryLoadBannerAds();
//        Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
//    }
//    void RetryLoadBannerAds()
//    {
//        if (!bannerAdsAvailable)
//        {
//            Debug.Log("Banner Ad Load Failed. Try again after 3s");
//            AdsController.instance.DelayLoadAgainBanner();
//        }
//    }
//    public void Request_Banner2()
//    {
//        if (!AdsController.SCREEN_SHOW_BANNER)
//        {
//            return;
//        }
//        LoadBanner();
//    }
//    public void ShowBannerAd()
//    {
//        if (bannerAdsAvailable)
//        {
//            IronSource.Agent.displayBanner();
//        }
//        else
//        {
//            RetryLoadBannerAds();
//        }
//    }

//    public void HideBannerAd()
//    {
//        IronSource.Agent.hideBanner();
//    }
//    void BannerAdClickedEvent()
//    {
//        Debug.Log("unity-script: I got BannerAdClickedEvent");
//    }
//    //Notifies the presentation of a full screen content following user click
//    void BannerAdScreenPresentedEvent()
//    {
//        Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
//    }
//    //Notifies the presented screen has been dismissed
//    void BannerAdScreenDismissedEvent()
//    {
//        Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
//    }
//    //Invoked when the user leaves the app
//    void BannerAdLeftApplicationEvent()
//    {
//        Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
//    }
//#endregion
//#region MREC Ad Methods

//    private void InitializeMRecAds()
//    {
//        // Attach Callbacks
//        //MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
//        //MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdFailedEvent;
//        //MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
//        //MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaidEvent;

//    }
//    private void LoadMRecAds()
//    {

//        // MRECs are automatically sized to 300x250.
//        //MaxSdk.CreateMRec(MRecAdUnitId, MaxSdkBase.AdViewPosition.TopCenter);
//    }

//    public void ShowMRec()
//    {
//        //MaxSdk.ShowMRec(MRecAdUnitId);
//    }
//    public void HideMRec()
//    {
//        //MaxSdk.HideMRec(MRecAdUnitId);
//    }

//    //private void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//    //{
//    //    // MRec ad is ready to be shown.
//    //    // If you have already called MaxSdk.ShowMRec(MRecAdUnitId) it will automatically be shown on the next MRec refresh.
//    //    Debug.Log("MRec ad loaded");
//    //}

//    //private void OnMRecAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
//    //{
//    //    // MRec ad failed to load. MAX will automatically try loading a new ad internally.
//    //    Debug.Log("MRec ad failed to load with error code: " + errorInfo.Code);
//    //}

//    //private void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//    //{
//    //    Debug.Log("MRec ad clicked");
//    //}

//    //private void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//    //{
//    //    // MRec ad revenue paid. Use this callback to track user revenue.
//    //    Debug.Log("MRec ad revenue paid");

//    //    // Ad revenue
//    //    double revenue = adInfo.Revenue;

//    //    // Miscellaneous data
//    //    string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
//    //    string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
//    //    string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
//    //    string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
//    //}

//#endregion
//#region AOA Ad
//    private bool isFirstLoad = true;
//    public void OnApplicationPause(bool pauseStatus)
//    {
//        //        OnPauseAOAGoogleAds(pauseStatus);

//        IronSource.Agent.onApplicationPause(pauseStatus);
//        //if (!pauseStatus)
//        //{
//        //    AppOpenAdManager.Instance.ShowAdIfAvailable();
//        //   // ShowAdIfReady();
//        //}
//    }

//    //private void InitAOA()
//    //{
//    //    MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
//    //    MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAppOpenLoadedEvent;
//    //    MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAppOpenLoadFailedEvent;
//    //    MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAppOpenFailedToDisplayEvent;

//    //    MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
//    //    //ShowAdIfReady();
//    //}
//    //public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//    //{
//    //    if (Config.ACTIVE_DEBUG_LOG)
//    //    {
//    //        Debug.Log("Aoa OnAppOpenDismissedEvent, ID: " + AppOpenAdUnitId);
//    //    }
//    //    MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
//    //    AdsController.instance.CloseAOAShow();
//    //}
//    //public void OnAppOpenLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//    //{
//    //    if (Config.ACTIVE_DEBUG_LOG)
//    //    {
//    //        Debug.Log("Aoa loaded successfully, ID: " + AppOpenAdUnitId);
//    //    }
//    //    if (isFirstLoad)
//    //    {
//    //        isFirstLoad = false;
//    //        ShowAdIfReady();
//    //        if (!Config.FIRST_LOAD_ADS_DONE)
//    //        {
//    //            Config.FIRST_LOAD_ADS_DONE = true;
//    //        }
//    //        //#if !UNITY_EDITOR
//    //        LoadInterstitial();
//    //        LoadRewardedAd();
//    //        //#endif
//    //    }
//    //}

//    //private void OnAppOpenLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
//    //{
//    //    if (isFirstLoad)
//    //    {
//    //        isFirstLoad = false;
//    //        //#if !UNITY_EDITOR
//    //        LoadInterstitial();
//    //        LoadRewardedAd();
//    //        //#endif
//    //    }
//    //    if (Config.ACTIVE_DEBUG_LOG)
//    //    {
//    //        Debug.Log(errorInfo.Code + "  Load AOA Failed. : " + errorInfo.ToString());
//    //    }
//    //}
//    //private void OnAppOpenFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
//    //{
//    //    // Rewarded ad failed to display. We recommend loading the next ad
//    //    if (Config.ACTIVE_DEBUG_LOG)
//    //    {
//    //        Debug.Log(errorInfo.Code + "  AOA ad failed to display with error code: " + errorInfo.ToString());
//    //    }
//    //    MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
//    //}

//    //public void ShowAdIfReady()
//    //{
//    //    if (Config.ACTIVE_DEBUG_LOG)
//    //    {
//    //        Debug.Log("AOA ShowAdIfReady : " + Config.GetRemoveAd() + "   --   " + MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId));
//    //    }
//    //    if (Config.GetRemoveAd()) {
//    //        return;
//    //    }

//    //    if (MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId))
//    //    {
//    //        if (Config.ACTIVE_DEBUG_LOG)
//    //        {
//    //            Debug.Log("Showwwwww App Open Ad");
//    //        }
//    //        MaxSdk.ShowAppOpenAd(AppOpenAdUnitId);
//    //    }
//    //    else
//    //    {
//    //        if (Config.ACTIVE_DEBUG_LOG)
//    //        {
//    //            Debug.Log("Load App Open Ad");
//    //        }
//    //        MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
//    //    }
//    //}
//    //public void OnApplicationPause(bool pauseStatus)
//    //{
//    //    if (!pauseStatus)
//    //    {
//    //        ShowAdIfReady();
//    //    }
//    //}

//#endregion
//    public void OnDestroy()
//    {

//        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;

//        AppOpenAdManager.Instance.OnDestroy();
//    }

//#region Ad Impression
//    void ImpressionSuccessEvent(IronSourceImpressionData impressionData)
//    {
//        Debug.Log("unity - script: I got ImpressionSuccessEvent ToString(): " + impressionData.ToString());
//        // Debug.Log("unity - script: I got ImpressionSuccessEvent allData: " + impressionData.allData);

//        //if (impressionData != null && !string.IsNullOrEmpty(impressionData.adNetwork))
//        //{
//        //    SendEventRealtime(impressionData);
//        //}
//    }

//    void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
//    {
//        Debug.Log("unity - script: I got ImpressionDataReadyEvent ToString(): " + impressionData.ToString());

//        SendEventRealtime(impressionData);
//        //Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);
//#if ACTIVE_ADJUST
//        if (impressionData != null && !string.IsNullOrEmpty(impressionData.adNetwork))
//        {
//            SendEventRealtime(impressionData);
//            //--------
//            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceIronSource);
//            adjustAdRevenue.setRevenue(impressionData.revenue.Value, "USD");
//            // optional fields
//            adjustAdRevenue.setAdRevenueNetwork(impressionData.adNetwork);
//            adjustAdRevenue.setAdRevenueUnit(impressionData.adUnit);
//            adjustAdRevenue.setAdRevenuePlacement(impressionData.placement);
//            // track Adjust ad revenue
//            Adjust.trackAdRevenue(adjustAdRevenue);
//        }
//#endif
//    }

//    private static void SendEventRealtime(IronSourceImpressionData data)
//    {
//#if ACTIVE_FIREBASE_ANALYTIC
//        /*
//        Firebase.Analytics.Parameter[] AdParameters = {
//             new Firebase.Analytics.Parameter("ad_platform", "iron_source"),
//             new Firebase.Analytics.Parameter("ad_source", data.adNetwork),
//             new Firebase.Analytics.Parameter("ad_unit_name",data.adUnit),
//             new Firebase.Analytics.Parameter("currency","USD"),
//             new Firebase.Analytics.Parameter("value",data.revenue.Value),
//             new Firebase.Analytics.Parameter("placement",data.placement),
//             new Firebase.Analytics.Parameter("country_code",data.country),
//             new Firebase.Analytics.Parameter("ad_format",data.instanceName),
//        };

//        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", AdParameters);
//        */
//        if (data != null) { 
//        Firebase.Analytics.Parameter[] AdParameters = {
//        new Firebase.Analytics.Parameter("ad_platform", "ironSource"),
//        new Firebase.Analytics.Parameter("ad_source", data.adNetwork),
//        new Firebase.Analytics.Parameter("ad_unit_name", data.instanceName),
//        //new Firebase.Analytics.Parameter("ad_unit_name", data.adUnit),
//        new Firebase.Analytics.Parameter("ad_format", data.adUnit),
//        new Firebase.Analytics.Parameter("currency","USD"),
//        new Firebase.Analytics.Parameter("value", data.revenue.Value)
//      };
//        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", AdParameters);
//        }
//#endif
//    }

//#endregion
//}