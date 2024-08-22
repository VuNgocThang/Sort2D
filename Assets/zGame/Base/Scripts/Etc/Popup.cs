using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

// by nt.Dev93
namespace ntDev
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] public EasyButton[] btnClose;

        bool init = false;
        public virtual void Init()
        {
            if (!init)
            {
                init = true;
                foreach (EasyButton btn in btnClose)
                {
                    btn.OnClick(Hide);
                }
            }

            gameObject.SetActive(true);

            //CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            //canvasGroup.alpha = 0;
            //canvasGroup.DOFade(1, 0.3f);

            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }

        public virtual void Hide()
        {
            //CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            //canvasGroup.alpha = 1;
            //canvasGroup.DOFade(0, 0.3f);
            transform.localScale = Vector3.one;

            transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
                ManagerEvent.RaiseEvent(EventCMD.EVENT_POPUP_CLOSE, this);
            });
        }
    }
}