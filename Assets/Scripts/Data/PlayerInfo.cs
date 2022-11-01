using Newtonsoft.Json;

namespace MyGame
{
    [System.Serializable]
    public class PlayerInfo
    {
        [JsonProperty("playerName")]
        public string PlayerName;

        [JsonProperty("healthPoints")]
        public float HealthPoints;

        [JsonProperty("levelName")]
        public string LevelName;

        [JsonProperty("soundVolume")]
        public float SoundVolume;

        [JsonProperty("musicVolume")]
        public float MusicVolume;

        public PlayerInfo() //Have to have default constructor because of Json deserialization
        {

        }
        public override string ToString()
        {
            return $"Student name: {PlayerName} \nPoints: {HealthPoints}\nLevel name: {LevelName}\nMusic Vol: {MusicVolume}\nSound Vol: {SoundVolume}\n";
        }


    }

}