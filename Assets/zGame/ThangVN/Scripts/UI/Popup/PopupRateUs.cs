using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;

public class PopupRateUs : Popup
{
    [SerializeField] EasyButton btnSubmit;
    [SerializeField] private List<GameObject> listStars;
    [SerializeField] private int rate;

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_RATE_US, ShowRate);
        btnSubmit.OnClick(() => { Debug.Log("Rate: " + rate); });
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
        base.Init();
        foreach (var star in listStars)
        {
            star.SetActive(false);
        }
    }

    public override void Hide()
    {
        base.Hide();
    }
}