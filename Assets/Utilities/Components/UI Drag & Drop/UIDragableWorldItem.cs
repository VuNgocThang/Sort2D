﻿

using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.Common;

namespace Utilities.Components
{
    /// <summary>
    /// Drag an item from UI-Canvas to world
    /// </summary>
    public class UIDragableWorldItem : UIDragableItem
    {
        public GameObject worldObjPrefab;
        protected GameObject mWorldObj;

        protected override void Awake()
        {
            base.Awake();

            GetDragHandler().dontShowItem = true;
        }

        public override void BeginDrag(PointerEventData eventData)
        {
            base.BeginDrag(eventData);

            GetWorldItem().SetActive(true);
            GetWorldItem().transform.position = renderCamera.MousePointToWorldPoint();
        }

        public override void Drag(PointerEventData eventData)
        {
            base.Drag(eventData);

            GetWorldItem().transform.position = renderCamera.MousePointToWorldPoint();
        }

        public override void EndDrag(PointerEventData eventData)
        {
            base.EndDrag(eventData);

            GetWorldItem().SetActive(false);
            GetWorldItem().transform.position = renderCamera.MousePointToWorldPoint();
        }

        protected GameObject GetWorldItem()
        {
            if (mWorldObj == null)
                mWorldObj = Instantiate(worldObjPrefab);

            return mWorldObj;
        }
    }
}