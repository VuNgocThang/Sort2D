using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int id;
    public bool isPainted;
    public bool isBought;
    public Image img;
    public EasyButton btn;
    public ItemDecor itemDecor;


    private void Awake()
    {
        //btn.OnClick(() =>
        //{
        //    if (isBought)
        //    {
        //        //PopupDecorateBook popupDecorateBook = FindObjectOfType<PopupDecorateBook>();

        //        //if (popupDecorateBook != null)
        //        //{
        //        //    popupDecorateBook.SpawnItemDrag(this);
        //        //}
        //    }
        //    else
        //    {
        //        if (SaveGame.Pigment >= itemDecor.cost)
        //        {
        //            GameManager.SubPigment(itemDecor.cost);
        //            isBought = true;
        //            //btnBuyItem.gameObject.SetActive(false);
        //            SaveBoughtItemDecor();
        //        }
        //        else
        //        {
        //            EasyUI.Toast.Toast.Show("Not enough book!", 1f);
        //        }
        //    }
        //});

        delay = new WaitForSeconds(0.1f);
    }

    public void Init(int id, Sprite sprite)
    {
        this.id = id;
        img.sprite = sprite;
        img.SetNativeSize();
    }

    void SaveBoughtItemDecor()
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

        ItemDecorated itemDecorated = new ItemDecorated();
        itemDecorated.idItemDecorated = id;
        itemDecorated.isBought = true;

        listItemDecoratedCache.Add(itemDecorated);

        dataCache.listBookDecorated = listBookDecoratedCache;
        SaveGame.ListBookDecorated = dataCache;
        Debug.Log("save item decorate bought");
    }

    public bool isPointerDown = false;
    public bool isLongPressed = false;
    private DateTime pressTime;
    private WaitForSeconds delay;

    [Range(0.3f, 5f)] public float holdDuration = 0.5f;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("On pointer down");
        if (isPainted || !isBought) return;

        isPointerDown = true;
        if (btn.img.enabled)
        {
            PopupDecorateBook popupDecorateBook = FindObjectOfType<PopupDecorateBook>();

            if (popupDecorateBook != null)
            {
                popupDecorateBook.scroll.enabled = false;
                popupDecorateBook.SpawnItemDrag(this, eventData);
            }
        }
        //pressTime = DateTime.Now;
        //StartCoroutine(Timer(eventData));
    }


    public void Refresh()
    {
        isPointerDown = false;
        isLongPressed = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointer Up");
        isPointerDown = false;
        isLongPressed = false;
    }

    IEnumerator Timer(PointerEventData eventData)
    {
        while (isPointerDown && !isLongPressed)
        {
            double elapsedSeconds = (DateTime.Now - pressTime).TotalSeconds;

            if (elapsedSeconds >= holdDuration)
            {
                isLongPressed = true;
                if (btn.img.enabled)
                {
                    // su kien keo
                    PopupDecorateBook popupDecorateBook = FindObjectOfType<PopupDecorateBook>();

                    if (popupDecorateBook != null)
                    {
                        popupDecorateBook.scroll.enabled = false;
                        popupDecorateBook.SpawnItemDrag(this, eventData);
                    }
                }

                yield break;
            }

            yield return delay;
        }
    }
}