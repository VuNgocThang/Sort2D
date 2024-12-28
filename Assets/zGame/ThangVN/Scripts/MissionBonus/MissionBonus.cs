using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissionBonus : MonoBehaviour
{
    public List<Mission> missions;

    public Mission missionPrefab;

    public void Init(List<DataMission> listDataMission)
    {
        for (int i = 0; i < listDataMission.Count; i++)
        {
            DataMission dataMission = listDataMission[i];

            Mission mission = Instantiate(missionPrefab, transform);
            mission.Init(dataMission);
            missions.Add(mission);
        }
    }

    public bool CompletedAll()
    {
        bool isCompleted = false;

        for (int i = 0; i < missions.Count; i++)
        {
            if (!missions[i].Completed())
            {
                isCompleted = false;

                return isCompleted;
            }
            else
            {
                isCompleted = true;
            }
        }

        return isCompleted;
    }

    public void ChangeSpriteIfDone()
    {
        for (int i = 0; i < missions.Count; i++)
        {
            missions[i].gameObject.SetActive(false);
        }
    }
}
