using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using UserID = int;

namespace Lib.Core.Entities.Network.Client.Users;

[ByteSize(4)]
[Mnemonic("kill")]
public class MSG_KILLUSER : EventParams, IProtocolC2S
{
    public UserID TargetID;
}