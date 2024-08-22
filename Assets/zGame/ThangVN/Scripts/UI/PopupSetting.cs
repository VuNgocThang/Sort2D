using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.SceneManagement;
using BaseGame;

public class PopupSetting : Popup
{
    public EasyButton btnSound, btnMusic, btnVibrate, btnRestart, btnHome;
    public GameObject imgMusicOff, imgSoundOff, imgVibrateOff;
    public static async void Show()
    {
        PopupSetting pop = await ManagerPopup.ShowPopup<PopupSetting>();
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

        btnRestart.OnClick(() =>
        {
            ManagerEvent.ClearEvent();
            StartCoroutine(LoadScene("SceneGame"));
        });

        btnHome.OnClick(() =>
        {
            ManagerEvent.ClearEvent();
            StartCoroutine(LoadScene("SceneHome"));
        });
    }

    public override void Init()
    {
        base.Init();
        LogicGame.Instance.isPauseGame = true;
        Debug.Log("init setting in game");
        imgMusicOff.SetActive(!SaveGame.Music);
        imgSoundOff.SetActive(!SaveGame.Sound);
        imgVibrateOff.SetActive(!SaveGame.Vibrate);
    }

    public override void Hide()
    {
        base.Hide();
        LogicGame.Instance.isPauseGame = false;
    }

    void ToggleBtnMusic()
    {
        if (SaveGame.Music) ManagerAudio.MuteMusic();
        else
        {
            ManagerAudio.PlayMusic(ManagerAudio.Data.musicInGame);
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

    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneName);
    }
}
