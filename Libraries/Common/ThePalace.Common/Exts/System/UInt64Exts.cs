namespace System;

public static class UInt64Exts
{
    //static UInt64Exts() { }

    public static byte[] GetBytes(this ulong value)
    {
        return BitConverter.GetBytes(value);
    }

    public static class Types
    {
        public static readonly Type UInt64 = typeof(ulong);
        public static readonly Type UInt64Array = typeof(ulong[]);
        public static readonly Type UInt64List = typeof(List<ulong>);
    }
}