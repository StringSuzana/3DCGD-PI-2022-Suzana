using Newtonsoft.Json;
using System;
using UnityEngine;

namespace MyGame
{

    public class JsonSerializationOption : ISerializationOption
    {
        public string ContentType_Json => "application/json";


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