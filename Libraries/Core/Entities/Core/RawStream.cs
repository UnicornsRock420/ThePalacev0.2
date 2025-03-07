using ThePalace.Core.Exts;
using ThePalace.Core.Interfaces.Data;
using sint32 = System.Int32;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Core;

public partial class RawStream : EventArgs, IDisposable, IData, IStruct
{
    [Flags]
    public enum RawStreamOptions : uint
    {
        None = 0x00,
        IncrementPosition = 0x01,
    }

    public static explicit operator uint8[](RawStream p) => p.Data ?? [];
    public static explicit operator char[](RawStream p) => p.Data?.GetChars() ?? [];
    public static explicit operator RawStream(uint8[] v) => new(v);
    public static explicit operator RawStream(char[] v) => new(v);
    public RawStream() =>
        this._stream = new();
    public RawStream(IEnumerable<uint8>? data = null) =>
        this._stream = new(data?.ToArray() ?? []);
    public RawStream(IEnumerable<char>? data = null) =>
        this._stream = new(data?.GetBytes() ?? []);
    public RawStream(params uint8[] data) =>
        this._stream = new(data ?? []);
    public RawStream(params char[] data) =>
        this._stream = new(data?.GetBytes() ?? []);
    public RawStream(MemoryStream? stream = null) =>
        this._stream = stream ?? new();

    ~RawStream() => this.Dispose();

    public virtual void Dispose()
    {
        this._stream?.Clear();
        this._stream?.Dispose();
        this._stream = null;

        GC.SuppressFinalize(this);
    }

    public static RawStream New() => new();
    public static RawStream FromEnumerable(IEnumerable<uint8>? data = null) => new(data);
    public static RawStream FromEnumerable(IEnumerable<char>? data = null) => new(data);
    public static RawStream FromBytes(uint8[]? data = null) => new(data);
    public static RawStream FromChars(char[]? data = null) => new(data);

    protected MemoryStream? _stream;
    public virtual uint8[]? Data
    {
        get => this._stream?.GetBuffer() ?? [];
        set => this._stream = new(value ?? []);
    }
    public virtual MemoryStream? Stream
    {
        get => this._stream;
        set => this._stream = value;
    }

    public virtual sint32 Count =>
        (sint32)(this._stream?.Length ?? 0);
    public virtual sint32 Length =>
        (sint32)(this._stream?.Length ?? 0);

    #region Read Methods
    public uint8[]? GetData(int max = 0, int offset = 0, bool purge = false)
    {
        if ((this._stream?.Length ?? 0) < 1) return null;

        if (max < 1 ||
            max > (int)this._stream.Length)
        {
            max = (int)this._stream.Length;
        }

        if (offset > max) return null;

        if (offset < 0)
        {
            offset = 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var buffer = new byte[max];
        this._stream.Read(buffer, 0, buffer.Length);

        this._stream.Position = _position;

        return buffer;
    }

    public sbyte ReadSByte(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }
        if (offset > this.Count - 1)
        {
            return 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = (sbyte)this._stream.ReadByte();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public short ReadInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }
        if (offset > this.Count - 2)
        {
            return 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = (short)this._stream.ReadUInt16();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public sint32 ReadInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }
        if (offset > this.Count - 4)
        {
            return 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = this._stream.ReadInt32();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public byte ReadByte(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }
        if (offset > this.Count - 1)
        {
            return 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = (byte)this._stream.ReadByte();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public ushort ReadUInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }
        if (offset > this.Count - 2)
        {
            return 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = this._stream.ReadUInt16();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public uint ReadUInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }
        if (offset > this.Count - 4)
        {
            return 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = this._stream.ReadUInt32();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public string? ReadPString(int max, int size = 0, int offset = 0, int delta = 0, int modulo = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((this._stream?.Length ?? 0) < 1) return null;

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

        if (delta > 0)
        {
            length -= delta;
        }

        if (max > 0 && length > max)
        {
            length = max;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset + size;
        }

        var buffer = new byte[length];
        var readCount = this._stream.Read(buffer, 0, buffer.Length);
        if (readCount < 1) return null;

        if (modulo > 0 && (length + size) % modulo > 0)
        {
            buffer = new byte[(modulo - ((length + size) % modulo))];
            readCount = this._stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) throw new EndOfStreamException();
        }

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return (buffer.GetString() ?? string.Empty).TrimEnd('\0');
    }

    public string? ReadCString(long offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        if ((this._stream?.Length ?? 0) < 1) return null;

        if (offset < 1)
        {
            offset = 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var stringBytes = new List<byte>();
        var buffer = new byte[1];

        do
        {
            var readCount = this._stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) return null;

            if (buffer[0] == 0) break;

            stringBytes.Add(buffer[0]);

        } while (stringBytes.Count <= 0x7FFF);

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return stringBytes.GetString();
    }
    #endregion

    #region Peek Methods
    public long Seek(long offset = 0, SeekOrigin origin = SeekOrigin.Begin)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        return this._stream.Seek(offset, origin);
    }

    public byte PeekByte(long offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = (byte)this._stream.ReadByte();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public short PeekSInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = (byte)this._stream.ReadInt16();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public sint32 PeekSInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = (byte)this._stream.ReadInt32();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public ushort PeekUInt16(long offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = (byte)this._stream.ReadUInt16();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public uint PeekUInt32(long offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((this._stream?.Length ?? 0) < 1) return 0;

        if (offset < 1)
        {
            offset = 0;
        }

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var result = (byte)this._stream.ReadUInt32();

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return result;
    }

    public string? PeekPString(int max, int size = 0, int offset = 0, RawStreamOptions opts = RawStreamOptions.None)
    {
        if ((this._stream?.Length ?? 0) < 1) return null;

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

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset + size;
        }

        var buffer = new byte[length];
        this._stream.Read(buffer, 0, buffer.Length);

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }

        return buffer.GetString();

    }
    #endregion

    #region Write Methods
    public void SetData(IEnumerable<uint8>? data = null) =>
        this._stream = new(data?.ToArray() ?? []);
    public void SetData(uint8[]? data = null) =>
        this._stream = new(data ?? []);

    public void AppendBytes(uint8[]? data = null)
    {
        if (this.Count < 1)
        {
            this._stream = new(data);

            return;
        }

        this._stream.Write(data);
    }

    public void WriteByte(byte value)
    {
        this._stream ??= new();

        var _position = this._stream.Position;

        this._stream.Write([value]);
    }

    public void WriteBytes(byte[] value, int max = 0, int offset = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        this._stream ??= new();

        var _position = this._stream.Position;

        if (offset > 0)
        {
            this._stream.Position = offset;
        }

        var length = value.Length;
        if (length >= max)
        {
            length = max;
        }

        if (max < 1 ||
            offset < 1)
            this._stream.Write(value);
        else
            this._stream.Write(value
                .Skip(offset)
                .Take(length)
                .ToArray());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }
    }

