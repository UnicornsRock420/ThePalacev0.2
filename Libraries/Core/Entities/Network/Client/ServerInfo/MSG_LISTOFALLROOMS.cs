using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.ServerInfo;

[Mnemonic("rLst")]
public class MSG_LISTOFALLROOMS : EventParams, IProtocolC2S
{
}