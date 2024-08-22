using ntDev;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemFreeCoin : MonoBehaviour
{
    public GameObject iconCoin, iconHeart, imgActive, imgInActive, iconAds;
    public TextMeshProUGUI txtCountCoin, txtInactive;
    public EasyButton btnClaim;
    public bool isClaimed;
    public int countCoin, index;


    private void Awake()
    {
        btnClaim.OnClick(() => ClaimRewardAds());
    }

    public void Show(int currentIndex)
    {
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
            Debug.Log(SaveGame.DataFreeCoin.listDataFreeCoin[i].index);
            if (SaveGame.DataFreeCoin.listDataFreeCoin[i].index == index)
            {
                isClaimed = SaveGame.DataFreeCoin.listDataFreeCoin[i].isClaimed;
            }
        }

        if (currentIndex >= index && !isClaimed)
        {
            imgActive.SetActive(true);
            imgInActive.SetActive(false);
        }
        else
        {
            imgActive.SetActive(false);
            imgInActive.SetActive(true);
        }

        if (isClaimed)
        {
            iconAds.SetActive(false);
            txtInactive.text = "Claimed";

        }
        else
        {
            iconAds.SetActive(true);
            txtInactive.text = "Free";
        }
    }

    void ClaimRewardAds()
    {
        if (isClaimed) return;

        if (index == SaveGame.DataFreeCoin.currentIndex)
        {
            // reward ads here
            if (countCoin > 0)
            {
                SaveGame.Coin += countCoin;
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
    }

    void UpdateStateClaim()
    {
        imgActive.SetActive(false);
        imgInActive.SetActive(true);
        txtInactive.text = "Claimed";
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
