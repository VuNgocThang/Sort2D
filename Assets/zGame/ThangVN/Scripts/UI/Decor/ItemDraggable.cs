using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int id;
    public Image imgItemDrag;
    public RectTransform rectTransform;

    CanvasGroup canvasGroup;
    Vector2 originalPosition;
    PopupDecorateBook popupDecorateBook;
    ImageItem linkedImageItem;
    Slot linkedSlot;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(ImageItem imageItem, Vector2 startPos, Slot slot)
    {
        originalPosition = startPos;
        linkedImageItem = imageItem;
        linkedImageItem.gameObject.SetActive(false);
        linkedSlot = slot;
        linkedSlot.imgLine.gameObject.SetActive(true);

        popupDecorateBook = FindObjectOfType<PopupDecorateBook>();
    }

    public void SetInParent(ImageItem imageItem, Slot slot)
    {
        linkedImageItem = imageItem;
        linkedSlot = slot;
        this.gameObject.transform.SetParent(linkedSlot.gameObject.transform);
        rectTransform.anchoredPosition = Vector2.zero;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        linkedSlot.imgLine.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        float distance = Vector2.Distance(rectTransform.position, linkedSlot.rectTransform.position);

        if (distance <= 100)
        {
            this.gameObject.transform.SetParent(linkedSlot.gameObject.transform);
            rectTransform.anchoredPosition = Vector2.zero;
            AddNewBook();
        }
        else
        {
            //rectTransform.anchoredPosition = originalPosition;
            //linkedImageItem.gameObject.SetActive(true);
            //this.gameObject.SetActive(false);
        }

        linkedSlot.imgLine.gameObject.SetActive(false);

        if (popupDecorateBook != null)
        {
            popupDecorateBook.ClearCurrentDraggingItem();
        }
    }


    void AddNewBook()
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

        for (int i = 0; i < listItemDecoratedCache.Count; i++)
        {
            Debug.Log($"listItemDecoratedCache[{i}].idItemDecorated: " + listItemDecoratedCache[i].idItemDecorated);
        }
        //return;
        ItemDecorated itemDecorated = new ItemDecorated();
        itemDecorated.idItemDecorated = id;
        itemDecorated.isPainted = true;

        //for (int i = 0; i < listItemDecoratedCache.Count; i++)
        //{
        //    if (listItemDecoratedCache[i].idItemDecorated != id)
        //    {

        //    }
        //}
        listItemDecoratedCache.Add(itemDecorated);

        for (int i = 0; i < listItemDecoratedCache.Count; i++)
        {
            Debug.Log($"listItemDecoratedCache[{i}].idItemDecorated: " + listItemDecoratedCache[i].idItemDecorated);
        }

        for (int i = 0; i < listBookDecoratedCache.Count; i++)
        {
            if (listBookDecoratedCache[i].idBookDecorated == SaveGame.CurrentBook)
            {
                listBookDecoratedCache[i].listItemDecorated = listItemDecoratedCache;
            }
        }
        dataCache.listBookDecorated = listBookDecoratedCache;
        SaveGame.ListBookDecorated = dataCache;
        Debug.Log("save item decorate");

    }
}
