using System;
using System.Collections.Generic;

// by nt.Dev93
namespace ntDev
{
    [Serializable]
    public class BalanceListRatio
    {
        public List<BalanceRatio> ListRatio;

        public void Init(float[] arr, bool bigFirst = false)
        {
            List<float> list = new List<float>();
            list.AddRange(arr);
            Init(list, bigFirst);
        }

        public void Init(List<float> list, bool bigFirst = false)
        {
            ListRatio.Clear();
            for (int i = 0; i < list.Count; ++i)
            {
                BalanceRatio balanceRatio = new BalanceRatio(list[i]);
                ListRatio.Add(balanceRatio);
            }

            for (int i = 0; i < ListRatio.Count - 1; ++i)
            {
                for (int j = i + 1; j < ListRatio.Count; ++j)
                {
                    if (bigFirst ? (ListRatio[j].chance > ListRatio[i].chance) : (ListRatio[j].chance < ListRatio[i].chance))
                    {
                        var tg = ListRatio[i];
                        ListRatio[i] = ListRatio[j];
                        ListRatio[j] = tg;
                    }
                }
            }
        }

        public int Calculate()
        {
            for (int i = 0; i < ListRatio.Count - 1; ++i)
                if (ListRatio[i].Calculate())
                    return i;
            return ListRatio.Count - 1;
        }
    }
}
