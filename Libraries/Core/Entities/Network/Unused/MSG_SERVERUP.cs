using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Entities.Shared;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("init")]
    [MessagePackObject(true, AllowPrivate = true)]
    public partial class MSG_SERVERUP : IProtocol
    {
    }
}