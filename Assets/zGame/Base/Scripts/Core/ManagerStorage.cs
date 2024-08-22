// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Runtime.Serialization.Formatters.Binary;
// using UnityEngine;

// // by nt.Dev93
// namespace ntDev
// {
//     public static class ManagerStorage
//     {
//         static string path = "/Save.dat";
//         static string FullFilePath = Application.persistentDataPath + path;
//         static FileStream fileStream;

//         public static void Save()
//         {
//             // Binary
//             // if (fileStream != null) fileStream.Close();
//             // fileStream = File.Open(FullFilePath, File.Exists(FullFilePath) ? FileMode.Truncate : FileMode.Create);
//             // BinaryFormatter Formatter = new BinaryFormatter();
//             // Formatter.Serialize(fileStream, SaveGame.Instance);

//             // String Json to Base64
//             // string str = JsonUtility.ToJson(SaveGame.Instance);
//             // SaveToFireBase();
//             // CoreGame.UpdateAccount();
//             // try { File.WriteAllBytes(FullFilePath, System.Text.Encoding.UTF8.GetBytes(str)); }
//             // catch (Exception) { };
//             // Ez.Log("Local Saved: " + FullFilePath + " - " + str);
//         }

//         public static SaveGame Load()
//         {
//             //     SaveGame save = new SaveGame();
//             //     if (File.Exists(FullFilePath))
//             //     {
//             //         // Binary
//             //         // if (fileStream != null) fileStream.Close();
//             //         // fileStream = File.Open(FullFilePath, FileMode.Open, FileAccess.Read);
//             //         // fileStream.Position = 0;
//             //         // BinaryFormatter Formatter = new BinaryFormatter();
//             //         // save = (SaveGame)Formatter.Deserialize(fileStream);

//             //         // String Json to Base64
//             //         byte[] arrBytes = File.ReadAllBytes(FullFilePath);
//             //         string str = System.Text.Encoding.UTF8.GetString(arrBytes);
//             //         save = JsonUtility.FromJson<SaveGame>(str);
//             //         Ez.Log("Local Loaded: " + FullFilePath + " - " + str);
//             //     }
//             return null;
//         }

//         public static void ClearGame()
//         {
//             // Binary
//             // if (fileStream != null) fileStream.Close();
//             // fileStream = File.Open(FullFilePath, File.Exists(FullFilePath) ? FileMode.Truncate : FileMode.Create);
//             // BinaryFormatter Formatter = new BinaryFormatter();
//             // Formatter.Serialize(fileStream, new SaveGame());

//             // String Json to Base64
//             // string str = JsonUtility.ToJson(new SaveGame());
//             // File.WriteAllBytes(FullFilePath, System.Text.Encoding.UTF8.GetBytes(str));
//         }

//         public static string LoadFile(string pPath)
//         {
//             TextAsset textAsset = Resources.Load<TextAsset>(pPath);
//             if (textAsset != null)
//             {
//                 string content = "";
//                 content = textAsset.text;
//                 Resources.UnloadAsset(textAsset);
//                 return content;
//             }
//             return "";
//         }

// #if UNITY_EDITOR
//         [UnityEditor.MenuItem("Game Manager/Delete Save")]
//         private static void DeleteSave()
//         {
//             ManagerStorage.ClearGame();
//         }
// #endif
//     }
// }