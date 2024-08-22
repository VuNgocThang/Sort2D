using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupDecor : Popup
{
    [SerializeField] EasyButton btnBack, btnGallery, btnPlusColorPlate;
    [SerializeField] RectTransform select;
    [SerializeField] List<GameObject> listSelect;
    [SerializeField] TextMeshProUGUI txtColorPlate;

    private void Awake()
    {
        btnBack.OnClick(() => BackHome());

        btnGallery.OnClick(() =>
        {
            PopupGallery.Show();
        });

        btnPlusColorPlate.OnClick(() => PopupGoToLevel.Show());
    }

    public static async void Show()
    {
        PopupDecor pop = await ManagerPopup.ShowPopup<PopupDecor>();
        HomeUI.Instance.animator.Play("Hide");
        pop.Init();
    }

    public static async void ShowWithAnim(int start, int end, float duration)
    {
        PopupDecor pop = await ManagerPopup.ShowPopup<PopupDecor>();
        HomeUI.Instance.animator.Play("Hide");

        pop.InitWithAnim(start, end, duration);
    }

    public override void Init()
    {
        base.Init();
        ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = false;
        txtColorPlate.text = SaveGame.Pigment.ToString();

        for (int i = 0; i < listSelect.Count; i++)
        {
            listSelect[i].SetActive(false);
        }
        if (LogicSetupRoom.instance.listGameObject.Count > SaveGame.CurrentObject)
        {
            listSelect[SaveGame.CurrentObject].SetActive(true);
            LogicSetupRoom.instance.listGameObject[SaveGame.CurrentObject].SetActive(true);
            SaveGame.CanShow = true;
        }

    }

    public void InitWithAnim(int start, int end, float duration)
    {
        base.Init();
        ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = false;

        StartCoroutine(CountPigment(start, end, duration));

        for (int i = 0; i < listSelect.Count; i++)
        {
            listSelect[i].SetActive(false);
        }
        if (LogicSetupRoom.instance.listGameObject.Count > SaveGame.CurrentObject)
        {
            listSelect[SaveGame.CurrentObject].SetActive(true);
            LogicSetupRoom.instance.listGameObject[SaveGame.CurrentObject].SetActive(true);
            SaveGame.CanShow = true;
        }
    }

    private IEnumerator CountPigment(int start, int end, float duration)
    {
        HomeUI.Instance.animUsePigment.Play("Show");

        yield return new WaitForSeconds(0.3f);

        float elapsed = 0.0f;

        int currentPigment = start;

        while (elapsed < duration)
        {
            Debug.Log(elapsed);
            elapsed += Time.deltaTime;
            currentPigment = (int)Mathf.Lerp(start, end, elapsed / duration);
            txtColorPlate.text = currentPigment.ToString();
            yield return null;
        }

        currentPigment = end;
        txtColorPlate.text = currentPigment.ToString();
    }

    public override void Hide()
    {
        ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = true;
        base.Hide();
    }

    public void BackHome()
    {
        Debug.Log("BACKHOME");
        HomeUI.Instance.animator.Play("Show");
        HomeUI.Instance.DisableObject();
        ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = true;

        base.Hide();
    }


}
