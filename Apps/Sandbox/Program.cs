using ThePalace.Core.Entities.Core;
using ThePalace.Core.Factories.Threading;
using ThePalace.Core.Interfaces.Data;
using static ThePalace.Core.Factories.Threading.Job;

namespace Sandbox
{
    public partial class Program : Form
    {
        //private static readonly ManualResetEvent _event = new(false);

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //// To customize application configuration such as set high DPI settings or default 

            //// see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Program());

            //Experiment4();
            //Experiment5();
            //Experiment6();
        }

        public Program()
        {
            InitializeComponent();
        }

        private static void Experiment4()
        {
            Func<string, bool>? where = l => l == "123";
            var test = new List<string> { "Test", "123" }
                .Where(where)
                .ToList();
        }

        private static void Experiment5()
        {
            var iStructTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => !t.IsInterface && t.GetInterfaces().Contains(typeof(IStruct)));

            var container = new DIContainer();
            container.RegisterTypes(iStructTypes);
        }

        private static void Experiment6()
        {
            var taskManager = new TaskManager();
            var job = taskManager.CreateTask(() =>
            {
                Console.WriteLine("Test123");
            }, null, RunOptions.RunNow | RunOptions.RunOnce);

            taskManager.Run();
        }

    }
}