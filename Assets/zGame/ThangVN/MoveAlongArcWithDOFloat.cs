using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveAlongArcWithDOFloat : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float height = 2f;
    public float duration = 2f;

    void Start()
    {
        transform.position = pointA.position;

        DOTween.To(() => 0f, x =>
        {
            float t = x;
            float yOffset = Mathf.Sin(t * Mathf.PI) * height;
            Vector3 newPos = Vector3.Lerp(pointA.position, pointB.position, t);
            newPos.y += yOffset;
            transform.position = newPos;
        }, 1f, duration).SetEase(Ease.Linear);

        //    id = LeanTween.value(pTarget.gameObject, pFrom, pTo, time)
        //.setOnUpdate((float val) =>
        //{
        //    float curve = pAnim.Evaluate(val);
        //    pTarget.transform.localScale = defaulScale * curve;
        //})
        //.setOnComplete(() =>
        //{
        //    pTarget.transform.localScale = defaulScale;
        //    if (pOnFinished != null) pOnFinished();
        //}).id;
    }
}