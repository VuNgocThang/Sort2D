using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataMission
{
    public ColorEnum missionType;
    public Sprite spriteMission;
    public int quantity;
}

[Serializable]
public class DataCustomer
{
    public int idCustomer;
    public Sprite spriteCustomer;
    public Sprite spriteCompleted;
    public List<DataMission> missions;
}

[Serializable]
public class DataCustomerMission
{
    public List<DataCustomer> listCustomers;
    public float timer;
}

[CreateAssetMenu(fileName = "CustomerMissionData", menuName = "ScriptableObjects/CustomerMissionData")]
public class CustomerMissionData : ScriptableObject
{
    public List<DataCustomerMission> listLevelBonus;
}
