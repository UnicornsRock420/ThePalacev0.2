using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Assets;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Assets;

[Mnemonic("sAst")]
public class MSG_ASSETSEND : EventParams, IProtocolS2C
{
    public AssetRec AssetInfo;
}