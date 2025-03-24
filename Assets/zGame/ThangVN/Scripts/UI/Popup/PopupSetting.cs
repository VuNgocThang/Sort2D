using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.SceneManagement;
using BaseGame;

public class PopupSetting : Popup
{
    public EasyButton btnSound, btnMusic, btnVibrate, btnReplay, btnHome, btnSoundOff, btnMusicOff, btnVibrateOff;
    public static async void Show()
    {
        PopupSetting pop = await ManagerPopup.ShowPopup<PopupSetting>();
        pop.Init();
    }

    private void Awake()
    {
        btnMusic.OnClick(ToggleBtnMusic);
        btnSound.OnClick(ToggleBtnSound);
        btnVibrate.OnClick(ToggleBtnVibrate);
        btnMusicOff.OnClick(ToggleBtnMusic);
        btnSoundOff.OnClick(ToggleBtnSound);
        btnVibrateOff.OnClick(ToggleBtnVibrate);

        btnReplay.OnClick(() =>
        {
            if (!LogicGame.Instance.isWin)
                LogicGame.Instance.SaveDataGame();
            PopupReplay.Show();
        });

        btnHome.OnClick(() =>
        {
            if (SaveGame.Level >= GameConfig.LEVEL_INTER)
                SaveGame.CanShowInter = true;

            if (!LogicGame.Instance.isWin)
                LogicGame.Instance.SaveDataGame();
            ManagerEvent.ClearEvent();

            SceneManager.LoadScene("SceneHome");
        });
    }

    public override void Init()
    {
        base.Init();
        LogicGame.Instance.isPauseGame = true;
        btnMusic.gameObject.SetActive(SaveGame.Music);
        btnSound.gameObject.SetActive(SaveGame.Sound);
        btnVibrate.gameObject.SetActive(SaveGame.Vibrate);

        btnMusicOff.gameObject.SetActive(!SaveGame.Music);
        btnSoundOff.gameObject.SetActive(!SaveGame.Sound);
        btnVibrateOff.gameObject.SetActive(!SaveGame.Vibrate);
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
        SaveGame.Vibrate = !SaveGame.Vibrate;
        StartCoroutine(Wait(btnVibrate.gameObject, btnVibrateOff.gameObject, SaveGame.Vibrate));
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Wait(GameObject btnOn, GameObject btnOff, bool isActive)
    {
        yield return new WaitForSeconds(0.2f);
        btnOn.SetActive(isActive);
        btnOff.SetActive(!isActive);
    }
}