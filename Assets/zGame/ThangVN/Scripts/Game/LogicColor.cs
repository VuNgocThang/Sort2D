using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogicColor : MonoBehaviour
{
    [SerializeField] public List<GameObject> listMeshes;
    //[SerializeField] public GameObject lockNoMove;
    //[SerializeField] public TrailRenderer trail;
    public GameObject trail;
    public SpriteRenderer spriteRender;
    public GameObject nBoxText;
    public TextMeshPro txtCount;

    public void Init(int index, int layer)
    {

        transform.localPosition = Vector3.zero;
        foreach (var mesh in listMeshes)
        {
            mesh.SetActive(false);
        }

        listMeshes[index].SetActive(true);
        spriteRender = listMeshes[index].GetComponent<SpriteRenderer>();
        spriteRender.sortingOrder = layer;
    }

    public void InitTutorial()
    {
        foreach (var mesh in listMeshes)
        {
            mesh.SetActive(false);
        }

        listMeshes[0].SetActive(true);
        spriteRender = listMeshes[0].GetComponent<SpriteRenderer>();
        spriteRender.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        spriteRender.sortingOrder = 10;
    }

    public void RefreshColor()
    {
        this.transform.localPosition = Vector3.zero;
        listMeshes[0].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void SetLayer(int Row)
    {
        int changeLayer = (GameConfig.OFFSET_LAYER - Row) > 1 ? GameConfig.OFFSET_LAYER - Row : 1;
        spriteRender.sortingOrder = changeLayer;
    }

    //public void InitLockNoMove()
    //{
    //    foreach (var mesh in listMeshes)
    //    {
    //        mesh.SetActive(false);
    //    }

    //    lockNoMove.SetActive(true);
    //}

    //public void 
}
