using System.Runtime.Serialization;
using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network;

[Mnemonic("ryit")]
public class MSG_DIYIT : EventParams, IProtocolS2C
{
    [IgnoreDataMember] public string? IpAddress;
}