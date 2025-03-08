using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Assets;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Assets;

[Mnemonic("rAst")]
public class MSG_ASSETREGI : EventParams, IProtocolC2S
{
    public AssetRec? AssetInfo;
}