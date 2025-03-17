using Lib.Common.Interfaces.Threading;
using Lib.Core.Interfaces.Data;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Core.Interfaces.Network;

public interface IProtocol : IStruct, ICmd, IEventParams
{
}