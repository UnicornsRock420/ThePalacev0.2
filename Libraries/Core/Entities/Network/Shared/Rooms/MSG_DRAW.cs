using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Rooms;

[Mnemonic("draw")]
public class MSG_DRAW : EventParams, IProtocolC2S, IProtocolS2C
{
    public DrawCmdRec? DrawCmdInfo;
}