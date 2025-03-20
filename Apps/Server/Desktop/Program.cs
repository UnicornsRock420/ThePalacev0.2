using System.Collections.Concurrent;
using Lib.Common.Desktop.Singletons;
using Lib.Common.Enums.App;
using Lib.Common.Interfaces.Threading;
using Lib.Common.Server.Interfaces;

namespace ThePalace.Server.Desktop;

public partial class Program : Form, IServerApp
{
    public Program()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        //// To customize application configuration such as set high DPI settings or default font,
        //// see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Program());
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