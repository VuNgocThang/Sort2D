using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ITEMDRAG : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image imgItemDrag;
    public int id;
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private IMGITEM linkedImgItem;
    private Slot linkedSlot;
    private TESTUIMOVEMENT testUIMovement;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Initialize(IMGITEM imgItem, Vector2 startPosition, Slot linkedSlot)
    {
        originalPosition = startPosition;
        linkedImgItem = imgItem;
        linkedImgItem.gameObject.SetActive(false);
        this.linkedSlot = linkedSlot;
        this.linkedSlot.imgLine.gameObject.SetActive(true);

        testUIMovement = FindObjectOfType<TESTUIMOVEMENT>();
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

        rectTransform.anchoredPosition = originalPosition;

        linkedImgItem.gameObject.SetActive(true);
        if (linkedSlot != null)
            linkedSlot.imgLine.gameObject.SetActive(false);

        this.gameObject.SetActive(false);

        if (testUIMovement != null)
        {
            testUIMovement.ClearCurrentDraggingItem();
        }
    }
}
