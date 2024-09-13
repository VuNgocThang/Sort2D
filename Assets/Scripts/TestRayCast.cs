using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestRayCast : MonoBehaviour
{
    public GameObject selectingObj;
    public Vector3 selectedPlateStartPos;
    RaycastHit hitObj;
    [SerializeField] LayerMask layerPlate;
    [SerializeField] LayerMask layerMove;
    [SerializeField] EventSystem currentEvent;
    public bool isDrag;

    private void Update()
    {
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
        else if (selectingObj != null)
        {
            //MoveSelectPlate(Input.mousePosition);
        }
    }

    void TouchDrag()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandSelect();
                    break;

                case TouchPhase.Moved:
                    //MoveSelectPlate(touch.position);
                    break;

                case TouchPhase.Ended:
                    PutDownPlate();
                    break;
            }
        }
    }


    void HandSelect()
    {
        if (selectingObj != null) return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitObj, 100f, layerPlate))
        {
            Debug.Log("hit");
            Debug.Log(hitObj.collider.name);
            GameObject obj = hitObj.collider.gameObject;
            isDrag = true;
            selectingObj = obj;

        }
    }

    void PutDownPlate()
    {
        isDrag = false;
        selectingObj = null;
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
