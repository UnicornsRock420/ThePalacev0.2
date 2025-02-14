using ThePalace.Common.Client.Constants;
using ThePalace.Common.Server.Entities.Business.Client.Network;
using ThePalace.Core.Entities.EventParams;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;
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

            BO_LOGON? test = null;

            Experiment2();

            //var taskManager = new TaskManager();
            //var job = taskManager.CreateTask(() =>
            //{
            //    Console.WriteLine("Test123");
            //}, null, RunOptions.RunNow | RunOptions.RunOnce);

            //taskManager.Run();
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

                ms.PalaceDeserialize(hdr, typeof(MSG_Header));

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
                        msg,
                        msgType);

                    if (((MSG_LISTOFALLROOMS)msg).Rooms.Count != refNum)
                        throw new InvalidDataException(nameof(MSG_LISTOFALLROOMS) + "-S2C: Deserialization Error!");
                }
            }

            var boObj = DispatchBO(msg);
        }

        private static void Experiment2()
        {
            var packetBytes = (byte[]?)null;
            var hdr = new MSG_Header();
            var msg = (IProtocol?)null;
            var msgType = (Type?)null;
            var refNum = 456;

            using (var ms = new MemoryStream())
            {
                var seed = (uint)Cipher.WizKeytoSeed(ClientConstants.RegCodeSeed);
                var crc = Cipher.ComputeLicenseCrc(seed);
                var ctr = (uint)Cipher.GetSeedFromReg(seed, crc);

                ms.PalaceSerialize(
                    new MSG_LOGON
                    {
                        RegInfo = new RegistrationRec
                        {
                            UserName = "Janus (Test Client)",
                            Reserved = ClientConstants.ClientAgent,
                            UlUploadCaps = (UploadCapabilities)0x41,
                            UlDownloadCaps = (DownloadCapabilities)0x0151,
                            Ul2DEngineCaps = (Upload2DEngineCaps)0x01,
                            Ul2DGraphicsCaps = (Upload2DGraphicsCaps)0x01,

                            Crc = crc,
                            Counter = ctr,

                            PuidCRC = crc,
                            PuidCtr = ctr,
                        }
                    },
                    SerializerOptions.IncludeHeader);

                packetBytes = ms.ToArray();
                var msgHex = packetBytes.ToHex();

                ms.Seek(0, SeekOrigin.Begin);

                ms.PalaceDeserialize(hdr, typeof(MSG_Header));

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
                        msg,
                        msgType);
                }
            }

            var boObj = DispatchBO(msg);
        }

        private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);
        private static IEventHandler DispatchBO(IProtocol msg)
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
            if (boType == null) return null;

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

                return boObj;
            }

            return null;
        }
    }
}