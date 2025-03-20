using Lib.Common.Client.Interfaces;
using Lib.Common.Enums.App;
using Lib.Common.Interfaces.Threading;
using Lib.Settings.Singletons;

namespace ThePalace.Client.Headless;

public class Program : IClientApp
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    public static void Main()
    {
    }

    public bool IsServer => SettingsManager.Current.IsServer = false;
    public IReadOnlyDictionary<ThreadQueues, IJob> Jobs { get; }

    public void Initialize()
    {
        throw new NotImplementedException();
    }
}