using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Assets;

[Mnemonic("qAst")]
public partial class MSG_ASSETQUERY : EventParams, IProtocolC2S, IProtocolS2C
{
    public LegacyAssetTypes AssetType;
    public AssetSpec AssetSpec;
}