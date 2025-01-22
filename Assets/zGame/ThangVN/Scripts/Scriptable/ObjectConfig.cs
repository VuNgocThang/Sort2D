using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EnumObject
{
    SUBGOLD,
    SUBBOOK,
    SUBHEART,
}


[Serializable]
public class DataObject
{
    public EnumObject enumObject;
    public GameObject obj;
}


[CreateAssetMenu(fileName = "ObjectConfig", menuName = "ScriptableObjects/ObjectConfig")]
public class ObjectConfig : ScriptableObject
{
    public List<DataObject> dataObjs;

    public GameObject GetObject(EnumObject enumO)
    {
        GameObject obj = null;
        for (int i = 0; i < dataObjs.Count; i++)
        {
            if (dataObjs[i].enumObject == enumO)
            {
                obj = dataObjs[i].obj;
                return obj;
            }
        }

        return null;
    }
}
