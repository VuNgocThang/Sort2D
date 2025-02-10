using ntDev;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDecor : MonoBehaviour
{
    public int id;
    public float percent;
    public ImageItem imageItem;
    public Image bgItem;
    public TextMeshProUGUI txtCost;
    public EasyButton btnBuy, btnBuyImageItem;

    public int cost;
    [SerializeField] Color colorNotEnoughBG;
    [SerializeField] Color colorNotEnoughText;

    private void Awake()
    {
        btnBuy.OnClick(() =>
        {
            //if (SaveGame.Pigment >= cost)
            //{
            //    GameManager.SubPigment(cost);
            //    imageItem.isBought = true;
            //    btnBuy.gameObject.SetActive(false);
            //    SaveBoughtItemDecor();
            //}
            //else
            //{
            //    EasyUI.Toast.Toast.Show("Not enough book!", 0.5f);
            //}
        });

        btnBuyImageItem.OnClick(() =>
        {
            if (SaveGame.Pigment >= cost)
            {
                if (imageItem.isBought) return;
                ManagerEvent.RaiseEvent(EventCMD.EVENT_SUB_BOOK, cost);
                GameManager.SubPigment(cost);
                imageItem.isBought = true;
                btnBuy.gameObject.SetActive(false);
                SaveBoughtItemDecor();

                if (!SaveGame.IsDoneTutorialDecor)
                {
                    TutorialDecor.Instance.ShowStep(1);
                    TutorialDecor.Instance.PlayAnimationHand();
                }
            }
            else
            {
                if (imageItem.isBought) return;

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

    private void Update()
    {
        if (this.cost > SaveGame.Pigment)
        {
            bgItem.color = colorNotEnoughBG;
            txtCost.color = colorNotEnoughText;

            if (imageItem.isBought)
            {
                bgItem.color = new Color(1, 1, 1, 1);
                txtCost.color = new Color(1, 1, 1, 1);
            }
        }
        else
        {
            bgItem.color = new Color(1, 1, 1, 1);
            txtCost.color = new Color(1, 1, 1, 1);
        }
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
