using BaseGame;
using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSettingHome : Popup
{
    public EasyButton btnSound, btnMusic, btnVibrate, btnSoundOff, btnMusicOff, btnVibrateOff;
    //public GameObject imgMusicOff, imgSoundOff, imgVibrateOff;
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
        btnMusicOff.OnClick(() =>
        {
            ToggleBtnMusic();
        });
        btnSoundOff.OnClick(() =>
        {
            ToggleBtnSound();
        });
        btnVibrateOff.OnClick(() =>
        {
            ToggleBtnVibrate();
        });
    }

    public override void Init()
    {
        base.Init();
        //imgMusicOff.SetActive(!SaveGame.Music);
        //imgSoundOff.SetActive(!SaveGame.Sound);
        //imgVibrateOff.SetActive(!SaveGame.Vibrate);
        btnMusic.gameObject.SetActive(SaveGame.Music);
        btnSound.gameObject.SetActive(SaveGame.Sound);
        btnVibrate.gameObject.SetActive(SaveGame.Vibrate);

        btnMusicOff.gameObject.SetActive(!SaveGame.Music);
        btnSoundOff.gameObject.SetActive(!SaveGame.Sound);
        btnVibrateOff.gameObject.SetActive(!SaveGame.Vibrate);
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
        StartCoroutine(Wait(btnMusic.gameObject, btnMusicOff.gameObject, SaveGame.Music));

    }

    void ToggleBtnSound()
    {
        if (SaveGame.Sound) ManagerAudio.MuteSound();
        else ManagerAudio.UnMuteSound();

        SaveGame.Sound = !SaveGame.Sound;
        StartCoroutine(Wait(btnSound.gameObject, btnSoundOff.gameObject, SaveGame.Sound));
    }
    void ToggleBtnVibrate()
    {
        //if (SaveGame.Vibrate) ManagerAudio.MuteSound();
        //else ManagerAudio.UnMuteSound();
        SaveGame.Vibrate = !SaveGame.Vibrate;
        StartCoroutine(Wait(btnVibrate.gameObject, btnVibrateOff.gameObject, SaveGame.Vibrate));
    }

    IEnumerator Wait(GameObject btnOn, GameObject btnOff, bool isActive)
    {
        yield return new WaitForSeconds(0.2f);
        btnOn.SetActive(isActive);
        btnOff.SetActive(!isActive);
    }

}
