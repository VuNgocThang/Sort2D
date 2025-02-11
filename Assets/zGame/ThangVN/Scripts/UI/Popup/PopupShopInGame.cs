using BaseGame;
using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupShopInGame : Popup
{
    [SerializeField] EasyButton btnPrev, btnNext, btnBuyUseCoin, btnBuyAds;
    [SerializeField] BoosterData boosterData;
    public Image imgIcon, imgIconBuyUseIcon, imgIconBuyAds;
    public TextMeshProUGUI txtNameBooster, txtCoinUse, txtCoin;
    public BoosterEnum boosterEnum;
    public CanvasGroup canvasGroup;
    public Transform nParentSubGold;

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_SUB_GOLD, PlayAnimSubGold);
    }

    public static async void Show(int index)
    {
        PopupShopInGame pop = await ManagerPopup.ShowPopup<PopupShopInGame>();

        pop.Initialized(index);
    }


    public void Initialized(int index)
    {
        base.Init();
        txtCoin.text = SaveGame.Coin.ToString();
        LogicGame.Instance.isPauseGame = true;
        canvasGroup.blocksRaycasts = true;
        boosterEnum = (BoosterEnum)index;
        btnBuyUseCoin.enabled = true;
        btnBuyUseCoin.enabled = true;

        btnBuyUseCoin.OnClick(() =>
        {
            btnBuyUseCoin.enabled = false;
            btnBuyAds.enabled = false;

            BuyUseCoin(boosterEnum, false);
            StartCoroutine(RaiseEventHide());
            //Hide();
        });
        btnBuyAds.OnClick(() =>
        {
            BuyUseCoin(boosterEnum, true);
            StartCoroutine(RaiseEventHide());
            //Hide();
        });


        for (int i = 0; i < boosterData.listBooster.Count; i++)
        {
            if (index == (int)boosterData.listBooster[i].boosterEnum)
            {
                imgIcon.sprite = boosterData.listBooster[i].spriteBooster;
                imgIcon.SetNativeSize();
                imgIconBuyUseIcon.sprite = boosterData.listBooster[i].spriteIcon;
                imgIconBuyAds.sprite = boosterData.listBooster[i].spriteIcon;
                txtNameBooster.text = boosterData.listBooster[i].nameBooster;
                txtCoinUse.text = boosterData.listBooster[i].cost.ToString();
            }
        }
    }

    IEnumerator RaiseEventHide()
    {
        yield return new WaitForSeconds(0.5f);
        Hide();
    }

    public override void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        transform.localScale = Vector3.one;

        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
            ManagerEvent.RaiseEvent(EventCMD.EVENT_POPUP_CLOSE, this);
            LogicGame.Instance.isPauseGame = false;
        });
    }

    public void PlayAnimSubGold(object e)
    {
        GameObject obj = PoolManager.Spawn(ScriptableObjectData.ObjectConfig.GetObject(EnumObject.SUBGOLD));
        obj.transform.SetParent(nParentSubGold);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        SubGold subBook = obj.GetComponent<SubGold>();
        subBook.Init((int)e);
        subBook.gameObject.SetActive(true);
    }

    public void BuyUseCoin(BoosterEnum boosterEnum, bool useAds)
    {
        switch (boosterEnum)
        {
            case BoosterEnum.BoosterSwap:
                if (useAds)
                {
                    SaveGame.Swap++;
                }
                else
                {
                    if (GameConfig.EnoughCoinBuySwap)
                    {
                        Debug.Log("swap");
                        ManagerAudio.PlaySound(ManagerAudio.Data.soundDropGold);
                        //SaveGame.Coin -= GameConfig.COIN_SWAP;
                        ManagerEvent.RaiseEvent(EventCMD.EVENT_SUB_GOLD, GameConfig.COIN_SWAP);
                        GameManager.SubGold(GameConfig.COIN_SWAP);
                        txtCoin.text = SaveGame.Coin.ToString();
                        SaveGame.Swap++;
                    }
                    else
                    {
                        EasyUI.Toast.Toast.Show("Not enough money!", 0.5f);
                    }
                }
                break;
            case BoosterEnum.BoosterHammer:
                if (useAds)
                {
                    SaveGame.Hammer++;
                }
                else
                {
                    if (GameConfig.EnoughCoinBuyHammer)
                    {
                        Debug.Log("Hammer");
                        ManagerAudio.PlaySound(ManagerAudio.Data.soundDropGold);

                        //SaveGame.Coin -= GameConfig.COIN_HAMMER;
                        ManagerEvent.RaiseEvent(EventCMD.EVENT_SUB_GOLD, GameConfig.COIN_HAMMER);

                        GameManager.SubGold(GameConfig.COIN_HAMMER);
                        txtCoin.text = SaveGame.Coin.ToString();
                        SaveGame.Hammer++;
                    }
                    else
                    {
                        EasyUI.Toast.Toast.Show("Not enough money!", 0.5f);
                    }
                }

                break;

            case BoosterEnum.BoosterRefresh:
                if (useAds)
                {
                    SaveGame.Refresh++;

                }
                else
                {
                    if (GameConfig.EnoughCoinBuyRefresh)
                    {
                        Debug.Log("Refresh");
                        ManagerAudio.PlaySound(ManagerAudio.Data.soundDropGold);

                        //SaveGame.Coin -= GameConfig.COIN_REFRESH;
                        ManagerEvent.RaiseEvent(EventCMD.EVENT_SUB_GOLD, GameConfig.COIN_REFRESH);

                        GameManager.SubGold(GameConfig.COIN_REFRESH);
                        txtCoin.text = SaveGame.Coin.ToString();
                        SaveGame.Refresh++;
                    }
                    else
                    {
                        EasyUI.Toast.Toast.Show("Not enough money!", 0.5f);
                    }
                }
                break;
        }
    }

}