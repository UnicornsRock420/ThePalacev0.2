using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Network;

[Mnemonic("bye ")]
public class MSG_LOGOFF : EventParams, IProtocolC2S
{
}