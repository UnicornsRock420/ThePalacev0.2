namespace System;

public static class StreamExts
{
    //public static class Types
    //{
    //}

    //static StreamExts()
    //{
    //}

    public static sbyte ReadSByte<TStream>(this TStream stream, int offset = 0)
        where TStream : Stream
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        var buffer = new byte[1];
        var readCount = stream.Read(buffer, 0, buffer.Length);
        if (readCount < 1) throw new EndOfStreamException(nameof(stream));

        return (sbyte)buffer[0];
    }

    public static short ReadInt16<TStream>(this TStream stream, int offset = 0)
        where TStream : Stream
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        var buffer = new byte[2];
        var readCount = stream.Read(buffer, 0, buffer.Length);
        if (readCount < 1) throw new EndOfStreamException(nameof(stream));

        return BitConverter.ToInt16(buffer);
    }

    public static int ReadInt32<TStream>(this TStream stream, int offset = 0)
        where TStream : Stream
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        var buffer = new byte[4];
        var readCount = stream.Read(buffer, 0, buffer.Length);
        if (readCount < 1) throw new EndOfStreamException(nameof(stream));

        return BitConverter.ToInt32(buffer);
    }

    public static long ReadInt64<TStream>(this TStream stream, int offset = 0)
        where TStream : Stream
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        var buffer = new byte[8];
        var readCount = stream.Read(buffer, 0, buffer.Length);
        if (readCount < 1) throw new EndOfStreamException(nameof(stream));

        return BitConverter.ToInt64(buffer);
    }

    public static byte ReadByte<TStream>(this TStream stream, int offset = 0)
        where TStream : Stream
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        var buffer = new byte[1];
        var readCount = stream.Read(buffer, 0, buffer.Length);
        if (readCount < 1) throw new EndOfStreamException(nameof(stream));

        return buffer[0];
    }

    public static ushort ReadUInt16<TStream>(this TStream stream, int offset = 0)
        where TStream : Stream
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        var buffer = new byte[2];
        var readCount = stream.Read(buffer, 0, buffer.Length);
        if (readCount < 1) throw new EndOfStreamException(nameof(stream));

        return BitConverter.ToUInt16(buffer);
    }

    public static uint ReadUInt32<TStream>(this TStream stream, int offset = 0)
        where TStream : Stream
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        var buffer = new byte[4];
        var readCount = stream.Read(buffer, 0, buffer.Length);
        if (readCount < 1) throw new EndOfStreamException(nameof(stream));

        return BitConverter.ToUInt32(buffer);
    }

    public static ulong ReadUInt64<TStream>(this TStream stream, int offset = 0)
        where TStream : Stream
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        var buffer = new byte[8];
        var readCount = stream.Read(buffer, 0, buffer.Length);
        if (readCount < 1) throw new EndOfStreamException(nameof(stream));

        return BitConverter.ToUInt64(buffer);
    }

    public static void WriteInt16(this Stream stream, short value, int offset = 0)
    {
        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        stream.Write(BitConverter.GetBytes(value));
    }

    public static void WriteInt32(this Stream stream, int value, int offset = 0)
    {
        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        stream.Write(BitConverter.GetBytes(value));
    }

    public static void WriteInt64(this Stream stream, long value, int offset = 0)
    {
        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        stream.Write(BitConverter.GetBytes(value));
    }

    public static void WriteUInt16(this Stream stream, ushort value, int offset = 0)
    {
        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        stream.Write(BitConverter.GetBytes(value));
    }

    public static void WriteUInt32(this Stream stream, uint value, int offset = 0)
    {
        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        stream.Write(BitConverter.GetBytes(value));
    }

    public static void WriteUInt64(this Stream stream, ulong value, int offset = 0)
    {
        if (offset > 0) stream.Seek(offset, SeekOrigin.Begin);

        stream.Write(BitConverter.GetBytes(value));
    }

    public static void Clear(this MemoryStream stream, int offset = 0, bool clearBytes = true)
    {
        if (stream == null)
        {
            stream = new MemoryStream();
        }
        else
        {
            if (clearBytes)
            {
                var buffer = stream.ToArray();
                Array.Clear(buffer, 0, buffer.Length);
            }

            stream.Position = offset;
            stream.SetLength(offset);
        }
    }
}