using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BaseGame;
using TMPro;
using UnityEngine.XR;

public class PopupFreeCoin : Popup
{
    public FreeCoinData freeCoinData;
    public ItemFreeCoin itemFreeCoin;
    public Transform nContent;
    public List<ItemFreeCoin> listItem;
    public EasyButton btnClaime50, btnClosePopup;
    [SerializeField] GameObject imgActive, imgDeactive, imgClaimed, icon, bgCollectCoin, hand;

    [Header("CollectCoin")] public List<GameObject> pileOfCoins;
    public Vector3[] initPosCoin;
    public Quaternion[] initRotCoin;
    public Transform endPosCoin;
    [SerializeField] private Transform nParentCollectCoin;

    [SerializeField] private int currentCoin;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] TextMeshProUGUI txtGold;

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_FREECOIN, UpdateListContent);
        btnClaime50.OnClick(() =>
        {
            if (!SaveGame.DataFreeCoin.isClaimed50)
            {
                hand.SetActive(false);

                if (DailyTaskManager.Instance != null)
                    DailyTaskManager.Instance.ExecuteDailyTask(TaskType.CollectFreeCoins, 1);
                Claimed50();
            }
        });

        btnClosePopup.OnClick(Hide);
    }

    public static async Task<bool> Show()
    {
        PopupFreeCoin pop = await ManagerPopup.ShowPopup<PopupFreeCoin>();
        pop.Init();

        return true;
    }

    public override void Init()
    {
        Debug.Log("1");
        // base.Init();
        transform.localScale = Vector3.one;
        RefreshData();
        InitClaim50();
        InitListItemFreeCoin();
        InitFirstTutorial();
        SaveGame.IsTutFreeCoin = true;
        SaveGame.ShowFreeCoin = true;
    }

    private void InitListItemFreeCoin()
    {
        listItem.Clear();

        for (int i = 0; i < freeCoinData.listDataFreeCoin.Count; i++)
        {
            ItemFreeCoin item = Instantiate(itemFreeCoin, nContent);
            listItem.Add(item);
            item.isClaimed = freeCoinData.listDataFreeCoin[i].isClaimed;
            item.countCoin = freeCoinData.listDataFreeCoin[i].countCoin;
            item.index = freeCoinData.listDataFreeCoin[i].index;
            item.Show(SaveGame.DataFreeCoin.currentIndex, this);
        }
    }


    public override void Hide()
    {
        transform.localScale = Vector3.one;

        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            for (int i = 0; i < listItem.Count; i++)
            {
                listItem[i].gameObject.SetActive(false);
            }

            gameObject.SetActive(false);
            ManagerEvent.RaiseEvent(EventCMD.EVENT_POPUP_CLOSE, this);
        });
    }

    public void UpdateListContent(object e)
    {
        hand.SetActive(false);

        for (int i = 0; i < listItem.Count; i++)
        {
            if (listItem[i].index == SaveGame.DataFreeCoin.currentIndex)
            {
                listItem[i].imgActive.SetActive(true);
                listItem[i].imgInActive.SetActive(false);
            }
        }
    }

    private void Claimed50()
    {
        currentCoin = SaveGame.Coin;
        GameManager.AddGold(50);
        SaveGame.DataFreeCoin.isClaimed50 = true;
        SaveGame.DataFreeCoin = SaveGame.DataFreeCoin;

        InitClaim50();
        ReceiveReward(btnClaime50.transform);
    }

    private void InitClaim50()
    {
        if (SaveGame.DataFreeCoin.isClaimed50)
        {
            imgActive.SetActive(false);
            imgDeactive.SetActive(true);
            icon.SetActive(false);
            imgClaimed.SetActive(true);
        }
        else
        {
            imgActive.SetActive(true);
            imgDeactive.SetActive(false);
            icon.SetActive(true);
            imgClaimed.SetActive(false);
        }
    }

    private void InitFirstTutorial()
    {
        hand.SetActive(!SaveGame.IsTutFreeCoin);
    }

    private void RefreshData()
    {
        txtGold.text = SaveGame.Coin.ToString();

        if (SaveGame.NewDayFreeCoin == DateTime.Now.DayOfYear) return;

        SaveGame.ShowFreeCoin = false;

        SaveGame.NewDayFreeCoin = DateTime.Now.DayOfYear;
        SaveGame.DataFreeCoin.listDataFreeCoin.Clear();
        SaveGame.DataFreeCoin.currentIndex = 0;
        SaveGame.DataFreeCoin.isClaimed50 = false;

        SaveGame.DataFreeCoin = SaveGame.DataFreeCoin;
        ManagerEvent.RaiseEvent(EventCMD.EVENT_FREECOIN);
    }

    public void Reset()
    {
        for (int i = 0; i < pileOfCoins.Count; i++)
        {
            pileOfCoins[i].transform.localPosition = initPosCoin[i];
            pileOfCoins[i].transform.rotation = initRotCoin[i];
        }
    }

    public void ReceiveReward(Transform nParent)
    {
        bgCollectCoin.SetActive(true);
        nParentCollectCoin.transform.position = nParent.transform.position;
        nParentCollectCoin.transform.position = nParent.transform.position;
        Reset();

        var sequence = DOTween.Sequence();

        var delaySpawn = 0f;
        float totalSpawnTime = (pileOfCoins.Count - 1) * 0.05f + 0.1f;

        for (int i = 0; i < pileOfCoins.Count; i++)
        {
            sequence.Insert(delaySpawn, pileOfCoins[i].transform.DOScale(1f, 0.2f)
                .SetEase(Ease.InOutCirc));
            delaySpawn += 0.05f;
        }

        var delayMove = totalSpawnTime;
        for (int i = 0; i < pileOfCoins.Count; i++)
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

            delayMove += 0.05f;
        }

        sequence.AppendInterval(0.05f)
            .OnComplete(() =>
            {
                bgCollectCoin.SetActive(false);
                // UpdateMoney(SaveGame.Coin);
            });

        for (int i = 0; i < pileOfCoins.Count; i++)
        {
            sequence.Join(pileOfCoins[i].transform.DOScale(0f, 0.3f)
                .SetEase(Ease.InOutCirc));
        }
    }

    private void UpdateMoney(int targetMoney)
    {
        ManagerAudio.PlaySound(ManagerAudio.Data.soundClaimGold);
        if (this.gameObject.activeSelf)
            StartCoroutine(CountMoney(currentCoin, targetMoney, duration));
    }

    private IEnumerator CountMoney(int start, int end, float duration)
    {
        var elapsed = 0.0f;

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
}