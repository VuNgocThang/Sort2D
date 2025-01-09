using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectColor : MonoBehaviour
{
    [SerializeField] EasyButton btnSelect;
    public Image bg;
    public GameObject imgAds;
    public bool CanSelect;
    //public bool IsBought;
    public Color color;

    private void Awake()
    {
        btnSelect.OnClick(() =>
        {
            //if (!IsBought) return;

            if (CanSelect)
            {
                // raise event change color;
                ManagerEvent.RaiseEvent(EventCMD.EVENT_CHANGE_COLOR, color);

            }
            else
            {
                Debug.Log("Watch ads to canselect");
                // watch ads to canSelect

                if (imgAds == null) return;

                imgAds.SetActive(false);
                CanSelect = true;
            }
        });
    }


    public void Init(Color _color, bool _CanSelect)
    {
        this.color = _color;
        this.CanSelect = _CanSelect;
        bg.color = _color;
    }
}