    public void WriteInt16(short value, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        this._stream ??= new();

        var _position = this._stream.Position;

        this._stream.Write(value.WriteInt16());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }
    }

    public void WriteInt32(sint32 value, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        this._stream ??= new();

        var _position = this._stream.Position;

        this._stream.Write(value.WriteInt32());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }
    }

    public void WriteInt16(ushort value, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        this._stream ??= new();

        var _position = this._stream.Position;

        this._stream.Write(value.WriteUInt16());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }
    }

    public void WriteInt32(uint value, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        this._stream ??= new();

        var _position = this._stream.Position;

        this._stream.Write(value.WriteUInt32());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }
    }

    public void WritePString(string value, int max, int size = 0, int modulo = 0, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        this._stream ??= new();

        if (size < 1)
        {
            size = 1;
        }

        var _position = this._stream.Position;

        this._stream.Write(value.WritePString(max, size, modulo));

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }
    }

    public void WriteCString(string value, RawStreamOptions opts = RawStreamOptions.IncrementPosition)
    {
        this._stream ??= new();

        var _position = this._stream.Position;

        this._stream.Write(value.WriteCString());

        if (!RawStreamOptions.IncrementPosition.IsSet<RawStreamOptions, uint>(opts))
        {
            this._stream.Position = _position;
        }
    }
    #endregion

    #region Helper Methods
    public void Clear(int offset = 0, bool clearBytes = true)
    {
        if (this._stream == null)
            this._stream = new();
        else
        {
            if (clearBytes)
            {
                var buffer = this._stream.GetBuffer();
                Array.Clear(buffer, offset, buffer.Length - offset);
            }

            this._stream.Position = offset;
            this._stream.SetLength(offset);
        }
    }

    public void DropBytes(int length = 0, int offset = 0)
    {
        if ((this._stream?.Length ?? 0) < 1) return;

        if (offset < 1)
        {
            offset = 0;
        }

        if (length < 1)
        {
            if (offset < 1)
            {
                this.Clear();

                return;
            }

            length = this.Count - offset;
        }

        if ((offset + length) > 0)
        {
            var _position = this._stream.Position;

            this._stream.Position = offset + length;

            if ((this._stream.Length - this._stream.Position) > 0)
            {
                var buffer = new byte[this._stream.Length - this._stream.Position];
                var byteCount = this._stream.Read(buffer, 0, buffer.Length);
                if (byteCount < 1) return;

                this.Clear(offset);

                this._stream.Write(buffer, 0, buffer.Length);
            }

            this._stream.Position = _position;
        }
    }

    public void PadBytes(int mod)
    {
        this._stream ??= new();

        for (var j = mod - (this.Count % mod); j > 0; j--)
        {
            this._stream.Write([0]);
        }
    }

    public static int PadOffset(int mod, int value)
    {
        return (value += value % mod);
    }
    #endregion
}