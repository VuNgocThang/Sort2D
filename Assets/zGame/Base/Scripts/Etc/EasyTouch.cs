using System;
using UnityEngine;
using UnityEngine.EventSystems;

// by nt.Dev93
namespace ntDev
{
    public class EasyTouch : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] bool IsController;
        [SerializeField] RectTransform rectBG;
        [SerializeField] Transform transControl;

        bool IsTouch = false;

        public Action OnEventTouch;
        public Action OnEventDrag;
        public Action<Vector3> OnEventDragV;
        public Action<Vector3> OnEventDragD;
        public Action<float> OnEventRelease;

        Vector3 defaultPos;
        void Start()
        {
            if (IsController && rectBG != null)
                defaultPos = rectBG.localPosition;
        }

        Vector3 startPos;
        Vector3 bgPos;
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            IsTouch = true;
            startPos = eventData.position;
            currentPos = startPos;
            lastPos = currentPos;
            OnEventTouch?.Invoke();
            timer = 0;

            if (IsController)
            {
                bgPos = startPos;
                bgPos.x /= transform.lossyScale.x;
                bgPos.y /= transform.lossyScale.y;
                if (rectBG != null) rectBG.localPosition = bgPos;
                if (transControl != null) transControl.localPosition = Vector3.zero;
            }
        }
        Vector3 currentPos;
        Vector3 lastPos;
        Vector3 V;
        Vector3 D;
        public virtual void OnDrag(PointerEventData eventData)
        {
            if (IsTouch)
            {
                currentPos = eventData.position;
                V = currentPos - startPos;
                D = currentPos - lastPos;
                OnEventDragV?.Invoke(V);
                OnEventDragD?.Invoke(D);
                OnEventDrag?.Invoke();
                lastPos = currentPos;

                if (IsController && transControl != null)
                {
                    transControl.localPosition = V.normalized * rectBG.sizeDelta.x / 2;
                    transControl.eulerAngles = new Vector3(0, 0, Mathf.Atan2(transControl.localPosition.y, transControl.localPosition.x) * Mathf.Rad2Deg);
                }
            }
        }
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            OnEventRelease?.Invoke(timer);
            timer = -1;
            if (rectBG != null) rectBG.localPosition = defaultPos;
            if (transControl != null) transControl.localPosition = Vector3.zero;
            IsTouch = false;
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (IsTouch)
            {
                OnEventRelease?.Invoke(timer);
                timer = -1;
            }
            // if (rectBG != null) rectBG.localPosition = defaultPos;
            // if (transControl != null) transControl.localPosition = Vector3.zero;
            // IsTouch = false;
        }

        float timer = -1;
        void Update()
        {
            if (timer >= 0) timer += Ez.TimeMod;
        }
    }
}
