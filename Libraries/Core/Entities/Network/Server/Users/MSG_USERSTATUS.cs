using System.Runtime.Serialization;
using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Users;

[Mnemonic("uSta")]
public class MSG_USERSTATUS : EventParams, IProtocolS2C
{
    public UserFlags Flags;

    [IgnoreDataMember] public Guid Hash;
}