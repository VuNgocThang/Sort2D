using BaseGame;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSpineEvent : MonoBehaviour
{
    public SkeletonAnimation anim;
    [SerializeField] ParticleSystem smoke;
    [SpineEvent] public string hit = "HIT";
    public ColorPlate colorPlateDestroy;

    public SkeletonAnimation animPen;

    private void Awake()
    {
        anim.AnimationState.Event += RaiseAnimEvent;
    }

    public void PlayAnim()
    {
        //StartCoroutine(PlayAnimState());

        StartCoroutine(PlayAnimPen());
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
        if (colorPlateDestroy == null) return;
        colorPlateDestroy.ClearAll();

        ManagerAudio.PlaySound(ManagerAudio.Data.soundHammer);

        smoke.transform.localPosition = anim.transform.localPosition;
        smoke.gameObject.SetActive(true);
        smoke.Play();
    }

    IEnumerator PlayAnimState()
    {
        anim.state.SetAnimation(0, "hammer_hit", false);

        yield return new WaitForSeconds(1f);

        LogicGame.Instance.homeInGame.ExitUsingItem();

        this.gameObject.SetActive(false);
    }

    IEnumerator PlayAnimPen()
    {
        animPen.gameObject.SetActive(true);

        animPen.state.SetAnimation(0, "Draw", false);

        yield return new WaitForSeconds(2.8f);

        animPen.gameObject.SetActive(false);

        colorPlateDestroy.ClearAll();

        ManagerAudio.PlaySound(ManagerAudio.Data.soundHammer);

        smoke.transform.localPosition = animPen.transform.localPosition;
        smoke.gameObject.SetActive(true);
        smoke.Play();

        yield return new WaitForSeconds(0.5f);

        LogicGame.Instance.homeInGame.ExitUsingItem();

        this.gameObject.SetActive(false);
    }

}
