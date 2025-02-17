using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.SceneManagement;
using DG.Tweening;
using BaseGame;
using TMPro;
using Spine.Unity;

public class PopupWin : Popup
{
    public EasyButton btnContinue, btnClaimX2, btnHome;
    public TextMeshProUGUI txtGoldReward, txtPigmentReward, txtGold, txtPigment;
    public Transform vfx;
    public SkeletonGraphic spine;
    const string GLOW = "glow";

    // effect coin
    [Header("EffectCoin")] public GameObject pileCoin;
    public List<GameObject> pileOfCoins;
    public Vector3[] initPosCoin;
    public Quaternion[] initRotCoin;
    public Transform endPosCoin;

    //effect pigment
    [Header("EffectPigment")] public GameObject pilePigment;
    public List<GameObject> pileOfPigment;
    public Vector3[] initPosPigment;
    public Quaternion[] initRotPigment;
    public Transform endPosPigment;

    //
    int currentCoin;
    int currentPigment;
    float duration = 0.5f;

    public static async void Show()
    {
        PopupWin pop = await ManagerPopup.ShowPopup<PopupWin>();
        pop.Init();
    }

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_RECEIVE_REWARD, LoadSceneBonusLevel);

        btnContinue.OnClick(() =>
        {
            if (GameManager.ShowPopupBonus())
                PopupBonusLevel.Show();
            else
            {
                ManagerEvent.ClearEvent();
                if (SaveGame.Level == 2)
                {
                    InitPile();
                    ReceiveReward("SceneHome");
                }
                else
                {
                    InitPile();
                    ReceiveReward("SceneGame");
                }
            }
        });

        btnHome.OnClick(() =>
        {
            ManagerEvent.ClearEvent();

            InitPile();
            ReceiveReward("SceneHome");
        });

        // btnClaimX2.OnClick(() =>
        // {
        //     GameManager.AddGold(LogicGame.Instance.gold);
        //     GameManager.AddPigment(LogicGame.Instance.pigment);
        //     //InitPile();
        //     //ReceiveReward();
        //
        //     ManagerEvent.ClearEvent();
        //     if (GameManager.ShowPopupBonus())
        //         PopupBonusLevel.Show();
        //     else
        //     {
        //         StartCoroutine(LoadScene("SceneGame"));
        //     }
        // });
    }

    public override void Init()
    {
        base.Init();
        ManagerAudio.PlaySound(ManagerAudio.Data.soundPaperFireWorks);
        ManagerAudio.PlaySound(ManagerAudio.Data.soundPopupWin);

        Debug.Log("init popup win");

        currentCoin = SaveGame.Coin;
        currentPigment = SaveGame.Pigment;

        txtGold.text = SaveGame.Coin.ToString();
        txtPigment.text = SaveGame.Pigment.ToString();

        txtGoldReward.text = LogicGame.Instance.gold.ToString();
        txtPigmentReward.text = LogicGame.Instance.pigment.ToString();

        GameManager.AddGold(LogicGame.Instance.gold);
        GameManager.AddPigment(LogicGame.Instance.pigment);

        if (DailyTaskManager.Instance != null)
            DailyTaskManager.Instance.ExecuteDailyTask(TaskType.CollectBooks, LogicGame.Instance.pigment);
    }

    private void Update()
    {
        if (vfx != null)
            vfx.Rotate(new Vector3(0, 0, 1) * -20f * Time.deltaTime);
    }

    IEnumerator LoadScene(string sceneName)
    {
        // yield return new WaitForSeconds(10.5f);
        // yield return new WaitForSeconds(duration);
        yield return null;
        ManagerEvent.ClearEvent();
        SceneManager.LoadScene(sceneName);
    }

    private void LoadSceneBonusLevel(object e)
    {
        string sceneName = e as string;
        InitPile();
        ReceiveReward(sceneName);
    }

    public void InitPile()
    {
        for (int i = 0; i < pileOfCoins.Count; i++)
        {
            initPosCoin[i] = pileOfCoins[i].transform.position;
            initRotCoin[i] = Quaternion.Euler(0, 0, 0);
        }

        for (int i = 0; i < pileOfPigment.Count; i++)
        {
            initPosPigment[i] = pileOfPigment[i].transform.position;
            initRotPigment[i] = Quaternion.Euler(0, 0, 0);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < pileOfCoins.Count; i++)
        {
            pileOfCoins[i].transform.position = initPosCoin[i];
            pileOfCoins[i].transform.rotation = initRotCoin[i];
        }

        for (int i = 0; i < pileOfPigment.Count; i++)
        {
            pileOfPigment[i].transform.position = initPosPigment[i];
            pileOfPigment[i].transform.rotation = initRotPigment[i];
        }
    }

    public void ReceiveReward(string sceneName)
    {
        Reset();

        var sequence = DOTween.Sequence();

        var delaySpawn = 0f;
        float totalSpawnTime = (pileOfCoins.Count - 1) * 0.05f + 0.1f;
        int maxCount = Mathf.Max(pileOfCoins.Count, pileOfPigment.Count);

        for (int i = 0; i < maxCount; i++)
        {
            if (i < pileOfCoins.Count)
            {
                sequence.Insert(delaySpawn, pileOfCoins[i].transform.DOScale(1f, 0.2f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < pileOfPigment.Count)
            {
                sequence.Insert(delaySpawn, pileOfPigment[i].transform.DOScale(1f, 0.2f)
                    .SetEase(Ease.InOutCirc));
            }

            delaySpawn += 0.05f;
        }

        var delayMove = totalSpawnTime;
        for (int i = 0; i < maxCount; i++)
        {
            if (i < pileOfCoins.Count && endPosCoin != null)
            {
                var coinTween = pileOfCoins[i].GetComponent<RectTransform>()
                    .DOMove(endPosCoin.position, 0.5f)
                    .SetEase(Ease.InOutCirc);

                if (i == 0)
                {
                    coinTween.OnComplete(() => { UpdateMoney(SaveGame.Coin); });
                }

                sequence.Insert(delayMove, coinTween);
            }

            if (i < pileOfPigment.Count && endPosPigment != null)
            {
                var pigmentTween = pileOfPigment[i].GetComponent<RectTransform>()
                    .DOMove(endPosPigment.position, 0.5f)
                    .SetEase(Ease.InOutCirc);

                if (i == 0)
                {
                    pigmentTween.OnComplete(() => { UpdatePigment(SaveGame.Pigment); });
                }

                sequence.Insert(delayMove, pigmentTween);
            }

            delayMove += 0.05f;
        }

        sequence.AppendInterval(0.05f)
            .OnComplete(() =>
            {
                // UpdateMoney(SaveGame.Coin);
                // UpdatePigment(SaveGame.Pigment);

                StartCoroutine(LoadScene(sceneName));
            });

        for (int i = 0; i < maxCount; i++)
        {
            if (i < pileOfCoins.Count)
            {
                sequence.Join(pileOfCoins[i].transform.DOScale(0f, 0.3f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < pileOfPigment.Count)
            {
                sequence.Join(pileOfPigment[i].transform.DOScale(0f, 0.3f)
                    .SetEase(Ease.InOutCirc));
            }
        }
    }

    private Coroutine tween;

    private void UpdateMoney(int targetMoney)
    {
        ManagerAudio.PlaySound(ManagerAudio.Data.soundClaimGold);
        if (tween != null)
        {
            StopCoroutine(tween);
        }
        tween = StartCoroutine(CountMoney(currentCoin, targetMoney, duration));
    }

    private void UpdatePigment(int targetPigment)
    {
        ManagerAudio.PlaySound(ManagerAudio.Data.soundClaimPigment);

        StartCoroutine(CountPigment(currentPigment, targetPigment, duration));
    }

    private IEnumerator CountMoney(int start, int end, float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentCoin = (int)Mathf.Lerp(start, end, elapsed / duration);
            txtGold.text = currentCoin.ToString();
            yield return null;
        }

        currentCoin = end;
        txtGold.text = currentCoin.ToString();
    }

    private IEnumerator CountPigment(int start, int end, float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentPigment = (int)Mathf.Lerp(start, end, elapsed / duration);
            txtPigment.text = currentPigment.ToString();
            yield return null;
        }

        currentPigment = end;
        txtPigment.text = currentPigment.ToString();
    }

    public void PlaySpineAnimation()
    {
        spine.AnimationState.SetAnimation(0, GLOW, false);
    }
}