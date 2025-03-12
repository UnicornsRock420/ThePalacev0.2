using ThePalace.Common.Enums.App;
using ThePalace.Common.Interfaces.Threading;

namespace ThePalace.Core.Interfaces.Core;

public interface IApp<out TSessionState>
    where TSessionState : ISessionState
{
    TSessionState SessionState { get; }
    IReadOnlyDictionary<ThreadQueues, IJob> Jobs { get; }

    void Initialize();
}