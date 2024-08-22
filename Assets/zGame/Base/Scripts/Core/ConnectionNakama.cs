// using System.Threading.Tasks;
// using UnityEngine;
// using Nakama;
// using Sirenix.Utilities;

// namespace ntDev
// {
//     public static class ConnectionNakama
//     {
//         static string Token = "";

//         static Client Client;
//         public static ISession Session;
//         public static ISocket Socket;

//         public static bool CanConnect => !IsConnecting && !IsConnected;
//         public static bool IsConnecting => Client != null && Session != null && Socket != null && Socket.IsConnecting;
//         public static bool IsConnected => Client != null && Session != null && Socket != null && Socket.IsConnected;

//         public static async Task Connect(string url = "localhost", int port = 7350, string key = "default")
//         {
//             Client = new Client("http", url, port, key, UnityWebRequestAdapter.Instance);

//             Token = ES3.Load<string>("Token", "");
//             if (!Token.IsNullOrWhitespace())
//             {
//                 var session = Nakama.Session.Restore(Token);
//                 if (!session.IsExpired) Session = session;
//             }

//             if (Session == null)
//             {
//                 string deviceID = ES3.Load<string>("DeviceID", "");
//                 if (Token.IsNullOrWhitespace())
//                 {
//                     deviceID = SystemInfo.deviceUniqueIdentifier;
//                     if (string.Equals(deviceID, SystemInfo.unsupportedIdentifier))
//                         deviceID = System.Guid.NewGuid().ToString();
//                     ES3.Save("DeviceID", deviceID);
//                 }
//                 Session = await Client.AuthenticateDeviceAsync(deviceID);
//                 ES3.Save("Token", Session.AuthToken);
//             }

//             Socket = Client.NewSocket(true);
//             await Socket.ConnectAsync(Session, true);
//         }
//     }
// }
