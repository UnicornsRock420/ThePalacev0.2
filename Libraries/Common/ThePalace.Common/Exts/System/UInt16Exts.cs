namespace ThePalace.Common.Exts.System;

public static class UInt16Exts
{
    //static UInt16Exts() { }

    public static byte[] GetBytes(this ushort value)
    {
        return BitConverter.GetBytes(value);
    }

    public static ushort SwapShort(this ushort value)
    {
        return BitConverter.ToUInt16(
            BitConverter
                .GetBytes(value)
                .Reverse()
                .ToArray());
    }

    public static class Types
    {
        public static readonly Type UInt16 = typeof(ushort);
        public static readonly Type UInt16Array = typeof(ushort[]);
        public static readonly Type UInt16List = typeof(List<ushort>);
    }
}