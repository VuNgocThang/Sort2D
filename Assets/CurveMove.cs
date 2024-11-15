using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurveMove : MonoBehaviour
{
    //public Transform pointA; // Điểm bắt đầu
    //public Transform pointB; // Điểm kết thúc
    //public Transform controlPoint; // Điểm điều khiển tạo đường cong
    public float speed = 1f; // Tốc độ di chuyển

    private float t = 0f; // Giá trị t từ 0 đến 1 để nội suy trên đường cong

    void Update()
    {
      
    }

    public void Move(Transform pointA, Transform pointB, Transform controlPoint, float t)
    {
        // Tính vị trí trên đường cong Bezier
        Vector3 newPosition = CalculateQuadraticBezierPoint(t, pointA.position, controlPoint.position, pointB.position);

        // Di chuyển đối tượng đến vị trí mới
        transform.position = newPosition;
    }

    // Hàm tính toán điểm trên đường cong Bezier
    private Vector2 CalculateQuadraticBezierPoint(float t, Vector3 a, Vector3 b, Vector3 c)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * a; // (1 - t)^2 * A
        point += 2 * u * t * b; // 2 * (1 - t) * t * B
        point += tt * c; // t^2 * C

        return point;
    }

}
