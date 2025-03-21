using System.Runtime.Serialization;
using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Network;

[Mnemonic("tiyr")]
public class MSG_TIYID : EventParams, IProtocolS2C
{
    [IgnoreDataMember] public string? IpAddress;
}