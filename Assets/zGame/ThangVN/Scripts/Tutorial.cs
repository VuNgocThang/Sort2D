using DG.Tweening;
using ntDev;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public EasyButton btnContinue;
    public GameObject imgContinue;
    public GameObject imgCoverTut;
    public List<GameObject> listSteps;
    public int currentIndex;

    [SerializeField] RectTransform hand;

    private void Awake()
    {
        btnContinue.OnClick(() =>
        {
            imgContinue.SetActive(false);
            LogicGame.Instance.isPauseGame = false;
            SaveGame.IsDoneTutorial = true;
            LogicGame.Instance.CheckClear();
            LogicGame.Instance.canvasTutorial.enabled = false;
        });
    }

    public void Init(RectTransform slot_2, Camera cam)
    {
        if (SaveGame.Level == 0 && !SaveGame.IsDoneTutorial)
        {
            //Debug.Log("Init");

            imgCoverTut.SetActive(true);

            PlayProgressTut(0);

            Vector3 screenPos2 = RectTransformUtility.WorldToScreenPoint(cam, slot_2.position);

            hand.position = screenPos2;
        }

    }

    public void InitHandArrow(Transform arrow, Camera cam)
    {

        Vector3 pos = cam.WorldToScreenPoint(arrow.position);

        hand.DOMove(pos, 0.5f).OnComplete(() =>
        {
            PlayProgressTut(1);
            imgCoverTut.SetActive(false);
        });
    }


    public void PlayProgressTut(int index)
    {
        if (index == 2)
        {
            imgContinue.SetActive(true);
            btnContinue.enabled = true;
            hand.gameObject.SetActive(false);
        }

        for (int i = 0; i < listSteps.Count; i++)
        {
            listSteps[i].SetActive(false);
        }

        listSteps[index].SetActive(true);
    }
}
