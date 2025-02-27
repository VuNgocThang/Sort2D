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
    [SerializeField] RectTransform imgTut, hand;
    [SerializeField] ScrollRect scroll;

    [SerializeField] DataConfigDecor dataBookConfig;
    private const float paddingTop = 50f;
    private const float cellSizeY = 469f;
    private const float spacingY = 50f;

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
        //base.Init();
        transform.localScale = Vector3.one;
        scroll.enabled = true;

        SaveGame.Redecorated = false;
        ManagerPopup.HidePopup<PopupDecorateBook>();

        txtColorPlate.text = SaveGame.Pigment.ToString();

        for (int i = 0; i < listBookItems.Count; i++)
            listBookItems[i].gameObject.SetActive(false);

        listBookItems.Clear();
        listProgress.Clear();

        LoadListProgress();

        LoadListBookItems();

        MoveToCurrentBook();


        if (!SaveGame.IsDoneTutorialDecor)
            StartCoroutine(InitTutorialDecor());
    }

    private void LoadListBookItems()
    {
        for (int i = 0; i < dataBookConfig.listDataBooks.Count; i++)
        {
            BookItem book = Instantiate(bookItemPrefab, nContent);
            book.Init(i, dataBookConfig.listDataBooks[i].titleBook, dataBookConfig.listDataBooks[i].sprite,
                dataBookConfig);
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

    IEnumerator InitTutorialDecor()
    {
        scroll.enabled = false;
        yield return new WaitForEndOfFrame();
        TutorialDecor.Instance.InitTutFocus(listBookItems[0].GetComponent<RectTransform>());
        TutorialDecor.Instance.ShowStep(0);
        //imgTut.position = scroll.content.GetChild(0).GetComponent<RectTransform>().position;
        //hand.position = imgTut.position;
    }

    void MoveToCurrentBook()
    {
        float posMove = 0f;
        posMove = paddingTop + Mathf.Floor(SaveGame.CurrentBook / 2) * cellSizeY +
                  Mathf.Floor(SaveGame.CurrentBook / 2) * spacingY;
        RectTransform contentRect = scroll.content.GetComponent<RectTransform>();
        // Debug.Log("SaveGame.CurrentBook: " + SaveGame.CurrentBook + " __  " + Mathf.Floor(SaveGame.CurrentBook / 2));
        // Debug.Log("posMove : " + posMove);
        contentRect.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(contentRect.anchoredPosition.x, posMove);
    }


    public override void Hide()
    {
        base.Hide();
    }

    public void BackHome()
    {
        Debug.Log("BackHome");
        if (SaveGame.Level >= GameConfig.LEVEL_INTER)
        {
            string pWhere = "Decor To Home";
            PopupAdsBreak.Show(pWhere);
            // AdsController.instance.ShowInterAd(null, "Decor to Home");
        }

        HomeUI.Instance.animator.Play("Show");
        //HomeUI.Instance.DisableObject();

        base.Hide();
    }
}