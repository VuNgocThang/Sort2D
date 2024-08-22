using System;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    [Serializable]
    public class MODableData
    {
        public string key;
        public float value;
        public bool factor;
    }

    [Serializable]
    public class MODableValue
    {
        [SerializeField] float baseValue = 0;
        public float BaseValue
        {
            get => baseValue;
            set => baseValue = value;
        }
        public float Value => GetValue();
        [SerializeField] float CurrentValue;
        [SerializeField] List<MODableData> listAffected = new List<MODableData>();
        [SerializeField] ManagerTimer managerTimer = new ManagerTimer();
        public MODableValue(float baseValue)
        {
            this.baseValue = baseValue;
        }

        public void SetValue(string k, float v, float d = -1, bool f = true)
        {
            string strKEY = k + v + (f ? 1 : 0);
            if (d != -1) managerTimer.Set(strKEY, d, () => RemoveValue(strKEY));
            listAffected.Add(new MODableData { key = k, value = v, factor = f });
            GetValue();
        }

        public void RemoveValue(string k = "")
        {
            for (int c = listAffected.Count, i = c - 1; i >= 0; --i)
            {
                string strKEY = listAffected[i].key + listAffected[i].value + (listAffected[i].factor ? 1 : 0);
                if (k == "" || strKEY == k)
                    listAffected.RemoveAt(i);
            }
            GetValue();
        }

        float GetValue()
        {
            float a = 1;
            float d = 1;
            float t = 0;

            List<MODableData> list = new List<MODableData>();
            foreach (MODableData dataA in listAffected)
            {
                bool check = true;
                foreach (MODableData data in list)
                {
                    if (dataA.key == data.key && dataA.factor == data.factor)
                    {
                        check = false;
                        if (dataA.value > data.value) data.value = dataA.value;
                    }
                }
                if (check) list.Add(new MODableData { key = dataA.key, value = dataA.value, factor = dataA.factor });
            }

            foreach (MODableData data in list)
            {
                if (data.factor)
                {
                    if (data.value >= 0) d += data.value;
                    else a *= 1 - Math.Abs(data.value);
                }
                else t += data.value;
            }
            CurrentValue = (baseValue + t) * d * a;
            return CurrentValue;
        }
    }
}