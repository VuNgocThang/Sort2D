
using UnityEngine;

public static class CurveMove
{
    public static void Move(Transform color, Vector3 from, Vector3 to, Vector3 midPoint, float t)
    {
        // Tính vị trí trên đường cong Bezier
        Vector3 newPosition = CalculateQuadraticBezierPoint(t, from, midPoint, to);

        // Di chuyển đối tượng đến vị trí mới

        color.localPosition = newPosition;
        Debug.Log(color.localPosition);
    }

    public static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 a, Vector3 b, Vector3 c)
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
