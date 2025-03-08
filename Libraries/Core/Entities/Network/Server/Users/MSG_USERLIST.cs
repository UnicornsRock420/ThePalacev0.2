using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using sint32 = int;

namespace ThePalace.Core.Entities.Network.Server.Users;

[Mnemonic("rprs")]
public class MSG_USERLIST : EventParams, IStructRefNum, IProtocolS2C
{
    public List<UserRec>? Users;

    [IgnoreDataMember]
    public sint32 RefNum
    {
        get => Users?.Count ?? 0;
        set
        {
            if (value > 0) return;

            Users = [];
        }
    }
}