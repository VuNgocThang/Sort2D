using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : MonoBehaviour
{
    public static NetworkController instance;

    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        instance = this;
    }
    public void CheckNetwork()
    {
#if CHECK_NETWORK_CONNECT
        StopAllCoroutines();
        StartCoroutine(Check());
#endif
    }

    IEnumerator Check()
    {
        UnityWebRequest request = new UnityWebRequest("https://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
#if CHECK_NETWORK_CONNECT
            SystemPanel.instance.ShowDialog("Network failed! Please check again!");
#endif
        }
        else {
            if (Config.ACTIVE_DEBUG_LOG)
            {
                Debug.Log("Have network");
            }
        }
    }
}
