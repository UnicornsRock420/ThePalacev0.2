using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.Network;

[Mnemonic("bye ")]
public partial class MSG_LOGOFF : EventParams, IProtocolS2C
{
    public sint32 NbrUsers;
}