using ThePalace.Core.Enums;

namespace ThePalace.Core.Interfaces.Data
{
    public interface IStructSerializer : IStruct
    {
        void Deserialize(ref int refNum, Stream reader, SerializerOptions opts);
        void Serialize(ref int refNum, Stream writer, SerializerOptions opts);
    }
}