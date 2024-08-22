using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// by nt.Dev93
namespace ntDev
{
    public class SceneLoadScene : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Load());
        }

        IEnumerator Load()
        {
            ManagerLoading.HideLoadScene(0.3f);
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadSceneAsync(ManagerScene.nextScene);
        }
    }
}
