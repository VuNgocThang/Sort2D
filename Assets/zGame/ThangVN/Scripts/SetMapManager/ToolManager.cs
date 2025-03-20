using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public EasyButton btnTool;
    public GameObject tool;

    private void Awake()
    {
        btnTool.OnClick(() =>
        {
            Debug.Log("Open TOol");
            tool.SetActive(true);
        });
    }
}
