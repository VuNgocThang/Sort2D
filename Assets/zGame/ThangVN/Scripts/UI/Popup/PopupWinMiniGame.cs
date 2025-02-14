using ntDev;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using BaseGame;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PopupWinMiniGame : Popup
{
    [SerializeField] EasyButton btnContinue, btnHome;
    [SerializeField] SkeletonGraphic spineBox;
    [SerializeField] Animator animShow;
    [SerializeField] TextMeshProUGUI txtCoin, txtPigment;
    [SerializeField] Reward rewardPrefab;
    [SerializeField] Transform nReward;
    [SerializeField] private List<Reward> listReward;
    [SerializeField] CustomerMissionData customerMissionData;

    const string DROP = "drop";
    const string IDLE = "idle";

    private Sprite spriteBooster;
    private int countBooster;
    [SerializeField] private GameObject nPiles;

    // effect coin
    [Header("EffectCoin")] public List<GameObject> pileOfCoins;
    public Vector3[] initPosCoin;
    public Quaternion[] initRotCoin;
    public Transform endPosCoin, nCoin;

    //effect booster
    [Header("EffectBooster")] public List<Image> pileOfBoosters;
    public Vector3[] initPosBooster;
    public Quaternion[] initRotBooster;
    public Transform endPosBooster, nBooster;

    //
    int currentCoin;
    int currentBooster;
    float duration = 1f;

    private void Awake()
    {
        btnContinue.OnClick(() => { PlayAgain(); });

        btnHome.OnClick(() => { Continue(); });
    }

    public static async void Show()
    {
        PopupWinMiniGame pop = await ManagerPopup.ShowPopup<PopupWinMiniGame>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        currentCoin = SaveGame.Coin;
        txtCoin.text = $"{SaveGame.Coin}";
        txtPigment.text = $"{SaveGame.Pigment}";
        InitPile();
        InitReward();
        StartCoroutine(PlayAnimation());
    }

    void InitReward()
    {
        List<DataReward> rewards = customerMissionData.listLevelBonus[SaveGame.LevelBonus - 10001].listRewards;

        for (int i = 0; i < rewards.Count; i++)
        {
            Reward reward = Instantiate(rewardPrefab, nReward);
            reward.Init(rewards[i].typeReward, rewards[i].count);
            if (rewards[i].typeReward != TypeReward.GOLD)
            {
                spriteBooster = reward.ShowReward(reward.typeReward, reward.count);
                countBooster = reward.count;
            }

            listReward.Add((reward));
        }
    }

    public override void Hide()
    {
        base.Hide();
    }

    void PlayAgain()
    {
        animShow.enabled = false;
        ReceiveReward("SceneGame");
    }

    void Continue()
    {
        animShow.enabled = false;
        ReceiveReward("SceneHome");
    }

    IEnumerator LoadScene(string sceneName)
    {
        // yield return new WaitForSeconds(duration);
        yield return null;
        SaveGame.PlayBonus = false;
        ManagerEvent.ClearEvent();
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        spineBox.gameObject.SetActive(true);
        PlaySpine(DROP, false);
        yield return new WaitForSeconds(1f);
        if (animShow != null)
            animShow.Play("Show");

        yield return new WaitForSeconds(0.5f);
        if (animShow != null)
            animShow.Play("Move");
    }

    public void PlaySpine(string animationName, bool isLoop = false)
    {
        if (spineBox.AnimationState != null)
        {
            Debug.Log("Play spine");
            spineBox.AnimationState.SetAnimation(0, animationName, isLoop);
        }
    }

    void InitPile()
    {
        for (int i = 0; i < pileOfCoins.Count; i++)
        {
            initPosCoin[i] = pileOfCoins[i].transform.localPosition;
            initRotCoin[i] = Quaternion.Euler(0, 0, 0);
        }

        for (int i = 0; i < pileOfBoosters.Count; i++)
        {
            initPosBooster[i] = pileOfBoosters[i].transform.localPosition;
            initRotBooster[i] = Quaternion.Euler(0, 0, 0);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < pileOfCoins.Count; i++)
        {
            pileOfCoins[i].transform.localPosition = initPosCoin[i];
            pileOfCoins[i].transform.rotation = initRotCoin[i];
        }

        for (int i = 0; i < pileOfBoosters.Count; i++)
        {
            pileOfBoosters[i].sprite = spriteBooster;
            pileOfBoosters[i].SetNativeSize();
            pileOfBoosters[i].transform.localPosition = initPosBooster[i];
            pileOfBoosters[i].transform.rotation = initRotBooster[i];
        }
    }

    public void ReceiveReward(string sceneName)
    {
        Reset();
        nPiles.SetActive(true);
        nCoin.position = listReward[0].transform.position;

        if (listReward.Count > 1)
            nBooster.position = listReward[1].transform.position;

        var sequence = DOTween.Sequence();

        var delaySpawn = 0f;
        float totalSpawnTime = (pileOfCoins.Count - 1) * 0.05f + 0.1f;
        int maxCount = Mathf.Max(pileOfCoins.Count, countBooster);

        for (int i = 0; i < maxCount; i++)
        {
            if (i < pileOfCoins.Count)
            {
                sequence.Insert(delaySpawn, pileOfCoins[i].transform.DOScale(1f, 0.2f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < countBooster)
            {
                sequence.Insert(delaySpawn, pileOfBoosters[i].transform.DOScale(1f, 0.2f)
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

            if (i < countBooster && endPosBooster != null)
            {
                sequence.Insert(delayMove, pileOfBoosters[i].GetComponent<RectTransform>()
                    .DOMove(endPosBooster.position, 0.5f)
                    .SetEase(Ease.InOutCirc));
            }

            delayMove += 0.05f;
        }

        sequence.AppendInterval(0.05f)
            .OnComplete(() =>
            {
                nPiles.SetActive(false);
                // UpdateMoney(SaveGame.Coin);
                StartCoroutine(LoadScene(sceneName));
            });

        for (int i = 0; i < maxCount; i++)
        {
            if (i < pileOfCoins.Count)
            {
                sequence.Join(pileOfCoins[i].transform.DOScale(0f, 0.3f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < countBooster)
            {
                sequence.Join(pileOfBoosters[i].transform.DOScale(0f, 0.3f)
                    .SetEase(Ease.InOutCirc));
            }
        }
    }

    public void UpdateMoney(int targetMoney)
    {
        ManagerAudio.PlaySound(ManagerAudio.Data.soundClaimGold);
        StartCoroutine(CountMoney(currentCoin, targetMoney, duration));
    }

    private IEnumerator CountMoney(int start, int end, float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentCoin = (int)Mathf.Lerp(start, end, elapsed / duration);
            txtCoin.text = currentCoin.ToString();
            yield return null;
        }

        currentCoin = end;
        txtCoin.text = currentCoin.ToString();
    }
}