using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// by nt.Dev93
namespace ntDev
{
    internal struct Toast
    {
        internal string mess;
        internal float time;
        internal Toast(string m, float t)
        {
            mess = m;
            time = t;
        }
    }
    public class ManagerToast : MonoBehaviour
    {
        public static ManagerToast Instance;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        [SerializeField] CanvasGroup canvas;
        [SerializeField] HorizontalLayoutGroup layoutGroup;
        [SerializeField] LayoutElement layoutElement;
        [SerializeField] TextMeshProUGUI txtContent;

        List<Toast> listToast = new List<Toast>();

        public static void Show(string mess, float time = 3)
        {
            Instance.AddToast(mess, time);
        }

        protected void AddToast(string mess, float time)
        {
            if (listToast.Count > 0 && listToast[listToast.Count - 1].mess == mess) return;
            listToast.Add(new Toast(mess, time));
            if (listToast.Count == 1)
            {
                canvas.alpha = 0;
                StartCoroutine(ShowToast());
            }
        }

        public float maxWidth = 800;
        protected IEnumerator ShowToast()
        {
            while (listToast.Count > 0)
            {
                txtContent.text = listToast[0].mess;
                layoutElement.preferredWidth = -1;
                layoutGroup.childControlWidth = true;
                yield return null;
                if (txtContent.rectTransform.sizeDelta.x > maxWidth)
                {
                    layoutElement.preferredWidth = maxWidth;
                    layoutGroup.childControlWidth = true;
                }
                for (float i = 0; i <= 1; i += 0.03f)
                    yield return canvas.alpha = i;
                canvas.alpha = 1;
                yield return new WaitForSeconds(listToast[0].time);
                for (float i = 1; i >= 0; i -= 0.03f)
                    yield return canvas.alpha = i;
                canvas.alpha = 0;
                listToast.RemoveAt(0);
            }
        }
    }
}