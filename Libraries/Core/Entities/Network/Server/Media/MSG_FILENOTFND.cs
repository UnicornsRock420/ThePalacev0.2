using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Media;

[Mnemonic("fnfe")]
public class MSG_FILENOTFND : EventParams, IProtocolS2C
{
    [Str255] public string? FileName;
}