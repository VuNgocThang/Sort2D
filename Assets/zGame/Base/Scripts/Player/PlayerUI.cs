using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// by nt.Dev93
namespace ntDev
{
    public class PlayerUI : MonoBehaviour
    {
        EasyTransform easyTransform;
        EasyTransform EasyTransform
        {
            get
            {
                if (easyTransform == null) easyTransform = GetComponent<EasyTransform>();
                return easyTransform;
            }
        }

        [SerializeField] TextMeshProUGUI txtName;

        public void Init(Transform player, string name)
        {
            EasyTransform.FollowTarget = player;
            txtName.text = name.ToUpper();
        }
    }
}
