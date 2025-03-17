using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
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