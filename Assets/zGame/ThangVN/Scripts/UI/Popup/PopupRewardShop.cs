using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using DG.Tweening;
using Utilities.Common;
using BaseGame;
using TMPro;

public class PopupRewardShop : Popup
{
    [SerializeField] TextMeshProUGUI txtCoin;

    [Header("VisualCollectEffect")]
    [SerializeField] private List<GameObject> pileOfGolds;
    [SerializeField] private List<GameObject> pileOfCrystals;
    [SerializeField] private List<GameObject> pileOfMagicWands;
    [SerializeField] private List<GameObject> pileOfMagicCards;
    public Vector3[] initPosCoin;
    public Quaternion[] initRotCoin;

    public Vector3[] initPosCrystal;
    public Quaternion[] initRotCrystal;

    public Vector3[] initPosMagicCard;
    public Quaternion[] initRotMagicCards;

    public Vector3[] initPosWand;
    public Quaternion[] initRotWand;

    public Transform endPosCoin, endPosBooster, nBlack;

    [SerializeField] private int countGold, countCrystal, countMagicCard, countWand, currentCoin;
    [SerializeField] private GameObject nRewardCoin, nRewardCrytal, nRewardWand, nRewardMagicCard;


    public static async void Show(int _countCrytals, int _countMagicCards, int _countWands, int _coinBefore, bool hasCoin = false)
    {
        PopupRewardShop pop = await ManagerPopup.ShowPopup<PopupRewardShop>();
        pop.Init();
        pop.Initialized(_countCrytals, _countMagicCards, _countWands, _coinBefore, hasCoin);
    }

