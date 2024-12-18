using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testtttt : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        enabled = false;
        var ta = await ManagerAsset.LoadAssetAsync<TextAsset>($"Level_{SaveGame.Level}");
        Debug.Log(ta);
        var filePath = ta.text;
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogError("FUICK");
    }
}
