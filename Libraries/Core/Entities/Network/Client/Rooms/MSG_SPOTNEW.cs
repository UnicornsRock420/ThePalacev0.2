using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Rooms;

[Mnemonic("opSn")]
public class MSG_SPOTNEW : EventParams, IProtocolC2S
{
}