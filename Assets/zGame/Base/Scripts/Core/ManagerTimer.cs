using System;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    [Serializable]
    public class ManagerTimer
    {
        [SerializeField] public List<EasyTimer> listTimer;
        public bool standAlone = false;
        public ManagerTimer(bool b = false)
        {
            standAlone = b;
            listTimer = new List<EasyTimer>(2);
        }

        ~ManagerTimer()
        {
            ManagerGame.RemoveTimer(this);
        }

        public EasyTimer Get(string key)
        {
            ManagerGame.AddTimer(this);
            for (int i = listTimer.Count - 1; i >= 0; --i)
                if (key == listTimer[i].Key) return listTimer[i];

            EasyTimer newTimer = new EasyTimer(key);
            listTimer.Add(newTimer);
            return newTimer;
        }

        public void RemoveTimer(string key = "")
        {
            for (int i = listTimer.Count - 1; i >= 0; --i)
                if (key == "" || key == listTimer[i].Key)
                {
                    listTimer.Remove(listTimer[i]);
                    if (key != "") return;
                }
        }

        public EasyTimer Set(string key, float time, Action actDone = null, Action<float, float> actEachTime = null, float eachTime = -1)
        {
            var easyTimer = Get(key);
            easyTimer.Set(time, actDone, actEachTime, eachTime);
            return easyTimer;
        }

        public void AddStock(string key, int n = 1)
        {
            for (int i = listTimer.Count - 1; i >= 0; --i)
                if (key == listTimer[i].Key)
                {
                    listTimer[i].AddStock(n);
                    return;
                }
        }

        public void Stop(string key = "")
        {
            for (int i = listTimer.Count - 1; i >= 0; --i)
                if (key == "" || key == listTimer[i].Key)
                {
                    listTimer[i].Stop();
                    if (key != "") return;
                }
        }

        public void Pause(string key = "")
        {
            for (int i = listTimer.Count - 1; i >= 0; --i)
                if (key == "" || key == listTimer[i].Key)
                {
                    listTimer[i].Pause();
                    if (key != "") return;
                }
        }

        public void Resume(string key = "")
        {
            for (int i = listTimer.Count - 1; i >= 0; --i)
                if (key == "" || key == listTimer[i].Key)
                {
                    listTimer[i].Resume();
                    if (key != "") return;
                }
        }

        public void Update()
        {
            for (int i = listTimer.Count - 1; i >= 0; --i)
                listTimer[i].Update(standAlone);
        }

        public void Pass(long last)
        {
            for (int i = listTimer.Count - 1; i >= 0; --i)
                listTimer[i].Cumback(last);
        }
    }
}