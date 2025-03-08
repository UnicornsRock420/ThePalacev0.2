using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Data;
using sint32 = int;
using uint32 = uint;

namespace ThePalace.Core.Entities.Network.Shared.Network;

[ByteSize(12)]
public class MSG_Header : IStruct
{
    // Mnemonic
    public EventTypes EventType;

    public uint32 Length;
    public sint32 RefNum;
}