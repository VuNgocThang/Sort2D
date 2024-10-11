using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] Transform obj;
    float speed = 45f;

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, speed * Time.fixedDeltaTime);
    }
}
