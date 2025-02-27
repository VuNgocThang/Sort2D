using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour
{
    [SerializeField] private float loadingTime;
    public bool startLoading = false;
    [SerializeField] private Image imgFill;

    private string targetScene;
    private float x;
    public float count;
    private AsyncOperation asyncOperation;

    public void Start()
    {
        x = 0;
        count = 0;
        isOutOfLoading = false;
        startLoading = true;
        loadingTime = 2f;
        NetworkController.instance.CheckNetwork();
        StartCoroutine(LoadScene());

        //StartCoroutine(CheckAdsLoading());
        Invoke(nameof(Load), Config.TIME_WAIT_LOADING);
    }

    void Update()
    {
        if (startLoading)
        {
            x += Time.deltaTime / loadingTime;
            if (imgFill.fillAmount < 1)
            {
                Debug.Log("x: " + x);
                imgFill.fillAmount = x;
            }
            else
            {
                startLoading = false;
            }
        }
    }

    IEnumerator CheckAdsLoading()
    {
        while (!Config.FIRST_LOAD_ADS_DONE || count < 2f)
        {
            count += Time.deltaTime;
            yield return null;
        }

        CancelInvoke();
        Load();
    }

    private bool isOutOfLoading = false;

    void Load()
    {
        //StopAllCoroutines();
        isOutOfLoading = true;

        //if (!DataUseInGame.gameData.tutGameplay)
        //{
        //    SceneManager.LoadScene("HomeScene");
        //}
        //else SceneManager.LoadScene("FashionScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        string strScene = "SceneHome";
        string strSceneName = "SceneGame";

        if (SaveGame.Level == 0)
        {
            asyncOperation = SceneManager.LoadSceneAsync(strSceneName);
        }
        else
        {
            asyncOperation = SceneManager.LoadSceneAsync(strScene);
        }

        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            //m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //m_Text.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene

                if (isOutOfLoading)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                else
                {
                    if (!Config.FIRST_LOAD_ADS_DONE || count < 2f)
                    {
                        count += Time.deltaTime;
                    }
                    else
                    {
                        //asyncOperation.allowSceneActivation = true;
                        StartCoroutine(DelayLoadScene());
                    }
                }

                // if (Input.GetKeyDown(KeyCode.Space))
                //  //Activate the Scene
                // asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(1.5f);
        asyncOperation.allowSceneActivation = true;
    }
}