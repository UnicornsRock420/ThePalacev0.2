using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Users;

[Mnemonic("eprs")]
public class MSG_USEREXIT : EventParams, IProtocolS2C
{
}