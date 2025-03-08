using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Rooms;

[Mnemonic("opSn")]
public class MSG_SPOTNEW : EventParams, IProtocolC2S
{
}