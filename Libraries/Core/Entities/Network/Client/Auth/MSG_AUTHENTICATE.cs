using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Auth;

[Mnemonic("auth")]
public class MSG_AUTHENTICATE : EventParams, IProtocolC2S
{
}