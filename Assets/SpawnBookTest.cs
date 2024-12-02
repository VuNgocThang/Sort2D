using BaseGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBookTest : MonoBehaviour
{
    [SerializeField] Animator animParent, animBox;

    public void PlayAnimSpawn()
    {
        //Debug.Log("press R");

        animParent.Play("Spawn", -1, 0);
        animBox.Play("box_anim", -1, 0);
    }

    public void Spawn()
    {
        LogicGame.Instance.InitNextPlate();
        ManagerAudio.PlaySound(ManagerAudio.Data.soundOpenBox);
        //Debug.Log("Spawn Book");
    }
}
