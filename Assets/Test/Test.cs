using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Move(-1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Move(1);
        }

    }

    public void Move(float y)
    {
        transform.DOMove(new Vector3(0, transform.position.y + y, 0), 0.2f);
    }
}
