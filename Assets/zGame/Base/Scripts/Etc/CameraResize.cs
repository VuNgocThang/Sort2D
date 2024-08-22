using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class CameraResize : MonoBehaviour
    {
        Camera cam;
        public Camera Cam
        {
            get
            {
                if (cam == null) cam = GetComponent<Camera>();
                return cam;
            }
        }

        const float iPhanRatio = 2340f / 1080;

        [SerializeField] float minSize = 540;
        [SerializeField] float maxSize = 720;
        [SerializeField] bool Orthographic = true;

        void Start()
        {
            CheckCamera();
        }

        void CheckCamera()
        {
            if (Orthographic)
            {
                if (Cam.aspect <= 15f / 9) Cam.orthographicSize = maxSize;
                else if (Cam.aspect >= iPhanRatio) Cam.orthographicSize = minSize;
                else Cam.orthographicSize = minSize * (iPhanRatio / Cam.aspect);
            }
            else
            {
                if (Cam.aspect < (9 / 16f)) Cam.fieldOfView = 75 * Cam.aspect / (9 / 16f);
            }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            CheckCamera();
        }
#endif
    }
}
