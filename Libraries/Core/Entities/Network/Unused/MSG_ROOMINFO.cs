using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Interfaces.Network;
using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused;

[Mnemonic("ofNr")]
public class MSG_ROOMINFO : EventParams, IProtocol
{
    public RoomRec? RoomInfo;
}