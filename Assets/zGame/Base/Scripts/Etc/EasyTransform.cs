using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public enum UpdateType
    {
        UPDATE,
        FIXED_UPDATE,
        LATE_UPDATE
    }
    public class EasyTransform : MonoBehaviour
    {
        public bool IsManualUpdate;
        public bool IsTimeScale = true;
        public UpdateType UpdateType;
        public Transform FollowTarget;
        public Vector3 FollowTargetOffset;
        public Transform LookAtTarget;
        public Vector3 Rotate;
        public bool RotateRandom;
        int RotateDirection = 1;

        public bool IsScale;
        Vector3 ScaleFrom;
        public Vector3 ScaleTo;
        public float ScaleTime = 1f;
        float ScaleTimer = 0;

        public bool IsFade;
        CanvasGroup canvasGroup;
        float FadeFrom;
        public float FadeTo;
        public float FadeTime = 1f;
        float FadeTimer = 0;

        void OnValidate()
        {
            // Start();
            ManualUpdate();
        }

        void Start()
        {
            if (Rotate != Vector3.zero)
            {
                transform.eulerAngles += Rotate * Ez.Random(0, 360);
                if (RotateRandom) RotateDirection = Ez.Random(0, 2) == 1 ? 1 : -1;
            }
            if (IsScale) ScaleFrom = transform.localScale;
            if (IsFade)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    IsFade = false;
                    return;
                }
                FadeFrom = canvasGroup.alpha;
            }
        }

        public void ManualUpdate()
        {
            if (FollowTarget != null) transform.position = FollowTarget.position + FollowTargetOffset;
            if (LookAtTarget != null) transform.LookAt(LookAtTarget);
            if (Rotate != Vector3.zero) transform.eulerAngles += Rotate * (UpdateType != UpdateType.FIXED_UPDATE ? Ez.TimeMod : Ez.FixedTimeMod) * RotateDirection;
            if (IsScale)
            {
                if (IsTimeScale) ScaleTimer += (UpdateType != UpdateType.FIXED_UPDATE ? Ez.TimeMod : Ez.FixedTimeMod);
                else ScaleTimer += (UpdateType != UpdateType.FIXED_UPDATE ? Time.deltaTime : Time.fixedDeltaTime);
                float t = Mathf.Clamp01(ScaleTimer / ScaleTime);
                transform.localScale = Vector3.Lerp(ScaleFrom, ScaleTo, t);
                if (t >= 1)
                {
                    Vector3 tg = ScaleTo;
                    ScaleTo = ScaleFrom;
                    ScaleFrom = tg;
                    ScaleTimer = 0;
                }
            }
            if (IsFade)
            {
                if (IsTimeScale) FadeTimer += (UpdateType != UpdateType.FIXED_UPDATE ? Ez.TimeMod : Ez.FixedTimeMod);
                else FadeTimer += (UpdateType != UpdateType.FIXED_UPDATE ? Time.deltaTime : Time.fixedDeltaTime);
                float t = Mathf.Clamp01(FadeTimer / FadeTime);
                canvasGroup.alpha = Mathf.Lerp(FadeFrom, FadeTo, t);
                if (t >= 1)
                {
                    float tg = FadeTo;
                    FadeTo = FadeFrom;
                    FadeFrom = tg;
                    ScaleTimer = 0;
                }
            }
        }

        void Update()
        {
            if (IsManualUpdate) return;
            if (UpdateType == UpdateType.UPDATE) ManualUpdate();
        }

        void FixedUpdate()
        {
            if (IsManualUpdate) return;
            if (UpdateType == UpdateType.FIXED_UPDATE) ManualUpdate();
        }

        void LateUpdate()
        {
            if (IsManualUpdate) return;
            if (UpdateType == UpdateType.LATE_UPDATE) ManualUpdate();
        }
    }
}