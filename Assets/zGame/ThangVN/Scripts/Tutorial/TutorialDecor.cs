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
    [SerializeField] RectTransform imgTut;
    [SerializeField] GameObject handImgTut, PanelTut;
    [SerializeField] List<GameObject> listSteps;
    [SerializeField] EasyButton btnStep3;

    private void Awake()
    {
        Instance = this;

        btnStep3.OnClick(() =>
        {
            SaveGame.IsDoneTutGift = true;
            HideStep();
        });
    }

    public void InitTut()
    {
        hand.SetActive(true);
        nBlackTut.gameObject.SetActive(true);
    }

    public void SetParent(Transform obj)
    {
        obj.SetParent(nBlackTut);
    }

    public void InitTutFocus(RectTransform rect)
    {
        hand.SetActive(false);
        nBlackTut.gameObject.SetActive(false);
        //-232 548
        //414 469
        imgTut.sizeDelta = rect.sizeDelta;
        imgTut.position = rect.position;
        handImgTut.GetComponent<RectTransform>().position = imgTut.position;

        PanelTut.SetActive(true);
        handImgTut.SetActive(true);
    }

    public void ShowStep(int index)
    {
        HideStep();

        listSteps[index].SetActive(true);
    }

    public void PlayAnimationHand()
    {
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
    }
}
