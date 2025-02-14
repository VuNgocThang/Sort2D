using BaseGame;
using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.Common;

public class ItemDraggable : MonoBehaviour, IBeginDragHandler, /*IDragHandler,*/ IEndDragHandler
{
    public int id;
    public Image imgItemDrag;
    public RectTransform rectTransform;

    CanvasGroup canvasGroup;
    Vector2 originalPosition;
    [SerializeField] PopupDecorateBook popupDecorateBook;
    ImageItem linkedImageItem;
    public Slot linkedSlot;
    [SerializeField] Color defaultColor = new Color(255, 255, 255, 255);
    [SerializeField] Color redColor = new Color(150, 50, 50, 255);
    [SerializeField] bool isDragging = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(Sprite sprite, int id, Vector2 pos, ImageItem imageItem, Slot slot)
    {
        this.imgItemDrag.sprite = sprite;
        this.imgItemDrag.SetNativeSize();
        this.id = id;
        this.SetInParent(imageItem, slot);
        this.rectTransform.anchoredPosition = pos;
        this.transform.localScale = Vector3.one;
        this.gameObject.SetActive(true);
    }

    public void Initialize(ImageItem imageItem, Vector2 startPos, Slot slot)
    {
        originalPosition = startPos;
        linkedImageItem = imageItem;
        //linkedImageItem.gameObject.SetActive(false);
        //linkedImageItem.img.gameObject.SetActive(false);
        linkedImageItem.img.color = new Color(1, 1, 1, 0);

        linkedSlot = slot;
        linkedSlot.imgLine.gameObject.SetActive(true);
        rectTransform.localScale = Vector3.one;
        popupDecorateBook = FindObjectOfType<PopupDecorateBook>();
    }

    public void SetInParent(ImageItem imageItem, Slot slot)
    {
        linkedImageItem = imageItem;
        linkedSlot = slot;
        this.gameObject.transform.SetParent(linkedSlot.gameObject.transform);
        rectTransform.anchoredPosition = Vector2.zero;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        linkedSlot.imgLine.gameObject.SetActive(true);
        isDragging = true;
    }

    private void Update()
    {
        if (isDragging)
        {
            if (!SaveGame.IsDoneTutorialDecor)
            {
                popupDecorateBook.ResetNColorChangeParent();
                TutorialDecor.Instance.EndTutorialDecor();
            }


            if (IsRectTransformInsideParent(rectTransform, popupDecorateBook.nBookCover))
                imgItemDrag.color = defaultColor;
            else imgItemDrag.color = redColor;

            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform,
                Input.mousePosition, null, out mousePos);

            rectTransform.anchoredPosition = mousePos;

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                PutDownObject();
            }
        }
    }

    //public void OnDrag(PointerEventData eventData)
    //{
    //    //rectTransform.anchoredPosition += eventData.delta;

    //    //if (IsRectTransformInsideParent(rectTransform, popupDecorateBook.nBookCover)) imgItemDrag.color = defaultColor;
    //    //else imgItemDrag.color = redColor;
    //}

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");
        PutDownObject();
    }

    private void PutDownObject()
    {
        isDragging = false;
        canvasGroup.blocksRaycasts = true;

        float distance = Vector2.Distance(rectTransform.position, linkedSlot.rectTransform.position);

        if (IsRectTransformInsideParent(rectTransform, popupDecorateBook.nBookCover))
        {
            ManagerAudio.PlaySound(ManagerAudio.Data.soundStickers);

            if (distance <= 100)
            {
                this.gameObject.transform.SetParent(linkedSlot.gameObject.transform);
                linkedImageItem.itemDecor.SetActive(false);
                rectTransform.anchoredPosition = Vector2.zero;
                AddNewObjectBook(true);
                popupDecorateBook.OpenNewBook();
            }
            else
            {
                this.gameObject.transform.SetParent(linkedSlot.gameObject.transform);
                linkedImageItem.itemDecor.SetActive(false);
                AddNewObjectBook(false);
                popupDecorateBook.OpenNewBook();
            }

            linkedImageItem.isPainted = true;

            if (!SaveGame.IsDoneTutGift)
            {
                TutorialDecor.Instance.ShowStep(2);
            }
        }
        else
        {
            imgItemDrag.color = defaultColor;
            rectTransform.anchoredPosition = originalPosition;
            //linkedImageItem.gameObject.SetActive(true);
            //linkedImageItem.img.gameObject.SetActive(true);
            linkedImageItem.img.color = new Color(1, 1, 1, 1);
            linkedImageItem.Refresh();

            this.gameObject.SetActive(false);
        }


        linkedSlot.imgLine.gameObject.SetActive(false);

        if (popupDecorateBook != null)
        {
            popupDecorateBook.ClearCurrentDraggingItem();
        }
    }

    bool IsRectTransformInsideParent(RectTransform child, RectTransform parent)
    {
        Vector3[] childCorners = new Vector3[4];
        child.GetWorldCorners(childCorners);

        Vector3[] parentCorners = new Vector3[4];
        parent.GetWorldCorners(parentCorners);

        for (int i = 0; i < 4; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(parent, childCorners[i]))
            {
                return true;
            }
        }

        return false;
    }

    void AddNewObjectBook(bool isTruePos)
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

        for (int i = 0; i < listItemDecoratedCache.Count; i++)
        {
            Debug.Log($"listItemDecoratedCache[{i}].idItemDecorated: " + listItemDecoratedCache[i].idItemDecorated);
            if (listItemDecoratedCache[i].idItemDecorated == id)
            {
                listItemDecoratedCache[i].isPainted = true;
                listItemDecoratedCache[i].x = rectTransform.anchoredPosition.x;
                listItemDecoratedCache[i].y = rectTransform.anchoredPosition.y;
                listItemDecoratedCache[i].isTruePos = isTruePos;
                listItemDecoratedCache[i].percent = linkedImageItem.itemDecor.percent;
            }
        }

        float currentPercent = 0;
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

        popupDecorateBook.PlayParticle(this.transform);
        DailyTaskManager.Instance.ExecuteDailyTask(TaskType.DecorateBook, 1);

        dataCache.listBookDecorated = listBookDecoratedCache;
        SaveGame.ListBookDecorated = dataCache;

        Debug.Log("save item decorate");
    }
}