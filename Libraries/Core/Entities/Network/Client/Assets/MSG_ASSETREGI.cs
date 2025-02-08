using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Assets
{
    [Mnemonic("rAst")]
    public partial class MSG_ASSETREGI : IProtocolC2S
    {
        public AssetRec? AssetInfo;
    }
}