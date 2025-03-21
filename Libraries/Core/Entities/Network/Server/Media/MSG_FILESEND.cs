using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Media;

[Mnemonic("sFil")]
public class MSG_FILESEND : EventParams, IProtocolC2S
{
}