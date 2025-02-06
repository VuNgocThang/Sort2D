using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDecor : MonoBehaviour
{
    public static TutorialDecor Instance;
    public GameObject hand;
    [SerializeField] Transform nBlackTut;

    private void Awake()
    {
        Instance = this;
    }

    public void InitTut()
    {
        hand.SetActive(true);
        nBlackTut.gameObject.SetActive(true);
    }

    public void SetParent(Transform obj)
    {
        obj.SetParent(nBlackTut);
    }
}
