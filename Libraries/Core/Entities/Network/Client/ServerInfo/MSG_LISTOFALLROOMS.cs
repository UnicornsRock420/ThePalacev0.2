using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.ServerInfo;

[Mnemonic("rLst")]
public class MSG_LISTOFALLROOMS : EventParams, IProtocolC2S
{
}