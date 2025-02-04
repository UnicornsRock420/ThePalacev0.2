using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Interfaces;
using sint32 = System.Int32;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Network.Shared.Core
{
    public partial class RawData : IDisposable, IData, IStruct
    {
        [Flags]
        public enum RawDataOptions : uint
        {
            None = 0x00,
            UsePosition = 0x01,
            IncrementPosition = 0x02,
            PurgeReadData = 0x04,
            All = UsePosition | IncrementPosition,
        }

        protected int _position = 0;

        public static explicit operator uint8[](RawData p) => p.Data ?? [];
        public static explicit operator char[](RawData p) => p.Data?.GetChars() ?? [];
        public static explicit operator RawData(uint8[] v) => new(v);
        public static explicit operator RawData(char[] v) => new(v);
        public RawData() =>
            _data = [];
        public RawData(IEnumerable<uint8>? data = null) =>
            _data = new List<uint8>(data ?? []);
        public RawData(IEnumerable<char>? data = null) =>
            _data = new List<uint8>(data?.GetBytes() ?? []);
        public RawData(params uint8[] data) =>
            _data = new List<uint8>(data ?? []);
        public RawData(params char[] data) =>
            _data = new List<uint8>(data?.GetBytes() ?? []);

        public virtual void Dispose()
        {
            _data?.Clear();
            _data = null;

            GC.SuppressFinalize(this);
        }

        public static RawData New() =>
            new();
        public static RawData FromEnumerable(IEnumerable<uint8>? data = null) =>
            new(data);
        public static RawData FromEnumerable(IEnumerable<char>? data = null) =>
            new(data);
        public static RawData FromBytes(uint8[]? data = null) =>
            new(data);
        public static RawData FromChars(char[]? data = null) =>
            new(data);

        protected List<uint8>? _data;
        public virtual uint8[]? Data
        {
            get => _data?.ToArray() ?? [];
            set => _data = new List<uint8>(value ?? []);
        }

        public virtual sint32 Count =>
            _data?.Count ?? 0;
        public virtual sint32 Length =>
            _data?.Count ?? 0;

        #region Read Methods
        public uint8[]? GetData(int max = 0, int offset = 0, bool purge = false)
        {
            if ((_data?.Count ?? 0) < 1) return null;

            var result = _data
                .ToArray()
                .GetRange(max, offset)
                .ToArray();

            if (max > 0 &&
                purge)
            {
                _data?.RemoveRange(offset, max);
            }

            return result;
        }

        public sbyte ReadSByte(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.PurgeReadData)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > Count - 1)
            {
                return 0;
            }

            var result = (sbyte)_data[offset];

            _data.RemoveAt(offset);

            return result;
        }

        public short ReadSInt16(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.PurgeReadData)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > Count - 2)
            {
                return 0;
            }

            var result = _data.ReadSInt16(offset);

            _data.RemoveRange(offset, 2);

            return result;
        }

        public sint32 ReadSInt32(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.PurgeReadData)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > Count - 4)
            {
                return 0;
            }

            var result = _data.ReadSInt32(offset);

            _data.RemoveRange(offset, 4);

            return result;
        }

        public byte ReadByte(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.PurgeReadData)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > _data.Count - 1)
            {
                return 0;
            }

            var result = _data[offset];

            _data.RemoveAt(offset);

            return result;
        }

        public ushort ReadUInt16(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.PurgeReadData)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > _data.Count - 2)
            {
                return 0;
            }

            var result = _data.ReadUInt16(offset);

            _data.RemoveRange(offset, 2);

            return result;
        }

        public uint ReadUInt32(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.PurgeReadData)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > _data.Count - 4)
            {
                return 0;
            }

            var result = _data.ReadUInt32(offset);

            if ((rawDataOptions & RawDataOptions.PurgeReadData) == RawDataOptions.PurgeReadData)
            {
                _data.RemoveRange(offset, 4);
            }

            return result;
        }

        public string? ReadPString(int max, int size = 0, int offset = 0, int delta = 0, RawDataOptions rawDataOptions = RawDataOptions.PurgeReadData)
        {
            if ((_data?.Count ?? 0) < 1) return null;

            if (offset < 1)
            {
                offset = 0;
            }

            if (size < 1)
            {
                size = 1;
            }

            var length = 0;

            switch (size)
            {
                case 4:
                    length = ReadSInt32();

                    break;
                case 2:
                    length = ReadSInt16();

                    break;
                default:
                    length = ReadByte();
                    size = 1;

                    break;
            }

            if (delta > 0)
            {
                length -= delta;
            }

            if (max > 0 && length > max)
            {
                length = max;
            }

            var data = _data
                .ToList()
                .Skip(offset)
                .Take(length)
                .ToArray();

            max -= size;

            if (max > _data.Count)
            {
                max = _data.Count;
            }

            if (max > 0)
            {
                _data.RemoveRange(offset, length);
            }

            return (data.GetString() ?? string.Empty).TrimEnd('\0');
        }

        public string? ReadCString(int offset = 0, bool? peek = false)
        {
            if ((_data?.Count ?? 0) < 1) return null;

            if (offset < 1)
            {
                offset = 0;
            }

            var length = _data
                .Skip(offset)
                .ToList()
                .IndexOf(0);

            var data = _data
                .ToList()
                .Skip(offset)
                .Take(length)
                .ToArray();

            if (peek != null)
                if (peek.Value != true)
                    _data.RemoveRange(offset, length);

            return data.GetString();
        }
        #endregion

        #region Peek Methods
        public int Seek(int offset = 0, SeekOrigin origin = SeekOrigin.Begin)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            switch (origin)
            {
                case SeekOrigin.End:
                    {
                        offset = _data.Count - offset;
                        break;
                    }
                case SeekOrigin.Current:
                    {
                        offset += _position;
                        break;
                    }
            }

            if (offset < 0)
                return 0;
            else if (offset > _data.Count)
                return 0;

            _position = offset;

            return _position;
        }

        public byte PeekByte(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.All)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }

            if (RawDataOptions.UsePosition.IsBit<RawDataOptions, uint>(rawDataOptions))
            {
                offset += _position;
            }

            if (RawDataOptions.IncrementPosition.IsBit<RawDataOptions, uint>(rawDataOptions))
            {
                _position++;
            }

            return _data[offset];
        }

        public short PeekSInt16(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.All)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }

            if (RawDataOptions.UsePosition.IsBit<RawDataOptions, uint>(rawDataOptions))
            {
                offset += _position;
            }

            if (RawDataOptions.IncrementPosition.IsBit<RawDataOptions, uint>(rawDataOptions))
            {
                _position += 2;
            }

            return _data.ReadSInt16(offset);
        }

        public sint32 PeekSInt32(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.All)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }

            if (RawDataOptions.UsePosition.IsBit<RawDataOptions, uint>(rawDataOptions))
            {
                offset += _position;
            }

            if (RawDataOptions.IncrementPosition.IsBit<RawDataOptions, uint>(rawDataOptions))
            {
                _position += 4;
            }

            return _data.ReadSInt32(offset);
        }

        public ushort PeekUInt16(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.All)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }

            if (RawDataOptions.UsePosition.IsBit<RawDataOptions, uint>(rawDataOptions))
            {
                offset += _position;
            }

            if (RawDataOptions.IncrementPosition.IsBit<RawDataOptions, uint>(rawDataOptions))
            {
                _position += 2;
            }

            return _data.ReadUInt16(offset);
        }

        public uint PeekUInt32(int offset = 0, RawDataOptions rawDataOptions = RawDataOptions.All)
        {
            if ((_data?.Count ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }

            if (RawDataOptions.UsePosition.IsBit<RawDataOptions, uint>(rawDataOptions))
            {
                offset += _position;
            }

            if (RawDataOptions.IncrementPosition.IsBit<RawDataOptions, uint>(rawDataOptions))
            {
                _position += 4;
            }

            return _data.ReadUInt32(offset);
        }

        public string? PeekPString(int max, int size = 0, int offset = 0)
        {
            if ((_data?.Count ?? 0) < 1) return null;

            if (offset < 1)
            {
                offset = 0;
            }

            if (size < 1)
            {
                size = 1;
            }

            var length = 0;

            switch (size)
            {
                case 4:
                    length = PeekSInt32(offset, RawDataOptions.None);

                    break;
                case 2:
                    length = PeekSInt16(offset, RawDataOptions.None);

                    break;
                default:
                    length = PeekByte(offset, RawDataOptions.None);
                    size = 1;

                    break;
            }

            if (length > max)
            {
                length = max;
            }

            var data = _data.ToList()
                .Skip(offset + size)
                .Take(length)
                .ToArray();

            return data.GetString();

        }
        #endregion

        #region Write Methods
        public void SetData(IEnumerable<uint8>? data = null) =>
            _data = new List<uint8>(data ?? []);
        public void SetData(uint8[]? data = null) =>
            _data = new List<uint8>(data ?? []);

        public void AppendBytes(uint8[]? data = null)
        {
            if (Count < 1)
            {
                _data = new List<uint8>(data);

                return;
            }

            _data.AddRange(data);
        }

        public void WriteByte(byte value)
        {
            _data ??= [];

            _data.Add(value);
        }

        public void WriteBytes(byte[] value, int max = 0, int offset = 0)
        {
            _data ??= [];

            if (max < 1 &&
                offset < 1)
                _data.AddRange(value);
            else
                _data.AddRange(value
                    .Skip(offset)
                    .Take(max)
                    .ToList());
        }

        public void WriteInt16(short value)
        {
            _data ??= [];

            _data.AddRange(value.WriteInt16());
        }

        public void WriteInt32(sint32 value)
        {
            _data ??= [];

            _data.AddRange(value.WriteInt32());
        }

        public void WriteInt16(ushort source)
        {
            _data ??= [];

            _data.AddRange(source.WriteUInt16());
        }

        public void WriteInt32(uint source)
        {
            _data ??= [];

            _data.AddRange(source.WriteUInt32());
        }

        public void WritePString(string source, int max, int size = 0, bool padding = true)
        {
            _data ??= [];

            if (size < 1)
            {
                size = 1;
            }

            _data.AddRange(source.WritePString(max, size, padding));
        }

        public void WriteCString(string source)
        {
            _data ??= [];

            _data.AddRange(source.WriteCString());
        }
        #endregion

        #region Helper Methods
        public void Clear()
        {
            if (_data == null)
                _data = [];
            else
                _data.Clear();
        }

        public void DropBytes(int length = 0, int offset = 0)
        {
            if ((_data?.Count ?? 0) < 1) return;

            if (offset < 1)
            {
                offset = 0;
            }

            if (length < 1)
            {
                if (offset < 1)
                {
                    _data.Clear();

                    return;
                }

                length = _data.Count - offset;
            }

            _data.RemoveRange(offset, length);
        }

        public void PadBytes(int mod)
        {
            _data ??= [];

            for (var j = _data.Count % mod; j > 0; j--)
            {
                _data.Add(0);
            }
        }

        public static int PadOffset(int mod, int value)
        {
            value += value % mod;
            return value;
        }

        public void AlignBytes(int mod)
        {
            _data ??= [];

            for (var j = mod - _data.Count % mod; j > 0; j--)
            {
                _data.Add(0);
            }
        }

        public void Deserialize(Stream data)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}