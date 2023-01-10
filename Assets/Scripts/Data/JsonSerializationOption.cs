using System;
using MyGame;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{

    public class JsonSerializationOption : ISerializationOption
    {
        public string ContentTypeJson => "application/json";


        public T Deserialize<T>(string text)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<T>(text);
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Could not parse response {text}. {ex.Message}");
                return default;
            }
        }
    }
}