using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseGame;
using DG.Tweening;
using UnityEngine;
using ntDev;
using Spine.Unity;
using Utilities.Common;

public class PopupReward : Popup
{
    [SerializeField] protected EasyButton btnClaim, btnClaimx2;
    [SerializeField] protected Animator animShow;
    [SerializeField] protected int gold, countMagicWand, countCrytalBall, countMagicCard;
    [SerializeField] protected SkeletonGraphic spineBox;
    [SerializeField] protected GameObject nReward, nBtn;
    [SerializeField] protected List<RotateAroundCenter> listReward;

    [Header("VisualCollectEffect")] [SerializeField]
    protected List<GameObject> pileOfGolds;

    [SerializeField] protected List<GameObject> pileOfCrystals;
    [SerializeField] protected List<GameObject> pileOfMagicWands;
    [SerializeField] protected List<GameObject> pileOfMagicCards;
    public Vector3[] initPosCoin;
    public Quaternion[] initRotCoin;

    public Vector3[] initPosCrystal;
    public Quaternion[] initRotCrystal;

    public Vector3[] initPosMagicCard;
    public Quaternion[] initRotMagicCards;

    public Vector3[] initPosWand;
    public Quaternion[] initRotWand;

    public Transform endPosCoin, endPosBooster, nBlack;


    const string DROP = "drop";

    private void Awake()
    {
        btnClaim.OnClick(() =>
        {
            ClaimReward(1);
            // Hide();
        });

        btnClaimx2.OnClick(() =>
        {
            ClaimReward(2);
            // Hide();
        });
    }

    public static async void Show()
    {
        PopupReward pop = await ManagerPopup.ShowPopup<PopupReward>();
        pop.Init();
    }

    public override void Init()
    {
        // base.Init();
        //ClaimReward();
        ManagerAudio.PlaySound(ManagerAudio.Data.soundRewardDecor);
        this.transform.localScale = Vector3.one;
        Refresh();
        InitPile();
        StartCoroutine(PlayAnimation());
    }

    void InitPile()
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

    public void Reset()
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

    protected virtual void ReceiveReward(int countMagicWand, int countCrytalBall, int countMagicCard,
        bool isPopupRewardDecor = false)
    {
        Reset();
        for (int i = 0; i < listReward.Count; i++)
        {
            listReward[i].isClaim = true;
        }

        nBlack.SetActive(true);

        var sequence = DOTween.Sequence();

        float timeDelay = 0.05f;
        float delaySpawn = 0f;
        float totalSpawnTime = (pileOfGolds.Count - 1) * timeDelay + 0.1f;
        int maxCount = pileOfGolds.Count;


        for (int i = 0; i < maxCount; i++)
        {
            if (i < pileOfGolds.Count)
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
            if (i < pileOfGolds.Count && endPosCoin != null)
            {
                sequence.Insert(delayMove, pileOfGolds[i].GetComponent<RectTransform>()
                    .DOMove(endPosCoin.position, 0.5f)
                    .SetEase(Ease.InOutCirc));
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
                StartCoroutine(RaiseEventClaimDecor(isPopupRewardDecor));
                Reset();
            });

        for (int i = 0; i < maxCount; i++)
        {
            if (i < pileOfGolds.Count)
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


    void Refresh()
    {
        spineBox.gameObject.SetActive(false);
        nReward.SetActive(false);
        nBtn.SetActive(false);
        btnClaim.enabled = true;
        btnClaimx2.enabled = true;
        btnClaimx2.enabled = true;
        for (int i = 0; i < listReward.Count; i++)
        {
            listReward[i].isClaim = false;
        }
    }

    protected virtual void ClaimReward(int multi)
    {
        btnClaim.enabled = false;
        btnClaimx2.enabled = false;
        SaveGame.Coin += multi * gold;
        SaveGame.Hammer += multi * countMagicWand;
        SaveGame.Refresh += multi * countCrytalBall;
        SaveGame.Swap += multi * countMagicCard;

        ReceiveReward(multi * countMagicWand, multi * countCrytalBall, multi * countMagicCard);
    }

    public virtual IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        spineBox.gameObject.SetActive(true);
        PlaySpine(DROP, false);
        yield return new WaitForSeconds(1f);
        if (animShow != null)
            animShow.Play("Show");
    }

    public void PlaySpine(string animationName, bool isLoop = false)
    {
        if (spineBox.AnimationState != null)
        {
            Debug.Log("Play spine");
            spineBox.AnimationState.SetAnimation(0, animationName, isLoop);
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (animShow != null)
            animShow.Play("Default");
    }

    IEnumerator RaiseEventClaimDecor(bool isDecor)
    {
        yield return new WaitForSeconds(0.5f);
        if (isDecor)
            ManagerEvent.RaiseEvent(EventCMD.EVENT_CLAIM_REWARD_BOOK);
        Hide();
    }
}