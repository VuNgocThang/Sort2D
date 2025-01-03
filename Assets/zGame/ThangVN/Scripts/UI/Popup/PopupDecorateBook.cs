﻿using ntDev;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;

public class PopupDecorateBook : Popup
{
    public RectTransform nBookCover;
    [SerializeField] int idBookDecorated;
    [SerializeField] Image nColorChangeBook, nColorChangeBg1, nColorChangeBg2;
    [SerializeField] EasyButton btnSelectItem, btnSelectBgColor, btnPrev, btnNext, btnBack;
    [SerializeField] TextMeshProUGUI txtNameBook;
    [SerializeField] GameObject bgScrollViewItem, bgSelectColor, imgChooseItem, imgNotChooseItem, imgChooseBg, imgNotChooseBg, bgNewBook;
    [SerializeField] ItemDraggable currentItemDrag;
    [SerializeField] Transform nParent, nParentSlot, nContent;
    public List<ImageItem> listItems;
    public List<Slot> slots;
    public List<Sprite> sprites;
    public List<ItemDecor> listItemDecors;

    [SerializeField] ItemDraggable itemDragPrefab;
    [SerializeField] Slot slotPrefab;
    [SerializeField] ItemDecor itemDecorPrefab;

    public DataConfigDecor dataConfigDecor;
    public float total;

    public DataBook dataBook;
    [SerializeField] BookDecorated bookDecorated;

    // Select Color
    [SerializeField] EasyButton btnTick;
    [SerializeField] List<ItemSelectColor> listItemSelectColor;

    private void Awake()
    {
        btnSelectItem.OnClick(() =>
        {
            OnSelect(true);
            btnTick.gameObject.SetActive(false);
        });
        btnSelectBgColor.OnClick(() => OnSelect(false));
        btnBack.OnClick(() =>
        {
            if (currentItemDrag != null)
            {
                currentItemDrag.linkedSlot.imgLine.gameObject.SetActive(false);
                currentItemDrag.SetActive(false);
                currentItemDrag = null;
            }

            base.Hide();
            //PopupBookItem.Show(SaveGame.CurrentBook);
            PopupDecor.Show();
        });

        btnTick.OnClick(() =>
        {
            SaveCurrentColor();
        });

        ManagerEvent.RegEvent(EventCMD.EVENT_CHANGE_COLOR, ChangeColorBook);
    }

    public static async void Show(int index, bool IsRedecorated)
    {
        PopupDecorateBook pop = await ManagerPopup.ShowPopup<PopupDecorateBook>();
        pop.Init();
        pop.Initialize(index, IsRedecorated);
        //pop.RedecoratedInit(index);
    }

    public override void Init()
    {
        base.Init();
        OnSelect(true);
        ManagerPopup.HidePopup<PopupBookItem>();
    }

    void LoadDataBook()
    {
        idBookDecorated = SaveGame.CurrentBook;
        for (int i = 0; i < SaveGame.ListBookDecorated.listBookDecorated.Count; i++)
        {
            if (idBookDecorated == SaveGame.ListBookDecorated.listBookDecorated[i].idBookDecorated)
            {
                bookDecorated = SaveGame.ListBookDecorated.listBookDecorated[i];
            }
        }
    }

    //public void RedecoratedInit(int index)
    //{
    //    LoadDataBook();

    //    listItems.Clear();
    //    for (int i = 0; i < slots.Count; i++)
    //    {
    //        slots[i].gameObject.SetActive(false);
    //    }
    //    slots.Clear();
    //    sprites.Clear();

    //    for (int i = 0; i < listItemDecors.Count; i++)
    //    {
    //        listItemDecors[i].gameObject.SetActive(false);
    //    }
    //    listItemDecors.Clear();

    //    for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
    //    {
    //        if (dataConfigDecor.listDataBooks[i].idBook == index /*0*/)
    //        {
    //            dataBook = dataConfigDecor.listDataBooks[i];
    //            total = dataConfigDecor.listDataBooks[i].totalParts;
    //        }
    //    }

    //    InitColor();


    //    for (int i = 0; i < dataBook.listDataSlots.Count; i++)
    //    {
    //        int id = dataBook.listDataSlots[i].idSlot;
    //        Vector3 pos = dataBook.listDataSlots[i].pos;
    //        Sprite sprite = dataBook.listDataSlots[i].spriteLine;

    //        Slot slot = Instantiate(slotPrefab, nParentSlot);
    //        slot.Init(id, pos, sprite);
    //        slots.Add(slot);
    //    }

    //    for (int i = 0; i < dataBook.listDataItemDecor.Count; i++)
    //    {
    //        int id = dataBook.listDataItemDecor[i].idItemDecor;
    //        int cost = dataBook.listDataItemDecor[i].cost;
    //        Sprite sprite = dataBook.listDataItemDecor[i].spriteIcon;

