using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using sint16 = short;

namespace ThePalace.Core.Entities.Network.Shared.Users;

[Mnemonic("usrF")]
public class MSG_USERFACE : EventParams, IProtocolC2S, IProtocolS2C
{
    public sint16 FaceNbr;
}