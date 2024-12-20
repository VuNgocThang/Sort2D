using UnityEngine;
using UnityEngine.UI;

public class CameraResize : MonoBehaviour
{
    public Camera cam;
    public Camera Cam
    {
        get
        {
            if (cam == null) cam = GetComponent<Camera>();
            return cam;
        }
    }

    const float iPhanRatio = 1080f / 1920;

    [SerializeField] float minSize = 7;
    [SerializeField] float maxSize = 8;
    [SerializeField] bool Orthographic = true;
    [SerializeField] CanvasScaler canvasScaler;
    [SerializeField] Vector2 minReferenceResolution = new Vector2(1080, 2340);
    [SerializeField] Vector2 maxReferenceResolution = new Vector2(1080, 2640);

    [SerializeField] CanvasScaler canvasUIGame;

    private void Awake()
    {
        //CheckCamera();
    }

    void CheckCamera()
    {
        //Debug.LogError(iPhanRatio);
        //Debug.LogError(Cam.aspect);
        if (Orthographic)
        {
            //if (Cam.aspect <= 15f / 9)
            //{
            //    Debug.Log("asdad");
            //    Cam.orthographicSize = maxSize;
            //    canvasScaler.referenceResolution = maxReferenceResolution;
            //}
            //else 
            if (Cam.aspect == iPhanRatio)
            {
                Cam.orthographicSize = minSize;
                canvasScaler.referenceResolution = minReferenceResolution;
                canvasUIGame.referenceResolution = maxReferenceResolution;
            }
            else
            {
                Cam.orthographicSize = maxSize;
                canvasScaler.referenceResolution = maxReferenceResolution;
                canvasUIGame.referenceResolution = minReferenceResolution;
            }
            //else
            //{
            //    Debug.Log("...");
            //    Cam.orthographicSize = minSize * (iPhanRatio / Cam.aspect);
            //    canvasScaler.referenceResolution = maxReferenceResolution;
            //}
        }
        else
        {
            if (Cam.aspect < (9 / 16f)) Cam.fieldOfView = 75 * Cam.aspect / (9 / 16f);
        }

        if (Cam.aspect < 0.5625)
        {
            canvasUIGame.matchWidthOrHeight = 0;
        }
        else
        {
            canvasUIGame.matchWidthOrHeight = 1;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        CheckCamera();
    }
#endif
}
