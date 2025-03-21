using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Rooms;

[Mnemonic("endr")]
public class MSG_ROOMDESCEND : EventParams, IProtocolS2C
{
}