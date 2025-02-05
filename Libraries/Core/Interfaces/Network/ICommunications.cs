using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Interfaces.Network
{
    public interface ICommunications : IStruct
    {
        public string? Text { get; set; }
    }
}