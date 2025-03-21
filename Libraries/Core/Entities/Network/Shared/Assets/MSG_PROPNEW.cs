using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Assets;

[Mnemonic("prPn")]
[ByteSize(12)]
public class MSG_PROPNEW : EventParams, IProtocolC2S, IProtocolS2C
{
    public Point Pos;
    public AssetSpec PropSpec;
}