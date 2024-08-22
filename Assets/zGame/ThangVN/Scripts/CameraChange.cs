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

    Vector3 endPos = new Vector3(0f, 25f, -4.4f);
    Vector3 endRot = new Vector3(80f, 0f, 0f);
    private void Awake()
    {
        Ins = this;
    }

    private void Start()
    {
        startPos = cam.transform.position;
        startRot = cam.transform.localEulerAngles;
    }

    public void ChangeCameraUsingItem()
    {
        cam.transform.DOMove(endPos, 0.3f);
        cam.transform.DORotate(endRot, 0.3f);
    }

    public void ExitUsingItemCamera()
    {
        cam.transform.DOMove(startPos, 0.3f);
        cam.transform.DORotate(startRot, 0.3f);
    }
}
