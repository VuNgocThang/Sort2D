using BaseGame;
using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PopupRestart : Popup
{
    public EasyButton btnRestart, btnHome;
    [SerializeField] float countdownTimer;
    [SerializeField] TextMeshProUGUI txtHeart;
    [SerializeField] Transform nParentSub;


    public static async void Show()
    {
        PopupRestart pop = await ManagerPopup.ShowPopup<PopupRestart>();
        pop.Init();
    }

    private void Awake()
    {
        btnRestart.OnClick(() =>
        {
            btnRestart.enabled = false;

            if (SaveGame.Heart > 0)
            {
                ManagerEvent.RaiseEvent(EventCMD.EVENT_SUB_HEART);
                txtHeart.text = SaveGame.Heart.ToString();
                StartCoroutine(LoadGame());
            }
            else
            {
                ManagerEvent.RaiseEvent(EventCMD.EVENT_SUB_HEART);
                txtHeart.text = SaveGame.Heart.ToString();
                StartCoroutine(ShowPopupOutOfHeart());
            }
        });

        btnHome.OnClick(() =>
        {
            ManagerEvent.ClearEvent();
            SceneManager.LoadScene("SceneHome");
        });

        ManagerEvent.RegEvent(EventCMD.EVENT_SUB_HEART, PlayAnimSubHeart);
    }

    public override void Init()
    {
        base.Init();
        ManagerPopup.HidePopup<PopupLose>();
        InitHeart();
        ShowHeart();
    }

    void InitHeart()
    {
        if (PlayerPrefs.HasKey(GameConfig.LAST_HEART_LOSS))
        {
            float timeSinceLastLoss = (float)(DateTime.Now - DateTime.Parse(PlayerPrefs.GetString(GameConfig.LAST_HEART_LOSS))).TotalSeconds;

            int increaseHeart = (int)(timeSinceLastLoss / GameConfig.TIME_COUNT_DOWN);

            float timeSub = timeSinceLastLoss % GameConfig.TIME_COUNT_DOWN;

            if (GameConfig.MAX_HEART >= SaveGame.Heart)
            {
                SaveGame.Heart += increaseHeart;
                SaveGame.Heart = Mathf.Min(SaveGame.Heart, GameConfig.MAX_HEART);
            }
            countdownTimer = SaveGame.CountDownTimer - timeSub;
            countdownTimer = Mathf.Max(countdownTimer, 0);

            if (SaveGame.Heart >= GameConfig.MAX_HEART)
            {
                countdownTimer = GameConfig.TIME_COUNT_DOWN;
            }
        }
        else
        {
            countdownTimer = GameConfig.TIME_COUNT_DOWN;
        }
    }

    private void Update()
    {
        Debug.Log("saveHeart" + SaveGame.Heart);
        if (SaveGame.Heart >= GameConfig.MAX_HEART)
        {
            //txtCountdownHeart.text = "FULL";
        }
        else
        {
            if (countdownTimer > 0)
            {
                countdownTimer -= Time.deltaTime;

                float minutes = Mathf.Floor(countdownTimer / 60);
                float seconds = Mathf.RoundToInt(countdownTimer % 60);

                //txtCountdownHeart.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            }

            if (countdownTimer <= 0 && SaveGame.Heart < GameConfig.MAX_HEART)
            {
                SaveGame.Heart++;
                ShowHeart();
                countdownTimer = GameConfig.TIME_COUNT_DOWN;
                PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            }
        }
    }

    public void ShowHeart()
    {
        int heart = SaveGame.Heart + 1;
        if (heart >= 5) heart = 5;
        txtHeart.text = heart.ToString();
    }

    void PlayAnimSubHeart(object e)
    {
        GameObject obj = PoolManager.Spawn(ScriptableObjectData.ObjectConfig.GetObject(EnumObject.SUBHEART));
        obj.transform.SetParent(nParentSub);
        obj.transform.localPosition = Vector3.zero;
        obj.gameObject.SetActive(true);
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(0.5f);
        ManagerEvent.ClearEvent();
        LoadScene("SceneGame");
    }

    IEnumerator ShowPopupOutOfHeart()
    {
        yield return new WaitForSeconds(0.5f);
        PopupOutOfHeart.Show();
    }

    public override void Hide()
    {
        base.Hide();
        Debug.Log("Hide popup Restart");
    }

    void LoadScene(string strScene)
    {
        transform.localScale = Vector3.one;

        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
            ManagerEvent.ClearEvent();
            SceneManager.LoadScene(strScene);
        });
    }

}
