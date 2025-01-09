using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerAnimState : MonoBehaviour
{
    public SkeletonGraphic catBonus;
    public SkeletonGraphic catNormal;

    const string IDLE = "idle";
    const string READING = "readingbook";
    const string BONUS = "bonus";

    public enum CharacterStateController
    {
        Idle,
        Reading,
        Bonus
    }

    public CharacterStateController currentState;
    public float lastActionTime;
    private const float readingTimeout = 4f;
    private const float bonusCooldown = 3f;
    public bool canPerformBonus = true;

    private void Start()
    {
        currentState = CharacterStateController.Idle;
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

    private void Update()
    {
        if (GameManager.IsBonusGame()) return;

        float timeSinceLastAction = Time.time - lastActionTime;
        //Debug.Log("timeSinceLastAction: " + timeSinceLastAction);

        if (currentState == CharacterStateController.Idle && timeSinceLastAction > readingTimeout)
        {
            currentState = CharacterStateController.Reading;
            PlayAnimReading();
            //Debug.Log("Switched to Reading state");
        }

        if (currentState == CharacterStateController.Reading)
        {
            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    ActionToIdle();
            //}
        }

        if (currentState == CharacterStateController.Reading || currentState == CharacterStateController.Idle)
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    Debug.Log("space");
            //    PerformAction();
            //}
        }
    }

    public void ActionToIdle()
    {
        if (GameManager.IsBonusGame()) return;

        lastActionTime = Time.time;
        currentState = CharacterStateController.Idle;
        PlayAnimIdle();
    }

    public void ActionToBonus()
    {
        if (GameManager.IsBonusGame()) return;

        if (canPerformBonus)
        {
            //Debug.Log("Play bonus");
            currentState = CharacterStateController.Bonus;
            PlayAnimBonus();
            lastActionTime = Time.time;
            canPerformBonus = false;
            Invoke(nameof(ResetBonusCooldown), bonusCooldown);
            //Debug.Log("Performed Bonus action");

            StartCoroutine(PlayIdle());
        }
        else
        {
            //Debug.Log("Cannot perform Bonus yet, still in cooldown");
        }
    }

    private void ResetBonusCooldown()
    {
        canPerformBonus = true;
        //Debug.Log("Bonus cooldown reset, can perform Bonus again");
    }

    IEnumerator PlayIdle()
    {
        yield return new WaitForSeconds(1.5f);
        currentState = CharacterStateController.Idle;
        PlayAnimIdle();
    }
}
