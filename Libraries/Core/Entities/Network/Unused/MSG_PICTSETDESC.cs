using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("sPct")]
    public class MSG_PICTSETDESC : EventParams, IProtocol
    {
    }
}