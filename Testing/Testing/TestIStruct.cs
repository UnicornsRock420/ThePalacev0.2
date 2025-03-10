using ThePalace.Common.Client.Constants;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Enums;
using ThePalace.Core.Helpers.Core;
using sint16 = short;

namespace ThePalace.Testing;

public static class TestIStruct
{
    public static MSG_LISTOFALLROOMS MSG_LISTOFALLROOMS =>
        new()
        {
            Rooms =
            [
                new()
                {
                    PrimaryID = 1,
                    Flags = (sint16)RoomFlags.NoPainting,
                    RefNum = 12,

                    Name = "Testing 123"
                },

                new()
                {
                    PrimaryID = 2,
                    Flags = (sint16)RoomFlags.WizardsOnly,
                    RefNum = 24,

                    Name = "Testing 456"
                }
            ]
        };

    public static MSG_LOGON MSG_LOGON
    {
        get
        {
            var seed = (uint)Cipher.WizKeytoSeed(ClientConstants.RegCodeSeed);
            var crc = Cipher.ComputeLicenseCrc(seed);
            var ctr = (uint)Cipher.GetSeedFromReg(seed, crc);

            return new MSG_LOGON
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
                    PuidCtr = ctr
                }
            };
        }
    }

    public static MSG_USERDESC MSG_USERDESC =>
        new()
        {
            FaceNbr = 1,
            ColorNbr = 2,
            PropSpec =
            [
                new AssetSpec(12345),
                new AssetSpec(54321),
                new AssetSpec(918284)
            ]
        };
}