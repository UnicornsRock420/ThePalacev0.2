using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Rooms;

[Mnemonic("endr")]
public class MSG_ROOMDESCEND : EventParams, IProtocolS2C
{
}