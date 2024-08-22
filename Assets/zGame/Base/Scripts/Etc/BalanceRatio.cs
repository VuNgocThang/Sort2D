using System;

// by nt.Dev93
namespace ntDev
{
    [Serializable]
    public class BalanceRatio
    {
        public float chance;
        public int win;
        public int lose;
        public int total;
        public float sureWin;

        public BalanceRatio(float chance)
        {
            lose = 0;
            total = 0;
            this.chance = chance;
            if (chance == 0) sureWin = -1;
            else sureWin = 1 / chance;
        }
        public bool Calculate()
        {
            if (sureWin == -1) return false;
            if (total > 0 && win / total > chance)
            {
                Lose();
                return false;
            }
            else if (lose >= sureWin)
            {
                Win();
                ++total;
                return true;
            }
            else
            {
                bool b = Ez.Random(0, 1f) < chance;
                if (b) Win();
                else Lose();
                return b;
            }
        }
        void Win()
        {
            ++total;
            ++win;
            lose = 0;
        }
        void Lose()
        {
            ++total;
            ++lose;
        }
    }
}
