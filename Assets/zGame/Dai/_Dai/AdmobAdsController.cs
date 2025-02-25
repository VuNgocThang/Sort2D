//using com.adjust.sdk;
//using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AdmobAdsController
//{
//    //idapp test : ca-app-pub-3940256099942544~3347511713
//    const float RATE_VALUE = 1000000f;

//    public bool inited = false;
//    public bool initAdVR = false;
//    public bool initAdInter = false;

//    public RewardedAd rewardedAdNormal1;
//    protected string adUnitId = "";


//    private int interstitialRetryAttempt;
//    private int rewardedRetryAttempt;
//    private int rewardedRetryAttemptVip;

//    bool isLoadFailInter = false;
//    bool isLoadFailReward = false;
//    bool isLoadFailRewardVIP = false;

//    //void Start()
//    public void Setup()
//    {
//        inited = false;
//        initAdVR = false;
//        initAdInter = false;
//#if UNITY_ANDROID
//        adUnitId = "ca-app-pub-6286675003708772/1899326524";
//        //adUnitId = "ca-app-pub-3940256099942544/5224354917";//ID TEST FIXME
//#elif UNITY_IOS
//            adUnitId = "ca-app-pub-8721698442392956/5186487389";
//#else
//            adUnitId = "unexpected_platform";
//#endif
//        MobileAds.SetiOSAppPauseOnBackground(true);
//        // Initialize the Google Mobile Ads SDK.
//        MobileAds.Initialize(initStatus =>
//        {
//            //Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
//            //foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
//            //{
//            //    string className = keyValuePair.Key;
//            //    AdapterStatus status = keyValuePair.Value;
//            //    switch (status.InitializationState)
//            //    {
//            //        case AdapterState.NotReady:
//            //            // The adapter initialization did not complete.
//            //            MonoBehaviour.print("Adapter: " + className + " not ready.");
//            //            break;
//            //        case AdapterState.Ready:
//            //            // The adapter was successfully initialized.
//            //            MonoBehaviour.print("Adapter: " + className + " is initialized.");
//            //            break;
//            //    }
//            //}
//            //init complete
//            //UnityMainThreadDispatcher.Instance().Enqueue(StartInitAds);
//            UnityMainThreadDispatcher.Instance().Enqueue(LoadRewardDelay);
//            if (!Config.GetRemoveAd())
//            {
//                //UnityMainThreadDispatcher.Instance().Enqueue(StartInitInter);
//                UnityMainThreadDispatcher.Instance().Enqueue(LoadInterDelay);
//            }
//            UnityMainThreadDispatcher.Instance().Enqueue(EndInitAds);

//            //Debug.Log("aaaaaaaaaaaaaaa : init succest");
//        });
//    }
//    private void EndInitAds()
//    {
//        inited = true;
//        AdsController.instance.EndInitAds();
//    }
//    public void ActiveStartInitInter()
//    {
//        if (!Config.GetRemoveAd())
//        {
//            UnityMainThreadDispatcher.Instance().Enqueue(StartInitInter);
//        }
//    }
//    public void ActiveStartReward()
//    {
//        UnityMainThreadDispatcher.Instance().Enqueue(StartInitAds);
//    }
//    void LoadRewardDelay()
//    {
//        AdsController.instance.DelayLoadReward();
//    }

//    void LoadInterDelay()
//    {
//        AdsController.instance.DelayLoadInter();
//    }

//    public void StartInitAds()
//    {
//        if (initAdVR)
//        {
//            return;
//        }
//        //ConfigGP.isActiveShow = false;
//        rewardedAdNormal1 = CreateAndLoadRewardedAd(adUnitId);

