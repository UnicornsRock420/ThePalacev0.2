using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Network;

[Mnemonic("bye ")]
public class MSG_LOGOFF : EventParams, IProtocolC2S
{
}