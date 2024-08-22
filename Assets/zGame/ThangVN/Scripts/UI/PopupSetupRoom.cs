using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetupRoom : Popup
{
    [SerializeField] EasyButton btnTick;
    [SerializeField] int currentSprite;
    [SerializeField] int idObjectToAdd;
    [SerializeField] ListRoomData listRoomData;
    [SerializeField] ButtonSelectRoom btnSelectRoom1, btnSelectRoom2, btnSelectRoom3, btnSelectRoom4;
    [SerializeField] GameObject imgAds, nUsePigment;
    [SerializeField] bool isWatchAds;

    public bool HasObjectRoom(int idObjectRoom)
    {
        for (int i = 0; i < SaveGame.ListRoomPainted.listRoomPainted.Count; i++)
        {
            for (int j = 0; j < SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted.Count; j++)
            {
                if (idObjectRoom == SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].idObject)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_SELECT_ROOM, SelectRoomIndex);

        btnTick.OnClick(() =>
        {
            ExecuteTickOnClick();
        });
    }

    private void ExecuteTickOnClick()
    {
        if (!HasObjectRoom(idObjectToAdd))
        {
            if (currentSprite == 3 && !isWatchAds)
            {
                //To do da xem quang cao
                isWatchAds = true;
                Debug.Log("watch ads");
                imgAds.SetActive(false);
                AddRoomWatchedAds();
                for (int i = 0; i < LogicSetupRoom.instance.listRoomObject.Count; i++)
                {
                    if (LogicSetupRoom.instance.listRoomObject[i].id == SaveGame.CurrentObject)
                    {
                        LogicSetupRoom.instance.listRoomObject[i].isWatchAds = true;
                    }
                }
            }
            else
            {

                if (!GameConfig.EnoughPigment)
                {
                    PopupGoToLevel.Show();
                }
                else
                {
                    if (SaveGame.Pigment >= 300)
                    {
                        start = SaveGame.Pigment;

                        SaveGame.Pigment -= 300;
                    }

                    //Todo Chua co room thi ad data room moi vao
                    AddNewRoom();

                    // todo particle

                    LogicSetupRoom.instance.PlayParticle(SaveGame.CurrentObject);

                    for (int i = 0; i < LogicSetupRoom.instance.listRoomObject.Count; i++)
                    {
                        if (LogicSetupRoom.instance.listRoomObject[i].id == SaveGame.CurrentObject)
                        {
                            LogicSetupRoom.instance.listRoomObject[SaveGame.CurrentObject].gameObject.layer = 11;
                        }
                    }

                    if (SaveGame.CurrentObject < LogicSetupRoom.instance.listGameObject.Count)
                    {
                        if (SaveGame.CurrentObject == 5) SaveGame.CurrentObject += 2;
                        else SaveGame.CurrentObject++;

                        if (LogicSetupRoom.instance.listGameObject.Count < SaveGame.CurrentObject - 1)
                        {
                            if (LogicSetupRoom.instance.listGameObject[SaveGame.CurrentObject] != null)
                                LogicSetupRoom.instance.listGameObject[SaveGame.CurrentObject].SetActive(true);
                        }
                    }

                    ScreenshotManager.Instance.CaptureScreenshotWithoutUI();
                    //Hide();
                    HideWithAnim(start, SaveGame.Pigment, 0.75f);
                }

            }
        }
        else
        {
            if (!GameConfig.EnoughPigment)
            {
                PopupGoToLevel.Show();
            }
            else
            {
                //todo lam gi khi da co room
                if (SaveGame.Pigment >= 300)
                {
                    start = SaveGame.Pigment;
                    SaveGame.Pigment -= 300;
                }
                UpdateExistedRoom();

                for (int i = 0; i < LogicSetupRoom.instance.listRoomObject.Count; i++)
                {
                    if (LogicSetupRoom.instance.listRoomObject[i].id == SaveGame.CurrentObject)
                    {
                        LogicSetupRoom.instance.listRoomObject[SaveGame.CurrentObject].gameObject.layer = 11;
                    }
                }

                if (SaveGame.CurrentObject < LogicSetupRoom.instance.listGameObject.Count)
                {
                    if (SaveGame.CurrentObject == 5) SaveGame.CurrentObject += 2;
                    else SaveGame.CurrentObject++;

                    if (LogicSetupRoom.instance.listGameObject.Count < SaveGame.CurrentObject - 1)
                    {
                        if (LogicSetupRoom.instance.listGameObject[SaveGame.CurrentObject] != null)
                            LogicSetupRoom.instance.listGameObject[SaveGame.CurrentObject].SetActive(true);
                    }
                }
                ScreenshotManager.Instance.CaptureScreenshotWithoutUI();
                //Hide();
                HideWithAnim(start, SaveGame.Pigment, 0.75f);
            }
        }
    }

    int start;

    public static async void Show(int idRoom, int idObj, bool isRoomWatchAds)
    {
        Debug.Log("SHow");
        PopupSetupRoom pop = await ManagerPopup.ShowPopup<PopupSetupRoom>();
        pop.Initialized(idRoom, idObj, isRoomWatchAds);
    }

    public override void Hide()
    {
        base.Hide();
        SaveGame.CanShow = true;
        ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = true;
        PopupDecor.Show();

        List<RoomObject> listRoom = LogicSetupRoom.instance.SelectedRoomObject(idObjectToAdd);
        for (int i = 0; i < listRoom.Count; i++)
        {
            if (listRoom[i].id == idObjectToAdd)
            {
                listRoom[i].Refresh();
            }
        }

    }

    public void HideWithAnim(int start, int end, float duration)
    {
        base.Hide();

        SaveGame.CanShow = true;
        ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = true;
        PopupDecor.ShowWithAnim(start, end, duration);
    }

    public void Initialized(int idRoom, int idObj, bool isRoomWatchAds)
    {
        ManagerPopup.HidePopup<PopupDecor>();
        idObjectToAdd = idObj;
        isWatchAds = isRoomWatchAds;
        ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = false;

        btnSelectRoom1.Init(idRoom, idObj, 0);
        btnSelectRoom2.Init(idRoom, idObj, 1);
        btnSelectRoom3.Init(idRoom, idObj, 2);
        btnSelectRoom4.Init(idRoom, idObj, 3);
        base.Init();
    }

    public void SelectRoomIndex(object e)
    {
        currentSprite = (int)e;
        Debug.Log(currentSprite + idObjectToAdd * GameConfig.ROW_COUNT + 1 + " index");

        if (currentSprite == 3 && !isWatchAds)
        {
            imgAds.SetActive(true);
        }
        else imgAds.SetActive(false);

        List<RoomObject> listRoom = LogicSetupRoom.instance.SelectedRoomObject(idObjectToAdd);
        foreach (RoomObject roomObject in listRoom)
        {
            roomObject.SetUpMaterial(currentSprite + idObjectToAdd * GameConfig.ROW_COUNT);
        }
    }

    void AddNewRoom()
    {
        ListObjetRoomPainted listObjectRoom = new ListObjetRoomPainted();
        ObjectRoomPainted objectRoom = new ObjectRoomPainted();
        List<ObjectRoomPainted> listObjectPaintedCache = new List<ObjectRoomPainted>();

        objectRoom.idObject = idObjectToAdd;
        objectRoom.isPainted = true;
        objectRoom.currentSprite = currentSprite;
        listObjectPaintedCache.Add(objectRoom);
        listObjectRoom.listObjectPainted = listObjectPaintedCache;

        //Todo Set data

        ListRoomPainted dataCache = SaveGame.ListRoomPainted;
        dataCache.listRoomPainted.Add(listObjectRoom);
        SaveGame.ListRoomPainted = dataCache;
        Debug.Log("Save Data: " + idObjectToAdd);
    }

    void AddRoomWatchedAds()
    {
        ListObjetRoomPainted listObjectRoom = new ListObjetRoomPainted();
        ObjectRoomPainted objectRoom = new ObjectRoomPainted();
        List<ObjectRoomPainted> listObjectPaintedCache = new List<ObjectRoomPainted>();

        objectRoom.idObject = idObjectToAdd;
        objectRoom.isWatchAds = true;

        listObjectPaintedCache.Add(objectRoom);
        listObjectRoom.listObjectPainted = listObjectPaintedCache;
        ListRoomPainted dataCache = SaveGame.ListRoomPainted;
        dataCache.listRoomPainted.Add(listObjectRoom);
        SaveGame.ListRoomPainted = dataCache;
        Debug.Log("Save Is Watch Ads: " + idObjectToAdd);

    }

    void UpdateExistedRoom()
    {
        ListRoomPainted dataCache = SaveGame.ListRoomPainted;

        for (int i = 0; i < dataCache.listRoomPainted.Count; i++)
        {
            int idRoom = dataCache.listRoomPainted[i].idRoom;

            if (SaveGame.CurrentRoom == idRoom)
            {

                for (int j = 0; j < dataCache.listRoomPainted[i].listObjectPainted.Count; j++)
                {
                    int idObject = dataCache.listRoomPainted[i].listObjectPainted[j].idObject;

                    if (idObjectToAdd == idObject)
                    {
                        dataCache.listRoomPainted[i].listObjectPainted[j].currentSprite = currentSprite;
                        Debug.Log("wtf update????");
                    }
                }
            }
        }

        SaveGame.ListRoomPainted = dataCache;
    }


    //void LoadExistedData()
    //{
    //    for (int i = 0; i < SaveGame.ListRoomPainted.listRoomPainted.Count; i++)
    //    {
    //        int idRoom = SaveGame.ListRoomPainted.listRoomPainted[i].idRoom;
    //        if (SaveGame.CurrentRoom == idRoom)
    //        {
    //            for (int j = 0; j < SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted.Count; j++)
    //            {
    //                int idObject = SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].idObject;
    //                int currentSprite = SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].currentSprite;
    //                if (idObjectToAdd == idObject)
    //                {
    //                    LogicSetupRoom.instance.listRoomObject[idObjectToAdd].SetUpMaterial(currentSprite + GameConfig.ROW_COUNT * idObject);
    //                }
    //            }
    //        }
    //    }
    //}

}
