﻿using System;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Interfaces.Data;
using sint32 = System.Int32;
using uint8 = System.Byte;

namespace ThePalace.Core.Factories
{
    public partial class RawStream : IDisposable, IData, IStruct
    {
        [Flags]
        public enum RawStreamOptions : uint
        {
            None = 0x00,
            UsePosition = 0x01,
            IncrementPosition = 0x02,
            All = UsePosition | IncrementPosition,
        }

        public static explicit operator uint8[](RawStream p) => p.Data ?? [];
        public static explicit operator char[](RawStream p) => p.Data?.GetChars() ?? [];
        public static explicit operator RawStream(uint8[] v) => new(v);
        public static explicit operator RawStream(char[] v) => new(v);
        public RawStream() =>
            _stream = new();
        public RawStream(IEnumerable<uint8>? data = null) =>
            _stream = new MemoryStream(data?.ToArray() ?? []);
        public RawStream(IEnumerable<char>? data = null) =>
            _stream = new MemoryStream(data?.GetBytes() ?? []);
        public RawStream(params uint8[] data) =>
            _stream = new MemoryStream(data ?? []);
        public RawStream(params char[] data) =>
            _stream = new MemoryStream(data?.GetBytes() ?? []);

        public virtual void Dispose()
        {
            _stream?.Dispose();
            _stream = null;

            GC.SuppressFinalize(this);
        }

        public static RawStream New() =>
            new();
        public static RawStream FromEnumerable(IEnumerable<uint8>? data = null) =>
            new(data);
        public static RawStream FromEnumerable(IEnumerable<char>? data = null) =>
            new(data);
        public static RawStream FromBytes(uint8[]? data = null) =>
            new(data);
        public static RawStream FromChars(char[]? data = null) =>
            new(data);

        protected MemoryStream? _stream;
        public virtual uint8[]? Data
        {
            get => _stream?.ToArray() ?? [];
            set => _stream = new MemoryStream(value ?? []);
        }

        public virtual sint32 Count =>
            (sint32)(_stream?.Length ?? 0);
        public virtual sint32 Length =>
            (sint32)(_stream?.Length ?? 0);

        #region Read Methods
        public uint8[]? GetData(int max = 0, int offset = 0, bool purge = false)
        {
            if ((_stream?.Length ?? 0) < 1) return null;

            if (max < 1 ||
                max > _stream.Length)
            {
                max = (int)_stream.Length;
            }
            if (offset > max) return null;
            if (offset < 0)
            {
                offset = 0;
            }

            var position = _stream.Position;
            _stream.Position = offset;

            var buffer = new byte[max];
            _stream.Read(buffer, 0, buffer.Length);

            _stream.Position = position;

            return buffer;
        }

        public sbyte ReadSByte(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > Count - 1)
            {
                return 0;
            }

            var result = (sbyte)_stream[offset];

            _stream.RemoveAt(offset);

            return result;
        }

