using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network;

[Mnemonic("sInf")]
public class MSG_EXTENDEDINFO : EventParams, IProtocolC2S, IProtocolS2C
{
    public ServerExtInfoTypes Flags;
}