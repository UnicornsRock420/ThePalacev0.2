using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Rooms
{
    [Mnemonic("draw")]
    public partial class MSG_DRAW : EventParams, IProtocolC2S, IProtocolS2C
    {
        public DrawCmdRec? DrawCmdInfo;
    }
}