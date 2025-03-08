using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Rooms;

[Mnemonic("ofNr")]
public class MSG_ROOMINFO : EventParams, IProtocolC2S
{
    public RoomRec? RoomInfo;
}