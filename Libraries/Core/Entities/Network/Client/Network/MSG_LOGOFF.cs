using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Network;

[Mnemonic("bye ")]
public class MSG_LOGOFF : EventParams, IProtocolC2S
{
}