using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ntDev;

public class DoSomething : MonoBehaviour
{
    public Component[] arr;
    public void Do()
    {
        arr = GetComponentsInChildren<Component>();
        foreach (Component component in arr)
            if (component == null)
            {
                Debug.Log("Found");
            }
    }

    public void RemoveCollider()
    {
        Renderer[] arr = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < arr.Length; ++i)
        {
            arr[i].enabled = false;
            // DestroyImmediate(arr[i]);
        }
    }

    public void AddCollider()
    {
        Renderer[] arr = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < arr.Length; ++i)
        {
            arr[i].gameObject.AddComponent<MeshCollider>();
        }
    }

    [SerializeField] string Name;
    public void RenamePath()
    {
        MapPath[] arr = GetComponentsInChildren<MapPath>();
        for (int i = 0; i < arr.Length; ++i)
        {
            arr[i].gameObject.name = Name + " " + (i + 1);
            arr[i].ID = (i + 1);
        }
    }
    [SerializeField] Transform targetTransform;
    public void SetPosition()
    {
        transform.position = targetTransform.position;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(DoSomething)), CanEditMultipleObjects]
public class DoSomething_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DoSomething myScript = (DoSomething)target;
        if (GUILayout.Button("Do"))
        {
            myScript.Do();
        }
        if (GUILayout.Button("Remove Collider"))
        {
            myScript.RemoveCollider();
        }
        if (GUILayout.Button("Remove Collider"))
        {
            myScript.RemoveCollider();
        }
        if (GUILayout.Button("Add Collider"))
        {
            myScript.AddCollider();
        }
        if (GUILayout.Button("Rename"))
        {
            myScript.RenamePath();
        }
        if (GUILayout.Button("Set Position"))
        {
            myScript.SetPosition();
        }
    }
}
#endif