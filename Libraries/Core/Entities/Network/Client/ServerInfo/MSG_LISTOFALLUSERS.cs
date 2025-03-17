using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.ServerInfo;

[ByteSize(0)]
[Mnemonic("uLst")]
public class MSG_LISTOFALLUSERS : EventParams, IProtocolC2S
{
}