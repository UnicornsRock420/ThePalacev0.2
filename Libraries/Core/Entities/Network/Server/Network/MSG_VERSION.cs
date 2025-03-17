using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Server.Network;

[Mnemonic("vers")]
public class MSG_VERSION : EventParams, IProtocolS2C
{
    public sint32 build;
    public sint32 major;
    public sint32 minor;
    public sint32 revision;
}