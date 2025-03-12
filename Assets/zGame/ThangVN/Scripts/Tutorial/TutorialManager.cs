using ntDev;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                Debug.Log("sdadad");
                LogicGame.Instance.InitTutorial();
            }
            else
            {
                //ManagerEvent.RaiseEvent(EventCMD.EVENT_SPAWN_PLATE);

                int indexBooster = FindBoosterEnum();
                bool canShowGift = GameConfig.CanShowGift;
                bool hasBooster = indexBooster != -1;

                LogicGame.Instance.isPauseGame = hasBooster && canShowGift;

                if (hasBooster && canShowGift)
                {
                    Debug.Log(SaveGame.Level + " ___ " + SaveGame.LevelGift);
                    SaveGame.LevelGift = SaveGame.Level + 2;
                    PopupGift.Show(indexBooster);
                }
                else
                {
                    GameManager.ShowInterAds("Level");
                }
            }
        }

        private static int FindBoosterEnum()
        {
            int intResult = -1;

            Dictionary<int, int> counts = new Dictionary<int, int>
            {
                { 0, SaveGame.Hammer },
                { 1, SaveGame.Swap },
                { 2, SaveGame.Refresh }
            };

            List<int> listCanBeResults =
                counts.Where(booster => booster.Value <= 1).Select(booster => booster.Key).ToList();

            if (listCanBeResults.Count == 1)
            {
                intResult = listCanBeResults[0];
            }
            else if (listCanBeResults.Count > 1)
            {
                intResult = listCanBeResults[Random.Range(0, listCanBeResults.Count)];
            }
            else if (listCanBeResults.Count == 0)
            {
                intResult = -1;
            }

            return intResult;
        }
    }
}