using BaseGame;
using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSettingHome : Popup
{
    public EasyButton btnSound, btnMusic, btnVibrate;
    public GameObject imgMusicOff, imgSoundOff, imgVibrateOff;
    public static async void Show()
    {
        PopupSettingHome pop = await ManagerPopup.ShowPopup<PopupSettingHome>();
        pop.Init();
    }

    private void Awake()
    {
        btnMusic.OnClick(() =>
        {
            ToggleBtnMusic();

        });
        btnSound.OnClick(() =>
        {
            ToggleBtnSound();
        });
        btnVibrate.OnClick(() =>
        {
            ToggleBtnVibrate();
        });
    }

    public override void Init()
    {
        base.Init();
        imgMusicOff.SetActive(!SaveGame.Music);
        imgSoundOff.SetActive(!SaveGame.Sound);
        imgVibrateOff.SetActive(!SaveGame.Vibrate);
    }

    void ToggleBtnMusic()
    {
        if (SaveGame.Music) ManagerAudio.MuteMusic();
        else
        {
            ManagerAudio.PlayMusic(ManagerAudio.Data.musicBG);
            ManagerAudio.UnMuteMusic();
        }

        SaveGame.Music = !SaveGame.Music;
        imgMusicOff.SetActive(!SaveGame.Music);
    }

    void ToggleBtnSound()
    {
        if (SaveGame.Sound) ManagerAudio.MuteSound();
        else ManagerAudio.UnMuteSound();

        SaveGame.Sound = !SaveGame.Sound;
        imgSoundOff.SetActive(!SaveGame.Sound);
    }
    void ToggleBtnVibrate()
    {
        //if (SaveGame.Vibrate) ManagerAudio.MuteSound();
        //else ManagerAudio.UnMuteSound();
        SaveGame.Vibrate = !SaveGame.Vibrate;
        imgVibrateOff.SetActive(!SaveGame.Vibrate);
    }

}
