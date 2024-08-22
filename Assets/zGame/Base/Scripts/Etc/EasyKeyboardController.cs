using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class EasyKeyboardController : MonoBehaviour
    {
        void Update()
        {
            KeyboardControl();
        }

        bool[] arrMove = new bool[] { false, false, false, false, false, false };
        float value = 1f;
        void KeyboardControl()
        {
            if (Input.GetKeyDown(KeyCode.W))
                arrMove[0] = true;
            else if (Input.GetKeyUp(KeyCode.W))
                arrMove[0] = false;
            if (Input.GetKeyDown(KeyCode.S))
                arrMove[1] = true;
            else if (Input.GetKeyUp(KeyCode.S))
                arrMove[1] = false;
            if (Input.GetKeyDown(KeyCode.A))
                arrMove[2] = true;
            else if (Input.GetKeyUp(KeyCode.A))
                arrMove[2] = false;
            if (Input.GetKeyDown(KeyCode.D))
                arrMove[3] = true;
            else if (Input.GetKeyUp(KeyCode.D))
                arrMove[3] = false;
            if (Input.GetKeyDown(KeyCode.Q))
                arrMove[4] = true;
            else if (Input.GetKeyUp(KeyCode.Q))
                arrMove[4] = false;
            if (Input.GetKeyDown(KeyCode.E))
                arrMove[5] = true;
            else if (Input.GetKeyUp(KeyCode.E))
                arrMove[5] = false;

            if (arrMove[0])
                transform.position += new Vector3(0, 0, value * Ez.TimeMod);
            if (arrMove[1])
                transform.position -= new Vector3(0, 0, value * Ez.TimeMod);
            if (arrMove[2])
                transform.position -= new Vector3(value * Ez.TimeMod, 0, 0);
            if (arrMove[3])
                transform.position += new Vector3(value * Ez.TimeMod, 0, 0);
            if (arrMove[4])
                transform.position -= new Vector3(0, value * Ez.TimeMod, 0);
            if (arrMove[5])
                transform.position += new Vector3(0, value * Ez.TimeMod, 0);
        }
    }
}
