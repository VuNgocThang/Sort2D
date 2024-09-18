using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShowPopup : MonoBehaviour
{

    private void Start()
    {
        PopupUnlockColor.Show((int)NewColorEnum.ColorYellow);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PopupUnlockColor.Show((int)NewColorEnum.ColorOrange);
        }
    }
}
