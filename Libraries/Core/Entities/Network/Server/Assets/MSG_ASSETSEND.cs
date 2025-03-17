using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Assets;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Assets;

[Mnemonic("sAst")]
public class MSG_ASSETSEND : EventParams, IProtocolS2C
{
    public AssetRec AssetRec;
}