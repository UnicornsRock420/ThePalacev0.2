using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Entities.Network.Shared.Core;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Enums;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Interfaces;
using sint16 = System.Int16;

namespace Sandbox
{
    public partial class Program : Form
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //// To customize application configuration such as set high DPI settings or default font,
            //// see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Program());

            var packetBytes = (byte[]?)null;
            var hdr = new MSG_Header();
            var msg = (IProtocol?)null;

            using (var ms = new MemoryStream())
            {
                ms.PalaceSerialize(
                    new MSG_LISTOFALLROOMS
                    {
                        Rooms = new List<ListRec>
                        {
                            new ListRec
                            {
                                PrimaryID = 1,
                                Flags = 0,
                                RefNum = 12,

                                Name = "Testing 123",
                            },
                            new ListRec
                            {
                                PrimaryID = 2,
                                Flags = (sint16)RoomFlags.RF_WizardsOnly,
                                RefNum = 24,

                                Name = "Testing 456",
                            },
                        }
                    },
                    456,
                    SerializerOptions.IncludeHeader);

                packetBytes = ms.ToArray();

                ms.Seek(0, SeekOrigin.Begin);

                ms.PalaceDeserialize(hdr, typeof(MSG_Header));

                if ((ms.Length - ms.Position) != hdr.Length)
                    throw new InvalidDataException(nameof(hdr));

                var eventType = hdr.EventType.ToString();
                var msgType = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(t => t.GetTypes())
                    .Where(t => t.Name == eventType)
                    .FirstOrDefault();
                if (msgType != null)
                {
                    msg = (IProtocol?)msgType.GetInstance();

                    ms.PalaceDeserialize(
                        msg,
                        msgType,
                        hdr.RefNum);
                }
            }

            var msgHex = packetBytes.ToHex();
        }

        public Program()
        {
            InitializeComponent();
        }
    }
}