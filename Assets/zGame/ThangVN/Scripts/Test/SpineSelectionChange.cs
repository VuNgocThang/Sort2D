using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpineSelectionChange : MonoBehaviour
{
    public SkeletonGraphic catBonus;
    public SkeletonGraphic catNormal;

    const string IDLE = "idle";
    const string READING = "readingbook";
    const string BONUS = "bonus";


    private void Start()
    {
        currentState = CharacterState.Idle;
        lastActionTime = Time.time;

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

    public void PlayAnimBonus()
    {
        catBonus.gameObject.SetActive(true);
        catNormal.gameObject.SetActive(false);

        catBonus.AnimationState.SetAnimation(0, BONUS, false);
    }

    public void PlayAnimReading()
    {
        catBonus.gameObject.SetActive(false);
        catNormal.gameObject.SetActive(true);

        SetStartingAnimation(READING, true);
    }

    public void PlayAnimIdle()
    {
        catBonus.gameObject.SetActive(false);
        catNormal.gameObject.SetActive(true);

        SetStartingAnimation(IDLE, true);
    }

    public enum CharacterState
    {
        Idle,
        Reading,
        Bonus
    }

    public CharacterState currentState;
    public float lastActionTime;
    private const float readingTimeout = 5f;
    private const float bonusTimeout = 3f;


    private void Update()
    {
        float timeSinceLastAction = Time.time - lastActionTime;
        //Debug.Log("lastActioneTime: " + lastActionTime);
        //Debug.Log("timeSinceLastAction: " + timeSinceLastAction);

        switch (currentState)
        {
            case CharacterState.Idle:
                if (timeSinceLastAction > readingTimeout)
                {
                    currentState = CharacterState.Reading;
                    PlayAnimReading();
                    //Debug.Log("Switched to Reading state");
                }
                break;
            case CharacterState.Reading:
                break;
            case CharacterState.Bonus:
                //PlayAnimBonus();
                //if (timeSinceLastAction > bonusTimeout)
                //{
                //    currentState = CharacterState.Idle;
                //    PlayAnimIdle();
                //    Debug.Log("Switched to Idle state after Bonus");
                //}
                break;
        }

    }

    public void ActionToIdle()
    {
        lastActionTime = Time.time;
        currentState = CharacterState.Idle;
        PlayAnimIdle();
    }

    public void PerformAction()
    {
        currentState = CharacterState.Bonus;
        lastActionTime = Time.time;
        //Debug.Log("Performed Bonus action");
    }
}


