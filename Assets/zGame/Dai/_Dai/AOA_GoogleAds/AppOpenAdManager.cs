using System;
using System.Collections;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;



public class AppOpenAdManager
{
    //#if UNITY_ANDROID
    //    private const string ID_TIER_1 = "TIER_1_HERE";
    //    private const string ID_TIER_2 = "TIER_2_HERE";
    //    private const string ID_TIER_3 = "TIER_3_HERE";

    //#elif UNITY_IOS
    //    private const string ID_TIER_1 = "";
    //    private const string ID_TIER_2 = "";
    //    private const string ID_TIER_3 = "";
    //#else
    //    private const string ID_TIER_1 = "";
    //    private const string ID_TIER_2 = "";
    //    private const string ID_TIER_3 = "";
    //#endif
    //    //----BANNER COLLAP-----
    //#if UNITY_ANDROID
    //    private static string _adUnitId = "";
    //#elif UNITY_IOS
    //    private static string _adUnitId = "";
    //#else
    //    private static string _adUnitId = "";
    //#endif
    //    //--------------------
    //    //-----Backfill-----
    //#if UNITY_ANDROID
    //    private static string _adInterID = "";
    //    private static string _adRewardID = "";
    //#elif UNITY_IOS
    //    private static string _adInterID = "";
    //    private static string _adRewardID = "";
    //#else
    //    private static string _adInterID = "";
    //    private static string _adRewardID = "";
    //#endif
    //---------------

    static AppOpenAdManager instance;

    private AppOpenAd ad;

    private DateTime loadTime;

    static bool inited = false;

    private bool isShowingAd = false;

    public bool showFirstOpen = false;

    public static bool ConfigOpenApp = true;
    public static bool ConfigResumeApp = true;

    public static bool ResumeFromAds = false;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    private bool IsAdAvailable => ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;

    private int tierIndex = 1;

    public void InitActive()
    {
        if (inited)
        {
            return;
        }
        inited = true;
    }
    public void CheckWaitLoadAd()
    {
        if (isWaitRewardAd)
        {
            LoadRewardedAd();
        }
        if (isWaitInterAd)
        {
            LoadInterstitialAd();
        }
        if (isWaitBannerAd)
        {
            LoadBannerAd();
        }
    }
    public void LoadAd()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            //if (Config.GetRemoveAd())
            //{
            //    if (!Config.FIRST_LOAD_ADS_DONE)
            //    {
            //        Config.FIRST_LOAD_ADS_DONE = true;
            //    }
            //    //return;
            //}

            // if (IAP_AD_REMOVED)
            //     return;

            //HideBannerCollap();
            //UnityMainThreadDispatcher.Instance().EnqueueTime2(() =>
            //{
            //    LoadBannerAd();
            //});

            // LoadBannerAd();//

            //Debug.Log("-----------DDDDDDDDDDDDD---------------");

            //#if UNITY_ANDROID
            //            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
            //#elif UNITY_IPHONE
            //                    string adUnitId = "ca-app-pub-3940256099942544/2934735716";
            //#else
            //                    string adUnitId = "unexpected_platform";
            //#endif

            //            // Create a 320x50 banner at the top of the screen.
            //            BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
            //            // Create an empty ad request.
            //            AdRequest request = new AdRequest();
            //            // Load the banner with the request.
            //            bannerView.LoadAd(request);

            ////RequestConfiguration.Builder.SetTestDeviceIds(new System.Collections.Generic.List<string>() { "33BE2250B43518CCDA7DE426D04EE231" });
            //System.Collections.Generic.List<string> deviceIds = new System.Collections.Generic.List<string>();
            ////This is the my device ID for testing
            //deviceIds.Add("33BE2250B43518CCDA7DE426D04EE231");
            //RequestConfiguration requestConfiguration = new RequestConfiguration.Builder().SetTestDeviceIds(deviceIds).build();
            //MobileAds.SetRequestConfiguration(requestConfiguration);

