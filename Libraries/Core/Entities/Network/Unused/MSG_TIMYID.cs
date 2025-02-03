using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Entities.Shared;

namespace ThePalace.Network.EventTypes.Client.Network
{
    [Mnemonic("timy")]
    [MessagePackObject(true, AllowPrivate = true)]
    public partial class MSG_TIMYID : IProtocol
    {
    }
}