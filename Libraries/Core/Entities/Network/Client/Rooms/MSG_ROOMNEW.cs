using Lib.Common.Attributes;
using Lib.Core.Attributes.Auth;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Rooms;

[Mnemonic("nRom")]
[Restricted]
public class MSG_ROOMNEW : EventParams, IProtocolC2S
{
}