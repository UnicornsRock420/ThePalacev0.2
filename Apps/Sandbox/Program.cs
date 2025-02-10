using ThePalace.Core.Entities.EventParams;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Factories.Threading;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;
using static ThePalace.Core.Factories.Threading.Job;
using sint16 = System.Int16;

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

            //Func<string, bool>? where = l => l == "123";
            //var test = new List<string> { "Test", "123" }
            //    .Where(where)
            //    .ToList();

            //Experiment1();

            //var iStructTypes = AppDomain.CurrentDomain
            //    .GetAssemblies()
            //    .SelectMany(t => t.GetTypes())
            //    .Where(t => !t.IsInterface && t.GetInterfaces().Contains(typeof(IStruct)));

            //var container = new DIContainer();
            //container.RegisterTypes(iStructTypes);

            //Experiment1();

            var taskManager = new TaskManager();
            var job = taskManager.CreateTask(() =>
            {
                Console.WriteLine("Test123");
            }, null, RunOptions.RunNow | RunOptions.RunOnce);

            taskManager.Run();
        }

        public Program()
        {
            InitializeComponent();
        }

        private static void Experiment1()
        {
            var packetBytes = (byte[]?)null;
            var hdr = new MSG_Header();
            var msg = (IProtocol?)null;
            var msgType = (Type?)null;
            var refNum = 456;

            using (var ms = new MemoryStream())
            {
                ms.PalaceSerialize(
                    ref refNum,
                    new MSG_LISTOFALLROOMS
                    {
                        Rooms = new()
                        {
                            new()
                            {
                                PrimaryID = 1,
                                Flags = (sint16)RoomFlags.NoPainting,
                                RefNum = 12,

                                Name = "Testing 123",
                            },
                            new()
                            {
                                PrimaryID = 2,
                                Flags = (sint16)RoomFlags.WizardsOnly,
                                RefNum = 24,

                                Name = "Testing 456",
                            },
                        }
                    },
                    SerializerOptions.IncludeHeader);

                packetBytes = ms.ToArray();
                var msgHex = packetBytes.ToHex();

                ms.Seek(0, SeekOrigin.Begin);

                ms.PalaceDeserialize(ref hdr.RefNum, hdr, typeof(MSG_Header));

                if ((ms.Length - ms.Position) != hdr.Length)
                    throw new InvalidDataException(nameof(hdr));

                var eventType = hdr.EventType.ToString();
                msgType = AppDomain.CurrentDomain
                   .GetAssemblies()
                   .SelectMany(t => t.GetTypes())
                   .Where(t => t.Name == eventType)
                   .FirstOrDefault();
                if (msgType != null)
                {
                    msg = (IProtocol?)msgType.GetInstance();

                    ms.PalaceDeserialize(
                        ref hdr.RefNum,
                        msg,
                        msgType);
                }
            }

            DispatchBO(msg);
        }

        private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);
        private static void DispatchBO(IProtocol msg)
        {
            var msgType = msg.GetType();

            var boType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(t =>
                    t.GetTypes()
                        .Where(t =>
                        {
                            if (t.IsInterface) return false;

                            var itrfs = t.GetInterfaces();

                            if (!itrfs.Contains(CONST_TYPE_IEventHandler)) return false;

                            if (!itrfs.Any(i => i.IsGenericType && i.GetGenericArguments().Contains(msgType))) return false;

                            return true;
                        }))
                .Select(t =>
                {
                    foreach (var i in t.GetInterfaces() ?? [])
                        if (i == CONST_TYPE_IEventHandler)
                            return t;

                    return null;
                })
                .FirstOrDefault();

            var boObj = boType?.GetInstance<IEventHandler>();
            if (boObj != null)
            {
                boObj.Handle(
                    new { },
                    new ProtocolEventParams
                    {
                        SourceID = 0,
                        RefNum = 123,
                        Request = (IProtocol?)msg,
                        ConnectionState = null,
                    });
            }
        }
    }
}