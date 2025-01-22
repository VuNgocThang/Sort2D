using DG.Tweening;
using ntDev;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.Common;

public class PopupDecorateBook : Popup
{
    public RectTransform nBookCover;
    [SerializeField] ParticleSystem particle;
    [SerializeField] Transform nParticle, nParentSub;
    [SerializeField] int idBookDecorated;
    [SerializeField] Image nColorChangeBook, nColorChangeBg1, nColorChangeBg2;
    [SerializeField] EasyButton btnSelectItem, btnSelectBgColor, btnPrev, btnNext, btnBack;
    [SerializeField] TextMeshProUGUI txtNameBook, txtColorPlate, txtCurrentProgress;
    [SerializeField] GameObject bgScrollViewItem, bgSelectColor, imgChooseItem, imgNotChooseItem, imgChooseBg, imgNotChooseBg, nCurrentProgress, nBot;
    [SerializeField] public ItemDraggable currentItemDrag;
    [SerializeField] Transform nParent, nParentSlot, nContent, nColorChangeParent;
    public List<ImageItem> listItems;
    public List<Slot> slots;
    public List<Sprite> sprites;
    public List<ItemDecor> listItemDecors;

    [SerializeField] ItemDraggable itemDragPrefab;
    [SerializeField] Slot slotPrefab;
    [SerializeField] ItemDecor itemDecorPrefab;

    public DataConfigDecor dataConfigDecor;
    public float total;

    public DataBook dataBook;
    [SerializeField] BookDecorated bookDecorated;

    // Select Color
    [SerializeField] EasyButton btnTick;
    [SerializeField] List<ItemSelectColor> listItemSelectColor;

    public ScrollRect scroll;
    public bool isDragging;

    private void Awake()
    {
        btnSelectItem.OnClick(() =>
        {
            OnSelect(true);
            btnTick.gameObject.SetActive(false);
        });
        btnSelectBgColor.OnClick(() =>
        {
            if (currentItemDrag != null) return;

            OnSelect(false);
        });
        btnBack.OnClick(() =>
        {
            if (currentItemDrag != null)
            {
                currentItemDrag.linkedSlot.imgLine.gameObject.SetActive(false);
                currentItemDrag.SetActive(false);
                currentItemDrag = null;
            }

            base.Hide();
            //PopupBookItem.Show(SaveGame.CurrentBook);
            PopupDecor.Show();
        });

        btnTick.OnClick(() =>
        {
            SaveCurrentColor();
        });

        ManagerEvent.RegEvent(EventCMD.EVENT_CHANGE_COLOR, ChangeColorBook);

        ManagerEvent.RegEvent(EventCMD.EVENT_SUB_BOOK, PlayAnimSubBook);
    }

    public static async void Show(int index, bool IsRedecorated)
    {
        PopupDecorateBook pop = await ManagerPopup.ShowPopup<PopupDecorateBook>();
        pop.Init();
        pop.Refresh();
        pop.Initialize(index, IsRedecorated);
        //pop.RedecoratedInit(index);
    }

    public void Refresh()
    {
        nColorChangeParent.transform.localScale = Vector3.one;
        nCurrentProgress.SetActive(false);
        nBot.SetActive(true);
    }

    public override void Init()
    {
        base.Init();
        btnTick.gameObject.SetActive(false);
        OnSelect(true);
        ManagerPopup.HidePopup<PopupBookItem>();
    }

