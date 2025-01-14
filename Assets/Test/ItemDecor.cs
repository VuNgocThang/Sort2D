using ntDev;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDecor : MonoBehaviour
{
    public int id;
    public float percent;
    public ImageItem imageItem;
    public TextMeshProUGUI txtCost;
    public EasyButton btnBuy;
    public int cost;

    private void Awake()
    {
        btnBuy.OnClick(() =>
        {
            if (SaveGame.Pigment >= cost)
            {
                GameManager.SubPigment(cost);
                imageItem.isBought = true;
                btnBuy.gameObject.SetActive(false);
                SaveBoughtItemDecor();
            }
            else
            {
                EasyUI.Toast.Toast.Show("Not enough book!", 0.5f);
            }
        });
    }

    public void Init(int id, int cost, float percent, Sprite sprite)
    {
        this.id = id;
        this.cost = cost;
        this.percent = percent;

        imageItem.Init(id, sprite);
        txtCost.text = cost.ToString();
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
        itemDecorated.percent = percent;

        listItemDecoratedCache.Add(itemDecorated);

        dataCache.listBookDecorated = listBookDecoratedCache;
        SaveGame.ListBookDecorated = dataCache;
        Debug.Log("save item decorate bought");
    }
}
