using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public Image imgCustomer;
    public int idCustomer;
    public MissionBonus missionBonus;

    public MissionBonus missionBonusPrefab;

    DataCustomer dataCustomer;

    public void Init(DataCustomer dataCustomer)
    {
        this.dataCustomer = dataCustomer;
        this.idCustomer = dataCustomer.idCustomer;
        imgCustomer.sprite = dataCustomer.spriteCustomer;
        imgCustomer.SetNativeSize();

        if (idCustomer >= 3)
            imgCustomer.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        else
            imgCustomer.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.35f);

        MissionBonus missionBonus = Instantiate(missionBonusPrefab, transform);
        missionBonus.Init(dataCustomer.missions);
        this.missionBonus = missionBonus;
    }

    public bool IsCompleted()
    {
        return missionBonus.CompletedAll();
    }

    public void ChangeSpriteIfDone()
    {
        imgCustomer.sprite = dataCustomer.spriteCompleted;
        imgCustomer.SetNativeSize();
        imgCustomer.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.1f);

        missionBonus.ChangeSpriteIfDone();
    }
}
