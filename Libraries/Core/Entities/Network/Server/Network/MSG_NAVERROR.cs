using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Network;

[Mnemonic("sErr")]
public class MSG_NAVERROR : EventParams, IProtocolS2C
{
}