    private void Update()
    {
        txtColorPlate.text = $"{SaveGame.Pigment}";
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
    public void Initialize(int index, bool IsRedecorated)
    {
        LoadDataBook();

        Debug.Log("Show PopupDecorateBook at index: " + index);
        listItems.Clear();
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        slots.Clear();
        sprites.Clear();

        for (int i = 0; i < listItemDecors.Count; i++)
        {
            listItemDecors[i].gameObject.SetActive(false);
        }
        listItemDecors.Clear();

        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            if (dataConfigDecor.listDataBooks[i].idBook == index /*0*/)
            {
                txtNameBook.text = dataConfigDecor.listDataBooks[i].titleBook;
                dataBook = dataConfigDecor.listDataBooks[i];
                total = dataConfigDecor.listDataBooks[i].totalParts;
            }
        }

        InitColor();

        for (int i = 0; i < dataBook.listDataSlots.Count; i++)
        {
            int id = dataBook.listDataSlots[i].idSlot;
            Vector3 pos = dataBook.listDataSlots[i].pos;
            Sprite sprite = dataBook.listDataSlots[i].spriteLine;

            Slot slot = Instantiate(slotPrefab, nParentSlot);
            slot.Init(id, pos, sprite);
            slots.Add(slot);
        }

        for (int i = 0; i < dataBook.listDataItemDecor.Count; i++)
        {
            int id = dataBook.listDataItemDecor[i].idItemDecor;
            int cost = dataBook.listDataItemDecor[i].cost;
            float percent = dataBook.listDataItemDecor[i].percent;
            Sprite sprite = dataBook.listDataItemDecor[i].spriteIcon;

            ItemDecor item = Instantiate(itemDecorPrefab, nContent);
            item.Init(id, cost, percent, sprite);

            listItems.Add(item.imageItem);
            listItemDecors.Add(item);
            sprites.Add(dataBook.listDataItemDecor[i].sprite);
        }

        for (int i = 0; i < listItemDecors.Count; i++)
        {
            for (int j = 0; j < bookDecorated.listItemDecorated.Count; j++)
            {
                if (bookDecorated.listItemDecorated[j].idItemDecorated == listItemDecors[i].id)
                {
                    if (!bookDecorated.listItemDecorated[j].isBought) continue;

                    listItems[i].isBought = true;
                    listItemDecors[i].btnBuy.gameObject.SetActive(false);
                }
            }
        }

        if (IsRedecorated) return;
        // Setup entry bg

        Dictionary<int, ImageItem> itemDict = listItems.ToDictionary(item => item.id);

        for (int i = 0; i < bookDecorated.listItemDecorated.Count; i++)
        {
            int idItemDecorated = bookDecorated.listItemDecorated[i].idItemDecorated;

            // Kiểm tra nếu item tồn tại trong dictionary
            if (itemDict.TryGetValue(idItemDecorated, out ImageItem itemData))
            {
                if (!bookDecorated.listItemDecorated[i].isPainted) continue;

                itemData.isPainted = true;
                itemData.img.gameObject.SetActive(false);

                // Lấy danh sách các slot có id khớp với item
                for (int k = 0; k < slots.Count; k++)
                {
                    if (slots[k].id == idItemDecorated)
                    {
                        Sprite sprite = sprites[idItemDecorated];
                        int id = idItemDecorated;
                        ImageItem imageItem = itemData;
                        Slot slot = slots[k];
                        Vector2 pos = new Vector2(bookDecorated.listItemDecorated[i].x, bookDecorated.listItemDecorated[i].y);

                        ItemDraggable itemDraggable = ItemDraggablePool.Instance.GetPooledObject();
                        itemDraggable.Init(sprite, id, pos, imageItem, slot);
                    }
                }
            }
        }

    }

