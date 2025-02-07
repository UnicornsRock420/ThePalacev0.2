using ThePalace.Core.Enums.Palace;

namespace ThePalace.Core.Interfaces.Data
{
    public interface IStructSerializer : IStruct
    {
        void Deserialize(ref int refNum, Stream reader, SerializerOptions opts);
        void Serialize(ref int refNum, Stream writer, SerializerOptions opts);
    }
}