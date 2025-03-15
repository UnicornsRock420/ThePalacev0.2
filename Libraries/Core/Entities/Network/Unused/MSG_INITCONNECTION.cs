using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("cLog")]
    public class MSG_INITCONNECTION : EventParams, IProtocol
    {
    }
}