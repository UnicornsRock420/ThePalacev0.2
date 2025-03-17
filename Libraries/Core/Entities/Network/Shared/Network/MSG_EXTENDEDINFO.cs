using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Network;

[Mnemonic("sInf")]
public class MSG_EXTENDEDINFO : EventParams, IProtocolC2S, IProtocolS2C
{
    public ServerExtInfoTypes Flags;
}