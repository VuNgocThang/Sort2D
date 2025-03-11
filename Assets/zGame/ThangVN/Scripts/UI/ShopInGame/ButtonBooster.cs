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
        public GameObject nCount, ads, imgLock, imgUnLocked, imgGrey, magic;
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
                    magic.SetActive(true);
                    nCount.SetActive(true);
                    ads.SetActive(false);
                    imgGrey.SetActive(false);
                    txtCount.text = numCount.ToString();
                }
                else
                {
                    magic.SetActive(false);
                    nCount.SetActive(false);
                    imgGrey.SetActive(true);
                    ads.SetActive(true);
                }
            }
            else
            {
                magic.SetActive(false);
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
                    magic.SetActive(true);
                    nCount.SetActive(true);
                    imgGrey.SetActive(false);
                    ads.SetActive(false);
                    txtCount.text = numCount.ToString();
                }
                else
                {
                    magic.SetActive(false);
                    nCount.SetActive(false);
                    imgGrey.SetActive(true);
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
