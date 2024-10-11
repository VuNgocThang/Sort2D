using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageOutline : MonoBehaviour
{
    public Image heartImage; 
    public Image outlineImage; 
    public Color outlineColor; 

    void Start()
    {
        CreateOutline();
    }

    void CreateOutline()
    {
        var collider = outlineImage.gameObject.AddComponent<PolygonCollider2D>();
        outlineImage.color = outlineColor; 
        outlineImage.rectTransform.sizeDelta = heartImage.rectTransform.sizeDelta * 1.1f; 
    }
}
