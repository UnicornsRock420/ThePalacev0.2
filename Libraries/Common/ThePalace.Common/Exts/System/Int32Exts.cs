namespace System;

public static class Int32Exts
{
    public static class Types
    {
        public static readonly Type Int32 = typeof(Int32);
        public static readonly Type Int32Array = typeof(Int32[]);
        public static readonly Type Int32List = typeof(List<Int32>);
    }

    //static Int32Exts() { }

    public static byte[] GetBytes(this Int32 value) =>
        BitConverter.GetBytes(value);

    public static Int32 SwapInt(this Int32 value) =>
        (int)BitConverter.ToUInt32(
            BitConverter
                .GetBytes(value)
                .Reverse()
                .ToArray());

    public static byte[] To24Bit(this int value, bool? isLittleEndian = null)
    {
        var b = BitConverter.GetBytes(value);
        return isLittleEndian ?? BitConverter.IsLittleEndian ? new[] { b[2], b[1], b[0] } : new[] { b[0], b[1], b[2] };
    }

    public static byte[] ToUInt31(this int value)
    {
        var data = BitConverter.GetBytes(value);
        data[3] = EnumExts.SetBit<byte, byte>(7, data[3], false);

        return data;
    }
}