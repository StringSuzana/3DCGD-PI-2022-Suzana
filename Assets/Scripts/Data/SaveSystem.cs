using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MyGame
{
    public class SaveSystem
    {
        public static void SavePlayerInfoToJson(PlayerInfo info)
        {
            string path = Application.persistentDataPath + "/player_"
                + info.PlayerName.ToUpper() + ".json";
            Debug.Log("Creating file: " + path);
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, info);
            }
        }
        public static PlayerInfo LoadPlayerInfoFromJson(string playerName)
        {
            PlayerInfo info = new PlayerInfo();
            string path = Application.persistentDataPath + "/player_" + playerName.ToUpper() + ".json";
            if (File.Exists(path))
            {
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializationOption serializationOption = new JsonSerializationOption();
                    JsonSerializer serializer = new JsonSerializer();
                    info = (PlayerInfo)serializer.Deserialize(file, typeof(PlayerInfo));
                }
               Debug.Log("Loaded from path: " + path);
                return info;
            }
            else
            {
                Debug.LogError("File not found.");
                return null;
            }
        }
    }
}

