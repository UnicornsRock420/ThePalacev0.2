using Lib.Core.Interfaces.Data;

namespace Lib.Core.Interfaces.Network;

public interface ICommunications : IStruct
{
    string? Text { get; set; }
}