using ntDev;
using Spine;
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
    public Vector2 originalPosition;
    public IMGITEM linkedImgItem;
    public Slot linkedSlot;
    public TESTUIMOVEMENT testUIMovement;

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

        //To do end drag
        float distance = Vector2.Distance(rectTransform.position, linkedSlot.GetComponent<RectTransform>().position);
        Debug.Log("distance: " + distance);
        if (distance <= 100)
        {
            this.gameObject.transform.SetParent(linkedSlot.gameObject.transform);
            rectTransform.anchoredPosition = Vector2.zero;
            linkedSlot.imgLine.gameObject.SetActive(false);
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;
            linkedSlot.imgLine.gameObject.SetActive(false);
            linkedImgItem.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }

        if (testUIMovement != null)
        {
            testUIMovement.ClearCurrentDraggingItem();
        }

    }

}
