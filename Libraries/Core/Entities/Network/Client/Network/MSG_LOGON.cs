using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Network;

[ByteSize(128)]
[Mnemonic("regi")]
public class MSG_LOGON : EventParams, IProtocolC2S
{
    public RegistrationRec RegInfo;

    public MSG_LOGON()
    {
        RegInfo = new RegistrationRec();
    }
}