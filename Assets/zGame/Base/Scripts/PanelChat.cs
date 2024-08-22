
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ntDev
{
    public class PanelChat : MonoBehaviour
    {
        [SerializeField] EasyButton btnShow;
        [SerializeField] EasyButton btnHide;
        [SerializeField] GameObject offChat;
        [SerializeField] TextMeshProUGUI txtCurrent;
        [SerializeField] GameObject onChat;
        [SerializeField] List<TextMeshProUGUI> listChat;
        [SerializeField] TMP_InputField inChat;
        [SerializeField] EasyButton btnSend;
        [SerializeField] RectTransform rectTransform;

        void Awake()
        {
            //ManagerEvent.RegEvent(EventCMD.EVENT_PUBLIC_MESSAGE, OnPublicMessage);

            btnShow.OnClick(OnClickShow);
            btnSend.OnClick(OnClickSend);
            btnHide.OnClick(OnClickHide);

            listChat.Refresh();

            OnClickHide();
            txtCurrent.text = "System: Welcome to The Oasis!";
        }

        //void OnPublicMessage(object o)
        //{
        //    ChatDataResponse chatData = o as ChatDataResponse;
        //    if (chatData.UserID == GlobalData.MyUserInfo.id)
        //        listMyChatSlot.GetClone().Init(chatData);
        //    else listChatSlot.GetClone().Init(chatData);

        //    StartCoroutine(Rebuild());

        //    while (listChatSlot.Count > 50)
        //        listChatSlot.RemoveAt(0);

        //    foreach (PlayerInfo info in GlobalData.ListPlayerInfo)
        //        if (info.UserID == chatData.UserID)
        //        {
        //            txtMsg.text = info.Username + ": " + chatData.Msg.ToStringUtf8();
        //            return;
        //        }
        //}

        void OnClickShow()
        {
            btnShow.gameObject.SetActive(false);
            btnHide.gameObject.SetActive(true);
            onChat.SetActive(true);
            offChat.SetActive(false);
            inChat.text = "";

            StartCoroutine(Rebuild());
        }

        IEnumerator Rebuild()
        {
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        void OnClickSend()
        {
            string msg = inChat.text.Trim();
            if (msg == "") return;

            //CityConnection.Instance.SendChat(inputTextChat.text.Trim());

            listChat.GetClone().text = "Me: " + msg;
            txtCurrent.text = "Me: " + msg;
            inChat.text = "";
        }

        void OnClickHide()
        {
            onChat.SetActive(false);
            offChat.SetActive(true);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                OnClickSend();
        }
    }
}
