using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDecor : MonoBehaviour
{
    [HideInInspector] public IMGITEM imgItem;
    public ImageItem imageItem;
    public TextMeshProUGUI txtCost;
    public GameObject nButtonBuy;

    private void Update()
    {
        nButtonBuy.SetActive(imageItem.img.gameObject.activeSelf);
    }
}
