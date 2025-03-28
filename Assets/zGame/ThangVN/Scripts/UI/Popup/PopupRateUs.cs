#if UNITY_ANDROID
using Google.Play.Review;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;

public class PopupRateUs : Popup
{
    [SerializeField] EasyButton btnSubmit, btnClosePopup;
    [SerializeField] private List<GameObject> listStars;
    [SerializeField] private int rate;
    bool isShowRate = false;

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_RATE_US, ShowRate);
        btnSubmit.OnClick(() =>
        {
            SaveGame.Submitted = true;
            SaveGame.CanRateUs = false;

            if (rate < 3) NormalRate();
            else ShowRateGoogle();
            Debug.Log("Rate: " + rate);
        });
        btnClosePopup.OnClick(LaterRateUs);
    }

    public static async void Show()
    {
        PopupRateUs pop = await ManagerPopup.ShowPopup<PopupRateUs>();
        pop.Init();
    }

    void ShowRate(object e)
    {
        int stars = (int)e;
        rate = stars;
        foreach (var star in listStars)
        {
            star.SetActive(false);
        }

        for (int i = 0; i < stars; i++)
        {
            listStars[i].SetActive(true);
        }

        btnSubmit.gameObject.SetActive(true);
    }

    public override void Init()
    {
        // base.Init();
        transform.localScale = Vector3.one;
        foreach (var star in listStars)
        {
            star.SetActive(false);
        }
    }

    private void LaterRateUs()
    {
        SaveGame.CanRateUs = false;
        Hide();
    }

    public override void Hide()
    {
        base.Hide();
    }

    void NormalRate()
    {
        Config.SetRate();
        Config.ActiveShowPopupRate();
        Hide();
    }

    void StoreRate()
    {
        Config.SetRate();
        Config.ActiveShowPopupRate();
        //mainContent.SetActive(false);
        //textThanks.SetActive(true);
        //btnClose.gameObject.SetActive(false);
        //btnCloseMini.gameObject.SetActive(true);
#if UNITY_ANDROID
        //Application.OpenURL("market://details?id=" + Application.identifier);
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    

#elif UNITY_IOS
            Device.RequestStoreReview();
#endif
    }

    public void ShowRateGoogle()
    {
        if (!isShowRate)
        {
            isShowRate = true;
            StartCoroutine(ShowRateNative());
        }
    }

    // Create instance of ReviewManager
#if UNITY_ANDROID
    private ReviewManager _reviewManager;
    //...
    IEnumerator ShowRateNative()
    {
        Config.showInterOnPause = false;
        PlayerPrefs.SetInt("rate", 1);
        PlayerPrefs.Save();
        _reviewManager = new ReviewManager();
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            StoreRate();
            yield break;
        }
        var _playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            StoreRate();
            yield break;
        }
        Config.SetRate();
        Config.ActiveShowPopupRate();
        //mainContent.SetActive(false);
        //textThanks.SetActive(true);
        //btnClose.gameObject.SetActive(false);
        //btnCloseMini.gameObject.SetActive(true);
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
        isShowRate = false;
        Hide();
    }
#elif UNITY_IOS
    //...
    IEnumerator ShowRateNative()
    {
        Config.showInterOnPause = false;
        ShowRateLink();
        yield return null;
        Config.SetRate();
        ratePopup.HidePopup();
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
        isShowRate = false;
    }
#endif
}