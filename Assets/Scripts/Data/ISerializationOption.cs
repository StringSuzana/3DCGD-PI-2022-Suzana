namespace MyGame
{
    public interface ISerializationOption
    {
        string ContentType_Json { get; }

        T Deserialize<T>(string text);
    }

}