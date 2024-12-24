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
        foreach (var mesh in listMeshes)
        {
            mesh.SetActive(false);
        }

        listMeshes[index].SetActive(true);
        spriteRender = listMeshes[index].GetComponent<SpriteRenderer>();
        spriteRender.sortingOrder = layer;
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
