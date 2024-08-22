using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// data config
[Serializable]
public class ObjectRoom
{
    public int idObject;
    public bool isPainted;
    public int colorNeed;
    public List<Sprite> sprite;
}


[Serializable]
public class ListObjectRoom
{
    public int idRoom;
    public List<ObjectRoom> listObjectRoom;
}

// data save load
[Serializable]
public class ObjectRoomPainted
{
    public int idObject;
    public int currentSprite;
    public bool isPainted;
    public bool isWatchAds;
}

[Serializable]
public class ListObjetRoomPainted
{
    public int idRoom;
    public List<ObjectRoomPainted> listObjectPainted;
}

[SerializeField]
public class ListRoomPainted
{
    public List<ListObjetRoomPainted> listRoomPainted;
}


// ScriptableObject
[CreateAssetMenu(fileName = "ListRoomData", menuName = "ScriptableObjects/ListRoomData")]

public class ListRoomData : ScriptableObject
{
    public List<ListObjectRoom> listRoom;
}
