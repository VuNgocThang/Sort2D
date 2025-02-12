using BaseGame;
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
        [SerializeField] int index;
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI txtName, txtExplain;
        [SerializeField] BoosterData boosterData;
        [SerializeField] EasyButton btnClosePopup;

        private void Awake()
        {
            btnClosePopup.OnClick(() =>
            {
                ShowTutorial(index);
                Hide();
            });
        }

        public static async void Show(int index)
        {
            PopupUnlockBooster pop = await ManagerPopup.ShowPopup<PopupUnlockBooster>();

            pop.Initialized(index);
        }

        public void Initialized(int index)
        {
            base.Init();
            this.index = index;
            LogicGame.Instance.isPauseGame = true;
            ManagerAudio.PlaySound(ManagerAudio.Data.soundUnlockBooster);

            Debug.Log("Show" + index);

            for (int i = 0; i < boosterData.listBooster.Count; i++)
            {
                if (index == (int)boosterData.listBooster[i].boosterEnum)
                {
                    icon.sprite = boosterData.listBooster[i].spriteIcon;
                    icon.SetNativeSize();
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
                //LogicGame.Instance.isPauseGame = false;
                gameObject.SetActive(false);
                ManagerEvent.RaiseEvent(EventCMD.EVENT_POPUP_CLOSE, this);
            });
            //ManagerEvent.RaiseEvent(EventCMD.EVENT_SPAWN_PLATE);
        }

        void ShowTutorial(int index)
        {
            switch (index)
            {
                case 0:
                    TutorialCamera.Instance.InitTutorialBoosterHammer();
                    break;

                case 1:
                    TutorialCamera.Instance.InitTutorialBoosterSwap();
                    break;

                case 2:
                    TutorialCamera.Instance.InitTutorialBoosterRefresh();
                    break;
                default: break;
            }
        }

        IEnumerator ReturnGame()
        {
            yield return new WaitForSeconds(0.25f);
            LogicGame.Instance.isPauseGame = false;
        }
    }
}