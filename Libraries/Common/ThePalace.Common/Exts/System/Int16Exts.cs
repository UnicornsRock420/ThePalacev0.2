namespace System;

public static class Int16Exts
{
    public static class Types
    {
        public static readonly Type Int16 = typeof(Int16);
        public static readonly Type Int16Array = typeof(Int16[]);
        public static readonly Type Int16List = typeof(List<Int16>);
    }

    //static Int16Exts() { }

    public static byte[] GetBytes(this Int16 value) =>
        BitConverter.GetBytes(value);

    public static Int16 SwapShort(this Int16 value) =>
        (short)BitConverter.ToUInt16(
            BitConverter
                .GetBytes(value)
                .Reverse()
                .ToArray());
}