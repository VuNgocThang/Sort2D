using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

// by nt.Dev93
namespace ntDev
{
    public static class ManagerTime
    {
        static int systemCount()
        {
            int result = Environment.TickCount;
            return result;
        }
        public static int SystemCount => systemCount();
        public static DateTime currentTime;
        static Action<DateTime> onDone;
        static Action onFail;
        public static void GetTime(Action<DateTime> onD, Action onF = null)
        {
            onDone = onD;
            onFail = onF;
            CoreGame.RunOnMainThread(() => CoreGame.Instance.StartCoroutine(StartGetDateTime()));
        }

        static IEnumerator StartGetDateTime()
        {
            UnityWebRequest request = UnityWebRequest.Get("https://script.google.com/macros/s/AKfycbyd5AcbAnWi2Yn0xhFRbyzS4qMq1VucMVgVvhul5XqS9HkAyJY/exec?tz=Iceland");
            request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    // Defective.JSON.JSONObject jSONObject = new Defective.JSON.JSONObject(request.downloadHandler.text);
                    // currentTime = new DateTime(jSONObject["year"].intValue, jSONObject["month"].intValue, jSONObject["day"].intValue, jSONObject["hours"].intValue, jSONObject["minutes"].intValue, jSONObject["seconds"].intValue);
                    Ez.Log("Time: " + currentTime);
                    onDone?.Invoke(currentTime);
                }
                catch { onFail?.Invoke(); }
            }
            else onFail?.Invoke();
            request.Dispose();
            onDone = null;
        }
    }
}