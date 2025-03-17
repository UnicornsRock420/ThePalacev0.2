using System.Runtime.Serialization;
using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Interfaces.Data;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Server.Users;

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