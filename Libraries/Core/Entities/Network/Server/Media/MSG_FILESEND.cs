using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Media;

[Mnemonic("sFil")]
public class MSG_FILESEND : EventParams, IProtocolC2S
{
}