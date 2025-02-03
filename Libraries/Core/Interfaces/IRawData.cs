using uint8 = System.Byte;

namespace ThePalace.Core.Interfaces
{
    public interface IRawData : IData
    {
        void AppendBytes(uint8[]? data);
    }
}