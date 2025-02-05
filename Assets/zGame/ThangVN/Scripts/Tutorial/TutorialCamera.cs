using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public static TutorialCamera Instance;
    [SerializeField] Camera cam;
    [SerializeField] RectTransform hand;
    [SerializeField] GameObject particleHand;
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] List<GameObject> listSteps;
    [SerializeField] PopupHome popupHome;
    [SerializeField] Transform nBlack;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        particleHand.gameObject.SetActive(false);
    }

    public void MoveHand(int index, int indexStep)
    {
        Vector3 pos = LogicGame.Instance.ListArrowPlate[index].transform.position;

        Vector3 position = cam.WorldToScreenPoint(pos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            position,
            cam,
            out Vector2 localPoint
        );

        hand.anchoredPosition = localPoint;
        listSteps[indexStep].SetActive(true);
        particleHand.gameObject.SetActive(true);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 anr;
        //    Debug.Log("Input: " + Input.mousePosition);
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //      canvasRectTransform,
        //      Input.mousePosition,
        //      cam,
        //      out anr
        //     );

        //    hand.anchoredPosition = anr;
        //}

        if (Input.GetKeyDown(KeyCode.N))
        {
            particleHand.gameObject.SetActive(false);
            MoveHand(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            nBlack.gameObject.SetActive(true);
            popupHome.nBar.transform.SetParent(nBlack);
            listSteps[3].gameObject.SetActive(true);
        }
    }
}
