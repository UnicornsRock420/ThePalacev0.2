namespace ThePalace.Common.Exts.System;

public static class Int64Exts
{
    //static Int64Exts() { }

    public static byte[] GetBytes(this long value)
    {
        return BitConverter.GetBytes(value);
    }

    public static class Types
    {
        public static readonly Type Int64 = typeof(long);
        public static readonly Type Int64Array = typeof(long[]);
        public static readonly Type Int64List = typeof(List<long>);
    }
}