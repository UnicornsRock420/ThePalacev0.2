using ThePalace.Core.Attributes;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Shared.Assets
{
    [Mnemonic("qAst")]
    public partial class MSG_ASSETQUERY : IProtocolC2S, IProtocolS2C
    {
        public LegacyAssetTypes AssetType;
        public AssetSpec AssetSpec;
    }
}