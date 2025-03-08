using uint8 = byte;

namespace ThePalace.Core.Interfaces.Data;

public interface IData : IDisposable
{
    uint8[]? Data { get; set; }
}