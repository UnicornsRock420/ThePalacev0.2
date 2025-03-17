using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Media;

[Mnemonic("sFil")]
public class MSG_FILESEND : EventParams, IProtocolC2S
{
}