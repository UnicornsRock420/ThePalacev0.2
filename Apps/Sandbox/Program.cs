using ThePalace.Common.Threading;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Media.SoundPlayer;
using static ThePalace.Common.Threading.Job;
using static ThePalace.Media.SoundPlayer.SoundManager;

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

            var data = File.ReadAllText(@"Data\MSG_ROOMDESC.data").FromHex();
            var roomDesc = new RoomDesc(data);
            roomDesc.Deserialize(roomDesc.Stream);

            //Experiment4();
            //Experiment5();
            //Experiment6();
            //Experiment7();
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

        private static void Experiment7()
        {
            SoundManager.Current.Play(@"Media\Yes.mp3");
            SoundManager.Current.PlaySound(@"Media\Boing.wav", PlaySoundFlags.SND_ASYNC | PlaySoundFlags.SND_NOWAIT | PlaySoundFlags.SND_SYSTEM | PlaySoundFlags.SND_LOOP);
        }
    }
}