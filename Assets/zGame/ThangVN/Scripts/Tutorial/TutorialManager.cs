using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThangVN
{
    public class TutorialManager
    {
        //public static TutorialManager Instance;

        //private void Awake()
        //{
        //    Instance = this;
        //}

        public static void ShowPopup(int indexCurrentLevel)
        {
            if (indexCurrentLevel == GameConfig.LEVEL_LOCK_COIN && !SaveGame.IsDoneTutLockCoin)
            {
                TutorialCamera.Instance.InitTutorialLockCoin();
            }
            else if (indexCurrentLevel == GameConfig.LEVEL_FROZEN && !SaveGame.IsDoneTutFrozen)
            {
                TutorialCamera.Instance.InitTutorialFrozen();

            }
            else if (indexCurrentLevel == GameConfig.LEVEL_REFRESH && !SaveGame.IsShowRefresh)
            {
                SaveGame.IsShowRefresh = true;
                PopupUnlockBooster.Show((int)BoosterEnum.BoosterRefresh);
            }
            else if (indexCurrentLevel == GameConfig.LEVEL_SWAP && !SaveGame.IsShowSwap)
            {
                SaveGame.IsShowSwap = true;
                PopupUnlockBooster.Show((int)BoosterEnum.BoosterSwap);
            }
            else if (indexCurrentLevel == GameConfig.LEVEL_HAMMER && !SaveGame.IsShowHammer)
            {
                SaveGame.IsShowHammer = true;

                PopupUnlockBooster.Show((int)BoosterEnum.BoosterHammer);
            }
            else if (indexCurrentLevel == GameConfig.LEVEL_YELLOW && !SaveGame.IsShowBook)
            {
                SaveGame.IsShowBook = true;

                PopupUnlockColor.Show((int)NewColorEnum.ColorYellow);
            }
            else if (indexCurrentLevel == GameConfig.LEVEL_PURPLE && !SaveGame.IsShowBook)
            {
                SaveGame.IsShowBook = true;

                PopupUnlockColor.Show((int)NewColorEnum.ColorPurple);
            }
            else if (indexCurrentLevel == GameConfig.LEVEL_PINK && !SaveGame.IsShowBook)
            {
                SaveGame.IsShowBook = true;

                PopupUnlockColor.Show((int)NewColorEnum.ColorPink);
            }
            else if (indexCurrentLevel == GameConfig.LEVEL_RANDOM && !SaveGame.IsShowBook)
            {
                SaveGame.IsShowBook = true;

                PopupUnlockColor.Show((int)NewColorEnum.ColorRandom);
            }
            else if (indexCurrentLevel == GameConfig.LEVEL_ORANGE && !SaveGame.IsShowBook)
            {
                SaveGame.IsShowBook = true;

                PopupUnlockColor.Show((int)NewColorEnum.ColorOrange);
            }
            else if (indexCurrentLevel == 0 && !SaveGame.IsDoneTutorial)
            {
                LogicGame.Instance.isPauseGame = false;
                //ManagerEvent.RaiseEvent(EventCMD.EVENT_SPAWN_PLATE);
                LogicGame.Instance.InitTutorial();
            }
            else
            {
                LogicGame.Instance.isPauseGame = false;
                //ManagerEvent.RaiseEvent(EventCMD.EVENT_SPAWN_PLATE);
            }
        }

    }
}
