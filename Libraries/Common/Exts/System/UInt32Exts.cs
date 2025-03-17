namespace System;

public static class UInt32Exts
{
    //static UInt32Exts() { }

    public static byte[] GetBytes(this uint value)
    {
        return BitConverter.GetBytes(value);
    }

    public static uint SwapInt(this uint value)
    {
        return BitConverter.ToUInt32(
            BitConverter
                .GetBytes(value)
                .Reverse()
                .ToArray());
    }

    public static byte[] ToUInt31(this uint value)
    {
        var data = BitConverter.GetBytes(value);
        data[3] = EnumExts.SetBit<byte>(7, data[3], false);

        return data;
    }

    public static class Types
    {
        public static readonly Type UInt32 = typeof(uint);
        public static readonly Type UInt32Array = typeof(uint[]);
        public static readonly Type UInt32List = typeof(List<uint>);
    }
}