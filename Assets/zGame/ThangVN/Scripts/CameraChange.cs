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
    [SerializeField] TestStack testStack;
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
        if (testStack != null)
        {
            if (testStack.cols >= testStack.rows)
            {
                float y = 0.3f * (max - testStack.cols);
                testStack.transform.position = new Vector3(0, 1.8f + y, 0);

                float scale = 6f / testStack.cols;
                nDesk.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                float y = 0.3f * (max - testStack.rows);
                testStack.transform.position = new Vector3(0, 1.8f + y, 0);

                float scale = 6f / testStack.rows;
                nDesk.localScale = new Vector3(scale, scale, scale);
            }
        }

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

                logicColor.transform.localPosition = new Vector3(0, 0, 0);
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

                logicColor.transform.localPosition = new Vector3(0, j * GameConfig.OFFSET_PLATE, -j * GameConfig.OFFSET_PLATE);
            }
        }
    }
}
