using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Entities.Shared;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("nPct")]
    [MessagePackObject(true, AllowPrivate = true)]
    public partial class MSG_PICTNEW : IProtocol
    {
    }
}