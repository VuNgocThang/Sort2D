using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThangVN
{
    public class ButtonBooster : MonoBehaviour
    {
        public EasyButton button;
        public Image imgIcon;
        public TextMeshProUGUI txtCount, txtLevelUnlock;
        public GameObject nCount, ads, imgLock, imgUnLocked;
        public int numCount;
        public int indexLevelUnlock;

        public virtual void Init()
        {
            if (SaveGame.Level >= indexLevelUnlock)
            {
                imgUnLocked.SetActive(true);
                imgLock.SetActive(false);
                if (numCount > 0)
                {
                    nCount.SetActive(true);
                    ads.SetActive(false);
                    txtCount.text = numCount.ToString();
                }
                else
                {
                    nCount.SetActive(false);
                    ads.SetActive(true);
                }
            }
            else
            {
                imgUnLocked.SetActive(false);
                imgLock.SetActive(true);
                txtLevelUnlock.text = $"level {indexLevelUnlock + 1}";
            }
        }

        public virtual void Update()
        {
            if (SaveGame.Level >= indexLevelUnlock)
            {
                imgUnLocked.SetActive(true);
                imgLock.SetActive(false);
                if (numCount > 0)
                {
                    nCount.SetActive(true);
                    ads.SetActive(false);
                    txtCount.text = numCount.ToString();
                }
                else
                {
                    nCount.SetActive(false);
                    ads.SetActive(true);
                }
            }
            else
            {
                imgUnLocked.SetActive(false);
                imgLock.SetActive(true);
                txtLevelUnlock.text = $"level {indexLevelUnlock + 1}";
            }
        }
    }
}