        public short ReadSInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > Count - 2)
            {
                return 0;
            }

            var result = _stream.ReadSInt16(offset);

            _stream.Position += 2;

            return result;
        }

        public sint32 ReadSInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > Count - 4)
            {
                return 0;
            }

            var result = _stream.ReadInt32(offset);

            if (RawStreamOptions.IncrementPosition.IsBit<RawStreamOptions, byte>(opts))
            {
                _stream.Position += 4;
            }

            return result;
        }

        public byte ReadByte(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > Count - 1)
            {
                return 0;
            }

            _stream.Position = offset;

            var result = (byte)_stream.ReadByte();

            if (RawStreamOptions.IncrementPosition.IsBit<RawStreamOptions, byte>(opts))
            {
                _stream.Position++;
            }

            return result;
        }

        public ushort ReadUInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > Count - 2)
            {
                return 0;
            }

            var result = _stream.ReadUInt16(offset);

            if (RawStreamOptions.IncrementPosition.IsBit<RawStreamOptions, byte>(opts))
            {
                _stream.Position += 2;
            }

            return result;
        }

        public uint ReadUInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }
            if (offset > Count - 4)
            {
                return 0;
            }

            var result = _stream.ReadUInt32(offset);

            if ((opts & RawStreamOptions.IncrementPosition) == RawStreamOptions.IncrementPosition)
            {
                _stream.Position += 4;
            }

            return result;
        }

        public string? ReadPString(int max, int size = 0, int offset = 0, int delta = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
        {
            if ((_stream?.Length ?? 0) < 1) return null;

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

            var data = _stream
                .ToList()
                .Skip(offset)
                .Take(length)
                .ToArray();

            max -= size;

            if (max > Count)
            {
                max = Count;
            }

            if (max > 0)
            {
                _stream.RemoveRange(offset, length);
            }

            return (data.GetString() ?? string.Empty).TrimEnd('\0');
        }

        public string? ReadCString(long offset = 0, bool? peek = false)
        {
            if ((_stream?.Length ?? 0) < 1) return null;

            if (offset < 1)
            {
                offset = 0;
            }

            var length = _stream
                .Skip(offset)
                .ToList()
                .IndexOf(0);

            var data = _stream
                .ToList()
                .Skip(offset)
                .Take(length)
                .ToArray();

            if (peek != null)
                if (peek.Value != true)
                    _stream.Position += length;

            return data.GetString();
        }
        #endregion

        #region Peek Methods
        public long Seek(long offset = 0, SeekOrigin origin = SeekOrigin.Begin)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            return _stream.Seek(offset, origin);
        }

        public byte PeekByte(long offset = 0, RawStreamOptions opts = RawStreamOptions.All)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }

            if (RawStreamOptions.UsePosition.IsBit<RawStreamOptions, uint>(opts))
            {
                offset += _stream.Position;
            }

            if (RawStreamOptions.IncrementPosition.IsBit<RawStreamOptions, uint>(opts))
            {
                _stream.Position++;
            }

            return _stream[(int)offset];
        }

        public short PeekSInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.All)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }

            if (RawStreamOptions.UsePosition.IsBit<RawStreamOptions, uint>(opts))
            {
                offset += _stream.Position;
            }

            if (RawStreamOptions.IncrementPosition.IsBit<RawStreamOptions, uint>(opts))
            {
                _stream.Position += 2;
            }

            return _stream.ReadSInt16(offset);
        }

        public sint32 PeekSInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.All)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }

            if (RawStreamOptions.UsePosition.IsBit<RawStreamOptions, uint>(opts))
            {
                offset += _stream.Position;
            }

            if (RawStreamOptions.IncrementPosition.IsBit<RawStreamOptions, uint>(opts))
            {
                _stream.Position += 4;
            }

            return _stream.ReadSInt32(offset);
        }

        public ushort PeekUInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.All)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }

            if (RawStreamOptions.UsePosition.IsBit<RawStreamOptions, uint>(opts))
            {
                offset += _stream.Position;
            }

            if (RawStreamOptions.IncrementPosition.IsBit<RawStreamOptions, uint>(opts))
            {
                _stream.Position += 2;
            }

            return _stream.ReadUInt16(offset);
        }

        public uint PeekUInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.All)
        {
            if ((_stream?.Length ?? 0) < 1) return 0;

            if (offset < 1)
            {
                offset = 0;
            }

            if (RawStreamOptions.UsePosition.IsBit<RawStreamOptions, uint>(opts))
            {
                offset += _stream.Position;
            }

            if (RawStreamOptions.IncrementPosition.IsBit<RawStreamOptions, uint>(opts))
            {
                _stream.Position += 4;
            }

            return _stream.ReadUInt32(offset);
        }

        public string? PeekPString(int max, int size = 0, int offset = 0)
        {
            if ((_stream?.Length ?? 0) < 1) return null;

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
                    length = PeekSInt32(offset, RawStreamOptions.None);

                    break;
                case 2:
                    length = PeekSInt16(offset, RawStreamOptions.None);

                    break;
                default:
                    length = PeekByte(offset, RawStreamOptions.None);
                    size = 1;

                    break;
            }

            if (length > max)
            {
                length = max;
            }

            var data = _stream.ToList()
                .Skip(offset + size)
                .Take(length)
                .ToArray();

            return data.GetString();

        }
        #endregion

        #region Write Methods
        public void SetData(IEnumerable<uint8>? data = null) =>
            _stream = new MemoryStream(data?.ToArray() ?? []);
        public void SetData(uint8[]? data = null) =>
            _stream = new MemoryStream(data ?? []);

        public void AppendBytes(uint8[]? data = null)
        {
            if (Count < 1)
            {
                _stream = new MemoryStream(data);

                return;
            }

            _stream.Write(data);
        }

        public void WriteByte(byte value)
        {
            _stream ??= new();

            _stream.Write([value]);
        }

        public void WriteBytes(byte[] value, int max = 0, int offset = 0)
        {
            _stream ??= new();

            if (max < 1 ||
                offset < 1)
                _stream.Write(value);
            else
                _stream.Write(value
                    .Skip(offset)
                    .Take(max)
                    .ToList());
        }

        public void WriteInt16(short value)
        {
            _stream ??= new();

            _stream.AddRange(value.WriteInt16());
        }

        public void WriteInt32(sint32 value)
        {
            _stream ??= new();

            _stream.AddRange(value.WriteInt32());
        }

        public void WriteInt16(ushort source)
        {
            _stream ??= new();

            _stream.AddRange(source.WriteUInt16());
        }

        public void WriteInt32(uint source)
        {
            _stream ??= new();

            _stream.AddRange(source.WriteUInt32());
        }

        public void WritePString(string source, int max, int size = 0, bool padding = true)
        {
            _stream ??= new();

            if (size < 1)
            {
                size = 1;
            }

            _stream.AddRange(source.WritePString(max, size, padding));
        }

        public void WriteCString(string source)
        {
            _stream ??= new();

            _stream.AddRange(source.WriteCString());
        }
        #endregion

        #region Helper Methods
        public void Clear()
        {
            if (_stream == null)
                _stream = [];
            else
                _stream.Clear();
        }

        public void DropBytes(int length = 0, int offset = 0)
        {
            if ((_stream?.Length ?? 0) < 1) return;

            if (offset < 1)
            {
                offset = 0;
            }

            if (length < 1)
            {
                if (offset < 1)
                {
                    _stream.Clear();

                    return;
                }

                length = Count - offset;
            }

            _stream.RemoveRange(offset, length);
        }

        public void PadBytes(int mod)
        {
            _stream ??= new();

            for (var j = Count % mod; j > 0; j--)
            {
                _stream.Add(0);
            }
        }

        public static int PadOffset(int mod, int value)
        {
            value += value % mod;
            return value;
        }

        public void AlignBytes(int mod)
        {
            _stream ??= new();

            for (var j = mod - Count % mod; j > 0; j--)
            {
                _stream.Add(0);
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