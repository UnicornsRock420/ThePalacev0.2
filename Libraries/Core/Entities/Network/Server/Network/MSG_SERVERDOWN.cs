using System.Runtime.Serialization;
using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Server.Network;

[ByteSize(4)]
[Mnemonic("down")]
public class MSG_SERVERDOWN : EventParams, IStructRefNum, IProtocolS2C
{
    [IgnoreDataMember] public ServerDownFlags ServerDownFlags;

    [CString] public string? WhyMessage;

    [IgnoreDataMember]
    public sint32 RefNum
    {
        get => (sint32)ServerDownFlags;
        set => ServerDownFlags = (ServerDownFlags)value;
    }
}