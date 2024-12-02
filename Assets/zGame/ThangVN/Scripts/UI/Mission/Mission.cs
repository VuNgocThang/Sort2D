using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum MissionType
{
    Red,
    Green,
    Blue,
    Yellow
}

public class Mission : MonoBehaviour
{
    public MissionType missionType;
    public int quantity;
    public int current;

    public Image imgIcon;
    public TextMeshProUGUI txtQuantity, txtCurrent;

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_MISSION_CUSTOMER, (e) =>
        {
            var data = e as MissionProgress;
            if (data != null)
            {
                UpdateProgressMission(data);
            }
        });
    }

    public void Init(DataMission dataMission)
    {
        missionType = dataMission.missionType;
        imgIcon.sprite = dataMission.spriteMission;
        quantity = dataMission.quantity;

        txtQuantity.text = quantity.ToString();
        txtCurrent.text = "0 /";
    }

    public bool Completed()
    {
        return (current >= quantity);
    }

    public void UpdateProgressMission(MissionProgress missionProgress)
    {
        MissionType typeCheck = missionProgress.missionType;
        int currentCheck = missionProgress.current;

        if (missionType == typeCheck)
        {
            current += currentCheck;

            if (current >= quantity) current = quantity;

            txtCurrent.text = $"{current} /";
        }
    }

    private void Update()
    {

    }

    void CheckDone()
    {

    }
}
