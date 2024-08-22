using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopupFreeCoin : Popup
{
    public FreeCoinData freeCoinData;
    public ItemFreeCoin itemFreeCoin;
    public Transform nContent;
    public List<ItemFreeCoin> listItem;
    public EasyButton btnClaime50;
    [SerializeField] GameObject imgActive, imgDeactive;
    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_FREECOIN, UpdateListContent);
        btnClaime50.OnClick(() =>
        {
            if (!SaveGame.DataFreeCoin.isClaimed50)
            {
                Claimed50();
            }
        });
    }

    public static async void Show()
    {
        PopupFreeCoin pop = await ManagerPopup.ShowPopup<PopupFreeCoin>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        listItem.Clear();
        RefreshData();

        InitClaim50();
        for (int i = 0; i < freeCoinData.listDataFreeCoin.Count; i++)
        {
            ItemFreeCoin item = Instantiate(itemFreeCoin, nContent);
            listItem.Add(item);
            item.isClaimed = freeCoinData.listDataFreeCoin[i].isClaimed;
            item.countCoin = freeCoinData.listDataFreeCoin[i].countCoin;
            item.index = freeCoinData.listDataFreeCoin[i].index;
            item.Show(SaveGame.DataFreeCoin.currentIndex);
        }
    }


    public override void Hide()
    {
        transform.localScale = Vector3.one;

        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            for (int i = 0; i < listItem.Count; i++)
            {
                listItem[i].gameObject.SetActive(false);
            }

            gameObject.SetActive(false);
            ManagerEvent.RaiseEvent(EventCMD.EVENT_POPUP_CLOSE, this);
        });
    }

    public void UpdateListContent(object e)
    {
        for (int i = 0; i < listItem.Count; i++)
        {
            if (listItem[i].index == SaveGame.DataFreeCoin.currentIndex)
            {
                listItem[i].imgActive.SetActive(true);
                listItem[i].imgInActive.SetActive(false);
            }
        }
    }

    public void Claimed50()
    {
        SaveGame.Coin += 50;
        SaveGame.DataFreeCoin.isClaimed50 = true;
        SaveGame.DataFreeCoin = SaveGame.DataFreeCoin;

        InitClaim50();
    }

    public void InitClaim50()
    {
        if (SaveGame.DataFreeCoin.isClaimed50)
        {
            imgActive.SetActive(false);
            imgDeactive.SetActive(true);
        }
        else
        {
            imgActive.SetActive(true);
            imgDeactive.SetActive(false);
        }
    }

    void RefreshData()
    {
        if (SaveGame.NewDay != DateTime.Now.DayOfYear)
        {
            SaveGame.NewDay = DateTime.Now.DayOfYear;

            SaveGame.DataFreeCoin.listDataFreeCoin.Clear();
            SaveGame.DataFreeCoin.currentIndex = 0;
            SaveGame.DataFreeCoin.isClaimed50 = false;

            SaveGame.DataFreeCoin = SaveGame.DataFreeCoin;
            ManagerEvent.RaiseEvent(EventCMD.EVENT_FREECOIN);
        }
    }

}
