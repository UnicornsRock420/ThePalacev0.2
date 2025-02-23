using System.Collections.Concurrent;
using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Common.Desktop.Factories;
using ThePalace.Common.Threading;
using static ThePalace.Common.Threading.Job;

namespace ThePalace.Client.Desktop
{
    public partial class Program : Form
    {
        private static readonly ConcurrentDictionary<string, int> _jobs = new();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //// To customize application configuration such as set high DPI settings or default font,
            //// see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var task = (Task?)null;
            var uiLoaded = false;

            task = TaskManager.Current.CreateTask(q =>
                {
                    // TODO: UI

                    if (!uiLoaded)
                    {
                        uiLoaded = true;

                        var sessionState = new DesktopSessionState();
                        var app = new App();

                        app.Initialize(sessionState);
                    }
                },
                null,
                RunOptions.UseSleepInterval,
                new TimeSpan(750));
            if (task != null)
            {
                _jobs["UI"] = task.Id;
            }

            task = TaskManager.Current.CreateTask(q =>
                {
                    // TODO: Networking-Receive
                },
                null,
                RunOptions.UseManualResetEvent);
            if (task != null)
            {
                _jobs["Networking-Receive"] = task.Id;
            }

            task = TaskManager.Current.CreateTask(q =>
                {
                    // TODO: Networking-Send
                },
                null,
                RunOptions.UseManualResetEvent);
            if (task != null)
            {
                _jobs["Networking-Send"] = task.Id;
            }

            task = TaskManager.Current.CreateTask(q =>
                {
                    // TODO: Media
                },
                null,
                RunOptions.UseManualResetEvent);
            if (task != null)
            {
                _jobs["Media"] = task.Id;
            }

            task = TaskManager.Current.CreateTask(q =>
                {
                    TaskManager.Current.Run();

                    TaskManager.Current.Shutdown();
                },
                null,
                RunOptions.UseSleepInterval | RunOptions.RunNow);
            if (task != null)
            {
                _jobs["Media"] = task.Id;
            }

            Application.Run(FormsManager.Current);
        }

        public Program()
        {
            InitializeComponent();
        }
    }
}