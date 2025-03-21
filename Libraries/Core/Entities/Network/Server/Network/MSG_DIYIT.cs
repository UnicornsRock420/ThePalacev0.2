using System.Runtime.Serialization;
using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Network;

[Mnemonic("ryit")]
public class MSG_DIYIT : EventParams, IProtocolS2C
{
    [IgnoreDataMember] public string? IpAddress;
}