    public override void Init()
    {
        transform.localScale = Vector3.one;
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void Initialized(int _countCrytals, int _countMagicCards, int _countWands, int _coinBefore, bool hasCoin = false)
    {
        currentCoin = _coinBefore;
        txtCoin.text = _coinBefore.ToString();

        if (hasCoin) countGold = 10;

        countCrystal = _countCrytals;
        countMagicCard = _countMagicCards;
        countWand = _countWands;
        InitPile();
        SetActiveParentReward(hasCoin);
        ReceiveReward(_countCrytals, _countMagicCards, _countWands);

    }

    private void SetActiveParentReward(bool hasCoin = false)
    {
        nRewardCoin.SetActive(hasCoin);
        nRewardCrytal.SetActive(countCrystal > 0);
        nRewardWand.SetActive(countWand > 0);
        nRewardMagicCard.SetActive(countMagicCard > 0);
    }

    private void InitPile()
    {
        for (int i = 0; i < pileOfGolds.Count; i++)
        {
            initPosCoin[i] = pileOfGolds[i].transform.localPosition;
            initRotCoin[i] = Quaternion.Euler(0, 0, 0);
        }

        for (int i = 0; i < pileOfCrystals.Count; i++)
        {
            initPosCrystal[i] = pileOfCrystals[i].transform.localPosition;
            initRotCrystal[i] = Quaternion.Euler(0, 0, 0);
        }

        for (int i = 0; i < pileOfMagicCards.Count; i++)
        {
            initPosMagicCard[i] = pileOfMagicCards[i].transform.localPosition;
            initRotMagicCards[i] = Quaternion.Euler(0, 0, 0);
        }

        for (int i = 0; i < pileOfMagicWands.Count; i++)
        {
            initPosWand[i] = pileOfMagicWands[i].transform.localPosition;
            initRotWand[i] = Quaternion.Euler(0, 0, 0);
        }
    }

    public void ResetAll()
    {
        for (int i = 0; i < pileOfGolds.Count; i++)
        {
            pileOfGolds[i].transform.localPosition = initPosCoin[i];
            pileOfGolds[i].transform.rotation = initRotCoin[i];
        }

        for (int i = 0; i < pileOfCrystals.Count; i++)
        {
            pileOfCrystals[i].transform.localPosition = initPosCrystal[i];
            pileOfCrystals[i].transform.rotation = initRotCrystal[i];
        }

        for (int i = 0; i < pileOfMagicCards.Count; i++)
        {
            pileOfMagicCards[i].transform.localPosition = initPosMagicCard[i];
            pileOfMagicCards[i].transform.rotation = initRotMagicCards[i];
        }

        for (int i = 0; i < pileOfMagicWands.Count; i++)
        {
            pileOfMagicWands[i].transform.localPosition = initPosWand[i];
            pileOfMagicWands[i].transform.rotation = initRotWand[i];
        }
    }

    private void ReceiveReward(int countMagicWand, int countCrytalBall, int countMagicCard)
    {
        ResetAll();

        nBlack.SetActive(true);

        var sequence = DOTween.Sequence();

        float timeDelay = 0.05f;
        float delaySpawn = 0f;
        float totalSpawnTime = (pileOfGolds.Count - 1) * timeDelay + 0.1f;
        int maxCount = pileOfGolds.Count;


        for (int i = 0; i < maxCount; i++)
        {
            if (i < countGold)
            {
                sequence.Insert(delaySpawn, pileOfGolds[i].transform.DOScale(1f, 0.2f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < countMagicWand)
            {
                sequence.Insert(delaySpawn, pileOfMagicWands[i].transform.DOScale(1f, 0.2f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < countCrytalBall)
            {
                sequence.Insert(delaySpawn, pileOfCrystals[i].transform.DOScale(1f, 0.2f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < countMagicCard)
            {
                sequence.Insert(delaySpawn, pileOfMagicCards[i].transform.DOScale(1f, 0.2f)
                    .SetEase(Ease.InOutCirc));
            }

            delaySpawn += 0.05f;
        }

        var delayMove = totalSpawnTime;
        for (int i = 0; i < maxCount; i++)
        {
            if (i < countGold && endPosCoin != null)
            {
                var coinTween = pileOfGolds[i].GetComponent<RectTransform>()
                   .DOMove(endPosCoin.position, 0.5f)
                   .SetEase(Ease.InOutCirc);

                if (i == 0)
                {
                    coinTween.OnComplete(() => { UpdateMoney(SaveGame.Coin); });
                }

                sequence.Insert(delayMove, coinTween);
            }

            if (i < countMagicWand && endPosBooster != null)
            {
                sequence.Insert(delayMove, pileOfMagicWands[i].GetComponent<RectTransform>()
                    .DOMove(endPosBooster.position, 0.5f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < countCrytalBall && endPosBooster != null)
            {
                sequence.Insert(delayMove, pileOfCrystals[i].GetComponent<RectTransform>()
                    .DOMove(endPosBooster.position, 0.5f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < countMagicCard && endPosBooster != null)
            {
                sequence.Insert(delayMove, pileOfMagicCards[i].GetComponent<RectTransform>()
                    .DOMove(endPosBooster.position, 0.5f)
                    .SetEase(Ease.InOutCirc));
            }

            delayMove += 0.05f;
        }

        sequence.AppendInterval(0.05f)
            .OnComplete(() =>
            {
                ManagerAudio.PlaySound(ManagerAudio.Data.soundClaimGold);
                nBlack.SetActive(false);
                ResetAll();
                Hide();
            });

        for (int i = 0; i < maxCount; i++)
        {
            if (i < countGold)
            {
                sequence.Join(pileOfGolds[i].transform.DOScale(0f, 0.3f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < countMagicWand)
            {
                sequence.Join(pileOfMagicWands[i].transform.DOScale(0f, 0.3f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < countCrytalBall)
            {
                sequence.Join(pileOfCrystals[i].transform.DOScale(0f, 0.3f)
                    .SetEase(Ease.InOutCirc));
            }

            if (i < countMagicCard)
            {
                sequence.Join(pileOfMagicCards[i].transform.DOScale(0f, 0.3f)
                    .SetEase(Ease.InOutCirc));
            }
        }
    }

    private Coroutine tween;
    float duration = 0.5f;


    private void UpdateMoney(int targetMoney)
    {
        ManagerAudio.PlaySound(ManagerAudio.Data.soundClaimGold);
        if (tween != null)
        {
            StopCoroutine(tween);
        }

        tween = StartCoroutine(CountMoney(currentCoin, targetMoney, duration));
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
