using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupGallery : Popup
{
    [SerializeField] EasyButton btnPlusColorPlate;
    [SerializeField] Transform nContent;
    [SerializeField] Scrollbar scrollBar;
    [SerializeField] List<Room> listRoom;
    [SerializeField] Room roomPrefab;
    float scroll_pos = 0;
    float[] pos;
    [SerializeField] int index;
    bool isDrag;
    [SerializeField] int countRoom;

    public static async void Show()
    {
        PopupGallery pop = await ManagerPopup.ShowPopup<PopupGallery>();
        pop.Init();
    }

    public override void Init()
    {
        ManagerPopup.HidePopup<PopupDecor>();
        base.Init();
        btnPlusColorPlate.OnClick(() => PopupGoToLevel.Show());
        countRoom = 1;
        for (int i = 0; i < listRoom.Count; i++)
        {
            listRoom[i].gameObject.SetActive(false);
        }
        listRoom.Clear();

        for (int i = 0; i < countRoom; i++)
        {
            Room room = Instantiate(roomPrefab, nContent);
            room.index = i;

            //string spritePath = $"Image/room{i}";
            //Sprite roomSprite = Resources.Load<Sprite>(spritePath);

            //if (roomSprite != null)
            //{
            //    room.img.sprite = roomSprite;
            //}
            //else
            //{
            //    Debug.LogWarning($"Sprite not found at path: {spritePath}");
            //}
            ScreenshotManager.Instance.LoadScreenshot(room.img, i);

            listRoom.Add(room);
        }

        pos = new float[listRoom.Count];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        scrollBar.value = pos[index];
    }

    public override void Hide()
    {
        base.Hide();
        PopupDecor.Show();
    }

    private void Update()
    {
        pos = new float[listRoom.Count];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollBar.value;
            for (int i = 0; i < pos.Length; ++i)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    index = i;
                }
            }
            isDrag = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;

            for (int i = 0; i < pos.Length; ++i)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    index = i;
                }
            }
        }

        if (!isDrag) scrollBar.value = Mathf.Lerp(scrollBar.value, pos[index], 0.1f);

        for (int i = 0; i < pos.Length; i++)
        {
            if (index == i) listRoom[i].transform.localScale = Vector2.Lerp(listRoom[i].transform.localScale, new Vector2(1f, 1f), 0.1f);
            else listRoom[i].transform.localScale = Vector2.Lerp(listRoom[i].transform.localScale, new Vector2(0.85f, 0.85f), 0.1f);
        }

    }
}
