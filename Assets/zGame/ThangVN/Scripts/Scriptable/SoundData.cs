using System;
using UnityEngine;


[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData")]
public class SoundData : ScriptableObject
{
    public AudioClip soundClick;
    public AudioClip soundSwitch;
    public AudioClip soundPopupWin;
    public AudioClip soundPopupLose;
    public AudioClip soundEasyButton;
    public AudioClip soundPaperFireWorks;
    public AudioClip soundArrowButton;
    public AudioClip soundMerge_2;
    public AudioClip soundMerge;
    public AudioClip soundEat1;
    public AudioClip soundEat2;
    public AudioClip soundEat3;
    public AudioClip soundCannotClick;

    public AudioClip musicInGame;
    public AudioClip musicBG;
}
