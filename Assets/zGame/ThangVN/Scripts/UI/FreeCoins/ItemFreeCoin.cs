using ntDev;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemFreeCoin : MonoBehaviour
{
    public GameObject iconCoin, iconHeart, imgActive, imgInActive, iconAds, imgClaimed;
    public TextMeshProUGUI txtCountCoin, txtInactive;
    public EasyButton btnClaim;
    public bool isClaimed;
    public int countCoin, index;
    public Image imgCoin;
    public List<Sprite> sprites;
    PopupFreeCoin popupFreeCoin;

    private void Awake()
    {
        btnClaim.OnClick(ClaimRewardAds);
    }

    public void Show(int currentIndex, PopupFreeCoin popupFreeCoin)
    {
        this.popupFreeCoin = popupFreeCoin;
        if (countCoin > 0)
        {
            iconHeart.SetActive(false);
            iconCoin.SetActive(true);
            txtCountCoin.text = countCoin.ToString();
        }
        else
        {
            iconHeart.SetActive(true);
            iconCoin.SetActive(false);
        }

        for (int i = 0; i < SaveGame.DataFreeCoin.listDataFreeCoin.Count; i++)
        {
            if (i >= 2) imgCoin.sprite = sprites[1];
            else imgCoin.sprite = sprites[0];
            //Debug.Log(SaveGame.DataFreeCoin.listDataFreeCoin[i].index);
            if (SaveGame.DataFreeCoin.listDataFreeCoin[i].index == index)
            {
                isClaimed = SaveGame.DataFreeCoin.listDataFreeCoin[i].isClaimed;
            }
        }

        if (currentIndex >= index && !isClaimed)
        {
            imgActive.SetActive(true);
            iconAds.SetActive(true);
            imgInActive.SetActive(false);
        }
        else
        {
            imgActive.SetActive(false);
            iconAds.SetActive(false);
            imgInActive.SetActive(true);
        }

        if (isClaimed)
        {
            // iconAds.SetActive(false);
            imgClaimed.SetActive(true);
            //txtInactive.text = "Claimed";
        }
        else
        {
            imgClaimed.SetActive(false);
            // iconAds.SetActive(true);
            //txtInactive.text = "Free";
        }
    }

    private void ClaimRewardAds()
    {
        if (isClaimed) return;

        if (index == SaveGame.DataFreeCoin.currentIndex)
        {
            // reward ads here
            if (!AdsController.instance.IsRewardedVideoAvailable())
            {
                EasyUI.Toast.Toast.Show("No Ads Now", 1f);
            }
            else
            {
                AdsController.instance.ShowRewardedVideo(successful =>
                {
                    if (successful)
                    {
                        if (countCoin > 0)
                        {
                            FirebaseCustom.LogDailyRewardFreeCoin(index);

                            if (DailyTaskManager.Instance != null)
                                DailyTaskManager.Instance.ExecuteDailyTask(TaskType.CollectFreeCoins, 1);

                            popupFreeCoin.ReceiveReward(this.transform);
                            GameManager.AddGold(countCoin);
                            isClaimed = true;
                            Debug.Log($" claimed {countCoin}");
                            UpdateStateClaim();
                        }
                        else
                        {
                            isClaimed = true;
                            Debug.Log("claimed heart");
                            UpdateStateClaim();
                        }
                    }
                }, null, "Free Coin");
            }
        }
    }

    void UpdateStateClaim()
    {
        imgActive.SetActive(false);
        imgInActive.SetActive(true);
        imgClaimed.SetActive(true);
        //txtInactive.text = "Claimed";
        iconAds.SetActive(false);
        Debug.Log("Save");
        SaveGame.DataFreeCoin.listDataFreeCoin.Add(new DataFreeCoin
        {
            index = index,
            isClaimed = true
        });
        SaveGame.DataFreeCoin.currentIndex = index + 1;
        SaveGame.DataFreeCoin = SaveGame.DataFreeCoin;
        ManagerEvent.RaiseEvent(EventCMD.EVENT_FREECOIN);
    }
}