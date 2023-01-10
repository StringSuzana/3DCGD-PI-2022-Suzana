using Newtonsoft.Json;

namespace Data
{
    [System.Serializable]
    public class PlayerInfo
    {
        [JsonProperty("playerName")] public string playerName;

        [JsonProperty("healthPoints")] public float healthPoints;

        [JsonProperty("levelName")] public string levelName;

        [JsonProperty("soundVolume")] public float soundVolume;

        [JsonProperty("musicVolume")] public float musicVolume;

        public PlayerInfo() //Have to have default constructor because of Json deserialization
        {
        }

        public override string ToString()
        {
            return
                $"Student name: {playerName} \nPoints: {healthPoints}\nLevel name: {levelName}\nMusic Vol: {musicVolume}\nSound Vol: {soundVolume}\n";
        }
    }
}