using System;
using System.Collections;
using System.Collections.Generic;
using ntDev;
using UnityEngine;

public class ButtonRateUs : MonoBehaviour
{
    [SerializeField] private EasyButton btnRate;
    [SerializeField] private int starRate;

    private void Awake()
    {
        btnRate.OnClick(() => { ManagerEvent.RaiseEvent(EventCMD.EVENT_RATE_US, starRate); });
    }
}