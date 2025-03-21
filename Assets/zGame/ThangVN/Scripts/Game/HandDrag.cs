using BaseGame;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandDrag : MonoBehaviour
{
    public ColorPlate selectingPlate;
    public Vector3 selectedPlateStartPos;
    RaycastHit hitPlate;
    [SerializeField] LayerMask layerPlate;
    [SerializeField] LayerMask layerMove;
    [SerializeField] EventSystem currentEvent;
    public bool isDrag;

    int currentLayer;

    private void Update()
    {
        if (!LogicGame.Instance.isUsingHand)
        {
            return;
        }
#if UNITY_EDITOR
        MouseDrag();
#else
        TouchDrag();
#endif
    }

    void MouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandSelect();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            PutDownPlate();
        }
        else if (selectingPlate != null)
        {
            MoveSelectPlate(Input.mousePosition);
        }
    }

    void TouchDrag()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    HandSelect();
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    MoveSelectPlate(touch.position);
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    PutDownPlate();
                    break;
            }
        }
    }

    bool CheckConditionToSelect(ColorPlate clicked)
    {
        if (clicked.ListValue.Count == 0 || clicked.status == Status.Frozen || clicked.status == Status.LockCoin ||
            clicked.status == Status.CannotPlace) return false;
        else return true;
    }


    void HandSelect()
    {
        if (SaveGame.Level == GameConfig.LEVEL_SWAP && !SaveGame.IsDoneTutSwap)
        {
            SaveGame.IsDoneTutSwap = true;
            TutorialCamera.Instance.EndTut();
        }

        if (selectingPlate != null) return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPlate, 100f, layerPlate))
        {
            ColorPlate clickedPlate = hitPlate.transform.GetComponent<ColorPlate>();
            //if (clickedPlate.ListValue.Count == 0) return;
            if (!CheckConditionToSelect(clickedPlate)) return;
            isDrag = true;
            selectingPlate = clickedPlate;
            currentLayer = selectingPlate.ListColor[0].spriteRender.sortingOrder;
        }
    }

    bool CheckConditionToPut(ColorPlate targetPlate)
    {
        if (targetPlate.status == Status.Frozen || targetPlate.isLocked || targetPlate.status == Status.Ads
            || targetPlate.status == Status.CannotPlace || targetPlate.status == Status.Empty) return false;
        else return true;
    }

    void PutDownPlate()
    {
        if (selectingPlate == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitPlateHolder, 100f, layerPlate))
        {
            if (hitPlateHolder.collider != null)
            {
                ColorPlate colorPlate = hitPlateHolder.transform.GetComponent<ColorPlate>();

                if (colorPlate == selectingPlate || !CheckConditionToPut(colorPlate))
                {
                    Debug.Log("aaaa");
                    for (int i = 0; i < selectingPlate.ListColor.Count; i++)
                    {
                        LogicColor c = selectingPlate.ListColor[i];
                        c.spriteRender.sortingOrder = currentLayer;
                        if (i == selectingPlate.ListColor.Count - 1)
                        {
                            c.transform.DOLocalMove(new Vector3(0, 0, -0.1f), 0.3f);
                        }
                        else
                        {
                            c.transform.DOLocalMove(new Vector3(0, 0, 0), 0.3f);
                        }
                        //c.transform.DOLocalMove(new Vector3(0, i * GameConfig.OFFSET_PLATE, 0), 0.3f);
                    }

                    selectingPlate.circleZZZ.SetActive(false);
                }
                else /*if (colorPlate.status != Status.CannotPlace)*/
                {
                    //Move to new Plate
                    ManagerAudio.PlaySound(ManagerAudio.Data.soundSwap);
                    selectingPlate.circleZZZ.SetActive(false);
                    LogicGame.Instance.SetColorUsingSwapItem(selectingPlate, colorPlate, currentLayer);
                    LogicGame.Instance.isUsingHand = false;

                    Debug.Log("Use Booster At Level: " + SaveGame.Level);
                    FirebaseCustom.LogUseBoosterAtLevel(SaveGame.Level);

                    SaveGame.Swap--;
                    isDrag = false;
                    if (DailyTaskManager.Instance != null)
                        DailyTaskManager.Instance.ExecuteDailyTask(TaskType.UseBoosterSwap, 1);

                    if (DailyTaskManager.Instance != null)
                        DailyTaskManager.Instance.ExecuteDailyTask(TaskType.UseBoosters, 1);
                    LogicGame.Instance.homeInGame.ExitUsingItem();
                }
            }
            else
            {
                Debug.Log("bbbb");
                for (int i = 0; i < selectingPlate.ListColor.Count; i++)
                {
                    LogicColor c = selectingPlate.ListColor[i];
                    if (i == selectingPlate.ListColor.Count - 1)
                    {
                        c.transform.DOLocalMove(new Vector3(0, 0, -0.1f), 0.3f);
                    }
                    else
                    {
                        c.transform.DOLocalMove(new Vector3(0, 0, 0), 0.3f);
                    }
                    //c.transform.DOLocalMove(new Vector3(0, i * GameConfig.OFFSET_PLATE, 0), 0.3f);
                }
            }
        }
        else
        {
            if (selectingPlate != null)
            {
                for (int i = 0; i < selectingPlate.ListColor.Count; i++)
                {
                    LogicColor c = selectingPlate.ListColor[i];
                    if (i == selectingPlate.ListColor.Count - 1)
                    {
                        c.transform.DOLocalMove(new Vector3(0, 0, -0.1f), 0.3f);
                    }
                    else
                    {
                        c.transform.DOLocalMove(new Vector3(0, 0, 0), 0.3f);
                    }
                    //c.transform.DOLocalMove(new Vector3(0, i * GameConfig.OFFSET_PLATE, 0), 0.3f);
                }

                selectingPlate.circleZZZ.SetActive(false);
            }
        }

        isDrag = false;
        selectingPlate = null;
    }

    void MoveSelectPlate(Vector2 position)
    {
        if (selectingPlate == null || selectingPlate.ListValue.Count == 0) return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(position), out var hit, 100f, layerMove))
        {
            for (int i = 0; i < selectingPlate.ListColor.Count; i++)
            {
                LogicColor c = selectingPlate.ListColor[i];
                c.spriteRender.sortingOrder = 15;
                c.transform.position = Vector3.MoveTowards(c.transform.position,
                    hit.point + new Vector3(0, 1 + i * GameConfig.OFFSET_PLATE, -i * GameConfig.OFFSET_PLATE), 1f);
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitPlateHolder, 100f, layerPlate))
        {
            selectingPlate.circleZZZ.SetActive(true);
            selectingPlate.circleZZZ.transform.position =
                hitPlateHolder.transform.position /*+ new Vector3(0, GameConfig.OFFSET_PLATE, 0)*/;
        }
        else
        {
            selectingPlate.circleZZZ.SetActive(false);
        }
    }

    //Check if clicking on UI elements
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(currentEvent);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        currentEvent.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    //private void OnDrawGizmos()
    //{
    //    if (selectingPlate != null)
    //    {
    //        Gizmos.color = Color.red;
    //        Vector3 direction = -(Camera.main.transform.position - selectingPlate.ListColor[0].transform.position) * 10f;
    //        Gizmos.DrawRay(selectingPlate.ListColor[0].transform.position, direction);
    //    }
    //}
}