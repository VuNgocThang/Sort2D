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

    [SerializeField] GameObject bgScrollViewItem,
        bgSelectColor,
        imgChooseItem,
        imgNotChooseItem,
        imgChooseBg,
        imgNotChooseBg,
        nCurrentProgress,
        nBot;

    [SerializeField] public ItemDraggable currentItemDrag;
    [SerializeField] Transform nParent, nParentSlot, nContent, nColorChangeParent, nParentColorChange;
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
    public BookDecorated bookDecorated = new BookDecorated();

    // Select Color
    [SerializeField] EasyButton btnTick;
    [SerializeField] List<ItemSelectColor> listItemSelectColor;

    public ScrollRect scroll;
    public bool isDragging;

    private void Awake()
    {
        btnPrev.OnClick(() => { scroll.horizontalScrollbar.value -= 0.5f; });

        btnNext.OnClick(() => { scroll.horizontalScrollbar.value += 0.5f; });

        btnSelectItem.OnClick(() =>
        {
            OnSelect(true);
            btnTick.gameObject.SetActive(false);

            if (!SaveGame.IsDoneTutorialDecor)
            {
                StartCoroutine(TutChooseColor(listItemDecors[0].GetComponent<RectTransform>()));
            }
        });
        btnSelectBgColor.OnClick(() =>
        {
            if (currentItemDrag != null) return;

            OnSelect(false);

            if (!SaveGame.IsDoneTutorialDecor)
            {
                StartCoroutine(TutChooseColor(listItemSelectColor[0].GetComponent<RectTransform>()));
            }
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
            if (!SaveGame.IsDoneTutorialDecor)
            {
                TutorialDecor.Instance.InitTutFocus(btnSelectItem.GetComponent<RectTransform>(), true);
            }
        });

        ManagerEvent.RegEvent(EventCMD.EVENT_CHANGE_COLOR, ChangeColorBook);

        ManagerEvent.RegEvent(EventCMD.EVENT_SUB_BOOK, PlayAnimSubBook);

        ManagerEvent.RegEvent(EventCMD.EVENT_CLAIM_REWARD_BOOK, RaiseEventCollectReward);

        CheckNull();
    }

    void CheckNull()
    {
        Debug.Log("Kiểm tra biến null...");
        Debug.Log(listItems != null ? "✅ listItems OK" : "❌ listItems bị null");
        Debug.Log(slots != null ? "✅ slots OK" : "❌ slots bị null");
        Debug.Log(sprites != null ? "✅ sprites OK" : "❌ sprites bị null");
        Debug.Log(listItemDecors != null ? "✅ listItemDecors OK" : "❌ listItemDecors bị null");
        Debug.Log(scroll != null ? "✅ scroll OK" : "❌ scroll bị null");
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
        //base.Init();
        transform.localScale = Vector3.one;
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

    void RefreshBookDecorated(BookDecorated bookDecorated)
    {
        bookDecorated.progress = 0;
        for (int i = 0; i < bookDecorated.listItemDecorated.Count; i++)
        {
            bookDecorated.colorPainted = GameConfig.DEFAULT_COLOR;
            bookDecorated.listItemDecorated[i].isTruePos = false;
            bookDecorated.listItemDecorated[i].isPainted = false;
            bookDecorated.listItemDecorated[i].x = 0;
            bookDecorated.listItemDecorated[i].y = 0;
        }

        nColorChangeBook.color = GameConfig.DEFAULT_COLOR;
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

        if (IsRedecorated)
        {
            RefreshBookDecorated(bookDecorated);
        }
        else
        {
            // Setup entry bg
            SetupEntryBG();
        }

        if (!SaveGame.IsDoneTutorialDecor)
            StartCoroutine(ResetPosContent());
    }


    IEnumerator ResetPosContent()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(nContent.GetComponent<RectTransform>());
        yield return new WaitForEndOfFrame();
        TutorialDecor.Instance.InitTutFocus(btnSelectBgColor.GetComponent<RectTransform>(), true);
        scroll.horizontalNormalizedPosition = 0;
        //scroll.content.anchoredPosition = new Vector2(0, scroll.content.anchoredPosition.y);
    }

    IEnumerator TutChooseColor(RectTransform rect)
    {
        yield return new WaitForEndOfFrame();
        TutorialDecor.Instance.SetParentDecorateBook(nColorChangeParent.GetComponent<RectTransform>());
        TutorialDecor.Instance.InitTutFocus(rect);
    }

    public void ResetNColorChangeParent()
    {
        nColorChangeParent.SetParent(nParentColorChange);
    }

    private void SetupEntryBG()
    {
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
                        Vector2 pos = new Vector2(bookDecorated.listItemDecorated[i].x,
                            bookDecorated.listItemDecorated[i].y);

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

        if (!SaveGame.IsDoneTutorialDecor)
        {
            btnTick.gameObject.SetActive(true);
            TutorialDecor.Instance.InitTutFocus(btnTick.GetComponent<RectTransform>(), true);
        }
    }

    public void PlayAnimSubBook(object e)
    {
        GameObject obj = PoolManager.Spawn(ScriptableObjectData.ObjectConfig.GetObject(EnumObject.SUBBOOK));
        obj.transform.SetParent(nParentSub);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
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

    private void UpdateCurrentProgressBookDecorated(List<BookDecorated> listBookDecoratedCache,
        List<ItemDecorated> listItemDecoratedCache)
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
        if (SaveGame.Redecorated)
        {
            ActionIfRedecorated();
        }
        else
        {
            ActionIfNotRedecorated();
        }
    }

    void ActionIfRedecorated()
    {
        int count = 0;

        if (bookDecorated.colorPainted != GameConfig.DEFAULT_COLOR) count++;
        for (int i = 0; i < bookDecorated.listItemDecorated.Count; i++)
        {
            if (bookDecorated.listItemDecorated[i].isPainted) count++;
        }

        if (count == dataConfigDecor.listDataBooks[idBookDecorated].totalParts)
        {
            StartCoroutine(PlayAnimBookDecorate());
        }
    }

    private void ActionIfNotRedecorated()
    {
        ListBookDecorated dataCache = SaveGame.ListBookDecorated;

        int count = 0;
        for (int i = 0;
             i < dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].listItemDecorated.Count;
             i++)
        {
            if (dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].listItemDecorated[i]
                .isPainted) count++;
        }

        if (dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].colorPainted !=
            GameConfig.DEFAULT_COLOR) count++;

        for (int i = 0; i < dataConfigDecor.listDataBooks.Count; i++)
        {
            int idBook = dataConfigDecor.listDataBooks[i].idBook;

            if (dataCache.listBookDecorated[dataCache.listBookDecorated.Count - 1].idBookDecorated == idBook)
            {
                //Debug.Log("count" + count);
                //Debug.Log("MaxCurrentBook: " + SaveGame.MaxCurrentBook + "  ____ " + idBook);

                if (count == dataConfigDecor.listDataBooks[i].totalParts)
                {
                    SaveIsSetupFull();

                    Debug.Log(" Open New Book");
                    //if(SaveGame.MaxCurrentBook == idBook) return;
                    if (SaveGame.MaxCurrentBook < dataConfigDecor.listDataBooks.Count - 1)
                    {
                        SaveGame.MaxCurrentBook = idBook + 1;
                        Debug.Log("SaveGame.MaxCurrentBook _1" + SaveGame.MaxCurrentBook);
                        dataCache.listBookDecorated.Add(new BookDecorated()
                        {
                            idBookDecorated = idBook + 1,
                            progress = 0,
                            isPainted = false,
                            isCollectedReward = false,
                            colorPainted = GameConfig.DEFAULT_COLOR,
                            isSetupFull = false,
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
                        else
                        {
                            Debug.Log("Done Done Done");
                        }
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

    void ShowCurrentProgress()
    {
    }

    IEnumerator PlayAnimBookDecorate()
    {
        float current = bookDecorated.progress;
        //txtCurrentProgress.text = $"Progress: {current * 100}%";

        nBot.SetActive(false);

        nColorChangeParent.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.5f);

        yield return new WaitForSeconds(0.5f);

        nCurrentProgress.SetActive(true);

        float targetProgress = current;
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            float progress = Mathf.Lerp(0f, targetProgress, elapsedTime / duration);
            txtCurrentProgress.text = $"Progress: {progress * 100:F2}%";
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        txtCurrentProgress.text = $"Progress: {targetProgress * 100}%";
        yield return new WaitForSeconds(1f);

        if (current == 1)
        {
            if (!bookDecorated.isCollectedReward)
            {
                //yield return new WaitForSeconds(1f);

                PopupRewardDecor.Show();
                bookDecorated.isCollectedReward = true;
                SaveIsCollectReward();
            }
            else
            {
                Debug.Log("show");
                PopupDecor.Show();
            }
        }
        else
        {
            Debug.Log("SaveGame.MaxCurrentBook _2" + SaveGame.MaxCurrentBook + " ___ " + idBookDecorated);

            if (SaveGame.MaxCurrentBook == idBookDecorated + 1 && !SaveGame.Redecorated)
            {
                PopupNewBook.Show();
            }
            else
            {
                Debug.Log("show2");
                PopupDecor.Show();
            }
        }
    }

    void SaveIsSetupFull()
    {
        ListBookDecorated dataCache = SaveGame.ListBookDecorated;
        List<BookDecorated> listBookDecoratedCache = dataCache.listBookDecorated;

        for (int i = 0; i < listBookDecoratedCache.Count; i++)
        {
            if (listBookDecoratedCache[i].idBookDecorated == bookDecorated.idBookDecorated)
            {
                listBookDecoratedCache[i].isSetupFull = true;
            }
        }

        dataCache.listBookDecorated = listBookDecoratedCache;
        SaveGame.ListBookDecorated = dataCache;
    }

    void SaveIsCollectReward()
    {
        ListBookDecorated dataCache = SaveGame.ListBookDecorated;
        List<BookDecorated> listBookDecoratedCache = dataCache.listBookDecorated;

        for (int i = 0; i < listBookDecoratedCache.Count; i++)
        {
            if (listBookDecoratedCache[i].idBookDecorated == bookDecorated.idBookDecorated)
            {
                listBookDecoratedCache[i].isCollectedReward = true;
            }
        }

        dataCache.listBookDecorated = listBookDecoratedCache;
        SaveGame.ListBookDecorated = dataCache;
    }

    void RaiseEventCollectReward(object e)
    {
        if (SaveGame.Redecorated)
        {
            PopupDecor.Show();
        }
        else
        {
            StartCoroutine(ShowPopupNewBook());
        }
    }

    IEnumerator ShowPopupNewBook()
    {
        yield return new WaitForSeconds(0.5f);
        PopupNewBook.Show();
    }
}