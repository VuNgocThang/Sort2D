using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineSelectionChange : MonoBehaviour
{
    public SkeletonGraphic catBonus;
    public SkeletonGraphic catNormal;

    const string IDLE = "idle";
    const string READING = "readingbook";

    private void Start()
    {
        catBonus.gameObject.SetActive(false);
        catNormal.gameObject.SetActive(true);

        if (catNormal != null)
        {
            SetStartingAnimation(IDLE, true);
        }

        


    }

    public void SetStartingAnimation(string animationName, bool isLoop = false)
    {
        if (catNormal.AnimationState != null)
        {
            catNormal.AnimationState.SetAnimation(0, animationName, isLoop);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("w");
            SetStartingAnimation(READING);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E");

            catBonus.gameObject.SetActive(true);
            catNormal.gameObject.SetActive(false);
        }
    }
}
