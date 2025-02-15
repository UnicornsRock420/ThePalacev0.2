using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Interfaces.Network
{
    public interface ICommunications : IStruct
    {
        string? Text { get; set; }
    }
}