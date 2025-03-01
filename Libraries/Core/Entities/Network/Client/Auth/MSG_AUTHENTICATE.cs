using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Auth;

[Mnemonic("auth")]
public partial class MSG_AUTHENTICATE : EventParams, IProtocolC2S
{
}