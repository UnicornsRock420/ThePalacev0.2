using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("wprs")]
    [MessagePackObject(true, AllowPrivate = true)]
    public partial class MSG_USERENTER : IProtocol
    {
        public UserRec? User;
    }
}