//        initAdVR = true;
//    }
//    public void LoadRewardedAd()
//    {
//        if (isLoadFailReward)
//        {
//            if (rewardedAdNormal1 == null || !rewardedAdNormal1.IsLoaded())
//            {
//                if (rewardedAdNormal1 != null)
//                {
//                    //rewardedAdNormal1.Destroy();
//                    AdRequest request = new AdRequest.Builder().Build();
//                    rewardedAdNormal1.LoadAd(request);
//                }
//                else
//                {
//                    rewardedAdNormal1 = CreateAndLoadRewardedAd(adUnitId);
//                }
//            }
//        }
//    }
//    //public void CreateNewVideo()
//    //{
//    //    ConfigGP.isActiveShow = false;
//    //    this.rewardedAd = new RewardedAd(adUnitId);
//    //    // Create an empty ad request.
//    //    AdRequest request = new AdRequest.Builder().Build();
//    //    // Load the rewarded ad with the request.
//    //    this.rewardedAd.LoadAd(request);
//    //    //// Called when an ad request has successfully loaded.
//    //    //this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
//    //    //// Called when an ad request failed to load.
//    //    //this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
//    //    //// Called when an ad is shown.
//    //    //this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
//    //    //// Called when an ad request failed to show.
//    //    //this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
//    //    //// Called when the user should be rewarded for interacting with the ad.
//    //    //this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
//    //    //// Called when the ad is closed.
//    //    //this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

//    //    this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
//    //    this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
//    //    this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
//    //}
//    public RewardedAd CreateAndLoadRewardedAd(string adUnitId)
//    {
//        isLoadFailReward = false;
//        //Debug.Log("aaaaaaaaaaaaaaa : start load reward video");
//        RewardedAd rewardedAd = new RewardedAd(adUnitId);

//        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
//        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
//        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
//        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
//        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
//        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;//FIXME
//        rewardedAd.OnPaidEvent += HandleAdPaidEvent;

//        // Create an empty ad request.
//        AdRequest request = new AdRequest.Builder().Build();
//        // Load the rewarded ad with the request.
//        rewardedAd.LoadAd(request);
//        return rewardedAd;
//    }
//    public bool IsRewardedVideoAvailable(string where = null)
//    {
//        if (Config.ACTIVE_TEST)
//        {
//            return true;
//        }
//        if (!initAdVR)
//        {
//            return false;
//        }
//        //#if UNITY_EDITOR
//        //        return true;
//        //#endif
//        bool check = false;
//        //if (!ConfigGP.isActiveShow)
//        {
//            if (this.rewardedAdNormal1 != null && this.rewardedAdNormal1.IsLoaded())
//            {
//                check = true;
//            }
//        }
//        return check;
//        //return (!ConfigGP.isActiveShow && this.rewardedAd != null && this.rewardedAd.IsLoaded());
//    }
//    //    public bool IsRewardedVideoAvailable(string where = null)
//    //    {
//    ////#if UNITY_EDITOR
//    ////        return true;
//    ////#endif
//    //        return (!ConfigGP.isActiveShow && this.rewardedAd != null && this.rewardedAd.IsLoaded());
//    //    }
//    public Action<bool> mOnRewardedAdCompleted;
//    public bool ShowRewardedVideo(ref Action<bool> pOnCompleted, string pWhere = null)
//    {
//        if (Config.ACTIVE_TEST)
//        {
//            pOnCompleted(true);
//            return true;
//        }
//        //#if UNITY_EDITOR
//        //        pOnCompleted(true);
//        //        return;
//        //#endif
//        bool isShow = false;
//        if (this.rewardedAdNormal1 != null && this.rewardedAdNormal1.IsLoaded())
//        {
//            Config.showInterOnPause = false;
//            this.rewardedAdNormal1.Show();
//            isShow = true;
//        }

//        if (isShow)
//        {
//            //ConfigGP.isActiveShow = true;
//            mOnRewardedAdCompleted = pOnCompleted;
//        }
//        else
//        {
//            //Debug.Log("admob google - False");
//        }
//        return isShow;
//    }
//    public void RefreshVideoRWClose()
//    {
//        rewardedAdNormal1 = CreateAndLoadRewardedAd(adUnitId);
//    }
//    public void VideoRewardComplete()
//    {
//        //ConfigGP.isActiveShow = false;
//        //ConfigGP.typeAdsEnd = 1;
//        if (mOnRewardedAdCompleted != null)
//        {
//            mOnRewardedAdCompleted(true);
//            mOnRewardedAdCompleted = null;
//            AdsController.instance.RewardVideoComplete();
//        }
//    }
//    public void VideoRewardFall()
//    {
//        //ConfigGP.isActiveShow = false;

