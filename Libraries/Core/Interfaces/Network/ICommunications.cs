using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Interfaces.Network
{
    public interface ICommunications : IStruct, IProtocolEcho
    {
        string? Text { get; set; }
    }
}