using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using RoomID = System.Int16;
using sint32 = System.Int32;
using uint32 = System.UInt32;
using uint16 = System.UInt16;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(128)]
    public partial class RegistrationRec : IProtocol
    {
        public uint32 Crc;
        public uint32 Counter;
        public Str31 UserName;
        public Str31 WizPassword;
        public sint32 AuxFlags;
        public uint32 PuidCtr;
        public uint32 PuidCRC;
        public uint32 DemoElapsed;
        public uint32 TotalElapsed;
        public uint32 DemoLimit;
        public RoomID DesiredRoom;

        [ByteSize(6)]
        public uint8[] Reserved;

        public uint16 UlRequestedProtocolVersionMajorVersion;
        public uint16 UlRequestedProtocolVersionMinorVersion;
        public UploadCapabilities UlUploadCaps;
        public DownloadCapabilities UlDownloadCaps;
        public Upload2DEngineCaps Ul2DEngineCaps;
        public Upload2DGraphicsCaps Ul2DGraphicsCaps;
        public Upload3DEngineCaps Ul3DEngineCaps;

        public void Deserialize(Stream data)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}