            LoadAOA();
            //LoadInterstitialAd();
            //----
#if NATIVE_AD_ADMOB
            RequestNativeAd();
#endif
            /*string s = GetDeviceID();
            Debug.Log("Device ID: "+s);
        
            RequestConfiguration requestConfiguration = new RequestConfiguration();
            requestConfiguration.TestDeviceIds.Add(s);
            MobileAds.SetRequestConfiguration(requestConfiguration);*/
        });
    }
    public static string GetDeviceID()
    {
        // Get Android ID
        AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");

        string android_id = clsSecure.CallStatic<string>("getString", objResolver, "android_id");

        // Get bytes of Android ID
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(android_id);

        // Encrypt bytes with md5
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        string device_id = hashString.PadLeft(32, '0');

        return device_id;
    }
    //-------------
    bool isActiveFirstLoad_Reward = false;
    public void LoadRewardPre()
    {
        if (isActiveFirstLoad_Reward)
        {
            return;
        }
        isActiveFirstLoad_Reward = true;
        Config.AddLogShowDebug("FirstLoadRewardAd_Admob");
        LoadRewardedAd();
    }
    bool isActiveFirstLoad_Inter = false;
    public void LoadInterPre()
    {
        if (isActiveFirstLoad_Inter)
        {
            return;
        }
        isActiveFirstLoad_Inter = true;
        Config.AddLogShowDebug("FirstLoadInter_Admob");
        LoadInterstitialAd();
    }
    bool isActiveFirstLoad_Banner = false;
    public void LoadBannerPre()
    {
        if (isActiveFirstLoad_Banner)
        {
            return;
        }
        isActiveFirstLoad_Banner = true;
        Config.AddLogShowDebug("FirstLoadBanner_Admob");
        HideBannerCollap();
        LoadBannerAd();
    }
    //---------------------------

    public void LoadAOA()
    {
        string id = ConfigIdsAds.ID_TIER_1;
        if (tierIndex == 2)
            id = ConfigIdsAds.ID_TIER_2;
        else if (tierIndex == 3)
            id = ConfigIdsAds.ID_TIER_3;

        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Start request Open App Ads Tier " + tierIndex);
        }
        var request = new AdRequest();
        //AdRequest request = new AdRequest.Builder().Build();

        AppOpenAd.Load(id, /*ScreenOrientation.Portrait,*/ request, ((appOpenAd, error) =>
        {
            //-----DAI FIX----
            //AdsController.instance.admob.FirstLoadAdsMax_WhenWaitFetch();
            //-------------

            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat("Failed to load the ad. (reason: {0}), tier {1}", error.GetMessage(), tierIndex);
                tierIndex++;
                if (tierIndex <= 3)
                    LoadAOA();
                else
                    tierIndex = 1;
                return;
            }

            if (ad != null)
            {
                ad.Destroy();
                ad = null;
            }

            // App open ad is loaded.
            ad = appOpenAd;
            tierIndex = 1;
            loadTime = DateTime.UtcNow;
            //if (!Config.FIRST_LOAD_ADS_DONE)
            //{
            //    Config.FIRST_LOAD_ADS_DONE = true;
            //}
            AdsController.instance.FirstLoadAOA_Done();
            if (!showFirstOpen && ConfigOpenApp && AdsController.AOA_FIRST_OPEN_ACTIVE)
            {
                showFirstOpen = true;
                if (!Config.isActiveAOA)
                {
                    return;
                }
                ShowAdIfAvailable();
            }
        }));
    }

    public void ShowAdIfAvailable()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            //-----DAI FIX----
            if (Config.ACTIVE_TEST) return;
            if (Config.GetRemoveAd())
            {
                return;
            }
            //----------------
            if (!IsAdAvailable)
            {
                LoadAOA();
                return;
            }
            //if (!IsAdAvailable || isShowingAd)
            if (isShowingAd)
            {
                return;
            }
            ad.OnAdFullScreenContentClosed += HandleAdDidDismissFullScreenContent;
            ad.OnAdFullScreenContentFailed += HandleAdFailedToPresentFullScreenContent;
            ad.OnAdFullScreenContentOpened += HandleAdDidPresentFullScreenContent;
            ad.OnAdImpressionRecorded += HandleAdDidRecordImpression;
            ad.OnAdPaid += HandlePaidEvent;
            Config.showInterOnPause = false;
            ad.Show();
        });

    }

    private void HandleAdDidDismissFullScreenContent()
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Closed app open ad");
        }
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        LoadAOA();
        AdsController.instance.CloseAOAShow();
    }

    private void HandleAdFailedToPresentFullScreenContent(AdError args)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.LogFormat("Failed to present the ad (reason: {0})", args.GetMessage());
        }
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        LoadAOA();
    }

    private void HandleAdDidPresentFullScreenContent()
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Displayed app open ad");
        }
        isShowingAd = true;
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            FirebaseManager.instance.LogAppOpenDisplayed();
        });
    }

    private void HandleAdDidRecordImpression()
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Recorded ad impression");
        }
    }

    private void HandlePaidEvent(AdValue args)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
            args.CurrencyCode, args.Value);
        }
    }
    //---------
    //---------BANNER------------

    static BannerView _bannerView;
    static bool isWaitBannerAd = false;
    public static void LoadBannerAd()
    {
        if (!inited)
        {
            isWaitBannerAd = true;
            return;
        }
        isWaitBannerAd = false;
        //if (!inited
        //    //|| string.IsNullOrEmpty(ConfigIdsAds._adUnitId)
        //    )
        //{
        //    return;
        //}
        if (isActiveCountTimeLoaded)
        {
            return;
        }
        isActiveCountTimeLoaded = true;
        AdsController.instance.ResetActiveLoadBannerCollap(6f);//thoi gian de uoc tinh load dc banner collap

        AdsController.instance.CancelReloadBannerCollapOnFailLoad();
        if (_bannerView != null)
        {
            DestroyBannerView();
        }
        isBannerCollapLoaded = false;
        isBannerLoaded = false;
        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        _bannerView = new BannerView(ConfigIdsAds._adUnitId, adaptiveSize, AdPosition.Bottom);
        _bannerView.OnBannerAdLoaded += () =>
        {
            isBannerLoaded = true;
            isBannerCollapLoaded = true;
            if (!firstShowBanner)
            {
                AdsController.instance.ActiveCountTimeResetCollapBanner();
            }

            isActiveCountTimeLoaded = false;
            AdsController.instance.CancelResetActiveLoadBannerCollap();
            // Debug.Log("Banner view loaded an ad with response : "
            //        + _bannerView.GetResponseInfo());
            if (AdsController.isCallShowBanner && !AdsController.instance.backFillBannerCollap)
            {
                UnityMainThreadDispatcher.Instance().EnqueueTime1(() =>
                {
                    HideBannerCollap();
                });
            }
            else
            {


                if (isActiveBannerCollap)
                {
                    isBannerLoaded = false;
                    // _bannerView.Show();
                    ShowBannerCollap();
                }
                else
                {
                    ShowBannerCollap();

                    //_bannerView.Show();
                    // HideBannerCollap();
                    isActiveBannerCollap = false;
                    UnityMainThreadDispatcher.Instance().EnqueueTime1(() =>
                    {
                        if (!isActiveBannerCollap)
                        {
                            //_bannerView.Hide();
                            HideBannerCollap();
                        }
                    });
                    //_bannerView.Hide();
                }
            }
        };

        _bannerView.OnBannerAdLoadFailed += (loadAdError) =>
        {
            isBannerLoaded = false;
            isBannerCollapLoaded = false;
            isActiveCountTimeLoaded = false;
            AdsController.instance.CancelResetActiveLoadBannerCollap();

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                AdsController.instance.ReloadBannerCollapOnFailLoad(5f);
            });
            //AdsController.instance.ReloadBannerCollapOnFailLoad(5f);
            // LoadBannerAd();
        };

        var adRequest = new AdRequest();
        //var adRequest = new AdRequest.Builder().Build();

        // Create an extra parameter that aligns the bottom of the expanded ad to the
        // bottom of the bannerView.
        adRequest.Extras.Add("collapsible", "bottom");
        adRequest.Extras.Add("collapsible_request_id", Guid.NewGuid().ToString());
        _bannerView.LoadAd(adRequest);
    }

    public static void HideBannerCollap()
    {
        isActiveBannerCollap = false;
        if (_bannerView != null)
        {
            _bannerView.Hide();
            //DestroyBannerView();
        }
    }
    static bool isActiveCountTimeLoaded = false;

    public static bool isActiveBannerCollap = false;

    static bool firstShowBanner = false;

    public static bool isBannerLoaded = false;
    public static bool isBannerCollapLoaded = false;

    public static void ShowBannerCollap()
    {
        firstShowBanner = true;
        isActiveBannerCollap = true;
        if (_bannerView != null)
        {
            if (isBannerLoaded)
            {
                isBannerLoaded = false;
            }
            if (AdsController.instance.backFillBannerCollap)
            {
                AdsController.instance.backFillBannerCollap = false;
                AdsController.instance.HideBannerAd();
            }
            _bannerView.Show();
        }
        else
        {
            LoadBannerAd();
        }
    }
    public static void ResetShowBannerCollap()
    {
        if (firstShowBanner && !isActiveCountTimeLoaded && !isBannerLoaded)
        {
            //HideBannerCollap();
            DestroyBannerView();
        }
        ShowBannerCollap();
    }

    static void DestroyBannerView()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    public static void ResetActiveLoadBannerCollap()
    {
        isActiveCountTimeLoaded = false;
    }
    //------inter -----
    private InterstitialAd _interstitialAd;
    bool isWaitInterAd = false;
    public void LoadInterstitialAd()
    {
        if (!inited)
        {
            isWaitInterAd = true;
            return;
        }
        isWaitInterAd = false;
        if (string.IsNullOrEmpty(ConfigIdsAds._adInterID))
        {
            return;
        }

        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Loading the interstitial ad.");
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(ConfigIdsAds._adInterID, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {

                    if (Config.ACTIVE_DEBUG_LOG)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    }
                    return;
                }

                if (Config.ACTIVE_DEBUG_LOG)
                {
                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());
                }

                _interstitialAd = ad;
                RegisterEventHandlers(_interstitialAd);
            });
    }

    public bool IsInterAvailableGoogle()
    {
        if (Config.ACTIVE_TEST)
        {
            return true;
        }
        //#if UNITY_EDITOR
        //        return true;
        //#endif
        bool check = false;
        if (inited && _interstitialAd != null && _interstitialAd.CanShowAd())
        {
            check = true;
        }

        return check;
    }
    public bool ShowInterstitialAd()
    {
        if (string.IsNullOrEmpty(ConfigIdsAds._adInterID))
        {
            return false;
        }
        if (Config.ACTIVE_TEST)
        {
            //if (pOnCompleted != null)
            //{
            //    pOnCompleted(true);
            //}
            return true;
        }
        bool isShow = false;
        if (IsInterAvailableGoogle())
        {
            Config.showInterOnPause = false;

            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("Showing interstitial ad.");
            }
            _interstitialAd.Show();
            isShow = true;
        }
        else
        {

            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }
        }
        //if (isShow)
        //{
        //    //mOnInterAdCompleted = pOnCompleted;
        //    if (pOnCompleted != null)
        //    {
        //        pOnCompleted(true);
        //    }
        //}
        //else
        //{
        //    if (pOnCompleted != null)
        //    {
        //        pOnCompleted(false);
        //    }
        //}
        return isShow;

    }
    public void RefreshInterClose()
    {
        /*if (mOnInterAdCompleted != null)
        {
            mOnInterAdCompleted(true);
            mOnInterAdCompleted = null;
        }*/
    }
    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            }
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("Interstitial ad recorded an impression.");
            }
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("Interstitial ad was clicked.");
            }
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("Interstitial ad full screen content opened.");
            }
            AdsController.interAdShowing = true;
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("Interstitial ad full screen content closed.");
            }
            LoadInterstitialAd();
            RefreshInterClose();
            AdsController.instance.CloseInterShow();
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                AdsController.interAdShowing = false;
                FirebaseManager.instance.LogInterDisplayed();
            });
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            }
            LoadInterstitialAd();

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                AdsController.interAdShowing = false;
            });
        };
    }
    //-------reward -----------
    bool isWaitRewardAd = false;
    private RewardedAd _rewardedAd;
    public void LoadRewardedAd()
    {
        if (!inited)
        {
            isWaitRewardAd = true;
            return;
        }
        isWaitRewardAd = false;
        if (string.IsNullOrEmpty(ConfigIdsAds._adRewardID))
        {
            return;
        }
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        if (Config.ACTIVE_DEBUG_LOG)
        {
            Debug.Log("Loading the rewarded ad.");
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(ConfigIdsAds._adRewardID, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    if (Config.ACTIVE_DEBUG_LOG)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    }
                    return;
                }

                if (Config.ACTIVE_DEBUG_LOG)
                {
                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());
                }
                _rewardedAd = ad;
                RegisterEventHandlers(_rewardedAd);
            });
    }
    public bool IsRewardedVideoAvailableGoogle()
    {
        if (Config.ACTIVE_TEST)
        {
            return true;
        }

        bool check = false;
        if (inited && _rewardedAd != null && _rewardedAd.CanShowAd())
        {
            check = true;
        }
        return check;
    }
    public Action<bool> mOnRewardedAdCompletedGoogle;
    private string strWhere = "unknown";
    public bool ShowRewardedAd(Action<bool> pOnCompleted, string pWhere = null)
    {
        if (string.IsNullOrEmpty(ConfigIdsAds._adRewardID))
        {
            return false;
        }
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
        //const string rewardMsg =
        //    "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        bool isShow = false;
        if (IsRewardedVideoAvailableGoogle())
        {
            Config.showInterOnPause = false;
            isShow = true;

            _rewardedAd.Show((GoogleMobileAds.Api.Reward reward) =>
            {
                // TODO: Reward the user.
                //Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    VideoRewardComplete();
                });
            });
        }
        else
        {
        }


        if (isShow)
        {
            mOnRewardedAdCompletedGoogle = pOnCompleted;
        }
        else
        {
            pOnCompleted(false);
            //pOnClose();
        }

        return isShow;

    }

    public void VideoRewardComplete()
    {
        if (mOnRewardedAdCompletedGoogle != null)
        {
            mOnRewardedAdCompletedGoogle(true);
            mOnRewardedAdCompletedGoogle = null;
        }
        AdsController.instance.RewardVideoComplete(strWhere);
    }
    public void ClickCloseVideo()
    {
        if (mOnRewardedAdCompletedGoogle != null)
        {
            mOnRewardedAdCompletedGoogle(false);
            mOnRewardedAdCompletedGoogle = null;
        }
        AdsController.instance.CloseVideoRewarded();
    }
    //-----
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            }
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("Rewarded ad recorded an impression.");
            }
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("Rewarded ad was clicked.");
            }
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("Rewarded ad full screen content opened.");
            }
            AdsController.rewardAdShowing = true;
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                FirebaseManager.instance.LogVRDisplayed();
            });
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("Rewarded ad full screen content closed.");
            }
            LoadRewardedAd();

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                ClickCloseVideo();
                AdsController.rewardAdShowing = false;
            });
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            }
            LoadRewardedAd();
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                AdsController.rewardAdShowing = false;
            });
        };
    }

    //--------------Native Ads------
#if NATIVE_AD_ADMOB
    public bool nativeAdLoaded=false;
    public NativeAd nativeAd;
    public void RequestNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder(ConfigIdsAds._adNativeId)
            .ForNativeAd()
            .Build();
        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;
        //adLoader.LoadAd(new AdRequest.Builder().Build());
        adLoader.LoadAd(new AdRequest());
    }

    private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
        Debug.Log("Native ad failed to load: " + args.LoadAdError.GetMessage());
        }
    }
    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
    {
        if (Config.ACTIVE_DEBUG_LOG)
        {
        Debug.Log("Native ad loaded.");
        }
        this.nativeAd = args.nativeAd;
        this.nativeAdLoaded = true;
        UnityMainThreadDispatcher.Instance().Enqueue1Frame(() =>
        {
            this.nativeAdLoaded = false;
        });
    }

    public void NativeAdDestroy()
    {
        if (nativeAd != null) {
            nativeAd.Destroy();
            this.nativeAdLoaded = false;
        }
    }
#endif
    //----------
    public void OnDestroy()
    {
#if NATIVE_AD_ADMOB
        NativeAdDestroy();
#endif
    }
}