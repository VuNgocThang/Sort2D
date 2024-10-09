using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupDecor : Popup
{
    [SerializeField] EasyButton btnBack, btnPlusColorPlate;
    [SerializeField] RectTransform select;
    [SerializeField] List<GameObject> listSelect;
    [SerializeField] TextMeshProUGUI txtColorPlate;
    [SerializeField] BookItem bookItemPrefab;
    [SerializeField] List<BookItem> listBookItems;
    [SerializeField] Transform nContent;
    [SerializeField] List<BookDecorated> listBookDecorated;
    [SerializeField] DataConfigDecor dataBookConfig;
    [SerializeField] List<float> listProgress;
    [SerializeField] ScrollRect scrollRect;


    private void Awake()
    {
        btnBack.OnClick(() => BackHome());

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
        //ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = false;
        txtColorPlate.text = SaveGame.Pigment.ToString();

        //for (int i = 0; i < listSelect.Count; i++)
        //{
        //    listSelect[i].SetActive(false);
        //}
        //if (LogicSetupRoom.instance.listGameObject.Count > SaveGame.CurrentObject)
        //{
        //    listSelect[SaveGame.CurrentObject].SetActive(true);
        //    LogicSetupRoom.instance.listGameObject[SaveGame.CurrentObject].SetActive(true);
        //    SaveGame.CanShow = true;
        //}
        for (int i = 0; i < listBookItems.Count; i++)
        {
            listBookItems[i].gameObject.SetActive(false);
        }

        listBookItems.Clear();
        listProgress.Clear();

        LoadDataBook();


        for (int i = 0; i < dataBookConfig.listDataBooks.Count; i++)
        {
            BookItem book = Instantiate(bookItemPrefab, nContent);
            book.Init(i, dataBookConfig.listDataBooks[i].titleBook);
            listBookItems.Add(book);
        }

        for (int i = 0; i < listProgress.Count; i++)
        {
            listBookItems[i].InitProgressText(listProgress[i]);
        }

        //scrollRect.verticalNormalizedPosition = 1f;

    }

    void LoadDataBook()
    {
        listBookDecorated = SaveGame.ListBookDecorated.listBookDecorated;

        for (int i = 0; i < listBookDecorated.Count; i++)
        {
            listProgress.Add(listBookDecorated[i].progress);
        }
    }


    public override void Hide()
    {
        //ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = true;
        base.Hide();
    }

    public void BackHome()
    {
        Debug.Log("BackHome");
        HomeUI.Instance.animator.Play("Show");
        HomeUI.Instance.DisableObject();
        //ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = true;

        base.Hide();
    }


}
