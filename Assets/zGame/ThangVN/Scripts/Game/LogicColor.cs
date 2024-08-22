using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicColor : MonoBehaviour
{
    [SerializeField] public List<GameObject> listMeshes;
    //[SerializeField] public GameObject lockNoMove;
    [SerializeField] public TrailRenderer trail;

    public void Init(int index)
    {
        foreach (var mesh in listMeshes)
        {
            mesh.SetActive(false);
        }

        listMeshes[index].SetActive(true);
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
