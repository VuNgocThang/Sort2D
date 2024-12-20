﻿

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Common
{
    public enum AnchorType
    {
        TopLeft,
        Top,
        TopRight,
        Right,
        BotRight,
        Bot,
        BotLeft,
        Left,
        Center,
        Stretch
    }

    public static class TransformExtension
    {
        public static Vector3 AddX(this Vector3 pVector, float pValue)
        {
            pVector.x += pValue;
            return pVector;
        }

        public static Vector3 AddY(this Vector3 pVector, float pValue)
        {
            pVector.y += pValue;
            return pVector;
        }

        public static Vector3 AddZ(this Vector3 pVector, float pValue)
        {
            pVector.z += pValue;
            return pVector;
        }

        public static Vector3 SetX(this Vector3 pVector, float pValue)
        {
            pVector.x = pValue;
            return pVector;
        }

        public static Vector3 SetY(this Vector3 pVector, float pValue)
        {
            pVector.y = pValue;
            return pVector;
        }

        public static Vector3 SetZ(this Vector3 pVector, float pValue)
        {
            pVector.z = pValue;
            return pVector;
        }

        // Since transforms return their position as a property,
        // you can't set the x/y/z values directly, so you have to
        // store a temporary Vector3
        // Or you can use these methods instead
        public static Transform SetPosX(this Transform transform, float x)
        {
            var pos = transform.position;
            pos.x = x;
            transform.position = pos;
            return transform;
        }

        public static Transform SetLocalPosX(this Transform transform, float x)
        {
            var pos = transform.localPosition;
            pos.x = x;
            transform.localPosition = pos;
            return transform;
        }

        public static Transform AddPosX(this Transform transform, float x)
        {
            var pos = transform.position;
            pos.x += x;
            transform.position = pos;
            return transform;
        }

        public static Transform SetPosY(this Transform transform, float y)
        {
            var pos = transform.position;
            pos.y = y;
            transform.position = pos;
            return transform;
        }

        public static Transform SetLocalPosY(this Transform transform, float y)
        {
            var pos = transform.localPosition;
            pos.y = y;
            transform.localPosition = pos;
            return transform;
        }

        public static Transform AddPosY(this Transform transform, float y)
        {
            var pos = transform.position;
            pos.y += y;
            transform.position = pos;
            return transform;
        }

        public static Transform SetPosZ(this Transform transform, float z)
        {
            var pos = transform.position;
            pos.z = z;
            transform.position = pos;
            return transform;
        }

        public static Transform SetLocalPosZ(this Transform transform, float z)
        {
            var pos = transform.localPosition;
            pos.z = z;
            transform.localPosition = pos;
            return transform;
        }

        public static Transform AddPosZ(this Transform transform, float z)
        {
            var pos = transform.position;
            pos.z += z;
            transform.position = pos;
            return transform;
        }

        public static Transform SetScaleX(this Transform transform, float x)
        {
            var scale = transform.localScale;
            scale.x = x;
            transform.localScale = scale;
            return transform;
        }

        public static Transform SetScaleY(this Transform transform, float y)
        {
            var scale = transform.localScale;
            scale.y = y;
            transform.localScale = scale;
            return transform;
        }

        public static Transform SetScaleZ(this Transform transform, float z)
        {
            var scale = transform.localScale;
            scale.z = z;
            transform.localScale = scale;
            return transform;
        }

        public static Transform FlipX(this Transform transform)
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            return transform;
        }

        public static Transform FlipX(this Transform transform, int direction)
        {
            if (direction > 0)
                direction = 1;
            else
                direction = -1;

            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;
            return transform;
        }

        public static Transform FlipY(this Transform transform)
        {
            var scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
            return transform;
        }

        public static Transform FlipY(this Transform transform, int direction)
        {
            if (direction > 0)
                direction = 1;
            else
                direction = -1;

            var scale = transform.localScale;
            scale.y = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;
            return transform;
        }

        public static Transform Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
            return transform;
        }

        public static Transform LookAtDirection(this Transform transform, Vector3 pDirection)
        {
            pDirection.y = 0;
            transform.LookAt(transform.position + pDirection);
            return transform;
        }

        public static List<Transform> GetChildren(this Transform transform)
        {
            var children = new List<Transform>();

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                children.Add(child);
            }

            return children;
        }

        /// <summary>
        /// Sort children by conditions
        /// E.g: transform.Sort(t => t.name);
        /// </summary>
        public static void Sort(this Transform transform, Func<Transform, IComparable> sortFunction)
        {
            var children = transform.GetChildren();
            var sortedChildren = children.OrderBy(sortFunction).ToList();

            for (int i = 0; i < sortedChildren.Count(); i++)
                sortedChildren[i].SetSiblingIndex(i);
        }

        public static IEnumerable<Transform> GetAllChildren(this Transform transform)
        {
            var openList = new Queue<Transform>();

            openList.Enqueue(transform);

            while (openList.Any())
            {
                var currentChild = openList.Dequeue();

                yield return currentChild;

                var children = transform.GetChildren();

                int lengthF2 = children.Count;
                for (int k = 0; k < lengthF2; k++)
                {
                    var child = children[k];
                    //foreach (var child in children)
                    //{
                    openList.Enqueue(child);
                }
            }
        }

        #region RectTransfrom

        public static void SetX(this RectTransform transform, float x)
        {
            var pos = transform.anchoredPosition;
            pos.x = x;
            transform.anchoredPosition = pos;
        }

        public static void SetY(this RectTransform transform, float y)
        {
            var pos = transform.anchoredPosition;
            pos.y = y;
            transform.anchoredPosition = pos;
        }

        public static Vector2 TopLeft(this RectTransform rect)
        {
            var pivot = rect.pivot;
            var x = rect.anchoredPosition.x - rect.rect.width * pivot.x;
            var y = rect.anchoredPosition.y + rect.rect.height * (1 - pivot.y);
            return new Vector2(x, y);
        }

        public static Vector2 TopRight(this RectTransform rect)
        {
            var pivot = rect.pivot;
            var x = rect.anchoredPosition.x + rect.rect.width * (1 - pivot.x);
            var y = rect.anchoredPosition.y + rect.rect.height * (1 - pivot.y);
            return new Vector2(x, y);
        }

        public static Vector2 BotLeft(this RectTransform rect)
        {
            var pivot = rect.pivot;
            var x = rect.anchoredPosition.x - rect.rect.width * pivot.x;
            var y = rect.anchoredPosition.y - rect.rect.height * pivot.y;
            return new Vector2(x, y);
        }

        public static Vector2 BotRight(this RectTransform rect)
        {
            var pivot = rect.pivot;
            var x = rect.anchoredPosition.x + rect.rect.width * (1 - pivot.x);
            var y = rect.anchoredPosition.y - rect.rect.height * pivot.y;
            return new Vector2(x, y);
        }

        public static Vector2 Center(this RectTransform rect)
        {
            var pivot = rect.pivot;
            var x = rect.anchoredPosition.x - rect.rect.width * pivot.x + rect.rect.width / 2f;
            var y = rect.anchoredPosition.y - rect.rect.height * pivot.y + rect.rect.height / 2f;
            return new Vector2(x, y);
        }

        /// <summary>
        /// Get corresponding anchored position of parent UI from anchored position of children
        /// Most used in scrollview when we need content move to where we can see child item
        /// </summary>
        public static Vector2 CovertAnchoredPosFromChildToParent(this RectTransform pItem, RectTransform pContent)
        {
            float contentWidth = pContent.rect.width;
            float contentHeight = pContent.rect.height;
            Vector2 itemAnchoredPos = pItem.anchoredPosition;
            Vector2 targetAnchored = pContent.anchoredPosition;
            targetAnchored.y = -itemAnchoredPos.y + (pContent.pivot.y - 0.5f) * contentHeight;
            targetAnchored.x = -itemAnchoredPos.x + (pContent.pivot.x - 0.5f) * contentWidth;
            targetAnchored.x -= contentWidth * (pItem.anchorMax.x - 0.5f);
            targetAnchored.y -= contentHeight * (pItem.anchorMax.y - 0.5f);
            return targetAnchored;
        }

        /// <summary>
        /// Calculate bounds of content which contains many UI objects
        /// </summary>
        public static Bounds GetBounds(List<RectTransform> pItems)
        {
            var contentTopRight = Vector2.zero;
            var contentBotLeft = Vector2.zero;
            for (int i = 0; i < pItems.Count; i++)
            {
                var topRight = pItems[i].TopRight();
                if (topRight.x > contentTopRight.x)
                    contentTopRight.x = topRight.x;
                if (topRight.y > contentTopRight.y)
                    contentTopRight.y = topRight.y;

                var botLeft = pItems[i].BotLeft();
                if (botLeft.x < contentBotLeft.x)
                    contentBotLeft.x = botLeft.x;
                if (botLeft.y < contentBotLeft.y)
                    contentBotLeft.y = botLeft.y;
            }

            float height = contentTopRight.y - contentBotLeft.y;
            float width = contentTopRight.x - contentBotLeft.x;
            Bounds bounds = new Bounds();
            bounds.size = new Vector2(width, height);
            bounds.min = contentBotLeft;
            bounds.max = contentTopRight;
            bounds.center = (contentBotLeft + contentTopRight) / 2f;
            return bounds;
        }

        public static Bounds Bounds(this RectTransform rect)
        {
            var size = new Vector2(rect.rect.width, rect.rect.height);
            return new Bounds(rect.Center(), size);
        }

        public static Vector3 WorldToCanvasPoint(this RectTransform mainCanvas, Vector3 worldPos)
        {
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPos);
            Vector2 worldPosition = new Vector2(
            ((viewportPosition.x * mainCanvas.sizeDelta.x) - (mainCanvas.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * mainCanvas.sizeDelta.y) - (mainCanvas.sizeDelta.y * 0.5f)));

            return worldPosition;
        }

        public static List<Vector2> GetScreenCoordinateOfUIRect(this RectTransform rt)
        {
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);

            var output = new List<Vector2>();
            int lengthF2 = corners.Length;
            for (int k = 0; k < lengthF2; k++)
            {
                var item = corners[k];
                //foreach (Vector3 item in corners)
                //{
                var pos = RectTransformUtility.WorldToScreenPoint(null, item);
                output.Add(pos);
            }
            return output;
        }

        public static AnchorType GetAnchorType(this RectTransform rect)
        {
            var anchorMin = rect.anchorMin;
            var anchorMax = rect.anchorMax;
            bool left, right, bot, top, center;
            left = right = bot = top = center = false;
            if (anchorMin.x == anchorMax.x && anchorMin.x == 0)
                left = true;
            if (anchorMin.x == anchorMax.x && anchorMin.x == 1)
                right = true;
            if (anchorMin.y == anchorMax.y && anchorMin.y == 0)
                bot = true;
            if (anchorMin.y == anchorMax.y && anchorMin.y == 1)
                top = true;
            if (anchorMin.y == anchorMax.y && anchorMin.y == 0.5f
                && anchorMin.x == anchorMax.x && anchorMin.x == 0.5f)
                center = true;

            AnchorType type = AnchorType.Stretch;
            if (top) type = AnchorType.Top;
            if (right) type = AnchorType.Right;
            if (left) type = AnchorType.Left;
            if (bot) type = AnchorType.Bot;
            if (center) type = AnchorType.Center;
            if (top == left == true) type = AnchorType.TopLeft;
            if (top == right == true) type = AnchorType.TopRight;
            if (bot == right == true) type = AnchorType.BotRight;
            if (bot == left == true) type = AnchorType.BotLeft;
            return type;
        }

        #endregion
    }
}