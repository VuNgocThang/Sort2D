using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupDecorateBook : Popup
{
    [SerializeField] Image nColorChangeBook, nColorChangeBg1, nColorChangeBg2;
    [SerializeField] EasyButton btnSelectItem, btnSelectBgColor, btnPrev, btnNext;
    [SerializeField] TextMeshProUGUI txtNameBook;
    [SerializeField] GameObject bgScrollViewItem, bgSelectColor, imgChooseItem, imgNotChooseItem, imgChooseBg, imgNotChooseBg;

    private void Awake()
    {
        btnSelectItem.OnClick(() => OnSelect(true));
        btnSelectBgColor.OnClick(() => OnSelect(false));
    }

    private void Start()
    {
        OnSelect(true);
    }

    public static async void Show(int index)
    {
        PopupDecorateBook pop = await ManagerPopup.ShowPopup<PopupDecorateBook>();
        pop.Init();
        pop.Initialize(index);
    }

    public override void Init()
    {
        base.Init();
    }

    public void Initialize(int index)
    {
        Debug.Log("Show PopupDecorateBook at index: " + index);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnSelect(bool isSelectItem)
    {
        //if (isSelectItem)
        //{
        //    bgScrollViewItem.SetActive(true);
        //    imgChooseItem.SetActive(true);
        //    imgNotChooseItem.SetActive(false);

        //    bgSelectColor.SetActive(false);
        //    imgChooseBg.SetActive(false);
        //    imgNotChooseBg.SetActive(true);
        //}
        bgScrollViewItem.SetActive(isSelectItem);
        imgChooseItem.SetActive(isSelectItem);
        imgNotChooseItem.SetActive(!isSelectItem);

        bgSelectColor.SetActive(!isSelectItem);
        imgChooseBg.SetActive(!isSelectItem);
        imgNotChooseBg.SetActive(isSelectItem);
    }
}

