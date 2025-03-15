using System.Runtime.Serialization;
using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network;

[Mnemonic("tiyr")]
public class MSG_TIYID : EventParams, IProtocolS2C
{
    [IgnoreDataMember] public string? IpAddress;
}