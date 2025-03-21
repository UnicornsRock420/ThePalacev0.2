using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Media;

[Mnemonic("qFil")]
public class MSG_FILEQUERY : EventParams, IProtocolC2S
{
    [Str255] public string? FileName;
}