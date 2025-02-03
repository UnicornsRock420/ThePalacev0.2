using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Network.Client.Users
{
    [ByteSize(4)]
    [Mnemonic("kill")]
    public partial class MSG_KILLUSER : IProtocolC2S
    {
        public uint32 TargetID;
    }
}