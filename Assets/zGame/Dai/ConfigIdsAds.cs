using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigIdsAds
{
    public const bool TEST_TYPE_USER = false;//false;
    //------------------
    public static bool TEST_GOOOGLE_ADS = false;//false
    public const int TYPE_MEDIATION_AD = Config.MEDIATION_MAX;//1-Max mediation ; 2- Ironsource Mediation
    public const int TYPE_PUB_G = Config.PUB_G_HOPEE;//1-RK ; 2-AB
#if UNITY_ANDROID
    //---AOA----
    public const string ID_TIER_1 = "ca-app-pub-9819920607806935/8444793771";// /112517806,23154201080/7421716960610
    public const string ID_TIER_2 = "ca-app-pub-9819920607806935/8444793771";//ca-app-pub-2227789348341993/2845941916
    public const string ID_TIER_3 = "ca-app-pub-9819920607806935/8444793771";
    //public const string ID_TIER_3 = "ca-app-pub-9819920607806935/4868981397";
    //---BANNER COLLAP----
    public static string _adUnitId = "";
    //----Native Ad google-----
    public static string _adNativeId = "";//ca-app-pub-3940256099942544/2247696110
    //------backfill-----
    public static string _adInterID = "ca-app-pub-9819920607806935/7131712108";//  /112517806,23154201080/3561716896169
    //public static string _adInterID = "ca-app-pub-9819920607806935/1199845101";//ca-app-pub-3940256099942544/1033173712
    //public static string _adInterID = "/21622890900,22442147457/anymind_vn_famousfashion_android_all_interstitial_l5_231207";//ca-app-pub-3940256099942544/1033173712
    //public static string _adInterID = "/104502601/interstitial.pubtestandroid";

    //public static string _adRewardID = "ca-app-pub-9819920607806935/6404850577";//ca-app-pub-3940256099942544/5354046379
    public static string _adRewardID = "ca-app-pub-9819920607806935/5778267831";//  /112517806,23154201080/6231716901618

    //------MAX mediation----
    public static MaxSdkBase.BannerPosition bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;
    public static MaxSdkBase.AdViewPosition adViewPositionMrec = MaxSdkBase.AdViewPosition.TopCenter;

    public const string MaxSdkKey = "ZoNyqu_piUmpl33-qkoIfRp6MTZGW9M5xk1mb1ZIWK6FN9EBu0TXSHeprC3LMPQI7S3kTc1-x7DJGSV8S-gvFJ";
    public const string InterstitialAdUnitId = "48f2dc17dea08fd0";//80e64f3491ce8c50
    public const string RewardedAdUnitId = "42b5076f696d468e";//3c1232b62dd79262
    public const string BannerAdUnitId = "d1bcc4a550f70821";//6553c1e7e081c41e
    public const string MRecAdUnitId = "";//7c7c851694553107

    public const string MAX_AOA_AdUnitId = "";

    //-------Ironsource mediation-------

    //public const string SdkKey = "1e777595d";

    //public static IronSourceBannerPosition bannerPosition = IronSourceBannerPosition.TOP;

    //----APS-----
    public const string ApsAmazonAppId = "";
    public const string ApsBannerIdTablet = "";
    public const string ApsBannerIdPhone = "";
    public const string ApsMrecId = "";
    public const string ApsInterId = "";
    public const string ApsRewardVideoId = "";

#elif UNITY_IOS
    //---AOA----
    public const string ID_TIER_1 = "TIER_1_HERE";
    public const string ID_TIER_2 = "TIER_2_HERE";
    public const string ID_TIER_3 = "TIER_3_HERE";
    //---BANNER COLLAP----
    public static string _adUnitId = "";
    //------backfill-----
    public static string _adInterID = "";
    public static string _adRewardID = "";
    //------MAX mediation----
    public static MaxSdkBase.BannerPosition bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;
    public static MaxSdkBase.AdViewPosition adViewPositionMrec = MaxSdkBase.AdViewPosition.TopCenter;

    public const string MaxSdkKey = "7PspscCcbGd6ohttmPcZTwGmZCihCW-Jwr7nSJN2a_9Mg0ERPs0tmGdKTK1gs__nr6XHQvK0vTNaTb1uR1mCIN";
    public const string InterstitialAdUnitId = "14aae71a5f1e483e";
    public const string RewardedAdUnitId = "d1e5eee8c0613deb";
    public const string BannerAdUnitId = "7f22d822f842ed1c";
    public const string MRecAdUnitId = "";
    //----APS-----
    public const string ApsAmazonAppId = "";
    public const string ApsBannerIdTablet = "";
    public const string ApsBannerIdPhone = "";
    public const string ApsMrecId = "";
    public const string ApsInterId = "";
    public const string ApsRewardVideoId = "";
#else
    //---AOA----
    public const string ID_TIER_1 = "TIER_1_HERE";
    public const string ID_TIER_2 = "TIER_2_HERE";
    public const string ID_TIER_3 = "TIER_3_HERE";
    //---BANNER COLLAP----
    public static string _adUnitId = "";
    //------backfill-----
    public static string _adInterID = "";
    public static string _adRewardID = "";
    //------MAX mediation----
    public static MaxSdkBase.BannerPosition bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;
    public static MaxSdkBase.AdViewPosition adViewPositionMrec = MaxSdkBase.AdViewPosition.TopCenter;

    public const string MaxSdkKey = "7PspscCcbGd6ohttmPcZTwGmZCihCW-Jwr7nSJN2a_9Mg0ERPs0tmGdKTK1gs__nr6XHQvK0vTNaTb1uR1mCIN";
    public const string InterstitialAdUnitId = "14aae71a5f1e483e";
    public const string RewardedAdUnitId = "d1e5eee8c0613deb";
    public const string BannerAdUnitId = "7f22d822f842ed1c";
    public const string MRecAdUnitId = "";
    //----APS-----
    public const string ApsAmazonAppId = "";
    public const string ApsBannerIdTablet = "";
    public const string ApsBannerIdPhone = "";
    public const string ApsMrecId = "";
    public const string ApsInterId = "";
    public const string ApsRewardVideoId = "";
#endif


    //-----Custom firebase remote config----

    const string KEY_REMOTE_COUNT_CAKE_INTER = "cf_count_cake_inter";
    //------------------
    //public static int CountCakeInter = 5;
    public static void FetchFirebaseDone()
    {
#if ACTIVE_FIREBASE_ANALYTIC
        // GameManager.pointToShowInter = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_REMOTE_COUNT_CAKE_INTER).LongValue;
#endif
    }
}
