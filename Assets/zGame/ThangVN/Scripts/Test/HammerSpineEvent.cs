using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSpineEvent : MonoBehaviour
{
    [SerializeField] SkeletonAnimation anim;
    [SerializeField] ParticleSystem smoke;
    [SpineEvent] public string hit = "HIT";

    private void Awake()
    {
        anim.AnimationState.Event += RaiseAnimEvent;
    }
    void RaiseAnimEvent(TrackEntry trackE, Spine.Event e)
    {
        if (e.Data.Name == hit)
        {
            ActiveVfx(e);
        }
    }

    void ActiveVfx(Spine.Event e)
    {
        smoke.transform.localPosition = anim.transform.localPosition;
        smoke.gameObject.SetActive(true);
        smoke.Play();
        Debug.Log("Fuck");
    }

}
