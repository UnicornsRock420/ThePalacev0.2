namespace System;

public static class Int128Exts
{
    //static Int128Exts() { }

    public static byte[] GetBytes(this Int128 value)
    {
        return BitConverter.GetBytes(value);
    }

    public static class Types
    {
        public static readonly Type Int128 = typeof(Int128);
        public static readonly Type Int128Array = typeof(Int128[]);
        public static readonly Type Int128List = typeof(List<Int128>);
    }
}