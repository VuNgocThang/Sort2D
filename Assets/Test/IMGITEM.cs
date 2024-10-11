using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IMGITEM : MonoBehaviour
{
    public EasyButton btn;
    public int id;
    public bool isPainted;
    public Image img;


    private void Awake()
    {
        btn.OnClick(() =>
        {
            TESTUIMOVEMENT testMove = FindObjectOfType<TESTUIMOVEMENT>();

            if (testMove != null)
            {
                testMove.SpawnItemDrag(this);
            }
        });
    }
}
