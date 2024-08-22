using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public ParticleSystem particleHitHammer;
    public Vector3 hitColorPlate;
    public ColorPlate colorPlateDestroy;

    public void ActiveVFX()
    {
        if (colorPlateDestroy == null) return;

        colorPlateDestroy.ClearAll();
        // sound here
        particleHitHammer.transform.position = hitColorPlate;
        particleHitHammer.Play();

        // anim shake here

    }

    public void DeactiveVFX()
    {
        gameObject.SetActive(false);
        LogicGame.Instance.homeInGame.ExitUsingItem();
    }
}
