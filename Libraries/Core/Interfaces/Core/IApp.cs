using Lib.Common.Enums.App;
using Lib.Common.Interfaces.Threading;
using Lib.Core.Singletons.EventsBus;

namespace Lib.Core.Interfaces.Core;

public interface IApp
{
    SessionManager SessionManager => SessionManager.Current;
    IReadOnlyDictionary<ThreadQueues, IJob> Jobs { get; }

    void Initialize();
}