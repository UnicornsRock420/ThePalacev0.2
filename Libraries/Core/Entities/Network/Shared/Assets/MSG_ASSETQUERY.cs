using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Assets;

[Mnemonic("qAst")]
public class MSG_ASSETQUERY : EventParams, IProtocolC2S, IProtocolS2C
{
    public AssetSpec AssetSpec;
    public LegacyAssetTypes AssetType;
}