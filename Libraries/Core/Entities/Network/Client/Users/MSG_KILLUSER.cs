using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Auth;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;
using UserID = int;

namespace Lib.Core.Entities.Network.Client.Users;

[ByteSize(4)]
[Mnemonic("kill")]
[Restricted]
public class MSG_KILLUSER : EventParams, IProtocolC2S
{
    public UserID TargetID;
}