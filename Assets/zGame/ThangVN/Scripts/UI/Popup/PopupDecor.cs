using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupDecor : Popup
{
    [SerializeField] Transform nContent;
    [SerializeField] RectTransform select;
    [SerializeField] EasyButton btnBack, btnPlusColorPlate;
    [SerializeField] TextMeshProUGUI txtColorPlate;
    [SerializeField] BookItem bookItemPrefab;
    [SerializeField] List<GameObject> listSelect;
    [SerializeField] List<BookItem> listBookItems;
    [SerializeField] List<BookDecorated> listBookDecorated;
    [SerializeField] List<float> listProgress;
    [SerializeField] RectTransform hand;
    [SerializeField] ScrollRect scroll;

    [SerializeField] DataConfigDecor dataBookConfig;

    private void Awake()
    {
        btnBack.OnClick(() =>
        {
            if (!SaveGame.IsDoneTutorialDecor) return;

            BackHome();
        });

        btnPlusColorPlate.OnClick(() => PopupGoToLevel.Show());
    }

    public static async void Show()
    {
        PopupDecor pop = await ManagerPopup.ShowPopup<PopupDecor>();
        HomeUI.Instance.animator.Play("Hide");
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        SaveGame.Redecorated = false;
        ManagerPopup.HidePopup<PopupDecorateBook>();

        txtColorPlate.text = SaveGame.Pigment.ToString();

        for (int i = 0; i < listBookItems.Count; i++)
            listBookItems[i].gameObject.SetActive(false);

        listBookItems.Clear();
        listProgress.Clear();

        LoadListProgress();

        LoadListBookItems();

        InitTutorialDecor();
    }

    private void LoadListBookItems()
    {
        for (int i = 0; i < dataBookConfig.listDataBooks.Count; i++)
        {
            BookItem book = Instantiate(bookItemPrefab, nContent);
            book.Init(i, dataBookConfig.listDataBooks[i].titleBook, dataBookConfig.listDataBooks[i].sprite, dataBookConfig);
            listBookItems.Add(book);
        }

        for (int i = 0; i < listProgress.Count; i++)
        {
            listBookItems[i].InitProgressText(listProgress[i]);
        }

        BookItem bookComingSoon = Instantiate(bookItemPrefab, nContent);
        bookComingSoon.InitComingSoonBook(dataBookConfig.listDataBooks.Count, dataBookConfig);
        listBookItems.Add(bookComingSoon);
    }

    void LoadListProgress()
    {
        listBookDecorated = SaveGame.ListBookDecorated.listBookDecorated;

        for (int i = 0; i < listBookDecorated.Count; i++)
        {
            listProgress.Add(listBookDecorated[i].progress);
        }
    }

    void InitTutorialDecor()
    {
        Debug.Log(scroll.content.GetChild(0).GetComponent<RectTransform>().position);
        Debug.Log(scroll.content.GetChild(0).GetComponent<RectTransform>().localPosition);
        if (!SaveGame.IsDoneTutorialDecor)
        {
            //hand.position = listBookItems[0].GetComponent<RectTransform>().position;
            hand.gameObject.SetActive(true);
        }
    }


    public override void Hide()
    {
        base.Hide();
    }

    public void BackHome()
    {
        Debug.Log("BackHome");
        HomeUI.Instance.animator.Play("Show");
        //HomeUI.Instance.DisableObject();

        base.Hide();
    }


}
