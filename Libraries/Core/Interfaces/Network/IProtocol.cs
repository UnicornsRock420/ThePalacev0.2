using ThePalace.Common.Interfaces.Threading;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Core.Interfaces.Network;

public interface IProtocol : IStruct, ICmd, IEventParams
{
}