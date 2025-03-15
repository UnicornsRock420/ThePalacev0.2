using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused;

[Mnemonic("ofNr")]
public partial class MSG_ROOMINFO : EventParams, IProtocol
{
    public RoomRec? RoomInfo;
}