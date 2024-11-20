using ntDev;
using UnityEngine;
using UnityEngine.UI;

public class ImageItem : MonoBehaviour
{
    public int id;
    public bool isPainted;
    public bool isBought;
    public Image img;
    public EasyButton btn;


    private void Awake()
    {

        btn.OnClick(() =>
        {
            if (isBought)
            {
                PopupDecorateBook popupDecorateBook = FindObjectOfType<PopupDecorateBook>();

                if (popupDecorateBook != null)
                {
                    popupDecorateBook.SpawnItemDrag(this);
                }
            }
            else
            {
                Debug.Log("can't selected");
            }
        });
    }
}
