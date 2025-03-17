using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Rooms;

[Mnemonic("endr")]
public class MSG_ROOMDESCEND : EventParams, IProtocolS2C
{
}