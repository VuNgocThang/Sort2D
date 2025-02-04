using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public SkeletonGraphic spine;
    public Image imgCustomer;
    public int idCustomer;
    public MissionBonus missionBonus;
    public MissionBonus missionBonusPrefab;
    public bool isMoved;
    DataCustomer dataCustomer;
    const string IDLE = "idle";
    const string CHEER = "cheer";

    public void Init(DataCustomer dataCustomer)
    {
        isMoved = false;
        this.dataCustomer = dataCustomer;
        this.idCustomer = dataCustomer.idCustomer;
        spine.Skeleton.SetSkin($"char{idCustomer + 1}");
        spine.AnimationState.SetAnimation(0, IDLE, true);
        //imgCustomer.sprite = dataCustomer.spriteCustomer;
        //imgCustomer.SetNativeSize();

        //if (idCustomer >= 3)
        //    imgCustomer.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        //else
        //    imgCustomer.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.35f);

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
        //imgCustomer.sprite = dataCustomer.spriteCompleted;
        //imgCustomer.SetNativeSize();
        //imgCustomer.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.1f);
        isMoved = true;
        missionBonus.gameObject.SetActive(false);
        spine.AnimationState.SetAnimation(0, CHEER, false);
        StartCoroutine(Disappear());
        //missionBonus.ChangeSpriteIfDone();
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }
}
