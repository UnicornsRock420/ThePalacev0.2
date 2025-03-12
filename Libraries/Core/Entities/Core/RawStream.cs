using ThePalace.Core.Exts;
using ThePalace.Core.Interfaces.Data;
using sint32 = int;
using uint8 = byte;

namespace ThePalace.Core.Entities.Core;

public class RawStream : EventArgs, IDisposable, IData, IStruct
{
    [Flags]
    public enum RawStreamOptions : uint
    {
        None = 0x00,
        IncrementPosition = 0x01
    }

    protected MemoryStream? _stream;

    public RawStream()
    {
        _stream = new MemoryStream();
    }

    public RawStream(IEnumerable<uint8>? data = null)
    {
        _stream = new MemoryStream(data?.ToArray() ?? []);
    }

    public RawStream(IEnumerable<char>? data = null)
    {
        _stream = new MemoryStream(data?.GetBytes() ?? []);
    }

    public RawStream(params uint8[] data)
    {
        _stream = new MemoryStream(data ?? []);
    }

    public RawStream(params char[] data)
    {
        _stream = new MemoryStream(data?.GetBytes() ?? []);
    }

    public RawStream(MemoryStream? stream = null)
    {
        _stream = stream ?? new MemoryStream();
    }

    public virtual MemoryStream? Stream
    {
        get => _stream;
        set => _stream = value;
    }

    public virtual sint32 Count =>
        (sint32)(_stream?.Length ?? 0);

    public virtual sint32 Length =>
        (sint32)(_stream?.Length ?? 0);

    public virtual uint8[]? Data
    {
        get => _stream?.ToArray() ?? [];
        set => _stream = new MemoryStream(value ?? []);
    }

    public virtual void Dispose()
    {
        _stream?.Clear();
        _stream?.Dispose();
        _stream = null;

        GC.SuppressFinalize(this);
    }

    public static explicit operator uint8[](RawStream p)
    {
        return p.Data ?? [];
    }

    public static explicit operator char[](RawStream p)
    {
        return p.Data?.GetChars() ?? [];
    }

    public static explicit operator RawStream(uint8[] v)
    {
        return new RawStream(v);
    }

    public static explicit operator RawStream(char[] v)
    {
        return new RawStream(v);
    }

    ~RawStream()
    {
        Dispose();
    }

    public static RawStream New()
    {
        return new RawStream();
    }

    public static RawStream FromEnumerable(IEnumerable<uint8>? data = null)
    {
        return new RawStream(data);
    }

    public static RawStream FromEnumerable(IEnumerable<char>? data = null)
    {
        return new RawStream(data);
    }

    public static RawStream FromBytes(uint8[]? data = null)
    {
        return new RawStream(data);
    }

    public static RawStream FromChars(char[]? data = null)
    {
        return new RawStream(data);
    }

    #region Read Methods

    public uint8[]? GetData(int max = 0, int offset = 0, bool purge = false)
    {
        if ((_stream?.Length ?? 0) < 1) return null;

        if (max < 1 ||
            max > (int)_stream.Length)
            max = (int)_stream.Length;

        if (offset > max) return null;

        if (offset < 0) offset = 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var buffer = new byte[max];
        _stream.Read(buffer, 0, buffer.Length);

        _stream.Position = _position;

        return buffer;
    }

    public sbyte ReadSByte(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;
        if (offset > Count - 1) return 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = (sbyte)_stream.ReadByte();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public short ReadInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;
        if (offset > Count - 2) return 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = (short)_stream.ReadUInt16();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public sint32 ReadInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;
        if (offset > Count - 4) return 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = _stream.ReadInt32();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public byte ReadByte(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;
        if (offset > Count - 1) return 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = (byte)_stream.ReadByte();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public ushort ReadUInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;
        if (offset > Count - 2) return 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = _stream.ReadUInt16();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public uint ReadUInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;
        if (offset > Count - 4) return 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = _stream.ReadUInt32();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public string? ReadPString(int max, int size = 0, int offset = 0, int delta = 0, int modulo = 0,
        RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((_stream?.Length ?? 0) < 1) return null;

        if (offset < 1) offset = 0;

        if (size < 1) size = 1;

        var length = 0;
        switch (size)
        {
            case 4:
                length = ReadInt32();

                break;
            case 2:
                length = ReadInt16();

                break;
            default:
                length = ReadByte();
                size = 1;

                break;
        }

        if (delta > 0) length -= delta;

        if (max > 0 && length > max) length = max;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset + size;

        var buffer = new byte[length];
        var readCount = _stream.Read(buffer, 0, buffer.Length);
        if (readCount < 1) return null;

        if (modulo > 0 && (length + size) % modulo > 0)
        {
            buffer = new byte[modulo - (length + size) % modulo];
            readCount = _stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) throw new EndOfStreamException();
        }

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return (buffer.GetString() ?? string.Empty).TrimEnd('\0');
    }

    public string? ReadCString(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((_stream?.Length ?? 0) < 1) return null;

        if (offset < 1) offset = 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var stringBytes = new List<byte>();
        var buffer = new byte[1];

        do
        {
            var readCount = _stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) return null;

            if (buffer[0] == 0) break;

            stringBytes.Add(buffer[0]);
        } while (stringBytes.Count <= 0x7FFF);

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return stringBytes.GetString();
    }

