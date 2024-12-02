using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionCustomerManager : MonoBehaviour
{
    public Customer customerPrefab;

    public CustomerMissionData data;
    public List<Customer> listCustomers;
    public Transform nContent;
    public TextMeshProUGUI txtQuantityCustomer;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        int indexLevelBonus = SaveGame.LevelBonus;

        txtQuantityCustomer.text = data.listLevelBonus[indexLevelBonus].listCustomers.Count.ToString();

        for (int i = 0; i < data.listLevelBonus[indexLevelBonus].listCustomers.Count; i++)
        {
            DataCustomer dataCustomer = data.listLevelBonus[indexLevelBonus].listCustomers[i];

            Customer customer = Instantiate(customerPrefab, nContent);
            customer.Init(dataCustomer);
            listCustomers.Add(customer);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ManagerEvent.RaiseEvent(EventCMD.EVENT_MISSION_CUSTOMER, new MissionProgress(MissionType.Blue, 1));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ManagerEvent.RaiseEvent(EventCMD.EVENT_MISSION_CUSTOMER, new MissionProgress(MissionType.Red, 2));
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ManagerEvent.RaiseEvent(EventCMD.EVENT_MISSION_CUSTOMER, new MissionProgress(MissionType.Green, 3));
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            ManagerEvent.RaiseEvent(EventCMD.EVENT_MISSION_CUSTOMER, new MissionProgress(MissionType.Yellow, 4));
        }
    }

    //public void ExecuteMissionCustomer(MissionType missionType, int amount)
    //{
    //    for (int i = 0; i < listCustomers.Count; i++)
    //    {
    //        for (int j = 0; j < listCustomers[i].missionBonus.missions.Count; j++)
    //        {
    //            Mission mission = listCustomers[i].missionBonus.missions[i];

    //            if (mission.missionType == missionType)
    //            {
    //                mission.current += amount;
    //            }
    //        }
    //    }
    //}
}
