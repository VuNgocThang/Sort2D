using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public Image imgCustomer;
    public MissionBonus missionBonus;

    public MissionBonus missionBonusPrefab;

    public void Init(DataCustomer dataCustomer)
    {
        imgCustomer.sprite = dataCustomer.spriteCustomer;
        MissionBonus missionBonus = Instantiate(missionBonusPrefab, transform);
        missionBonus.Init(dataCustomer.missions);
    }
}