    //        ItemDecor item = Instantiate(itemDecorPrefab, nContent);
    //        item.Init(id, cost, sprite);

    //        listItems.Add(item.imageItem);
    //        listItemDecors.Add(item);
    //        sprites.Add(dataBook.listDataItemDecor[i].sprite);
    //    }

    //    for (int i = 0; i < listItemDecors.Count; i++)
    //    {
    //        for (int j = 0; j < bookDecorated.listItemDecorated.Count; j++)
    //        {
    //            if (bookDecorated.listItemDecorated[j].idItemDecorated == listItemDecors[i].id)
    //            {
    //                if (!bookDecorated.listItemDecorated[j].isBought) continue;

    //                listItems[i].isBought = true;
    //                listItemDecors[i].btnBuy.gameObject.SetActive(false);
    //            }
    //        }
    //    }
    //}

    public void Initialize(int index, bool IsRedecorated)
    {
        LoadDataBook();

        Debug.Log("Show PopupDecorateBook at index: " + index);
        listItems.Clear();
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        slots.Clear();
        sprites.Clear();

        for (int i = 0; i < listItemDecors.Count; i++)
        {
            listItemDecors[i].gameObject.SetActive(false);
        }
        listItemDecors.Clear();

        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            if (dataConfigDecor.listDataBooks[i].idBook == index /*0*/)
            {
                dataBook = dataConfigDecor.listDataBooks[i];
                total = dataConfigDecor.listDataBooks[i].totalParts;
            }
        }

        InitColor();

        for (int i = 0; i < dataBook.listDataSlots.Count; i++)
        {
            int id = dataBook.listDataSlots[i].idSlot;
            Vector3 pos = dataBook.listDataSlots[i].pos;
            Sprite sprite = dataBook.listDataSlots[i].spriteLine;

            Slot slot = Instantiate(slotPrefab, nParentSlot);
            slot.Init(id, pos, sprite);
            slots.Add(slot);
        }

        for (int i = 0; i < dataBook.listDataItemDecor.Count; i++)
        {
            int id = dataBook.listDataItemDecor[i].idItemDecor;
            int cost = dataBook.listDataItemDecor[i].cost;
            Sprite sprite = dataBook.listDataItemDecor[i].spriteIcon;

            ItemDecor item = Instantiate(itemDecorPrefab, nContent);
            item.Init(id, cost, sprite);

            listItems.Add(item.imageItem);
            listItemDecors.Add(item);
            sprites.Add(dataBook.listDataItemDecor[i].sprite);
        }

        for (int i = 0; i < listItemDecors.Count; i++)
        {
            for (int j = 0; j < bookDecorated.listItemDecorated.Count; j++)
            {
                if (bookDecorated.listItemDecorated[j].idItemDecorated == listItemDecors[i].id)
                {
                    if (!bookDecorated.listItemDecorated[j].isBought) continue;

                    listItems[i].isBought = true;
                    listItemDecors[i].btnBuy.gameObject.SetActive(false);
                }
            }
        }

        if (IsRedecorated) return;
        // Setup entry bg

        Dictionary<int, ImageItem> itemDict = listItems.ToDictionary(item => item.id);

