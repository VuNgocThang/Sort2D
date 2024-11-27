using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtTitleBook, txtProgress;
    [SerializeField] Image imgIconBook;
    [SerializeField] int indexBook;
    [SerializeField] int total;
    [SerializeField] EasyButton btnSelect;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] GameObject nText;
    [SerializeField] DataConfigDecor dataConfigDecor;

    private void Awake()
    {
        btnSelect.OnClick(() =>
        {
            if (indexBook <= SaveGame.MaxCurrentBook)
            {
                if (ReachMax())
                    ShowBook(indexBook);
                else
                    ShowDecortateBook(indexBook);
            }
        });
    }

    void ShowBook(int index)
    {
        SaveGame.CurrentBook = indexBook;
        ManagerPopup.HidePopup<PopupDecor>();
        PopupBookItem.Show(index);
    }

    void ShowDecortateBook(int index)
    {
        SaveGame.CurrentBook = indexBook;
        ManagerPopup.HidePopup<PopupDecor>();
        PopupDecorateBook.Show(index);
    }

    public void Init(int _index, string title, DataConfigDecor _data)
    {
        indexBook = _index;
        txtTitleBook.text = title;
        dataConfigDecor = _data;

        if (indexBook <= SaveGame.MaxCurrentBook)
        {
            nText.SetActive(true);
        }
        if (indexBook <= SaveGame.MaxCurrentBook)
        {
            imgIconBook.sprite = defaultSprite;
        }

        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            if (dataConfigDecor.listDataBooks[i].idBook == indexBook)
            {
                total = dataConfigDecor.listDataBooks[i].totalParts;
            }
        }
    }

    public void InitProgressText(float percent)
    {
        txtProgress.text = $"{percent * 100}%";
    }

    public bool ReachMax()
    {
        bool IsReachMax = false;
        int countProgress = 0;
        ListBookDecorated dataCache = SaveGame.ListBookDecorated;
        for (int i = 0; i < dataCache.listBookDecorated.Count; i++)
        {
            if (dataCache.listBookDecorated[i].idBookDecorated == indexBook)
            {
                for (int j = 0; j < dataCache.listBookDecorated[i].listItemDecorated.Count; j++)
                {
                    if (dataCache.listBookDecorated[i].listItemDecorated[j].isPainted) countProgress++;
                }
            }
        }

        if (countProgress == total - 1) IsReachMax = true;

        return IsReachMax;
    }
}
