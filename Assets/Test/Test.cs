using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("on pointer dơn");
        LogicSetupRoom.instance.OnPointerClick();
        //Debug.Log(gameObject.name);
        //Todo Ray Cát

    }
   
}
