using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(SetMapManager))]
public class SetMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SetMapManager setMapManager = (SetMapManager)target;

        if (GUILayout.Button("Save Data"))
        {
            Debug.Log("Save Data");
            setMapManager.SaveData();
        }
    }

}
#endif