//        if (mOnRewardedAdCompleted != null)
//        {
//            mOnRewardedAdCompleted(false);
//            mOnRewardedAdCompleted = null;
//        }
//    }
//    //-----------normal---------
//    public void HandleRewardedAdLoaded(object sender, EventArgs args)
//    {
//        //Debug.Log("HandleRewardedAdLoaded event received");
//        rewardedRetryAttempt = 0;
//        isLoadFailReward = false;
//    }

//    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//    {
//        //Debug.Log(
//        //    "HandleRewardedAdFailedToLoad event received with message: "
//        //                     + args.LoadAdError.GetMessage());
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//            rewardedRetryAttempt++;
//            //double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
//            float retryDelay = 15;
//            //if (rewardedRetryAttempt == 1)
//            //{
//            //    retryDelay = 15;
//            //}
//            //else if (rewardedRetryAttempt == 2)
//            //{
//            //    retryDelay = 30;
//            //}
//            isLoadFailReward = true;
//            AdsController.instance.ReloadVideoReward((float)retryDelay);
//        });
//    }

//    public void HandleRewardedAdOpening(object sender, EventArgs args)
//    {
//        //lock screen FIXME
//        //Debug.Log("HandleRewardedAdOpening event received");
//    }

//    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
//    {
//        //Debug.Log(
//        //    "HandleRewardedAdFailedToShow event received with message: "
//        //                     + args.Message);
//        //isActiveShow = false;
//        UnityMainThreadDispatcher.Instance().Enqueue(VideoRewardFall);
//    }

//    public void HandleRewardedAdClosed(object sender, EventArgs args)
//    {
//        //Debug.Log("HandleRewardedAdClosed event received");
//        UnityMainThreadDispatcher.Instance().Enqueue(RefreshVideoRWClose);
//    }

//    public void HandleUserEarnedReward(object sender, Reward args)
//    {
//        string type = args.Type;
//        double amount = args.Amount;
//        //Debug.Log(
//        //    "HandleRewardedAdRewarded event received for "
//        //                + amount.ToString() + " " + type);

//        UnityMainThreadDispatcher.Instance().Enqueue(VideoRewardComplete);
//    }
//    public void HandleAdPaidEvent(object sender, AdValueEventArgs args)
//    {
//        AdValue adValue = args.AdValue;
//        // TODO: Send the paid event information to your preferred analytics server
//        // directly within this callback.

