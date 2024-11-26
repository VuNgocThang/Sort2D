using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicBonusLevel
{
    public bool ShowLevelBonus()
    {
        return (SaveGame.Level % 5 == 0);
    }


}
