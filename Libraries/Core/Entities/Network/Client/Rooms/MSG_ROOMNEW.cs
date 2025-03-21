using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Auth;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Rooms;

[Mnemonic("nRom")]
[Restricted]
public class MSG_ROOMNEW : EventParams, IProtocolC2S
{
}