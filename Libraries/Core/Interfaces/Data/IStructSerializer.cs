using ThePalace.Core.Enums;

namespace ThePalace.Core.Interfaces.Data;

public interface IStructSerializer : IStruct
{
    void Deserialize(Stream reader, SerializerOptions opts);
    void Serialize(Stream writer, SerializerOptions opts);
}