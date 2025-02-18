using ThePalace.Common.Threading;

namespace ThePalace.Client.Desktop
{
    public partial class Program : Form
    {
        private static readonly Dictionary<string, int> _jobs = new();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var task = (Task?)null;

            task = TaskManager.Instance.CreateTask(() =>
                {
                    // TODO: UI

                    Application.Run(new Program());

                    TaskManager.Instance.Shutdown();
                },
                null,
                Job.RunOptions.UseSleepInterval,
                new TimeSpan(750));
            if (task != null)
            {
                _jobs["UI"] = task.Id;
            }

            task = TaskManager.Instance.CreateTask(() =>
                {
                    // TODO: Networking
                },
                null,
                Job.RunOptions.UseManualResetEvent);
            if (task != null)
            {
                _jobs["Networking"] = task.Id;
            }

            task = TaskManager.Instance.CreateTask(() =>
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

            TaskManager.Instance.Run();
        }

        public Program()
        {
            InitializeComponent();
        }
    }
}