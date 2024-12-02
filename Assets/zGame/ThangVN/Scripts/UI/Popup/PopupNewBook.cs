using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupNewBook : Popup
{
    [SerializeField] EasyButton btnNewBook;

    private void Awake()
    {
        btnNewBook.OnClick(() =>
        {
            base.Hide();
            ManagerPopup.HidePopup<PopupDecorateBook>();
            PopupDecor.Show();
        });
    }

    public static async void Show()
    {
        PopupNewBook pop = await ManagerPopup.ShowPopup<PopupNewBook>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
    }
    public override void Hide()
    {
        base.Hide();
    }
}