        for (int i = 0; i < bookDecorated.listItemDecorated.Count; i++)
        {
            int idItemDecorated = bookDecorated.listItemDecorated[i].idItemDecorated;

            // Kiểm tra nếu item tồn tại trong dictionary
            if (itemDict.TryGetValue(idItemDecorated, out ImageItem itemData))
            {
                if (!bookDecorated.listItemDecorated[i].isPainted) continue;

                itemData.isPainted = true;
                itemData.img.gameObject.SetActive(false);

                // Lấy danh sách các slot có id khớp với item
                for (int k = 0; k < slots.Count; k++)
                {
                    if (slots[k].id == idItemDecorated)
                    {
                        Sprite sprite = sprites[idItemDecorated];
                        int id = idItemDecorated;
                        ImageItem imageItem = itemData;
                        Slot slot = slots[k];
                        Vector2 pos = new Vector2(bookDecorated.listItemDecorated[i].x, bookDecorated.listItemDecorated[i].y);

                        ItemDraggable itemDraggable = ItemDraggablePool.Instance.GetPooledObject();
                        itemDraggable.Init(sprite, id, pos, imageItem, slot);
                    }
                }
            }
        }

    }

    void InitColor()
    {
        for (int i = 0; i < dataBook.listColorDecor.Count; i++)
        {
            Color color = dataBook.listColorDecor[i].color;

            if (i == 0)
                listItemSelectColor[i].Init(color, true);
            else
                listItemSelectColor[i].Init(color, false);
        }

        nColorChangeBook.color = bookDecorated.colorPainted;
    }

    public override void Hide()
    {

        base.Hide();
    }

    public void OnSelect(bool isSelectItem)
    {
        bgScrollViewItem.SetActive(isSelectItem);
        imgChooseItem.SetActive(isSelectItem);
        imgNotChooseItem.SetActive(!isSelectItem);

        bgSelectColor.SetActive(!isSelectItem);
        imgChooseBg.SetActive(!isSelectItem);
        imgNotChooseBg.SetActive(isSelectItem);

        //btnTick.gameObject.SetActive(!isSelectItem);
    }

    public void SpawnItemDrag(ImageItem imageItem)
    {
        if (currentItemDrag != null) return;

        currentItemDrag = ItemDraggablePool.Instance.GetPooledObject();
        currentItemDrag.rectTransform.SetParent(nParent);
        currentItemDrag.gameObject.SetActive(true);
        RectTransform imageItemRect = imageItem.GetComponent<RectTransform>();
        currentItemDrag.id = imageItem.id;
        Vector2 startPos = currentItemDrag.rectTransform.position;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].id == imageItem.id)
            {
                currentItemDrag.Initialize(imageItem, startPos, slots[i]);
            }
        }

        currentItemDrag.rectTransform.position = imageItemRect.position;
        currentItemDrag.imgItemDrag.sprite = sprites[imageItem.id];
        currentItemDrag.imgItemDrag.SetNativeSize();
    }

    public void ClearCurrentDraggingItem()
    {
        if (currentItemDrag != null)
        {
            currentItemDrag = null;
        }
    }


    // Change Color Book

    public void ChangeColorBook(object e)
    {
        btnTick.gameObject.SetActive(true);
        nColorChangeBook.color = (Color)e;
    }

    public void SaveCurrentColor()
    {
        ListBookDecorated dataCache = SaveGame.ListBookDecorated;
        List<BookDecorated> listBookDecoratedCache = dataCache.listBookDecorated;
        List<ItemDecorated> listItemDecoratedCache = new List<ItemDecorated>();

        for (int i = 0; i < listBookDecoratedCache.Count; i++)
        {
            if (listBookDecoratedCache[i].idBookDecorated == SaveGame.CurrentBook)
            {
                listItemDecoratedCache = listBookDecoratedCache[i].listItemDecorated;
            }
        }

        for (int i = 0; i < dataCache.listBookDecorated.Count; i++)
        {
            if (dataCache.listBookDecorated[i].idBookDecorated == this.idBookDecorated)
            {
                dataCache.listBookDecorated[i].colorPainted = nColorChangeBook.color;
            }
        }

        UpdateCurrentProgressBookDecorated(listBookDecoratedCache, listItemDecoratedCache);

        dataCache.listBookDecorated = listBookDecoratedCache;
        SaveGame.ListBookDecorated = dataCache;
        OpenNewBook();
    }

    private void UpdateCurrentProgressBookDecorated(List<BookDecorated> listBookDecoratedCache, List<ItemDecorated> listItemDecoratedCache)
    {
        int countProgress = 0;

        for (int i = 0; i < listBookDecoratedCache.Count; i++)
        {
            if (listBookDecoratedCache[i].idBookDecorated == SaveGame.CurrentBook)
            {
                listBookDecoratedCache[i].listItemDecorated = listItemDecoratedCache;

                for (int j = 0; j < listBookDecoratedCache[i].listItemDecorated.Count; j++)
                {
                    if (listBookDecoratedCache[i].listItemDecorated[j].isTruePos) countProgress++;
                }

                if (listBookDecoratedCache[i].colorPainted != GameConfig.DEFAULT_COLOR) countProgress++;

                listBookDecoratedCache[i].progress = (float)countProgress / total;
            }
        }
    }

    public void OpenNewBook()
    {
        ListBookDecorated dataCache = SaveGame.ListBookDecorated;

        int count = 0;
        for (int i = 0; i < dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].listItemDecorated.Count; i++)
        {
            if (dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].listItemDecorated[i].isPainted) count++;

        }

        if (dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].colorPainted != GameConfig.DEFAULT_COLOR) count++;

        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            int idBook = dataConfigDecor.listDataBooks[i].idBook;

            if (dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].idBookDecorated == idBook)
            {
                Debug.Log("count" + count);
                Debug.Log("MaxCurrentBook: " + SaveGame.MaxCurrentBook + "  ____ " + idBook);
                if (count == dataConfigDecor.listDataBooks[i].totalParts)
                {
                    Debug.Log(" Open New Book");
                    //if(SaveGame.MaxCurrentBook == idBook) return;
                    SaveGame.MaxCurrentBook = idBook + 1;
                    dataCache.listBookDecorated.Add(new BookDecorated()
                    {
                        idBookDecorated = idBook + 1,
                        progress = 0,
                        isPainted = false,
                        colorPainted = GameConfig.DEFAULT_COLOR,
                        listItemDecorated = new List<ItemDecorated>()
                        {

                        }
                    });

                    SaveGame.ListBookDecorated = dataCache;
                    PopupNewBook.Show();
                    break;

                }
            }
        }
    }
}

