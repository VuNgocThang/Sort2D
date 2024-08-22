using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// by nt.Dev93
namespace ntDev
{
    public class ProgressShow : MonoBehaviour
    {
        public float CurrentValue;
        [SerializeField] float targetValue;
        public float TargetValue
        {
            get => targetValue;
            set
            {
                if (targetValue == value) return;
                targetValue = value;
                d = (targetValue - CurrentValue) / step;
            }
        }

        [SerializeField] float step = 60;
        [SerializeField] float d = 0.01f;
        Action<float> funcProgress;
        Action funcDone;

        public void Init(float c, Action<float> fP = null, Action fD = null)
        {
            CurrentValue = c;
            TargetValue = c;
            funcProgress = fP;
            funcDone = fD;
            fP?.Invoke(c);
        }

        void Update()
        {
            if (CurrentValue != TargetValue)
            {
                if (Math.Abs(TargetValue - CurrentValue) <= Mathf.Abs(d))
                {
                    CurrentValue = TargetValue;
                    funcProgress?.Invoke(CurrentValue);
                    funcDone?.Invoke();
                }
                else
                {
                    CurrentValue += d;
                    funcProgress?.Invoke(CurrentValue);
                }
            }
        }
    }
}