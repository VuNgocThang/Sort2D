using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[Serializable]
//public enum MissionType
//{
//    Red,
//    Green,
//    Blue,
//    Yellow,
//    Purple,
//    Pink
//}

public class Mission : MonoBehaviour
{
    public ColorEnum missionType;
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
        AddBorder();

        txtQuantity.text = quantity.ToString();
        txtCurrent.text = "0 /";
    }

    void AddBorder()
    {
        if (txtQuantity == null || txtCurrent == null) return;

        //Material materialtxtQuantity = txtQuantity.fontSharedMaterial;

        //materialtxtQuantity.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        //materialtxtQuantity.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        //materialtxtQuantity.SetFloat(ShaderUtilities.ID_OutlineSoftness, 0.1f);

        //Material materialtxtCurrent = txtCurrent.fontSharedMaterial;

        //materialtxtCurrent.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        //materialtxtCurrent.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        //materialtxtCurrent.SetFloat(ShaderUtilities.ID_OutlineSoftness, 0.1f);
    }

    public bool Completed()
    {
        return (current >= quantity);
    }

    public void UpdateProgressMission(MissionProgress missionProgress)
    {
        ColorEnum typeCheck = missionProgress.missionType;
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
