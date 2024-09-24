using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupBookItem : Popup
{
    [SerializeField] EasyButton btnDecorate;
    [SerializeField] TextMeshProUGUI txtProgress, txtNameBook, txtDecorate;

    private void Awake()
    {
        btnDecorate.OnClick(() =>
        {
            PopupDecorateBook.Show(0);
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
    }

    public override void Hide()
    {
        base.Hide();
    }
}
