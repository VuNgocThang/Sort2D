using Febucci.UI;
using ntDev;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;

public class PopupDecorateBook : Popup
{
    [SerializeField] int idBookDecorated;
    [SerializeField] Image nColorChangeBook, nColorChangeBg1, nColorChangeBg2;
    [SerializeField] EasyButton btnSelectItem, btnSelectBgColor, btnPrev, btnNext;
    [SerializeField] TextMeshProUGUI txtNameBook;
    [SerializeField] GameObject bgScrollViewItem, bgSelectColor, imgChooseItem, imgNotChooseItem, imgChooseBg, imgNotChooseBg;
    ItemDraggable currentItemDrag;
    [SerializeField] Transform nParent, nParentSlot, nContent;
    public List<ImageItem> listItems;
    public List<Slot> slots;
    public List<Sprite> sprites;
    public List<ItemDecor> listItemDecors;

    [SerializeField] ItemDraggable itemDragPrefab;
    [SerializeField] Slot slotPrefab;
    [SerializeField] ItemDecor itemDecorPrefab;

    [SerializeField] DataConfigDecor dataConfigDecor;
    DataBook dataBook;

    [SerializeField] BookDecorated bookDecorated;

    private void Awake()
    {
        btnSelectItem.OnClick(() => OnSelect(true));
        btnSelectBgColor.OnClick(() => OnSelect(false));
    }

    public static async void Show(int index)
    {
        PopupDecorateBook pop = await ManagerPopup.ShowPopup<PopupDecorateBook>();
        pop.Init();
        pop.Initialize(index);
    }

    public override void Init()
    {
        base.Init();
        OnSelect(true);
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

    public void Initialize(int index)
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
            if (dataConfigDecor.listDataBooks[i].idBook == /*index*/0)
            {
                dataBook = dataConfigDecor.listDataBooks[i];
            }
        }

        for (int i = 0; i < dataBook.listDataSlots.Count; i++)
        {
            Slot slot = Instantiate(slotPrefab, nParentSlot);
            slot.id = dataBook.listDataSlots[i].idSlot;
            slot.rectTransform.anchoredPosition = dataBook.listDataSlots[i].pos;
            slot.imgLine.sprite = dataBook.listDataSlots[i].spriteLine;
            slot.imgLine.SetNativeSize();
            slot.imgLine.gameObject.SetActive(false);
            slots.Add(slot);
        }

        for (int i = 0; i < dataBook.listDataItemDecor.Count; i++)
        {
            ItemDecor item = Instantiate(itemDecorPrefab, nContent);
            item.imageItem.id = dataBook.listDataItemDecor[i].idItemDecor;
            item.imageItem.img.sprite = dataBook.listDataItemDecor[i].spriteIcon;
            item.imageItem.img.SetNativeSize();
            item.txtCost.text = dataBook.listDataItemDecor[i].cost.ToString();
            listItems.Add(item.imageItem);
            listItemDecors.Add(item);
            sprites.Add(dataBook.listDataItemDecor[i].sprite);
        }

        Dictionary<int, ImageItem> itemDict = listItems.ToDictionary(item => item.id);

        for (int i = 0; i < bookDecorated.listItemDecorated.Count; i++)
        {
            int idItemDecorated = bookDecorated.listItemDecorated[i].idItemDecorated;

            // Kiểm tra nếu item tồn tại trong dictionary
            if (itemDict.TryGetValue(idItemDecorated, out ImageItem itemData))
            {
                itemData.isPainted = true;
                itemData.img.gameObject.SetActive(false);

                // Lấy danh sách các slot có id khớp với item
                for (int k = 0; k < slots.Count; k++)
                {
                    if (slots[k].id == idItemDecorated)
                    {
                        ItemDraggable itemDraggable = ItemDraggablePool.Instance.GetPooledObject();
                        itemDraggable.imgItemDrag.sprite = sprites[idItemDecorated];
                        itemDraggable.imgItemDrag.SetNativeSize();
                        itemDraggable.id = idItemDecorated;
                        itemDraggable.SetInParent(itemData, slots[k]);
                        itemDraggable.transform.localScale = Vector3.one;
                        itemDraggable.gameObject.SetActive(true);
                    }
                }
            }
        }


        //for (int i = 0; i < listItems.Count; i++)
        //{
        //    for (int j = 0; j < bookDecorated.listItemDecorated.Count; j++)
        //    {
        //        if (bookDecorated.listItemDecorated[j].idItemDecorated == listItems[i].id)
        //        {
        //            listItems[i].isPainted = true;
        //            listItems[i].img.gameObject.SetActive(false);

        //            int idIndex = listItems[i].id;

        //            for (int k = 0; k < slots.Count; k++)
        //            {
        //                if (slots[k].id == idIndex)
        //                {
        //                    ItemDraggable item = ItemDraggablePool.Instance.GetPooledObject();
        //                    item.imgItemDrag.sprite = sprites[listItems[i].id];
        //                    item.imgItemDrag.SetNativeSize();
        //                    item.id = listItems[i].id;
        //                    item.SetInParent(listItems[i], slots[k]);
        //                    item.transform.localScale = Vector3.one;
        //                    item.gameObject.SetActive(true);
        //                }
        //            }
        //        }
        //    }
        //}

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
}

