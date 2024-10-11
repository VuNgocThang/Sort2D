using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TESTUIMOVEMENT : MonoBehaviour
{
    public List<IMGITEM> listItems;
    public List<Slot> slots;
    public List<Sprite> sprites;

    public Transform nParent;
    public Transform nParentSlot;
    public Transform nContent;
    [SerializeField] ITEMDRAG itemDrag;
    [SerializeField] Slot slotPrefab;
    [SerializeField] ItemDecor itemDecorPrefab;
    [SerializeField] ITEMDRAG currentItemDrag;

    [SerializeField] DataConfigDecor dataConfigDecor;
    DataBook dataBook;

    private void Start()
    {
        //for (int i = 0; i < listItems.Count; i++)
        //{
        //    if (listItems[i].isPainted)
        //    {
        //        ITEMDRAG item = ItemDragPool.Instance.GetPooledObject();
        //        item.gameObject.SetActive(true);
        //        for (int j = 0; j < slots.Count; j++)
        //        {
        //            if (slots[j].id == listItems[i].id)
        //            {
        //                listItems[i].gameObject.SetActive(false);
        //                item.imgItemDrag.sprite = sprites[listItems[i].id];
        //                item.imgItemDrag.SetNativeSize();
        //                item.rectTransform.SetParent(slots[j].transform);
        //                item.rectTransform.localPosition = Vector2.zero;
        //            }
        //        }
        //    }
        //}
        listItems.Clear();
        slots.Clear();
        sprites.Clear();

        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            if (dataConfigDecor.listDataBooks[i].idBook == 0)
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
            item.imgItem.id = dataBook.listDataItemDecor[i].idItemDecor;
            item.imgItem.img.sprite = dataBook.listDataItemDecor[i].spriteIcon;
            item.txtCost.text = dataBook.listDataItemDecor[i].cost.ToString();
            listItems.Add(item.imgItem);
            sprites.Add(dataBook.listDataItemDecor[i].sprite);
        }
    }


    public void SpawnItemDrag(IMGITEM imgItem)
    {
        if (currentItemDrag != null)
        {
            Debug.Log("Existed.");
            return;
        }
        //currentItemDrag = Instantiate(itemDrag, nParent);
        currentItemDrag = ItemDragPool.Instance.GetPooledObject();
        currentItemDrag.rectTransform.SetParent(nParent);
        currentItemDrag.gameObject.SetActive(true);
        RectTransform imgItemRect = imgItem.GetComponent<RectTransform>();

        currentItemDrag.id = imgItem.id;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].id == imgItem.id)
            {
                currentItemDrag.Initialize(imgItem, currentItemDrag.rectTransform.position, slots[i]);
            }
        }

        currentItemDrag.rectTransform.position = imgItemRect.position;
        currentItemDrag.imgItemDrag.sprite = sprites[imgItem.id];
        currentItemDrag.imgItemDrag.SetNativeSize();
    }

    public void ClearCurrentDraggingItem()
    {
        currentItemDrag = null;
    }
}
