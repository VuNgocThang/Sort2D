using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupGDPR : MonoBehaviour
{
    public Button btnAgree, btnNoAgree;
    public void SetUp()
    {
        btnAgree.onClick.AddListener(OnClickAgree);
        btnNoAgree.onClick.AddListener(OnClikNoAgree);
    }
    public void OnClickAgree()
    {
        PlayerPrefs.SetInt("IronSource_Consent", 1);
        gameObject.SetActive(false);
        //LoadingManager.Instance.Init();
        AdsController.instance.InitStart();
    }

    public void OnClikNoAgree()
    {
        PlayerPrefs.SetInt("IronSource_Consent", 2);
        gameObject.SetActive(false);
        //LoadingManager.Instance.Init();
        AdsController.instance.InitStart();
    }
}
