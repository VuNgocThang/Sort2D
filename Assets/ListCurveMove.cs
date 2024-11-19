using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ListCurveMove : MonoBehaviour
{
    public Transform pointA; // Điểm bắt đầu
    public Transform pointB; // Điểm kết thúc
    public Transform controlPoint; // Điểm điều khiển tạo đường cong
    //public List<CurveMove> listCurveMove;

    //public float speed = 1f; // Tốc độ di chuyển
    //public float distanceDelay = 1f;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        SceneManager.LoadScene("SceneTest");
    //    }
    //    for (int i = 0; i < listCurveMove.Count; i++)
    //    {
    //        float t = Time.time * speed - i * distanceDelay;

    //        if (t >= 0 && t <= 1)
    //        {
    //            listCurveMove[i].Move(listCurveMove[i].transform, pointB, controlPoint, t);
    //        }
    //    }
    //}
}
