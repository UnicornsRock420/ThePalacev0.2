using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Entities.Shared.Types;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("wmsg")]
    [MessagePackObject(true, AllowPrivate = true)]
    public partial class MSG_WMSG : IProtocolCommunications, IProtocolSerializer
    {
        public CString Text;

        public void Deserialize()
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}