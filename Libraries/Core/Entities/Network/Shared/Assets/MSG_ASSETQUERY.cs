using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using ThePalace.Network.Enums;

namespace ThePalace.Core.Entities.Network.Shared.Assets
{
    [Mnemonic("qAst")]
    public partial class MSG_ASSETQUERY : IProtocolC2S, IProtocolS2C
    {
        public LegacyAssetTypes AssetType;
        public AssetSpec AssetSpec;
    }
}