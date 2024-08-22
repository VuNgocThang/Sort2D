using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThangVN
{
    public class ButtonBoosterRefresh : ButtonBooster
    {
        public override void Init()
        {
            indexLevelUnlock = GameConfig.LEVEL_REFRESH;
            numCount = SaveGame.Refresh;
            base.Init();
        }

        public override void Update()
        {
            numCount = SaveGame.Refresh;

            base.Update();
        }
    }
}
