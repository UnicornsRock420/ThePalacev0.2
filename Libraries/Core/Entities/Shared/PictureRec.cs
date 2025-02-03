using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Core;
using ThePalace.Core.Interfaces;
using sint16 = System.Int16;
using sint32 = System.Int32;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(12)]
    public partial class PictureRec : IDisposable, IData, IProtocol
    {
        public PictureRec()
        {
            _data = new();
        }
        public PictureRec(uint8[]? data = null)
        {
            _data = new(data);
        }

        public void Dispose()
        {
            _data?.Dispose();
            _data = null;

            GC.SuppressFinalize(this);
        }

        private RawData? _data;
        public uint8[]? Data
        {
            get => _data.Data;
            set => _data.Data = value;
        }

        public sint32 RefCon;
        public sint16 PicID;
        public sint16 PicNameOfst;
        public sint16 TransColor;
        public sint16 Reserved;

        public string? Name;
    }
}