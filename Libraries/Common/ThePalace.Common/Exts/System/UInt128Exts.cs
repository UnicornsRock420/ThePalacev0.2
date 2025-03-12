namespace System;

public static class UInt128Exts
{
    //static UInt128Exts() { }

    public static byte[] GetBytes(this UInt128 value)
    {
        return BitConverter.GetBytes(value);
    }

    public static class Types
    {
        public static readonly Type UInt128 = typeof(UInt128);
        public static readonly Type UInt128Array = typeof(UInt128[]);
        public static readonly Type UInt128List = typeof(List<UInt128>);
    }
}