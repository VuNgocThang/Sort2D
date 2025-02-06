using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using ThangVN;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupReplay : Popup
{
    public EasyButton btnReplay, btnHome;
    [SerializeField] float countdownTimer;
    [SerializeField] TextMeshProUGUI txtHeart;
    [SerializeField] Transform nParentSub;

    public static async void Show()
    {
        PopupReplay pop = await ManagerPopup.ShowPopup<PopupReplay>();
        pop.Init();
    }

    private void Awake()
    {
        btnReplay.OnClick(() =>
        {
            if (SaveGame.Heart > 0)
            {
                btnReplay.enabled = false;
                PlayAnim();
                SaveGame.Heart--;
                if (SaveGame.Heart == GameConfig.MAX_HEART)
                {
                    PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
                }
                LogicGame.Instance.DeleteSaveDataGame();
                StartCoroutine(LoadGame());
            }
            else
            {
                // Logic Heart == 0, Show Popup OutOfHeart
                PopupOutOfHeart.Show();
            }
        });

        btnHome.OnClick(() =>
        {
            ManagerEvent.ClearEvent();
            SceneManager.LoadScene("SceneHome");
        });
    }

    public override void Init()
    {
        base.Init();
        btnReplay.enabled = true;

        InitHeart();
    }

    private void InitHeart()
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
        txtHeart.text = $"{SaveGame.Heart}";
        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            SaveGame.CountDownTimer = countdownTimer;
        }

        if (countdownTimer <= 0 && SaveGame.Heart < GameConfig.MAX_HEART)
        {
            SaveGame.Heart++;
            SaveGame.Heart = Mathf.Min(SaveGame.Heart, GameConfig.MAX_HEART);
            SaveGame.CountDownTimer = GameConfig.TIME_COUNT_DOWN;
            countdownTimer = GameConfig.TIME_COUNT_DOWN;
            PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
        }
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

    void PlayAnim()
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

}
