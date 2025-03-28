﻿

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Inspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utilities.Components
{
    public class ProgressBar : MonoBehaviour
    {
        public enum FillDirection
        {
            Left,
            Right,
        }

        public Image imgBackground;
        public Image imgProgressValue;
        public TextMeshProUGUI txtProgessValue;
        public TextMeshProUGUI txtRank;
        /// <summary>
        /// False: image can fill, 
        /// Tue: image cannot fill, we have to use delta size
        /// </summary>
        public bool fillByBarWidth;
        [Tooltip("If fillByBarWidth = true")] public FillDirection fillByBarWidthDirection;
        /// <summary>
        /// Sometimg we don't fill empty bar as 0%
        /// </summary>
        [Range(0, 1)] public float minFillRatio = 0f;
        /// <summary>
        /// Sometime we don't fill full bar as 100%
        /// </summary>
        [Range(0, 1)] public float maxFillRatio = 1f;
        /// <summary>
        /// True: value text will display as countdown timer
        /// </summary>
        public bool isTimeCountdown;
        public bool isPercent;
        [SerializeField] private float mWidthOffset;

        [SerializeField, ReadOnly] private float mValue = -1;
        [SerializeField, ReadOnly] private float mMax = -1;

        private int mRank = -1;
        private Vector2 mBackgroundSize;
        private RectTransform mRectProgressValue;
        private float mFill;

        public virtual float FillAmount
        {
            get { return mFill; }
            set
            {
                mFill = maxFillRatio * value;
                if (mFill < minFillRatio && mFill > 0)
                    mFill = minFillRatio;

                FillBar(mFill);
            }
        }
        public virtual float Value
        {
            get { return mValue; }
            set
            {
                if (mValue != value)
                {
                    mValue = value;
                    if (mMax > 0)
                        FillAmount = mValue / mMax;
                    SetProgressDisplay();
                }
            }
        }
        public virtual float Max
        {
            get { return mMax; }
            set
            {
                if (mMax != value)
                {
                    mMax = value;
                    if (mMax > 0)
                        FillAmount = mValue / mMax;
                    SetProgressDisplay();
                }
            }
        }
        public virtual int Rank
        {
            get { return mRank; }
            set
            {
                if (mRank != value)
                {
                    mRank = value;
                    SetRankDisplay();
                }
                if (txtRank != null)
                    txtRank.enabled = mRank >= 0;
            }
        }

        public Vector2 BarSize()
        {
            return new Vector2(imgBackground.rectTransform.rect.width - mWidthOffset, imgBackground.rectTransform.rect.height);
        }

        private void FillBar(float pValue)
        {
            if (fillByBarWidth)
            {
                mBackgroundSize = new Vector2(imgBackground.rectTransform.rect.width, imgBackground.rectTransform.rect.height);
                mBackgroundSize.x -= mWidthOffset;
            }

            if (fillByBarWidth)
            {
                if (mRectProgressValue == null)
                    mRectProgressValue = imgProgressValue.rectTransform;
                mRectProgressValue.sizeDelta = new Vector2(mBackgroundSize.x * pValue, mRectProgressValue.sizeDelta.y);
            }
            else
                imgProgressValue.fillAmount = pValue;
        }

        public virtual void Active(bool pValue)
        {
            imgBackground.SetActive(pValue);
            imgProgressValue.SetActive(pValue);
            if (txtProgessValue != null)
                txtProgessValue.SetActive(pValue);
            if (txtRank != null)
                txtRank.gameObject.SetActive(pValue);
        }

        protected virtual void SetProgressDisplay()
        {
            if (txtProgessValue != null)
                txtProgessValue.enabled = mMax >= 0;

            if (isPercent)
            {
                if (mMax > 0 && txtProgessValue != null)
                    txtProgessValue.text = string.Format("{0}%", Mathf.RoundToInt(mValue / mMax * 100));
            }
            else if (isTimeCountdown)
            {
                if (mMax > 0 && txtProgessValue != null)
                    txtProgessValue.text = TimeHelper.FormatHHMMss(mMax - mValue, false);
            }
            else
            {
                if (mMax > 0 && txtProgessValue != null)
                    txtProgessValue.text = string.Format("{0}/{1}", mValue, mMax);
            }
        }

        protected virtual void SetRankDisplay()
        {
            if (txtRank != null)
                txtRank.text = mRank.ToString();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Validate();
        }
#endif

        [ContextMenu("Validate")]
        private void Validate()
        {
            if (imgBackground == null || imgProgressValue == null)
            {
                Image[] imgs = gameObject.GetComponentsInChildren<Image>();
                if (imgs.Length >= 2)
                {
                    imgBackground = imgs[0];
                    imgProgressValue = imgs[1];
                }
            }

            if (txtProgessValue == null)
                txtProgessValue = imgProgressValue.GetComponentInChildren<TextMeshProUGUI>();

            //Max = mMax;
            //= mValue;
            FillAmount = Value / Max;
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(ProgressBar))]
        public class ProgressBarEditor : Editor
        {
            ProgressBar mBar;

            private void OnEnable()
            {
                mBar = target as ProgressBar;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (mBar.txtProgessValue != null)
                    mBar.txtProgessValue.text = EditorGUILayout.TextField("Progress", mBar.txtProgessValue.text);
                if (mBar.txtRank != null)
                    mBar.txtRank.text = EditorGUILayout.TextField("Rank", mBar.txtRank.text);

                if (mBar.fillByBarWidth && mBar.imgBackground != null && mBar.imgProgressValue != null)
                {
                    var barTransform = mBar.imgProgressValue.transform as RectTransform;
                    var pivot = new Vector2(0, 0.5f);
                    switch (mBar.fillByBarWidthDirection)
                    {
                        case FillDirection.Left:
                            pivot = new Vector2(0, 0.5f);
                            break;
                        case FillDirection.Right:
                            pivot = new Vector2(1, 0.5f);
                            break;
                    }
                    Vector2 size = barTransform.rect.size;
                    size.x -= mBar.mWidthOffset;
                    Vector2 deltaPivot = barTransform.pivot - pivot;
                    Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
                    barTransform.pivot = pivot;
                    barTransform.localPosition -= deltaPosition;
                }
            }
        }
#endif
    }
}
