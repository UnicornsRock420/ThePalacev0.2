using ThePalace.Core.Enums;

namespace ThePalace.Core.Interfaces
{
    public interface IStructSerializer : IStruct
    {
        void Deserialize(int refNum, Stream reader, SerializerOptions opts);
        void Serialize(out int refNum, Stream writer, SerializerOptions opts);
    }
}