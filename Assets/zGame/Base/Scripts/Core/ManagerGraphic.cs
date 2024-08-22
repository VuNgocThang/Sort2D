using System.Collections;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public static class ManagerGraphic
    {
        public static bool IsAutoAdjust;

        static bool shadow;
        static Light nLight;

        static bool postProcessing;
        static GameObject nPostProcessing;

        public static void Init()
        {
            if (IsAutoAdjust) return;

            GameObject n = GameObject.FindGameObjectWithTag("Light");
            if (n != null)
            {
                nLight = n.GetComponent<Light>();
                shadow = nLight != null && nLight.shadows != LightShadows.None;
            }

            nPostProcessing = GameObject.FindGameObjectWithTag("PostProcessing");
            postProcessing = nPostProcessing != null && nPostProcessing.activeSelf;

            CoreGame.Instance.StartCoroutine(AutoInitGraphic());
        }

        static IEnumerator AutoInitGraphic()
        {
            yield return new WaitForSeconds(2);
            IsAutoAdjust = true;
            bool isDone = false;
            while (!isDone)
            {
                int totalFrames = 0;
                float totalTime = 0;
                while (totalFrames < 60)
                {
                    totalTime += Time.deltaTime;
                    ++totalFrames;
                    yield return null;
                }

                Ez.Log("FPS: " + (60 / totalTime));

                if (totalTime < 60 / 24f) isDone = true;
                else
                {
                    if (QualitySettings.GetQualityLevel() > 0)
                    {
                        QualitySettings.DecreaseLevel();
                        Ez.Log("Change Quality To: " + QualitySettings.GetQualityLevel());
                    }
                    else if (postProcessing)
                    {
                        Ez.Log("Disable Postprocessing");
                        nPostProcessing.SetActive(false);
                        postProcessing = false;
                    }
                    else if (shadow)
                    {
                        Ez.Log("Disable Shadow");
                        nLight.shadows = LightShadows.None;
                        shadow = false;
                    }
                    else isDone = true;
                }
                yield return new WaitForSeconds(3);
            }
            Ez.Log("Done Auto Graphic");
        }
    }
}