    #endregion

    #region Peek Methods

    public long Seek(long offset = 0, SeekOrigin origin = SeekOrigin.Begin)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        return _stream.Seek(offset, origin);
    }

    public byte PeekByte(long offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = (byte)_stream.ReadByte();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public short PeekSInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = (byte)_stream.ReadInt16();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public sint32 PeekSInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = (byte)_stream.ReadInt32();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public ushort PeekUInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = (byte)_stream.ReadUInt16();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public uint PeekUInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((_stream?.Length ?? 0) < 1) return 0;

        if (offset < 1) offset = 0;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var result = (byte)_stream.ReadUInt32();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return result;
    }

    public string? PeekPString(int max, int size = 0, int offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((_stream?.Length ?? 0) < 1) return null;

        if (offset < 1) offset = 0;

        if (size < 1) size = 1;

        var length = 0;
        switch (size)
        {
            case 4:
                length = PeekSInt32(offset);

                break;
            case 2:
                length = PeekSInt16(offset);

                break;
            default:
                length = PeekByte(offset);
                size = 1;

                break;
        }

        if (length > max) length = max;

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset + size;

        var buffer = new byte[length];
        _stream.Read(buffer, 0, buffer.Length);

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;

        return buffer.GetString();
    }

    #endregion

    #region Write Methods

    public void SetData(IEnumerable<uint8>? data = null)
    {
        _stream = new MemoryStream(data?.ToArray() ?? []);
    }

    public void SetData(uint8[]? data = null)
    {
        _stream = new MemoryStream(data ?? []);
    }

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
        _stream ??= new MemoryStream();

        var _position = _stream.Position;

        _stream.Write([value]);
    }

    public void WriteBytes(byte[] value, int max = 0, int offset = 0,
        RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        _stream ??= new MemoryStream();

        var _position = _stream.Position;

        if (offset > 0) _stream.Position = offset;

        var length = value.Length;
        if (length >= max) length = max;

        if (max < 1 ||
            offset < 1)
            _stream.Write(value);
        else
            _stream.Write(value
                .Skip(offset)
                .Take(length)
                .ToArray());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;
    }

    public void WriteInt16(short value, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        _stream ??= new MemoryStream();

        var _position = _stream.Position;

        _stream.Write(value.WriteInt16());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;
    }

    public void WriteInt32(sint32 value, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        _stream ??= new MemoryStream();

        var _position = _stream.Position;

        _stream.Write(value.WriteInt32());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;
    }

    public void WriteInt16(ushort value, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        _stream ??= new MemoryStream();

        var _position = _stream.Position;

        _stream.Write(value.WriteUInt16());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;
    }

    public void WriteInt32(uint value, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        _stream ??= new MemoryStream();

        var _position = _stream.Position;

        _stream.Write(value.WriteUInt32());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;
    }

    public void WritePString(string value, int max, int size = 0, int modulo = 0,
        RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        _stream ??= new MemoryStream();

        if (size < 1) size = 1;

        var _position = _stream.Position;

        _stream.Write(value.WritePString(max, size, modulo));

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;
    }

    public void WriteCString(string value, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        _stream ??= new MemoryStream();

        var _position = _stream.Position;

        _stream.Write(value.WriteCString());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts)) _stream.Position = _position;
    }

    #endregion

    #region Helper Methods

    public void Clear(int offset = 0, bool clearBytes = true)
    {
        if (_stream == null)
        {
            _stream = new MemoryStream();
        }
        else
        {
            if (clearBytes)
            {
                var buffer = _stream.ToArray();
                Array.Clear(buffer, offset, buffer.Length - offset);
            }

            _stream.Position = offset;
            _stream.SetLength(offset);
        }
    }

    public void DropBytes(int length = 0, int offset = 0)
    {
        if ((_stream?.Length ?? 0) < 1) return;

        if (offset < 1) offset = 0;

        if (length < 1)
        {
            if (offset < 1)
            {
                Clear();

                return;
            }

            length = Count - offset;
        }

        if (offset + length > 0)
        {
            var _position = _stream.Position;

            _stream.Position = offset + length;

            if (_stream.Length - _stream.Position > 0)
            {
                var buffer = new byte[_stream.Length - _stream.Position];
                var byteCount = _stream.Read(buffer, 0, buffer.Length);
                if (byteCount < 1) return;

                Clear(offset);

                _stream.Write(buffer, 0, buffer.Length);
            }

            _stream.Position = _position;
        }
    }

    public void PadBytes(int mod)
    {
        _stream ??= new MemoryStream();

        for (var j = mod - Count % mod; j > 0; j--) _stream.Write([0]);
    }

    public static int PadOffset(int mod, int value)
    {
        return value += value % mod;
    }

    #endregion
}