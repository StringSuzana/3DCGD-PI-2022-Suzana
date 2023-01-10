namespace Data
{
    public interface ISerializationOption
    {
        string ContentTypeJson { get; }

        T Deserialize<T>(string text);
    }

}