using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupBookItem : Popup
{
    [SerializeField] int idBookDecorated;

    [SerializeField] EasyButton btnDecorate, btnBack;
    [SerializeField] TextMeshProUGUI txtProgress, txtNameBook, txtDecorate;
    [SerializeField] BookDecorated bookDecorated;
    public List<Slot> slots;
    public List<Sprite> sprites;

    [SerializeField] Slot slotPrefab;
    [SerializeField] Transform nParentSlot;

    [SerializeField] DataConfigDecor dataConfigDecor;
    DataBook dataBook;

    private void Awake()
    {
        btnDecorate.OnClick(() =>
        {
            PopupDecorateBook.Show(SaveGame.CurrentBook);
        });

        btnBack.OnClick(() =>
        {
            base.Hide();
            PopupDecor.Show();
        });
    }
    public static async void Show(int index)
    {
        PopupBookItem pop = await ManagerPopup.ShowPopup<PopupBookItem>();
        pop.Init();
        pop.Initialize(index);
    }

    public override void Init()
    {
        base.Init();
    }

    public void Initialize(int index)
    {
        Debug.Log("Show PopupBookItem at index: " + index);
        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            if (dataConfigDecor.listDataBooks[i].idBook == index)
            {
                txtNameBook.text = dataConfigDecor.listDataBooks[i].titleBook;
            }
        }
        LoadDataBook();
        SpawnExistedItemInBook();

    }

    void LoadDataBook()
    {
        idBookDecorated = SaveGame.CurrentBook;
        for (int i = 0; i < SaveGame.ListBookDecorated.listBookDecorated.Count; i++)
        {
            if (idBookDecorated == SaveGame.ListBookDecorated.listBookDecorated[i].idBookDecorated)
            {
                bookDecorated = SaveGame.ListBookDecorated.listBookDecorated[i];
            }
        }
    }

    void SpawnExistedItemInBook()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        slots.Clear();
        sprites.Clear();

        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            if (dataConfigDecor.listDataBooks[i].idBook == idBookDecorated)
            {
                dataBook = dataConfigDecor.listDataBooks[i];
            }
        }

        for (int i = 0; i < dataBook.listDataSlots.Count; i++)
        {
            Slot slot = Instantiate(slotPrefab, nParentSlot);
            slot.id = dataBook.listDataSlots[i].idSlot;
            slot.rectTransform.anchoredPosition = dataBook.listDataSlots[i].pos;
            slot.imgLine.gameObject.SetActive(false);
            slots.Add(slot);
        }

        for (int i = 0; i < dataBook.listDataItemDecor.Count; i++)
        {
            sprites.Add(dataBook.listDataItemDecor[i].sprite);
        }


        for (int i = 0; i < bookDecorated.listItemDecorated.Count; i++)
        {
            int idIndex = bookDecorated.listItemDecorated[i].idItemDecorated;
            for (int j = 0; j < slots.Count; j++)
            {
                if (slots[j].id == idIndex)
                {
                    ItemDraggable item = ItemDraggablePool.Instance.GetPooledObject();
                    item.imgItemDrag.sprite = sprites[idIndex];
                    item.imgItemDrag.SetNativeSize();
                    item.SetInParent(null, slots[j]);
                    item.rectTransform.anchoredPosition = new Vector2(bookDecorated.listItemDecorated[i].x, bookDecorated.listItemDecorated[i].y);
                    item.transform.localScale = Vector3.one;
                    item.gameObject.SetActive(true);
                }
            }
        }


    }

    public override void Hide()
    {
        base.Hide();
    }
}
