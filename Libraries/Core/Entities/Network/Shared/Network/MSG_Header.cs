using Lib.Core.Attributes.Serialization;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using sint32 = int;
using uint32 = uint;

namespace Lib.Core.Entities.Network.Shared.Network;

[ByteSize(12)]
public class MSG_Header : IStruct
{
    // Mnemonic
    public EventTypes EventType;

    public uint32 Length;
    public sint32 RefNum;
}