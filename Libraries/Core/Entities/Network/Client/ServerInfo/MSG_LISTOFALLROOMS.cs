using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.ServerInfo;

[Mnemonic("rLst")]
public class MSG_LISTOFALLROOMS : EventParams, IProtocolC2S
{
}