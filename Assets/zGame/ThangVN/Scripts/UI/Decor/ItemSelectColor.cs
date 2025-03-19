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
                Debug.Log("Watch ads to can select");
                // watch ads to canSelect

                if (imgAds == null) return;
                if (!AdsController.instance.IsRewardedVideoAvailable())
                {
                    EasyUI.Toast.Toast.Show("No Ads Now", 1f);
                }
                else
                {
                    AdsController.instance.ShowRewardedVideo(successful =>
                    {
                        if (successful)
                        {
                            imgAds.SetActive(false);
                            CanSelect = true;
                        }
                    }, null, "Color Decor");
                }
            }
        });
    }


    public void Init(Color _color, bool _CanSelect)
    {
        this.color = _color;
        this.CanSelect = _CanSelect;
        bg.color = _color;
        if (imgAds != null)
            imgAds.SetActive(!_CanSelect);
    }
}