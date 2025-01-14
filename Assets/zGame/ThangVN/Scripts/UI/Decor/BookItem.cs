using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookItem : MonoBehaviour
{
    [SerializeField] EasyButton btnSelect;
    [SerializeField] int indexBook;
    [SerializeField] int total;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] TextMeshProUGUI txtTitleBook, txtProgress;
    [SerializeField] Image imgBookLock, bgBook, imgFilter;
    [SerializeField] GameObject nText;
    [SerializeField] DataConfigDecor dataConfigDecor;

    public List<Slot> slots;
    public List<Sprite> sprites;
    [SerializeField] Slot slotPrefab;
    [SerializeField] Transform nParentSlot;
    DataBook dataBook;
    [SerializeField] BookDecorated bookDecorated;


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
        PopupDecorateBook.Show(index, false);
    }

    public void Init(int _index, string title, Sprite sprite, DataConfigDecor _data)
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
            imgBookLock.gameObject.SetActive(false);
            bgBook.enabled = true;
            imgFilter?.gameObject.SetActive(true);

            LoadDataBook();
            ChangeColor();
            SpawnExistedItemInBook();
        }

        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            if (dataConfigDecor.listDataBooks[i].idBook == indexBook)
            {
                total = dataConfigDecor.listDataBooks[i].totalParts;
            }
        }
    }
    void ResizeImage(Image img)
    {
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(img.GetComponent<RectTransform>().sizeDelta.x * GameConfig.SCALE_X, img.GetComponent<RectTransform>().sizeDelta.y * GameConfig.SCALE_Y);
    }

    void ResizeRectTransform(RectTransform rect, Vector2 pos)
    {
        Vector2 relativePos = pos;
        float newX = relativePos.x * GameConfig.SCALE_X;
        float newY = relativePos.y * GameConfig.SCALE_Y;
        rect.anchoredPosition = new Vector2(newX, newY);
    }

    void LoadDataBook()
    {
        for (int i = 0; i < SaveGame.ListBookDecorated.listBookDecorated.Count; i++)
        {
            if (indexBook == SaveGame.ListBookDecorated.listBookDecorated[i].idBookDecorated)
            {
                bookDecorated = SaveGame.ListBookDecorated.listBookDecorated[i];
            }
        }
    }

    void ChangeColor()
    {
        bgBook.color = bookDecorated.colorPainted;
    }
    public void SpawnExistedItemInBook()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        slots.Clear();
        sprites.Clear();


        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            if (dataConfigDecor.listDataBooks[i].idBook == indexBook)
            {
                dataBook = dataConfigDecor.listDataBooks[i];
            }
        }

        for (int i = 0; i < dataBook.listDataSlots.Count; i++)
        {
            Slot slot = Instantiate(slotPrefab, nParentSlot);
            slot.id = dataBook.listDataSlots[i].idSlot;

            //Vector2 relativePos = dataBook.listDataSlots[i].pos;
            //float newX = relativePos.x * GameConfig.SCALE_X;
            //float newY = relativePos.y * GameConfig.SCALE_Y;
            //slot.rectTransform.anchoredPosition = new Vector2(newX, newY);

            ResizeRectTransform(slot.rectTransform, dataBook.listDataSlots[i].pos);

            slot.imgLine.gameObject.SetActive(false);
            slots.Add(slot);
        }

        for (int i = 0; i < dataBook.listDataItemDecor.Count; i++)
        {
            sprites.Add(dataBook.listDataItemDecor[i].sprite);
        }


        for (int i = 0; i < bookDecorated.listItemDecorated.Count; i++)
        {
            if (!bookDecorated.listItemDecorated[i].isPainted) continue;

            int idIndex = bookDecorated.listItemDecorated[i].idItemDecorated;
            for (int j = 0; j < slots.Count; j++)
            {
                if (slots[j].id == idIndex)
                {
                    ItemDraggable item = ItemDraggablePool.Instance.GetPooledObject();
                    item.imgItemDrag.sprite = sprites[idIndex];
                    item.imgItemDrag.SetNativeSize();
                    ResizeImage(item.imgItemDrag);
                    item.SetInParent(null, slots[j]);

                    //Vector2 relativePos = new Vector2(bookDecorated.listItemDecorated[i].x, bookDecorated.listItemDecorated[i].y);
                    //float newX = relativePos.x * GameConfig.SCALE_X;
                    //float newY = relativePos.y * GameConfig.SCALE_Y;
                    //item.rectTransform.anchoredPosition = new Vector2(newX, newY);

                    ResizeRectTransform(item.rectTransform, new Vector2(bookDecorated.listItemDecorated[i].x, bookDecorated.listItemDecorated[i].y));

                    //item.rectTransform.anchoredPosition = new Vector2(bookDecorated.listItemDecorated[i].x, bookDecorated.listItemDecorated[i].y);
                    item.transform.localScale = Vector3.one;
                    item.gameObject.SetActive(true);
                }
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

                if (dataCache.listBookDecorated[i].colorPainted != GameConfig.DEFAULT_COLOR) countProgress++;
            }
        }


        if (countProgress == total) IsReachMax = true;

        return IsReachMax;
    }
}

