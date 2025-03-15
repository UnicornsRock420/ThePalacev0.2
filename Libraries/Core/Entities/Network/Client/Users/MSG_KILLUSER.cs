using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using UserID = int;

namespace ThePalace.Core.Entities.Network.Client.Users;

[ByteSize(4)]
[Mnemonic("kill")]
public class MSG_KILLUSER : EventParams, IProtocolC2S
{
    public UserID TargetID;
}