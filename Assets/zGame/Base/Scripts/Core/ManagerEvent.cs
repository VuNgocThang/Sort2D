using System;
using System.Collections.Generic;

// by nt.Dev93
namespace ntDev
{
    public class EventObject
    {
        public EventCMD cmd;
        public Action<object> callBack;
        public EventObject(EventCMD c, Action<object> cal)
        {
            cmd = c;
            callBack = cal;
        }
    }

    public class RaiseEventObject
    {
        public EventCMD cmd;
        public object obj;
        public RaiseEventObject(EventCMD c, object o)
        {
            cmd = c;
            obj = o;
        }
    }

    public enum EventCMD
    {
        EVENT_NONE,
        EVENT_TUTORIAL_SHOW,
        EVENT_TUTORIAL_HIDE,
        EVENT_TUTORIAL_DONE,
        EVENT_SPAWN_RATE,
        EVENT_SPEED_UP,
        EVENT_POPUP_SHOW,
        EVENT_POPUP_CLOSE,
        EVENT_SPAWN,
        EVENT_ATTACK,
        EVENT_SKILL,
        EVENT_EFFECT,
        EVENT_FX,
        EVENT_DEATH,

        EVENT_UPDATE_COIN,
        EVENT_UPDATE_GEM,
        EVENT_ROOM_INIT,
        EVENT_ROOM_UNLOCK,
        EVENT_ROOM_UPGRADE,
        EVENT_END,
        EVENT_EVENT_OVER,
        EVENT_BUFF_OVER,
        EVENT_TUTORIAL_COMPLETE,
        EVENT_TUTORIAL_LOCK_ROOM,
        
        EVENT_SPAWN_EFFECT,
        EVENT_SPAWN_POPUP,
        EVENT_SPAWN_CLOCK,
        EVENT_BUFF_SR_OVER,
        EVENT_BUFF_LS_CD_OVER,
        
        EVENT_REFRESH_ROOM_LIST,
        EVENT_TOGGLE_MAIN_UI,
        EVENT_SPAWN_PLAYER,
        EVENT_ROOM_DONE,

        EVENT_POINT,
        EVENT_COUNT,
        EVENT_WIN,
        EVENT_LOSE,
        EVENT_SWITCH,
        EVENT_FREECOIN,
        EVENT_CHALLENGES,
        EVENT_SELECT_ROOM,
        EVENT_SPAWN_PLATE
    }

    public static class ManagerEvent
    {
        static List<EventObject> listEvent = new List<EventObject>();
        public static void RegEvent(EventCMD cmd, Action<object> cal)
        {
            if (cal != null)
            {
                foreach (EventObject o in listEvent)
                    if (o.cmd == cmd && o.callBack == cal) return;
                listEvent.Add(new EventObject(cmd, cal));
            }
        }
        public static void RaiseEvent(EventCMD cmd, object obj = null)
        {
            for (int i = 0; i < listEvent.Count; ++i)
            {
                if (listEvent[i].cmd == cmd)
                    listEvent[i].callBack(obj);
            }
        }
        public static void RaiseEventNextFrame(EventCMD cmd, object obj = null)
        {
            ManagerGame.ListEvent.Add(new RaiseEventObject(cmd, obj));
        }
        public static void RemoveEvent(EventCMD cmd, Action<object> cal = null)
        {
            for (int i = 0; i < listEvent.Count; ++i)
            {
                if (listEvent[i].cmd == cmd)
                    if (cal == null || listEvent[i].callBack == cal)
                        listEvent.RemoveAt(i);
            }
        }
        public static void ClearEvent()
        {
            listEvent.Clear();
        }
    }
}