    void InitColor()
    {
        for (int i = 0; i < dataBook.listColorDecor.Count; i++)
        {
            Color color = dataBook.listColorDecor[i].color;

            if (i == 0)
                listItemSelectColor[i].Init(color, true);
            else
                listItemSelectColor[i].Init(color, false);
        }

        nColorChangeBook.color = bookDecorated.colorPainted;
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnSelect(bool isSelectItem)
    {
        bgScrollViewItem.SetActive(isSelectItem);
        imgChooseItem.SetActive(isSelectItem);
        imgNotChooseItem.SetActive(!isSelectItem);

        bgSelectColor.SetActive(!isSelectItem);
        imgChooseBg.SetActive(!isSelectItem);
        imgNotChooseBg.SetActive(isSelectItem);

        //btnTick.gameObject.SetActive(!isSelectItem);
    }

    public void SpawnItemDrag(ImageItem imageItem, PointerEventData eventData)
    {
        if (currentItemDrag != null) return;

        currentItemDrag = ItemDraggablePool.Instance.GetPooledObject();
        currentItemDrag.rectTransform.SetParent(nParent);
        currentItemDrag.gameObject.SetActive(true);
        RectTransform imageItemRect = imageItem.GetComponent<RectTransform>();
        currentItemDrag.id = imageItem.id;
        Vector2 startPos = currentItemDrag.rectTransform.position;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].id == imageItem.id)
            {
                currentItemDrag.Initialize(imageItem, startPos, slots[i]);
            }
        }

        currentItemDrag.rectTransform.position = imageItemRect.position;
        currentItemDrag.imgItemDrag.sprite = sprites[imageItem.id];
        currentItemDrag.imgItemDrag.SetNativeSize();

        currentItemDrag.OnBeginDrag(eventData);
    }

    public void ClearCurrentDraggingItem()
    {
        scroll.enabled = true;

        if (currentItemDrag != null)
        {
            currentItemDrag = null;
        }
    }


    // Change Color Book
    public void ChangeColorBook(object e)
    {
        if (bookDecorated.colorPainted == (Color)e)
        {
            btnTick.gameObject.SetActive(false);
        }
        else
        {
            btnTick.gameObject.SetActive(true);
        }

        nColorChangeBook.color = (Color)e;
    }

    public void PlayAnimSubBook(object e)
    {
        GameObject obj = PoolManager.Spawn(ScriptableObjectData.ObjectConfig.GetObject(EnumObject.SUBBOOK));
        obj.transform.SetParent(nParentSub);
        obj.transform.localPosition = Vector3.zero;
        SubBook subBook = obj.GetComponent<SubBook>();
        subBook.Init((int)e);
        subBook.gameObject.SetActive(true);
    }

    public void SaveCurrentColor()
    {
        if (btnTick != null)
            btnTick.gameObject.SetActive(false);

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

        for (int i = 0; i < dataCache.listBookDecorated.Count; i++)
        {
            if (dataCache.listBookDecorated[i].idBookDecorated == this.idBookDecorated)
            {
                dataCache.listBookDecorated[i].colorPainted = nColorChangeBook.color;
            }
        }

        UpdateCurrentProgressBookDecorated(listBookDecoratedCache, listItemDecoratedCache);

        dataCache.listBookDecorated = listBookDecoratedCache;
        SaveGame.ListBookDecorated = dataCache;
        OpenNewBook();
    }

    private void UpdateCurrentProgressBookDecorated(List<BookDecorated> listBookDecoratedCache, List<ItemDecorated> listItemDecoratedCache)
    {
        float currentPercent = 0f;
        for (int i = 0; i < listBookDecoratedCache.Count; i++)
        {
            if (listBookDecoratedCache[i].idBookDecorated == SaveGame.CurrentBook)
            {
                listBookDecoratedCache[i].listItemDecorated = listItemDecoratedCache;

                for (int j = 0; j < listBookDecoratedCache[i].listItemDecorated.Count; j++)
                {
                    if (listBookDecoratedCache[i].listItemDecorated[j].isTruePos)
                        currentPercent += listBookDecoratedCache[i].listItemDecorated[j].percent;
                }

                if (listBookDecoratedCache[i].colorPainted != GameConfig.DEFAULT_COLOR)
                    currentPercent += GameConfig.PERCENT_COLOR;

                listBookDecoratedCache[i].progress = currentPercent / GameConfig.PERCENT_TOTAL;
            }
        }
    }

    public void OpenNewBook()
    {
        ListBookDecorated dataCache = SaveGame.ListBookDecorated;

        int count = 0;
        for (int i = 0; i < dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].listItemDecorated.Count; i++)
        {
            if (dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].listItemDecorated[i].isPainted) count++;
        }

        if (dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].colorPainted != GameConfig.DEFAULT_COLOR) count++;

        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            int idBook = dataConfigDecor.listDataBooks[i].idBook;

            if (dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].idBookDecorated == idBook)
            {
                //Debug.Log("count" + count);
                //Debug.Log("MaxCurrentBook: " + SaveGame.MaxCurrentBook + "  ____ " + idBook);

                if (count == dataConfigDecor.listDataBooks[i].totalParts)
                {
                    Debug.Log(" Open New Book");
                    //if(SaveGame.MaxCurrentBook == idBook) return;
                    if (SaveGame.MaxCurrentBook < dataConfigDecor.listDataBooks.Count - 1)
                    {
                        SaveGame.MaxCurrentBook = idBook + 1;
                        dataCache.listBookDecorated.Add(new BookDecorated()
                        {
                            idBookDecorated = idBook + 1,
                            progress = 0,
                            isPainted = false,
                            isCollectedReward = false,
                            colorPainted = GameConfig.DEFAULT_COLOR,
                            listItemDecorated = new List<ItemDecorated>()
                            {

                            }
                        });

                        SaveGame.ListBookDecorated = dataCache;
                        StartCoroutine(PlayAnimBookDecorate());
                        //PopupNewBook.Show();
                        break;
                    }
                    else
                    {
                        if (!SaveGame.Redecorated)
                            PopupDecor.Show();
                    }

                }
            }
        }
    }

    public void PlayParticle(Transform nSlot)
    {
        nParticle.transform.position = nSlot.position;
        particle.Play();
    }

    IEnumerator PlayAnimBookDecorate()
    {
        float current = bookDecorated.progress;
        txtCurrentProgress.text = $"Progress: {current * 100}%";
        nCurrentProgress.SetActive(true);
        nBot.SetActive(false);
        nColorChangeParent.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.5f);
        yield return new WaitForSeconds(1f);

        if (current == 1)
        {
            PopupRewardDecor.Show();
        }
        else
        {
            PopupNewBook.Show();
        }
    }
}

