using DG.Tweening;
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
            transform.localScale = Vector3.one;

            transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                LogicGame.Instance.isPauseGame = false;
                gameObject.SetActive(false);
                ManagerEvent.RaiseEvent(EventCMD.EVENT_POPUP_CLOSE, this);
            });
            //ManagerEvent.RaiseEvent(EventCMD.EVENT_SPAWN_PLATE);
        }

        IEnumerator ReturnGame()
        {
            yield return new WaitForSeconds(0.25f);
            LogicGame.Instance.isPauseGame = false;
        }
    }
}
