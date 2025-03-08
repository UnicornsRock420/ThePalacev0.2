namespace System;

public static class SByteExts
{
    //static SByteExts() { }

    public static byte[] GetBytes(this sbyte value)
    {
        return [(byte)value];
    }

    public static class Types
    {
        public static readonly Type SByte = typeof(sbyte);
        public static readonly Type SByteArray = typeof(sbyte[]);
        public static readonly Type SByteList = typeof(List<sbyte>);
    }
}