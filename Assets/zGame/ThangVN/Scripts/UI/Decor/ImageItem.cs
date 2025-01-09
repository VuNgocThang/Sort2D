using ntDev;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageItem : MonoBehaviour
{
    public int id;
    public bool isPainted;
    public bool isBought;
    public Image img;
    public EasyButton btn, btnBuyItem;
    public ItemDecor itemDecor;


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
                if (SaveGame.Pigment >= itemDecor.cost)
                {
                    GameManager.SubPigment(itemDecor.cost);
                    isBought = true;
                    btnBuyItem.gameObject.SetActive(false);
                    SaveBoughtItemDecor();
                }
                else
                {
                    EasyUI.Toast.Toast.Show("Not enough book!", 1f);
                }
            }
        });
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
}
