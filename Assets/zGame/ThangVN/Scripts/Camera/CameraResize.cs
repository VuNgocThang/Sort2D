using UnityEditor.Overlays;
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
    //[SerializeField] Canvas canvasOverLay;
    //[SerializeField] RectTransform nButtons;
    //[SerializeField] RectTransform targetCamera;
    //[SerializeField] GameObject worldObject;


    private void Awake()
    {
        //CheckCamera();
    }

    void CheckCamera()
    {
        //Debug.LogError(iPhanRatio);
        //Debug.Log(Screen.width + " ___ " + Screen.height);
        //Debug.LogError(Cam.aspect);
        if (Orthographic)
        {
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

        //if (Cam.aspect < 0.5625)
        //{
        //    Debug.Log("match = 0");
        //    canvasUIGame.matchWidthOrHeight = 0;

        //}
        //else
        //{
        //    Debug.Log("match = 1");
        //    canvasUIGame.matchWidthOrHeight = 1;

        //}

        ////Vector3 worldPosition = targetCamera.position;
        ////Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(cam, worldPosition);

        ////RectTransform overlayCanvasRect = canvasOverLay.GetComponent<RectTransform>();
        ////Vector2 localPosition;
        ////if (RectTransformUtility.ScreenPointToLocalPointInRectangle(overlayCanvasRect, screenPosition, canvasOverLay.worldCamera, out localPosition))
        ////{
        ////    nButtons.position = localPosition;
        ////}

        //Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, targetCamera.position);
        //Vector3 targetPos = cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, cam.nearClipPlane));
        //worldObject.transform.position = targetPos;

        //Vector2 posUI = cam.WorldToScreenPoint(targetPos);
        //nButtons.position = posUI;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        //CheckCamera();
    }
#endif
}
