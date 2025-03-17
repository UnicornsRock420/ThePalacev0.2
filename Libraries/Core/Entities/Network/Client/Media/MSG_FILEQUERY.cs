using Lib.Common.Attributes;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Media;

[Mnemonic("qFil")]
public class MSG_FILEQUERY : EventParams, IProtocolC2S
{
    [Str255] public string? FileName;
}