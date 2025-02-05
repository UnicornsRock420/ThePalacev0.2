using uint8 = System.Byte;

namespace ThePalace.Core.Interfaces.Data
{
    public interface IData : IDisposable
    {
        uint8[]? Data { get; set; }
    }
}