//        //MonoBehaviour.print($"HandleAdPaidEvent received with ad value (in micros):{ adValue.Value}, precision: { adValue.Precision}, currency:{ adValue.CurrencyCode} from ad network adapter { this.rewardedAdNormal1.GetResponseInfo().GetMediationAdapterClassName()}");
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {

//            //Firebase.Analytics.Parameter[] LTVParameters = {
//            //// Log ad value in micros.
//            //new Firebase.Analytics.Parameter("value", (adValue.Value/RATE_VALUE)),
//            //// These values below won’t be used in ROAS recipe.
//            //// But log for purposes of debugging and future reference.
//            //new Firebase.Analytics.Parameter("currency",
//            //adValue.CurrencyCode),
//            //new Firebase.Analytics.Parameter("precision",
//            //(int)adValue.Precision),
//            //new Firebase.Analytics.Parameter("adunitid", adUnitId),
//            //new Firebase.Analytics.Parameter("network",
//            //rewardedAdNormal1.GetResponseInfo().GetMediationAdapterClassName())
//            //};
//            //Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", LTVParameters);


//            AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
//            adRevenue.setRevenue(adValue.Value / 1000000f, adValue.CurrencyCode);
//            Adjust.trackAdRevenue(adRevenue);
//        });
//    }
//    //public void HandleAdPaidEvent(object sender, AdValueEventArgs args)
//    //{
//    //    UnityMainThreadDispatcher.Instance().Enqueue(() =>
//    //    {
//    //        AdValue adValue = args.AdValue;
//    //        // Log an event with ad value parameters
//    //        Firebase.Analytics.Parameter[] LTVParameters = {
//    //        // Log ad value in micros.
//    //        new Firebase.Analytics.Parameter("value", adValue.Value),
//    //        // These values below won’t be used in ROAS recipe.
//    //        // But log for purposes of debugging and future reference.
//    //        new Firebase.Analytics.Parameter("currency",
//    //        adValue.CurrencyCode),
//    //        new Firebase.Analytics.Parameter("precision",
//    //        (int)adValue.Precision),
//    //        new Firebase.Analytics.Parameter("adunitid", adUnitId),
//    //        new Firebase.Analytics.Parameter("network",
//    //        rewardedAdNormal1.GetResponseInfo().GetMediationAdapterClassName())
//    //        };
//    //        Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_reward",
//    //        LTVParameters);
//    //    });
//    //}
//    //---------
//    #region Inter
//#if UNITY_ANDROID
//    string adUnitIdInterNormal = "ca-app-pub-6286675003708772/3212408199";
//    //string adUnitIdInterHight = "ca-app-pub-2227789348341993/7500910133";

//    //string adUnitIdInterNormal = "ca-app-pub-3940256099942544/1033173712";//Test
//    //string adUnitIdInterHight = "ca-app-pub-3940256099942544/1033173712";//Test
//#elif UNITY_IOS
//        string adUnitIdInterNormal = "ca-app-pub-8721698442392956/4757760903";
//    //string adUnitIdInterHight = "ca-app-pub-2227789348341993/7585967784";
//#else
//        string adUnitIdInterNormal = "unexpected_platform";
//    //string adUnitIdInterHight = "agentDestination";
//#endif
//    //public bool isActiveShowInter = false;

//    //private InterstitialAd interstitialHight;
//    private InterstitialAd interstitialNormal;
//    private int idInterShow;//1-normal , 2-hight
//    public Action<bool> mOnInterAdCompleted;
//    public void StartInitInter()
//    {
//        if (AdsController.isUseInter && !initAdInter)
//        {
//            //interstitialHight = CreateNewInter(adUnitIdInterHight);
//            interstitialNormal = CreateNewInter(adUnitIdInterNormal);

//            initAdInter = true;
//        }
//    }
//    public void RefreshInterClose()
//    {
//        try
//        {
//            if (mOnInterAdCompleted != null)
//            {
//                mOnInterAdCompleted(true);
//                mOnInterAdCompleted = null;
//            }
//            if (interstitialNormal != null)
//            {
//                interstitialNormal.Destroy();
//            }
//        }
//        catch (Exception)
//        {

//        }

//        interstitialNormal = CreateNewInter(adUnitIdInterNormal);

//        //if (mOnInterAdCompleted != null)
//        //{
//        //    mOnInterAdCompleted(true);
//        //    mOnInterAdCompleted = null;
//        //}

//        ////if (MusicManager.instance != null)
//        ////{
//        ////    MusicManager.instance.ResumeMusic();
//        ////}
//        ////isActiveShowInter = false;


//        ////if (idInterShow == 2)
//        ////{
//        ////    if (interstitialHight != null)
//        ////    {
//        ////        interstitialHight.Destroy();
//        ////    }
//        ////    interstitialHight = CreateNewInter(adUnitIdInterHight);
//        ////}
//        ////else
//        ////{
//        //if (interstitialNormal != null)
//        //{
//        //    interstitialNormal.Destroy();
//        //}
//        //interstitialNormal = CreateNewInter(adUnitIdInterNormal);
//        ////}

//        ////AdsController.instance.CloseInterShow();
//    }
//    public void LoadInterstitial()
//    {
//        if (isLoadFailInter)
//        {
//            if (interstitialNormal == null || !interstitialNormal.IsLoaded())
//            {
//                if (interstitialNormal != null)
//                {
//                    interstitialNormal.Destroy();
//                }
//                interstitialNormal = CreateNewInter(adUnitIdInterNormal);
//            }
//        }
//    }
//    private InterstitialAd CreateNewInter(string adUnitId)
//    {
//        isLoadFailInter = false;
//        // Initialize an InterstitialAd.
//        InterstitialAd interstitial = new InterstitialAd(adUnitId);
//        // Called when an ad request has successfully loaded.
//        interstitial.OnAdLoaded += HandleOnAdLoaded;
//        // Called when an ad request failed to load.
//        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
//        // Called when an ad is shown.
//        //interstitial.OnAdOpening += HandleOnAdOpened;
//        // Called when the ad is closed.
//        interstitial.OnAdClosed += HandleOnAdClosed;
//        interstitial.OnAdFailedToShow += HandleOnAdFailedToShow;
//        // Called when the ad click caused the user to leave the application.
//        //interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
//        interstitial.OnPaidEvent += HandleAdInterPaidEvent;
//        // Create an empty ad request.
//        AdRequest request = new AdRequest.Builder().Build();
//        // Load the interstitial with the request.
//        interstitial.LoadAd(request);
//        return interstitial;
//    }
//    public bool IsInterAvailable()
//    {
//        if (Config.ACTIVE_TEST)
//        {
//            return true;
//        }
//        if (!initAdInter)
//        {
//            return false;
//        }
//        //#if UNITY_EDITOR
//        //        return true;
//        //#endif
//        bool check = false;
//        //if (!isActiveShowInter)
//        {
//            //if (this.interstitialHight != null && this.interstitialHight.IsLoaded())
//            //{
//            //    check = true;
//            //}
//            //else 
//            if (this.interstitialNormal != null && this.interstitialNormal.IsLoaded())
//            {
//                check = true;
//            }
//        }
//        return check;
//    }
//    public bool ShowInter(Action<bool> pOnCompleted)
//    {
//        if (Config.ACTIVE_TEST)
//        {
//            if (pOnCompleted != null)
//            {
//                pOnCompleted(true);
//            }
//            return true;
//        }
//        if (!initAdInter)
//        {
//            return false;
//        }
//        //#if UNITY_EDITOR
//        //        pOnCompleted(true);
//        //        return true;
//        //#endif
//        bool isShow = false;
//        //if (this.interstitialHight != null && this.interstitialHight.IsLoaded())
//        //{
//        //    this.interstitialHight.Show();
//        //    idInterShow = 2;

//        //    isShow = true;
//        //}
//        //else
//        {
//            if (this.interstitialNormal != null && this.interstitialNormal.IsLoaded())
//            {
//                Config.showInterOnPause = false;
//                this.interstitialNormal.Show();
//                idInterShow = 1;
//                isShow = true;
//            }
//        }

//        if (isShow)
//        {
//            //isActiveShowInter = true;
//            //mOnInterAdCompleted = pOnCompleted;
//            if (pOnCompleted != null)
//            {
//                mOnInterAdCompleted = pOnCompleted;
//                //pOnCompleted(true);
//            }
//        }
//        else
//        {
//            if (pOnCompleted != null)
//            {
//                pOnCompleted(true);
//            }
//            //Debug.Log("admob google - False");
//        }
//        return isShow;
//    }
//    public void HandleOnAdLoaded(object sender, EventArgs args)
//    {
//        //MonoBehaviour.print("HandleAdLoaded Inter event received");
//        interstitialRetryAttempt = 0;
//        isLoadFailInter = false;
//    }

//    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//    {
//        //MonoBehaviour.print("HandleFailedToReceiveAd Inter event received with message: "
//        //                    + args.LoadAdError.GetMessage());
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//            isLoadFailInter = true;
//            interstitialRetryAttempt++;
//        //double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
//        float retryDelay = 15;
//        //if (interstitialRetryAttempt == 1)
//        //{
//        //    retryDelay = 15;
//        //}
//        //else if (interstitialRetryAttempt == 2)
//        //{
//        //    retryDelay = 30;
//        //}
//        AdsController.instance.ReloadInter((float)retryDelay);
//        });
//    }

//    public void HandleOnAdOpened(object sender, EventArgs args)
//    {
//        //MonoBehaviour.print("HandleAdOpened event received");
//        //if (MusicManager.instance != null)
//        //{
//        //    MusicManager.instance.PauseMusic();
//        //}
//    }

//    public void HandleOnAdClosed(object sender, EventArgs args)
//    {
//        //MonoBehaviour.print("HandleAdClosed event received");
//        UnityMainThreadDispatcher.Instance().Enqueue(RefreshInterClose);
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//            AdsController.instance.CloseInterShow();
//        });
//        //MobileAdsEventExecutor.ExecuteInUpdate(() =>
//        //{
//        //    //RefreshInterClose();
//        //    UnityMainThreadDispatcher.Instance().Enqueue(RefreshInterClose);
//        //});
//    }
//    public void HandleOnAdFailedToShow(object sender, AdErrorEventArgs args)
//    {
//        //MonoBehaviour.print("HandleAdClosed event received");
//        UnityMainThreadDispatcher.Instance().Enqueue(RefreshInterClose);
//        //MobileAdsEventExecutor.ExecuteInUpdate(() =>
//        //{
//        //    //RefreshInterClose();
//        //    UnityMainThreadDispatcher.Instance().Enqueue(RefreshInterClose);
//        //});
//    }

//    public void HandleAdInterPaidEvent(object sender, AdValueEventArgs args)
//    {
//        AdValue adValue = args.AdValue;
//        // TODO: Send the paid event information to your preferred analytics server
//        // directly within this callback.

//        //MonoBehaviour.print($"HandleAdInterPaidEvent received with ad value (in micros):{ adValue.Value}, precision: { adValue.Precision}, currency:{ adValue.CurrencyCode} from ad network adapter { this.rewardedAdNormal1.GetResponseInfo().GetMediationAdapterClassName()}");
//        UnityMainThreadDispatcher.Instance().Enqueue(() =>
//        {
//        //Firebase.Analytics.Parameter[] LTVParameters = {
//        //    // Log ad value in micros.
//        //    new Firebase.Analytics.Parameter("value", (adValue.Value/RATE_VALUE)),
//        //    // These values below won’t be used in ROAS recipe.
//        //    // But log for purposes of debugging and future reference.
//        //    new Firebase.Analytics.Parameter("currency",
//        //    adValue.CurrencyCode),
//        //    new Firebase.Analytics.Parameter("precision",
//        //    (int)adValue.Precision),
//        //    new Firebase.Analytics.Parameter("adunitid", adUnitId),
//        //    new Firebase.Analytics.Parameter("network",
//        //    interstitialNormal.GetResponseInfo().GetMediationAdapterClassName())
//        //};
//        //Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", LTVParameters);

//        AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
//            adRevenue.setRevenue(adValue.Value / 1000000f, adValue.CurrencyCode);
//            Adjust.trackAdRevenue(adRevenue);
//        });
//    }
//    //public void HandleAdInterPaidEvent(object sender, AdValueEventArgs args)
//    //{
//    //    UnityMainThreadDispatcher.Instance().Enqueue(() =>
//    //    {
//    //        AdValue adValue = args.AdValue;
//    //        // Log an event with ad value parameters
//    //        Firebase.Analytics.Parameter[] LTVParameters = {
//    //        // Log ad value in micros.
//    //        new Firebase.Analytics.Parameter("value", adValue.Value),
//    //        // These values below won’t be used in ROAS recipe.
//    //        // But log for purposes of debugging and future reference.
//    //        new Firebase.Analytics.Parameter("currency",
//    //        adValue.CurrencyCode),
//    //        new Firebase.Analytics.Parameter("precision",
//    //        (int)adValue.Precision),
//    //        new Firebase.Analytics.Parameter("adunitid", adUnitId),
//    //        new Firebase.Analytics.Parameter("network",
//    //        interstitialNormal.GetResponseInfo().GetMediationAdapterClassName())
//    //        };
//    //        Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_inter",
//    //        LTVParameters);
//    //    });
//    //}

//    //public void HandleOnAdLeavingApplication(object sender, EventArgs args)
//    //{
//    //    //MonoBehaviour.print("HandleAdLeavingApplication event received");
//    //}
//    #endregion

//    #region banner
//    BannerView banner2View;
//    public void Request_Banner2()
//    {
//#if UNITY_EDITOR
//        string adUnitId = "unused";
//#elif UNITY_ANDROID
//                string adUnitId = "ca-app-pub-6286675003708772/6400257439";
//#elif UNITY_IOS
//                string adUnitId = "ca-app-pub-8721698442392956/7792130119";
//#else
//                string adUnitId = "unexpected_platform";
//#endif
//        //adUnitId = "ca-app-pub-3940256099942544/6300978111";//TEST
//        // Clean up interstitial before using it
//        if (banner2View != null)
//        {
//            banner2View.Destroy();
//        }

//        AdSize adSize = new AdSize(320, 50);
//        banner2View = new BannerView(adUnitId, adSize, AdPosition.Bottom);
//        //#if UNITY_IOS
//        // Add Event Handlers
//        //banner2View.OnAdLoaded += HandleOnAdLoaded_BanenrAd;
//        //#endif
//        //banner2View.OnAdFailedToLoad += HandleOnAdFailedToLoad_BanenrAd;
//        banner2View.OnAdOpening += HandleOnAdOpened_BanenrAd;
//        //banner2View.OnPaidEvent += HandleAdBannerPaidEvent;
//        //banner2View.OnAdClosed += HandleOnAdClosed_BanenrAd;
//        //banner2View.OnAdLeavingApplication += HandleOnAdLeavingApplication_BanenrAd;
//        AdRequest request = new AdRequest.Builder().Build();
//        // Load an interstitial ad
//        banner2View.LoadAd(request);

//    }
//    public void HandleOnAdOpened_BanenrAd(object sender, EventArgs args)
//    {
//        Config.showInterOnPause = false;
//    }

//    public void HandleAdBannerPaidEvent(object sender, AdValueEventArgs args)
//    {
//        AdValue adValue = args.AdValue;
//        // TODO: Send the paid event information to your preferred analytics server
//        // directly within this callback.

//        //MonoBehaviour.print($"HandleAdBannerPaidEvent received with ad value (in micros):{ adValue.Value}, precision: { adValue.Precision}, currency:{ adValue.CurrencyCode} from ad network adapter { this.rewardedAdNormal1.GetResponseInfo().GetMediationAdapterClassName()}");

//        //Firebase.Analytics.Parameter[] LTVParameters = {
//        //    // Log ad value in micros.
//        //    new Firebase.Analytics.Parameter("value", adValue.Value),
//        //    // These values below won’t be used in ROAS recipe.
//        //    // But log for purposes of debugging and future reference.
//        //    new Firebase.Analytics.Parameter("currency",
//        //    adValue.CurrencyCode),
//        //    new Firebase.Analytics.Parameter("precision",
//        //    (int)adValue.Precision),
//        //    new Firebase.Analytics.Parameter("adunitid", adUnitId),
//        //    new Firebase.Analytics.Parameter("network",
//        //    banner2View.GetResponseInfo().GetMediationAdapterClassName())
//        //    };
//        //Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", LTVParameters);
//    }
//    //public void HandleAdBannerPaidEvent(object sender, AdValueEventArgs args)
//    //{
//    //    UnityMainThreadDispatcher.Instance().Enqueue(() =>
//    //    {
//    //        AdValue adValue = args.AdValue;
//    //        // Log an event with ad value parameters
//    //        Firebase.Analytics.Parameter[] LTVParameters = {
//    //        // Log ad value in micros.
//    //        new Firebase.Analytics.Parameter("value", adValue.Value),
//    //        // These values below won’t be used in ROAS recipe.
//    //        // But log for purposes of debugging and future reference.
//    //        new Firebase.Analytics.Parameter("currency",
//    //        adValue.CurrencyCode),
//    //        new Firebase.Analytics.Parameter("precision",
//    //        (int)adValue.Precision),
//    //        new Firebase.Analytics.Parameter("adunitid", adUnitId),
//    //        new Firebase.Analytics.Parameter("network",
//    //        banner2View.GetResponseInfo().GetMediationAdapterClassName())
//    //        };
//    //        Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_banner",
//    //        LTVParameters);
//    //    });
//    //}


//    //public void HandleOnAdLoaded_BanenrAd(object sender, EventArgs args)
//    //{
//    //    if (Config.currSelectLevel_Sort <= 1)
//    //    {
//    //        HideBannerAd();
//    //    }
//    //}

//    public void ShowBannerAd()
//    {
//        if (banner2View != null)
//        {
//            banner2View.Show();
//        }
//    }

//    public void HideBannerAd()
//    {
//        if (banner2View != null)
//        {
//            banner2View.Hide();
//        }
//    }
//    #endregion
//    public void OnDestroy()
//    {
//        if (banner2View != null)
//        {
//            banner2View.Destroy();
//        }
//        //if (interstitialHight != null)
//        //{
//        //    interstitialHight.Destroy();
//        //}
//        if (interstitialNormal != null)
//        {
//            interstitialNormal.Destroy();
//        }
//        if (rewardedAdNormal1 != null)
//        {
//            rewardedAdNormal1.Destroy();
//        }
//    }
//}
