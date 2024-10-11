using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageItem : MonoBehaviour
{
    public int id;
    public bool isPainted;
    public Image img;
    public EasyButton btn;


    private void Awake()
    {
        btn.OnClick(() =>
        {
            PopupDecorateBook popupDecorateBook = FindObjectOfType<PopupDecorateBook>();

            if (popupDecorateBook != null)
            {
                popupDecorateBook.SpawnItemDrag(this);
            }
        });
    }
}
