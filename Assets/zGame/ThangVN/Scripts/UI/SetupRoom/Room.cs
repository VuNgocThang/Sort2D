using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public EasyButton btn;
    public int index;
    public Image img;

    private void Awake()
    {
        btn.OnClick(() =>
        {
            SaveGame.CurrentRoom = index;
            //Show Room with index
            Debug.Log(" SaveGame.CurrentRoom: " + SaveGame.CurrentRoom);
        });
    }
}
