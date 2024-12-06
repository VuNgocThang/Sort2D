using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    public List<float> listRatio = new List<float>();

    public List<int> listValue = new List<int>();

    public List<int> listResult = new List<int>();

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            while (listResult.Count < 2)
            {
                int a = GameManager.GetRandomWithRatio(listRatio);
                Debug.Log(a);
                if (!listResult.Contains(listValue[a]))
                {
                    listResult.Add(listValue[a]);
                    listValue.RemoveAt(a);
                    listRatio.RemoveAt(a);
                }
            }
        }
    }
}
