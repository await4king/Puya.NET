using System;

namespace Puya.Serialization
{
    public interface IJsonSerializable
    {
        string ToJson(JsonSerializationOptions options = null);
    }
}
