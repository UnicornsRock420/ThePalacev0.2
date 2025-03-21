using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Auth;

[Mnemonic("rep2")]
public class MSG_ALTLOGONREPLY : EventParams, IProtocolS2C
{
    public RegistrationRec? RegInfo;

    public MSG_ALTLOGONREPLY()
    {
        RegInfo = new RegistrationRec();
    }
}