using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThangVN
{
    public class ButtonBoosterHammer : ButtonBooster
    {
        public override void Init()
        {
            numCount = SaveGame.Hammer;
            indexLevelUnlock = GameConfig.LEVEL_HAMMER;
            base.Init();
        }

        public override void Update()
        {
            numCount = SaveGame.Hammer;
            base.Update();
        }
    }
}
