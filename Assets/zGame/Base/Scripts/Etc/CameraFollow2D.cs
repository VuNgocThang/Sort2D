using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class CameraFollow2D : MonoBehaviour
    {
        [SerializeField] Transform target;

        public Vector3 offSet;
        void LateUpdate()
        {
            Vector3 v = target.position;
            v.y = Mathf.Max(0, v.y - 2);
            offSet = new Vector3(Mathf.Sin(target.eulerAngles.y * Mathf.Deg2Rad), 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, v + offSet, 40 * Ez.TimeMod);
        }
    }
}