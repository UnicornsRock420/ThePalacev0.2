using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using sint32 = System.Int32;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Network.Shared.Network;

[ByteSize(12)]
public partial class MSG_Header : IStruct
{
    // Mnemonic
    public EventTypes EventType;

    public uint32 Length;
    public sint32 RefNum;
}