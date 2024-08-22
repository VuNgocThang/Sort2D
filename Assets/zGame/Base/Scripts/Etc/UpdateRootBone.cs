using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ntDev
{
    public class UpdateRootBone : MonoBehaviour
    {
        [SerializeField] Transform root;
        public void UpdateRoot()
        {
            SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.rootBone = root;

            Transform[] arr = root.GetComponentsInChildren<Transform>();
            Transform[] arrBones = new Transform[skinnedMeshRenderer.bones.Length];
            for (int i = 0; i < skinnedMeshRenderer.bones.Length; ++i)
            {
                foreach (Transform t in arr)
                {
                    if (skinnedMeshRenderer.bones[i].gameObject.name == t.gameObject.name)
                    {
                        arrBones[i] = t;
                        break;
                    }
                }
            }

            skinnedMeshRenderer.bones = arrBones;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UpdateRootBone)), CanEditMultipleObjects]
    public class UpdateRootBone_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UpdateRootBone myScript = (UpdateRootBone)target;
            if (GUILayout.Button("Fix it!"))
            {
                myScript.UpdateRoot();
            }
        }
    }
#endif
}