using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Media;

[Mnemonic("qFil")]
public class MSG_FILEQUERY : EventParams, IProtocolC2S
{
    [Str255] public string? FileName;
}