using System;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    [Serializable]
    public class EasyTimer
    {
        public float factor;
        [SerializeField] string key;
        public string Key => key;

        [SerializeField] double timerTotal = -1;
        public float Remain => (float)timerTotal;
        [SerializeField] double totalTime = -1;
        public double TotalTime
        {
            get => totalTime;
            set => totalTime = value;
        }

        [SerializeField] int stock = 0;
        public int Stock
        {
            get => stock;
            set => stock = value;
        }
        [SerializeField] bool pause = true;

        float timerEach = -1;
        float eachTime = -1;

        [HideInInspector] Action<float, float> actEachTime;
        [HideInInspector] Action actDone;

        public string Text => ((int)timerTotal + 1).ConvertTime(true);
        public bool IsRunning => !pause;

        public EasyTimer(string key, int factor = 1)
        {
            this.key = key;
            this.factor = factor;
        }

        public void Set(double time, Action actDone = null, Action<float, float> actEachTime = null, float eachTime = -1)
        {
            this.totalTime = time;
            this.timerTotal = time;
            this.actDone = actDone;
            this.actEachTime = actEachTime;
            this.eachTime = eachTime;
            if (this.eachTime == -1 && actEachTime != null) this.eachTime = 0;
            this.timerEach = 0;
            stock = 1;
            pause = false;
        }

        public void SetDone(Action actDone) => this.actDone = actDone;
        public void SetEachTime(Action<float, float> actEachTime, float eachTime)
        {
            this.actEachTime = actEachTime;
            this.eachTime = eachTime;
        }

        public void AddStock(int n = 1, bool resume = true)
        {
            stock += n;
            if (timerTotal <= 0) timerTotal = totalTime;
            pause = !resume;
        }

        public void Stop()
        {
            timerTotal = -1;
            stock = 0;
            pause = true;
        }

        public void Pause() => pause = true;
        public void Resume() => pause = false;

        public void Update(bool b)
        {
            if (pause) return;
            float d = b ? Time.deltaTime : Ez.TimeMod;
            if (actEachTime != null && eachTime >= 0)
            {
                timerEach += d * factor;
                if (timerEach > eachTime)
                {
                    actEachTime?.Invoke(Remain, (float)TotalTime);
                    timerEach = 0;
                }
            }
            if (timerTotal > 0)
            {
                timerTotal -= d * factor;
                if (timerTotal <= 0) timerTotal = 0;
            }
            else if (timerTotal == 0)
            {
                timerTotal = -1;
                --stock;
                if (stock > 0) timerTotal = totalTime;
                else pause = true;
                actDone?.Invoke();
            }
            else pause = true;
        }

        public void Cumback(long last)
        {
            if (stock == 0) return;
            Pass(last);
        }

        void Pass(long s)
        {
            if (s <= 0) return;
            if (stock < 1)
            {
                pause = true;
                return;
            }
            if (timerTotal > s) timerTotal -= s;
            else
            {
                s = s - (long)timerTotal;
                --stock;
                timerTotal = totalTime;
                actDone?.Invoke();
                Pass(s);
            }
        }
    }
}