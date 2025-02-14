using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDecor : MonoBehaviour
{
    public static TutorialDecor Instance;
    public GameObject hand;
    public Animator animHandTut;
    [SerializeField] Transform nBlackTut, nParent;
    [SerializeField] RectTransform imgTut, imgTutCircle;
    [SerializeField] GameObject handImgTut, PanelTut, PanelTutCircle;
    [SerializeField] List<GameObject> listSteps;
    [SerializeField] EasyButton btnStep3;
    [SerializeField] GameObject particle;

    private void Awake()
    {
        Instance = this;

        btnStep3.OnClick(() =>
        {
            SaveGame.IsDoneTutGift = true;
            HideStep();
        });
    }

    //public void InitTut(Transform obj)
    //{
    //    hand.transform.position = 
    //    hand.SetActive(true);
    //    nBlackTut.gameObject.SetActive(true);
    //}

    public void SetParent(Transform obj)
    {
        hand.transform.position = obj.transform.position;
        hand.SetActive(true);
        nBlackTut.gameObject.SetActive(true);
        obj.SetParent(nBlackTut);
    }

    public void InitTutFocus(RectTransform rect, bool isCircle = false)
    {
        hand.SetActive(false);
        nBlackTut.gameObject.SetActive(false);
        imgTut.sizeDelta = rect.sizeDelta;
        imgTut.position = rect.position;

        imgTutCircle.sizeDelta = rect.sizeDelta;
        imgTutCircle.position = rect.position;

        handImgTut.GetComponent<RectTransform>().position = imgTut.position;

        if (!isCircle)
        {
            PanelTutCircle.SetActive(false);
            PanelTut.SetActive(true);
        }
        else
        {
            PanelTutCircle.SetActive(true);
            PanelTut.SetActive(false);
        }

        handImgTut.SetActive(true);
    }


    public void ShowStep(int index)
    {
        HideStep();

        listSteps[index].SetActive(true);
    }

    public void PlayAnimationHand()
    {
        particle.gameObject.SetActive(false);
        animHandTut.enabled = true;
        animHandTut.Play("Move");
    }

    public void HideStep()
    {
        for (int i = 0; i < listSteps.Count; i++)
        {
            listSteps[i].SetActive(false);
        }
    }

    public void SetParentDecorateBook(Transform obj)
    {
        obj.SetParent(nParent);
    }

    public void EndTutorialDecor()
    {
        SaveGame.IsDoneTutorialDecor = true;
        HideStep();
        handImgTut.SetActive(false);
        PanelTut.SetActive(false);
        PanelTutCircle.SetActive(false);
    }
}
