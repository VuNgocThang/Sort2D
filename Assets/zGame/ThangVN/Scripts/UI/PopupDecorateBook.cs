using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupDecorateBook : Popup
{
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

    public void Initialize(int index)
    {
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
            slot.id = dataBook.listDataSlots[i].id;
            slot.rectTransform.anchoredPosition = dataBook.listDataSlots[i].pos;
            slot.imgLine.sprite = dataBook.listDataSlots[i].spriteLine;
            slot.imgLine.SetNativeSize();
            slot.imgLine.gameObject.SetActive(false);
            slots.Add(slot);
        }

        for (int i = 0; i < dataBook.listDataItemDecor.Count; i++)
        {
            ItemDecor item = Instantiate(itemDecorPrefab, nContent);
            item.imageItem.id = dataBook.listDataItemDecor[i].id;
            item.imageItem.img.sprite = dataBook.listDataItemDecor[i].spriteIcon;
            item.imageItem.img.SetNativeSize();
            item.txtCost.text = dataBook.listDataItemDecor[i].cost.ToString();
            listItems.Add(item.imageItem);
            listItemDecors.Add(item);
            sprites.Add(dataBook.listDataItemDecor[i].sprite);
        }
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
            if (slots[i].id == imageItem.id) currentItemDrag.Initialize(imageItem, startPos, slots[i]);
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

