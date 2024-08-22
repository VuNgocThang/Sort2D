using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public int id;
    public int startIndex;
    //public List<Material> listMaterials;
    [SerializeField] MeshRenderer mesh;
    public List<GameObject> listObjects;
    public bool isPainted, isWatchAds;
    public Animator anim;
    [SerializeField] BoxCollider boxCollider;
    public Vector3 posEffect;


    public void SetUpMaterial(int index)
    {
        mesh.enabled = true;

        Vector3 a = GameConfig.OFFSET_NROOM;
        Vector3 b = boxCollider.center;
        posEffect = new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        //GameConfig.OFFSET_NROOM - boxCollider.center;
        Debug.Log(boxCollider.center + "___" + posEffect);

        if (anim != null && !isPainted) anim.Play("Show", -1, 0);

        foreach (Material mat in mesh.materials)
        {
            mat.SetFloat("_Index", index + 1);
        }

        if (listObjects != null)
        {
            for (int i = 0; i < listObjects.Count; i++)
            {
                listObjects[i].SetActive(true);
                if (!isPainted)
                    listObjects[i].GetComponent<Animator>().Play("Show");
            }
        }
    }

    public void Refresh()
    {
        if (id <= 2)
        {
            foreach (Material mat in mesh.materials)
            {
                mat.SetFloat("_Index", startIndex);
            }
        }
        else
        {
            mesh.enabled = false;

            if (listObjects != null)
            {
                for (int i = 0; i < listObjects.Count; i++)
                {
                    listObjects[i].SetActive(false);
                }
            }
        }
    }
}
