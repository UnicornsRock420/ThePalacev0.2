﻿namespace System
{
    public static class SByteExts
    {
        public static class Types
        {
            public static readonly Type SByte = typeof(SByte);
            public static readonly Type SByteArray = typeof(SByte[]);
            public static readonly Type SByteList = typeof(List<SByte>);
        }

        //static SByteExts() { }

        public static byte[] GetBytes(this sbyte value) => [(byte)value];
    }
}