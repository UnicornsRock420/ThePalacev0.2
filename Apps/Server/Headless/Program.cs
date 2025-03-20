using System.Collections.Concurrent;
using Lib.Common.Enums.App;
using Lib.Common.Interfaces.Threading;
using Lib.Common.Server.Interfaces;
using Lib.Settings.Singletons;

namespace ThePalace.Server.Headless;

public class Program : IServerApp
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    public static void Main()
    {
    }

    public bool IsServer => SettingsManager.Current.IsServer = true;

    protected ConcurrentDictionary<ThreadQueues, IJob> _jobs = new();
    public IReadOnlyDictionary<ThreadQueues, IJob> Jobs { get; }

    public void Initialize()
    {
        throw new NotImplementedException();
    }

    public IServerSessionState ServerSessionState { get; }
}