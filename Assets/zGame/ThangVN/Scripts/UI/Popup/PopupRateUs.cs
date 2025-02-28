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

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_RATE_US, ShowRate);
        btnSubmit.OnClick(() =>
        {
            SaveGame.Submitted = true;
            SaveGame.CanRateUs = false;
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
}