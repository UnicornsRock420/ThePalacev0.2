using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using RoomID = short;

namespace ThePalace.Core.Entities.Network.Client.Network;

[Mnemonic("navR")]
public class MSG_ROOMGOTO : EventParams, IProtocolC2S
{
    public RoomID Dest;
}