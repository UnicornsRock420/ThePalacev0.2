using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Network;

[Mnemonic("sErr")]
public class MSG_NAVERROR : EventParams, IProtocolS2C
{
}