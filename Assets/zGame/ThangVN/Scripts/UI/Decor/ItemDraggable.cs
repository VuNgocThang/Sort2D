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


    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
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
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;
            linkedImageItem.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }

        linkedSlot.imgLine.gameObject.SetActive(false);

        if (popupDecorateBook != null)
        {
            popupDecorateBook.ClearCurrentDraggingItem();
        }
    }
}
