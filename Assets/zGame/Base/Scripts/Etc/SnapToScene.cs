using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    [ExecuteInEditMode]
    public class SnapToScene : MonoBehaviour
    {
        [SerializeField] float d = 1.25f;
        [SerializeField] bool update = true;
#if UNITY_EDITOR
        void Update()
        {
            if (Application.isPlaying) return;
            if (!update) return;
            if (!transform.hasChanged)
            {
                Vector3 pos = transform.position;
                pos.x = Mathf.RoundToInt(pos.x / d) * d;
                pos.z = Mathf.RoundToInt(pos.z / d) * d;
                transform.position = pos;

                Vector3 rotation = transform.eulerAngles;
                rotation.x = 0;
                rotation.y = Mathf.RoundToInt(rotation.y / 90) * 90;
                rotation.z = 0;
                transform.localEulerAngles = rotation;
            }
            else transform.hasChanged = false;
        }
#endif
    }
}