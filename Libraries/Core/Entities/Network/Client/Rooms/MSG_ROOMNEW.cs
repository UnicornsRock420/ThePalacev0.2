using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Rooms;

[Mnemonic("nRom")]
public class MSG_ROOMNEW : EventParams, IProtocolC2S
{
}