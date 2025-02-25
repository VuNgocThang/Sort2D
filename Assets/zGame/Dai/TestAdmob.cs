using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAdmob : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //MobileAds.Initialize(status =>
        //{
        //    AppOpenAdManager.Instance.InitActive();
        //    AppOpenAdManager.Instance.LoadAd();
        //    AppOpenAdManager.Instance.LoadRewardPre();
        //});
    }
    public void ActiveDebugAdmob(){
        MobileAds.OpenAdInspector(error => {
            // Error will be set if there was an issue and the inspector was not displayed.
            Debug.Log("ActiveDebugAdmob :" + error.ToString());
        });
    }
}
