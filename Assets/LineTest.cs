using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour
{
    public LineRenderer lineRenderer;

    void Start()
    {
        Vector3[] points = new Vector3[6];
        // Ba điểm đầu có cùng tọa độ x và y tăng dần
        points[0] = new Vector3(0, 0, 0);
        points[1] = new Vector3(0, 1, 0);
        points[2] = new Vector3(0, 2, 0);

        // Ba điểm tiếp theo có cùng tọa độ y và x tăng dần
        points[3] = new Vector3(1, 2, 0);
        points[4] = new Vector3(2, 2, 0);
        points[5] = new Vector3(3, 2, 0);

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        //// Thiết lập chiều rộng cố định
        //lineRenderer.startWidth = 0.1f;
        //lineRenderer.endWidth = 0.1f;

        //// Đảm bảo không sử dụng Width Curve
        //AnimationCurve curve = new AnimationCurve();
        //curve.AddKey(0.0f, 1.0f); // 0 tại điểm đầu
        //curve.AddKey(1.0f, 1.0f); // 1 tại điểm cuối
        //lineRenderer.widthCurve = curve;
    }
}
