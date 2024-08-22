using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public enum TypeParticle
    {
        Click,
        Eat,
        Move,
        Unlock,
        Special,
        Charging,
        ChangeColor,
        UpgradeSparkles,
        FrostExplosion
    }

    public TypeParticle typeParticle;

    private ParticleSystem particle;
    // Start is called before the first frame update

    public void OnParticleSystemStopped()
    {
        if (typeParticle == TypeParticle.Click)
        {
            LogicGame.Instance.clickParticlePool.Release(particle);
        }
        else if (typeParticle == TypeParticle.Eat)
        {
            LogicGame.Instance.eatParticlePool.Release(particle);
        }
        else if (typeParticle == TypeParticle.Move)
        {
        }
        else if (typeParticle == TypeParticle.Unlock)
        {
            LogicGame.Instance.unlockParticlePool.Release(particle);
        }
        else if (typeParticle == TypeParticle.Special)
        {
            LogicGame.Instance.specialParticlePool.Release(particle);
        }
        else if (typeParticle == TypeParticle.Charging)
        {
            LogicGame.Instance.chargingParticlePool.Release(particle);
        }
        else if (typeParticle == TypeParticle.ChangeColor)
        {
            LogicGame.Instance.changeColorParticlePool.Release(particle);
        }
        else if (typeParticle == TypeParticle.UpgradeSparkles)
        {
            LogicSetupRoom.instance.upgradeSparklesParticleePool.Release(particle);
        }
        else if (typeParticle == TypeParticle.FrostExplosion)
        {
            LogicGame.Instance.frostExplosionPool.Release(particle);
        }
    }
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
