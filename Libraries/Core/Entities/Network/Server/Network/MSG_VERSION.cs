using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using sint32 = int;

namespace ThePalace.Core.Entities.Network.Server.Network;

[Mnemonic("vers")]
public class MSG_VERSION : EventParams, IProtocolS2C
{
    public sint32 build;
    public sint32 major;
    public sint32 minor;
    public sint32 revision;
}