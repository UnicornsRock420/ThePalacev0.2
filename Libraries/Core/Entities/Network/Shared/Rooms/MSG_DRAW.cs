using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Shared.Rooms
{
    [Mnemonic("draw")]
    public partial class MSG_DRAW : IProtocolC2S, IProtocolS2C
    {
        public DrawCmdRec? DrawCmdInfo;
    }
}