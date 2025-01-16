using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupNewBook : Popup
{
    [SerializeField] EasyButton btnNewBook;
    [SerializeField] Image imgNewBook;
    [SerializeField] ParticleSystem particle;
    [SerializeField] DataConfigDecor dataConfigDecor;
    [SerializeField] GameObject nColorChange;

    private void Awake()
    {
        btnNewBook.OnClick(() =>
        {
            StartCoroutine(ChangeBook());
        });
    }

    IEnumerator ChangeBook()
    {
        particle.Play();
        yield return new WaitForSeconds(0.5f);
        btnNewBook.gameObject.SetActive(false);
        nColorChange.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        HideThis();
    }

    private void HideThis()
    {
        base.Hide();
        ManagerPopup.HidePopup<PopupDecorateBook>();
        PopupDecor.Show();
    }

    public static async void Show()
    {
        PopupNewBook pop = await ManagerPopup.ShowPopup<PopupNewBook>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        btnNewBook.gameObject.SetActive(true);
        imgNewBook.sprite = dataConfigDecor.listDataBooks[SaveGame.MaxCurrentBook].sprite;
        nColorChange.SetActive(false);

    }
    public override void Hide()
    {
        base.Hide();
    }
}
