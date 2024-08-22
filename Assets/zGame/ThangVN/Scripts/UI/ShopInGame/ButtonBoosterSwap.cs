using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThangVN
{
    public class ButtonBoosterSwap : ButtonBooster
    {
        public override void Init()
        {
            numCount = SaveGame.Swap;
            indexLevelUnlock = GameConfig.LEVEL_SWAP;
            base.Init();
        }

        public override void Update()
        {
            numCount = SaveGame.Swap;

            base.Update();
        }
    }
}
