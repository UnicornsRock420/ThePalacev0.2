using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Shared.Assets;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Assets;

[Mnemonic("rAst")]
public class MSG_ASSETREGI : EventParams, IProtocolC2S
{
    public AssetRec? AssetRec;
}