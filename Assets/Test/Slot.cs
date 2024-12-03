using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int id;
    public Image imgLine;
    public RectTransform rectTransform;

    public void Init(int id, Vector3 pos, Sprite sprite)
    {
        this.id = id;
        this.rectTransform.anchoredPosition = pos;
        this.imgLine.sprite = sprite;
        this.imgLine.SetNativeSize();
        this.imgLine.gameObject.SetActive(false);
    }
}
