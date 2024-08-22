// using System;
// using System.Collections.Generic;
// using BestHTTP;

// // by nt.Dev93
// namespace ntDev
// {
//     public static class ManagerNetwork
//     {
//         public static string SERVER_API = "";
//         public static int SERVER_API_PORT = 8888;

//         public static void Get(string url, List<string> listField, List<string> listValue, Action<string> onSuccess = null, Action<string> onFail = null, string token = "")
//         {
//             string str = SERVER_API + ":" + SERVER_API_PORT;
//             Ez.Log("Request: " + str + url);

//             HTTPRequest request = new HTTPRequest(new System.Uri(str + url), methodType: HTTPMethods.Get, (request, response) =>
//             {
//                 if (response == null)
//                 {
//                     onFail?.Invoke("DM Server " + str + url);
//                 }
//                 else if (response.IsSuccess)
//                 {
//                     Ez.Log("Response: " + response.DataAsText);
//                     onSuccess?.Invoke(response.DataAsText);
//                 }
//                 else
//                 {
//                     Ez.Log("Error code: " + response.StatusCode);
//                     Ez.Log("Data: " + response.DataAsText);
//                     onFail?.Invoke(response.DataAsText);
//                 }
//             });

//             for (int i = 0; i < listField.Count; ++i)
//             {
//                 Ez.Log("Field: " + listField[i] + " : " + listValue[i]);
//                 request.AddField(listField[i], listValue[i]);
//             }
//             if (token != "") request.SetHeader("Authorization", "Bearer " + token);
//             request.Send();
//         }

//         public static void Post(string url, List<string> listField, List<string> listValue, Action<string> onSuccess = null, Action<string> onFail = null, string token = "")
//         {
//             string str = SERVER_API + ":" + SERVER_API_PORT;
//             Ez.Log("Request: " + str + url);
//             HTTPRequest request = new HTTPRequest(new System.Uri(str + url), methodType: HTTPMethods.Post, (request, response) =>
//             {
//                 if (response.IsSuccess)
//                 {
//                     Ez.Log("Response: " + response.DataAsText);
//                     onSuccess?.Invoke(response.DataAsText);
//                 }
//                 else
//                 {
//                     Ez.Log("Error code: " + response.StatusCode);
//                     Ez.Log("Data: " + response.DataAsText);
//                     onFail?.Invoke(response.DataAsText);
//                 }
//             });

//             for (int i = 0; i < listField.Count; ++i)
//             {
//                 Ez.Log("Field: " + listField[i] + " : " + listValue[i]);
//                 request.AddField(listField[i], listValue[i]);
//             }
//             if (token != "") request.SetHeader("Authorization", "Bearer " + token);
//             request.Send();
//         }
//     }
// }