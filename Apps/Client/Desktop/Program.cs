using System.Collections.Concurrent;
using ThePalace.Common.Threading;

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
            var task = (Task?)null;

            task = TaskManager.Current.CreateTask(() =>
                {
                    // TODO: UI

                    Application.Run(new Program());

                    TaskManager.Current.Shutdown();
                },
                null,
                Job.RunOptions.UseSleepInterval,
                new TimeSpan(750));
            if (task != null)
            {
                _jobs["UI"] = task.Id;
            }

            task = TaskManager.Current.CreateTask(() =>
                {
                    // TODO: Networking-Receive
                },
                null,
                Job.RunOptions.UseManualResetEvent);
            if (task != null)
            {
                _jobs["Networking-Receive"] = task.Id;
            }

            task = TaskManager.Current.CreateTask(() =>
                {
                    // TODO: Networking-Send
                },
                null,
                Job.RunOptions.UseManualResetEvent);
            if (task != null)
            {
                _jobs["Networking-Send"] = task.Id;
            }

            task = TaskManager.Current.CreateTask(() =>
                {
                    // TODO: Media
                },
                null,
                Job.RunOptions.UseManualResetEvent);
            if (task != null)
            {
                _jobs["Media"] = task.Id;
            }

            //// To customize application configuration such as set high DPI settings or default font,
            //// see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            TaskManager.Current.Run();
        }

        public Program()
        {
            InitializeComponent();
        }
    }
}