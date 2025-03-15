using ThePalace.Common.Enums.App;
using ThePalace.Common.Interfaces.Threading;
using ThePalace.Core.Factories.Core;

namespace ThePalace.Core.Interfaces.Core;

public interface IApp
{
    SessionManager SessionManager => SessionManager.Current;
    IReadOnlyDictionary<ThreadQueues, IJob> Jobs { get; }

    void Initialize();
}