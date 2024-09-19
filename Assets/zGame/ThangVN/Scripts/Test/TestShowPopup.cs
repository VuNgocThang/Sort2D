using System.Collections;
using System.Collections.Generic;
using ThangVN;
using UnityEngine;

public class TestShowPopup : MonoBehaviour
{

    private void Start()
    {
        //PopupUnlockColor.Show((int)NewColorEnum.ColorYellow);

        PopupUnlockBooster.Show((int)BoosterEnum.BoosterHammer);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PopupUnlockColor.Show((int)NewColorEnum.ColorOrange);
        }
    }
}
