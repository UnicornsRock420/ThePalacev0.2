namespace System
{
    public static class StreamExts
    {
        //public static class Types
        //{
        //}

        //static StreamExts()
        //{
        //}

        public static sbyte ReadSByte(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[1];
            var readCount = stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) throw new EndOfStreamException(nameof(stream));

            return (sbyte)buffer[0];
        }

        public static short ReadInt16(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[2];
            var readCount = stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) throw new EndOfStreamException(nameof(stream));

            return BitConverter.ToInt16(buffer);
        }

        public static int ReadInt32(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[4];
            var readCount = stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) throw new EndOfStreamException(nameof(stream));

            return BitConverter.ToInt32(buffer);
        }

        public static long ReadInt64(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[8];
            var readCount = stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) throw new EndOfStreamException(nameof(stream));

            return BitConverter.ToInt64(buffer);
        }

        public static byte ReadByte(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[1];
            var readCount = stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) throw new EndOfStreamException(nameof(stream));

            return buffer[0];
        }

        public static ushort ReadUInt16(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[2];
            var readCount = stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) throw new EndOfStreamException(nameof(stream));

            return BitConverter.ToUInt16(buffer);
        }

        public static uint ReadUInt32(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[4];
            var readCount = stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) throw new EndOfStreamException(nameof(stream));

            return BitConverter.ToUInt32(buffer);
        }

        public static ulong ReadUInt64(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[8];
            var readCount = stream.Read(buffer, 0, buffer.Length);
            if (readCount < 1) throw new EndOfStreamException(nameof(stream));

            return BitConverter.ToUInt64(buffer);
        }

        public static void WriteInt16(this Stream stream, short value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static void WriteInt32(this Stream stream, int value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static void WriteInt64(this Stream stream, long value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static void WriteUInt16(this Stream stream, ushort value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static void WriteUInt32(this Stream stream, uint value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static void WriteUInt64(this Stream stream, ulong value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }
    }
}