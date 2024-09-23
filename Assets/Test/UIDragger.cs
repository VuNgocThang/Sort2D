using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragger : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Vector2 offset;
    public Image img;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        //img.color = new Color(0.5f, 0.5f, 0.5f, 1);
        img.color = new Color32(125, 125, 125, 255);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pointer Down");
    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector2 pointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out pointerPosition))
        {
            rectTransform.localPosition = pointerPosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("pointer up");
    }
}
