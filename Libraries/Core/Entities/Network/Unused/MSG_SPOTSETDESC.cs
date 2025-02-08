using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Entities.Shared;

namespace ThePalace.Network.EventTypes.Client.Rooms
{
    [Mnemonic("opSs")]
    [MessagePackObject(true, AllowPrivate = true)]
    public partial class MSG_SPOTSETDESC : IProtocol
    {
    }
}