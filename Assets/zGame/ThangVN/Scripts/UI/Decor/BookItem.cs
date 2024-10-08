using ntDev;
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
    [SerializeField] EasyButton btnSelect;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] GameObject nText;

    private void Awake()
    {
        btnSelect.OnClick(() =>
        {
            if (indexBook <= SaveGame.MaxCurrentBook)
            {
                ShowBook(indexBook);
            }
        });
    }

    void ShowBook(int index)
    {
        SaveGame.CurrentBook = indexBook;
        ManagerPopup.HidePopup<PopupDecor>();
        PopupBookItem.Show(index);
    }

    public void Init(int _index, string title)
    {
        indexBook = _index;
        txtTitleBook.text = title;
        if (indexBook <= SaveGame.MaxCurrentBook)
        {
            nText.SetActive(true);
        }
        if (indexBook <= SaveGame.MaxCurrentBook)
        {
            imgIconBook.sprite = defaultSprite;
        }
    }

    public void InitProgressText(float percent)
    {
        txtProgress.text = $"{percent * 100}%";
    }
}
