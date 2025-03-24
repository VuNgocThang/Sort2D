using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraChange : MonoBehaviour
{
    public static CameraChange Ins;
    [SerializeField] Camera cam;
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 startRot;
    float sizeDefault = 8f;
    float max = 6f;
    [SerializeField] Transform nDesk;

    Vector3 endPos = new Vector3(0f, 25f, -4.4f);
    Vector3 endRot = new Vector3(80f, 0f, 0f);
    private void Awake()
    {
        Ins = this;
    }

    private void Start()
    {
        //cam.orthographicSize = (testStack.cols / maxCol) * sizeDefault;

        startPos = cam.transform.position;
        startRot = cam.transform.localEulerAngles;
    }

    public void ChangeCameraUsingItem()
    {
        //cam.transform.DOMove(endPos, 0.3f);
        //cam.transform.DORotate(endRot, 0.3f);

        for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
        {
            if (LogicGame.Instance.ListColorPlate[i].ListValue.Count == 0) continue;
            for (int j = 0; j < LogicGame.Instance.ListColorPlate[i].ListColor.Count; j++)
            {
                LogicColor logicColor = LogicGame.Instance.ListColorPlate[i].ListColor[j];

                //logicColor.transform.localPosition = new Vector3(0, 0, 0);

                if (j == LogicGame.Instance.ListColorPlate[i].ListColor.Count - 1)
                {
                    logicColor.transform.localPosition = new Vector3(0, 0, -0.1f);
                }
                else
                {
                    logicColor.transform.localPosition = new Vector3(0, 0, 0);
                }
            }
        }
    }

    public void ExitUsingItemCamera()
    {
        for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
        {
            if (LogicGame.Instance.ListColorPlate[i].ListValue.Count == 0) continue;
            for (int j = 0; j < LogicGame.Instance.ListColorPlate[i].ListColor.Count; j++)
            {
                LogicColor logicColor = LogicGame.Instance.ListColorPlate[i].ListColor[j];

                logicColor.transform.localPosition = new Vector3(0, j * GameConfig.OFFSET_PLATE, -j * GameConfig.OFFSET_PLATE_Z);
            }
        }
    }
}
