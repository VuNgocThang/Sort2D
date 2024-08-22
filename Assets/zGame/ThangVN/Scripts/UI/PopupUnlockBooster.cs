using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThangVN
{
    public class PopupUnlockBooster : Popup
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI txtName, txtExplain;
        [SerializeField] BoosterData boosterData;

        public static async void Show(int index)
        {
            PopupUnlockBooster pop = await ManagerPopup.ShowPopup<PopupUnlockBooster>();

            pop.Initialized(index);
        }

        public void Initialized(int index)
        {
            base.Init();
            LogicGame.Instance.isPauseGame = true;
            Debug.Log("Show" + index);

            for (int i = 0; i < boosterData.listBooster.Count; i++)
            {
                if (index == (int)boosterData.listBooster[i].boosterEnum)
                {
                    icon.sprite = boosterData.listBooster[i].spriteIcon;
                    txtName.text = boosterData.listBooster[i].nameBooster;
                    txtExplain.text = boosterData.listBooster[i].textExplain;
                }
            }
        }

        public override void Hide()
        {
            base.Hide();
            LogicGame.Instance.isPauseGame = false;
            //ManagerEvent.RaiseEvent(EventCMD.EVENT_SPAWN_PLATE);
        }
    }
}
