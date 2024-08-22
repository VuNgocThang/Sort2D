// using BestHTTP.WebSocket;

// namespace ntDev
// {
//     public class WebSocketCMD
//     {
//         public const uint CMD_ERROR = 0;
//         public const uint CMD_PING = 1;
//         public const uint CMD_DISCONNECT = 10;

//         public const uint CMD_AUTHEN = 100;
//         public const uint CMD_JOIN_CHANNEL = 101;
//         public const uint CMD_MEMBER_LIST = 102;
//         public const uint CMD_SOMEONE_JOIN_CITY = 103;
//         public const uint CMD_SOMEONE_LEFT_CITY = 104;

//         public const uint CMD_MOVE = 200;
//         public const uint CMD_CHAT = 201;
//         public const uint CMD_VOICE = 202;
//     }
//     public abstract class ConnectionWebSocket<T> where T : class, new()
//     {
//         public static T Instance { get; } = new T();
//         protected WebSocket webSocket;
//         public const string SERVER_WEB_SOCKET = "";
//         public virtual void Connect() { }
//         public virtual void Close() { }
//     }
// }