using ThePalace.Common.Client.Constants;
using ThePalace.Common.Client.Entities.Business.Server.ServerInfo;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Factories.Core;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Core.Interfaces.Network;
using sint16 = System.Int16;

namespace ThePalace.Testing
{
    [TestClass]
    public sealed class TestEventBus
    {
        private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);

        public Type? GetBOType(IProtocol msg)
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

        [TestInitialize]
        public void TestInitialize()
        {
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
        }

        [TestMethod]
        public void MSG_LISTOFALLROOMS()
        {
            var packetBytes = (byte[]?)null;
            var hdr = new MSG_Header();
            var msg = (IProtocol?)new MSG_LISTOFALLROOMS
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
            };
            var msgType = (Type?)typeof(MSG_LISTOFALLROOMS);

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

            Assert.IsNotNull(boType);
        }

        [TestMethod]
        public void MSG_LOGON()
        {
            var seed = (uint)Cipher.WizKeytoSeed(ClientConstants.RegCodeSeed);
            var crc = Cipher.ComputeLicenseCrc(seed);
            var ctr = (uint)Cipher.GetSeedFromReg(seed, crc);

            var packetBytes = (byte[]?)null;
            var refNum = RndGenerator.Next() % 1337;
            var hdr = new MSG_Header
            {
                RefNum = refNum,
            };
            var msg = (IProtocol?)new MSG_LOGON
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
            };
            var msgType = (Type?)typeof(MSG_LOGON);

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

            Assert.IsNotNull(boType);
        }

        [TestMethod]
        public void MSG_USERDESC()
        {
            var packetBytes = (byte[]?)null;
            var refNum = RndGenerator.Next() % 1337;
            var hdr = new MSG_Header
            {
                RefNum = refNum,
            };
            var msg = (IProtocol?)new MSG_USERDESC
            {
                FaceNbr = 1,
                ColorNbr = 2,
                PropSpec =
                [
                    new AssetSpec(12345),
                    new AssetSpec(54321),
                    new AssetSpec(918284),
                ],
            };
            var msgType = (Type?)typeof(MSG_USERDESC);

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

            Assert.IsNotNull(boType);
        }
    }
}