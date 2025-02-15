using ThePalace.Common.Client.Constants;
using ThePalace.Common.Server.Entities.Business.Server.ServerInfo;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Factories.Core;
using ThePalace.Core.Factories.Threading;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.EventsBus;
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

            BO_LISTOFALLROOMS? test = null;

            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a =>
                {
                    return a.FullName?.Contains("ThePalace.Common.Server") == true;
                })
                .SelectMany(a => a.GetTypes())
                .Where(t =>
                {
                    return
                        t.GetInterfaces().Contains(typeof(IEventHandler)) &&
                        t.Namespace?.StartsWith("ThePalace.Common.Server.Entities.Business") == true;
                })
                .ToList();

            var eventBus = EventBus.Instance;
            foreach (var type in types)
            {
                eventBus.Subscribe(type);
            }

            Experiment1();
            Experiment2();
            Experiment3();

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

        private static readonly Type CONST_TYPE_MSG_Header = typeof(MSG_Header);
        private static void Experiment1()
        {
            var packetBytes = (byte[]?)null;
            var hdr = new MSG_Header
            {
                RefNum = 456,
            };
            var msg = (IProtocol?)null;
            var msgType = (Type?)null;

            using (var ms = new MemoryStream())
            {
                ms.PalaceSerialize(
                    hdr.RefNum,
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

                ms.PalaceDeserialize(hdr, CONST_TYPE_MSG_Header);

                if ((ms.Length - ms.Position) != hdr.Length) throw new InvalidDataException(nameof(hdr));

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

                    if (((MSG_LISTOFALLROOMS)msg).Rooms.Count != hdr.RefNum) throw new InvalidDataException(nameof(MSG_LISTOFALLROOMS) + "-S2C: Deserialization Error!");
                }
            }

            var eventBus = EventBus.Instance;
            var boType = GetBOType(msg);
            eventBus.Publish(
                null,
                boType,
                new ProtocolEventParams
                {
                    SourceID = 123,
                    RefNum = hdr.RefNum,
                    Request = msg,
                });
        }

        private static void Experiment2()
        {
            var packetBytes = (byte[]?)null;
            var hdr = new MSG_Header
            {
                RefNum = 456,
            };
            var msg = (IProtocol?)null;
            var msgType = (Type?)null;

            using (var ms = new MemoryStream())
            {
                var seed = (uint)Cipher.WizKeytoSeed(ClientConstants.RegCodeSeed);
                var crc = Cipher.ComputeLicenseCrc(seed);
                var ctr = (uint)Cipher.GetSeedFromReg(seed, crc);

                ms.PalaceSerialize(
                    hdr.RefNum,
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

                ms.PalaceDeserialize(hdr, CONST_TYPE_MSG_Header);

                if ((ms.Length - ms.Position) != hdr.Length) throw new InvalidDataException(nameof(hdr));

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

            var eventBus = EventBus.Instance;
            var boType = GetBOType(msg);
            eventBus.Publish(
                null,
                boType,
                new ProtocolEventParams
                {
                    SourceID = 123,
                    RefNum = hdr.RefNum,
                    Request = msg,
                });
        }

        private static void Experiment3()
        {
            var packetBytes = (byte[]?)null;
            var hdr = new MSG_Header
            {
                RefNum = RndGenerator.Next() % 1337,
            };
            var msg = (IProtocol?)null;
            var msgType = (Type?)null;

            using (var ms = new MemoryStream())
            {
                ms.PalaceSerialize(
                    hdr.RefNum,
                    new MSG_USERDESC
                    {
                        FaceNbr = 1,
                        ColorNbr = 2,
                        PropSpec =
                        [
                            new AssetSpec(12345),
                            new AssetSpec(54321),
                            new AssetSpec(918284),
                        ],
                    },
                    SerializerOptions.IncludeHeader);

                packetBytes = ms.ToArray();
                var msgHex = packetBytes.ToHex();

                ms.Seek(0, SeekOrigin.Begin);

                ms.PalaceDeserialize(hdr, CONST_TYPE_MSG_Header);

                if ((ms.Length - ms.Position) != hdr.Length) throw new InvalidDataException(nameof(hdr));

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

            var eventBus = EventBus.Instance;
            var boType = GetBOType(msg);
            eventBus.Publish(
                null,
                boType,
                new ProtocolEventParams
                {
                    SourceID = 123,
                    RefNum = hdr.RefNum,
                    Request = msg,
                });
        }

        private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);
        private static Type? GetBOType(IProtocol msg)
        {
            var msgType = msg.GetType();

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t =>
                {
                    if (t.IsInterface) return false;

                    var itrfs = t.GetInterfaces();

                    if (!itrfs.Contains(CONST_TYPE_IEventHandler)) return false;

                    if (!itrfs.Any(i => i.IsGenericType && i.GetGenericArguments().Contains(msgType))) return false;

                    return true;
                })
                .Select(t =>
                {
                    foreach (var i in t.GetInterfaces() ?? [])
                        if (i == CONST_TYPE_IEventHandler)
                            return t;

                    return null;
                })
                .FirstOrDefault();
        }
    }
}