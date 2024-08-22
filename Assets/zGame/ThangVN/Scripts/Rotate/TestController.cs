using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TestController : MonoBehaviour
{
    public EasyController easyController;
    public Transform nX;
    public Camera cam;
    public float speed = 50f;

    Vector3 initialPosition;
    Vector3 currentPosition;

    public float topBorder;
    public float bottomBorder;
    public float leftBorder;
    public float rightBorder;

    bool IsInside(Vector3 p)
    {
        if (p.x < leftBorder)
            return false;

        if (p.x > Screen.width - rightBorder)
            return false;

        if (p.y < bottomBorder)
            return false;

        if (p.y > Screen.height - topBorder)
            return false;

        return true;
    }
    //public Rect touchRegion = new Rect(100, 100, 500, 500);

    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //if (touchRegion.Contains(touch.position))
            //if (IsInside(touch.position))
            //{
            //Debug.Log("in region");
            if (touch.phase == TouchPhase.Began)
            {
                initialPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (LogicGame.Instance.isUsingHammer || LogicGame.Instance.isUsingHand || LogicGame.Instance.isPauseGame || !SaveGame.IsDoneTutorial || LogicGame.Instance.isWin || LogicGame.Instance.isLose) return;
                currentPosition = touch.position;

                Vector2 initialVector = initialPosition - Camera.main.WorldToScreenPoint(transform.position);
                Vector2 currentVector = currentPosition - Camera.main.WorldToScreenPoint(transform.position);

                float angle = Vector2.SignedAngle(initialVector, currentVector);
                //Debug.Log("angle: " + angle);

                if (angle > 3)
                {
                    //Debug.Log("Clockwise rotation detected.");
                    nX.localEulerAngles += new Vector3(0, 2f, 0);
                    //cam.transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
                    //cam.transform.LookAt(transform);
                }
                else if (angle < -3)
                {
                    //Debug.Log("Counterclockwise rotation detected.");
                    nX.localEulerAngles += new Vector3(0, -2f, 0);
                    //cam.transform.RotateAround(transform.position, Vector3.down, speed * Time.deltaTime);
                    //cam.transform.LookAt(transform);
                }

                initialPosition = currentPosition;
            }
        }
        //else
        //{
        //    Debug.Log("out region");
        //}
    }
}
