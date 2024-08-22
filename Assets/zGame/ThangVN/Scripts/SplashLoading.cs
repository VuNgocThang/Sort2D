using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ntDev;

public class SplashLoading : MonoBehaviour
{
    [SerializeField] Image imgFill;
    float x;
    bool startLoading = false;
    float loadingTime = 0.9f;

    void Start()
    {
        imgFill.fillAmount = 0;
        x = 0;
        startLoading = true;

        if (SaveGame.Level == 0)
        {
            StartCoroutine(LoadScene("SceneGame"));
        }
        else
        {
            StartCoroutine(LoadScene("SceneHome"));
        }
    }


    IEnumerator LoadScene(string strScene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(strScene);
    }



    void Update()
    {
        if (startLoading)
        {
            x += Time.deltaTime / loadingTime;
            if (imgFill.fillAmount < 1)
            {
                imgFill.fillAmount = x;
            }
            else
            {
                startLoading = false;
            }
        }
    }

}
