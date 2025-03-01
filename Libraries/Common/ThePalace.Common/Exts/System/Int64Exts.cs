namespace System;

public static class Int64Exts
{
    public static class Types
    {
        public static readonly Type Int64 = typeof(Int64);
        public static readonly Type Int64Array = typeof(Int64[]);
        public static readonly Type Int64List = typeof(List<Int64>);
    }

    //static Int64Exts() { }

    public static byte[] GetBytes(this Int64 value) =>
        BitConverter.GetBytes(value);
}