using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Media;

[Mnemonic("fnfe")]
public class MSG_FILENOTFND : EventParams, IProtocolS2C
{
    [Str255] public string? FileName;
}