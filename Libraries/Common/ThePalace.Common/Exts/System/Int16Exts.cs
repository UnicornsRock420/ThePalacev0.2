namespace System;

public static class Int16Exts
{
    //static Int16Exts() { }

    public static byte[] GetBytes(this short value)
    {
        return BitConverter.GetBytes(value);
    }

    public static short SwapShort(this short value)
    {
        return (short)BitConverter.ToUInt16(
            BitConverter
                .GetBytes(value)
                .Reverse()
                .ToArray());
    }

    public static class Types
    {
        public static readonly Type Int16 = typeof(short);
        public static readonly Type Int16Array = typeof(short[]);
        public static readonly Type Int16List = typeof(List<short>);
    }
}