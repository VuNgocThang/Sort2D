using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TESTUIMOVEMENT : MonoBehaviour
{
    public List<IMGITEM> listItems;
    public List<Slot> slots;
    public Transform nParent;
    [SerializeField] ITEMDRAG itemDrag;
    public List<Sprite> sprites;
    [SerializeField] ITEMDRAG currentItemDrag;

    private void Start()
    {
        for (int i = 0; i < listItems.Count; i++)
        {
            if (listItems[i].isPainted)
            {
                ITEMDRAG item = ItemDragPool.Instance.GetPooledObject();
                item.gameObject.SetActive(true);
                for (int j = 0; j < slots.Count; j++)
                {
                    if (slots[j].id == listItems[i].id)
                    {
                        item.imgItemDrag.sprite = sprites[listItems[i].id];
                        item.imgItemDrag.SetNativeSize();
                        item.rectTransform.SetParent(slots[j].transform);
                        item.rectTransform.localPosition = Vector2.zero;
                    }
                }
            }
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
