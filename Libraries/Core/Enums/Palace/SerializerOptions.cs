namespace ThePalace.Core.Enums
{
    [Flags]
    public enum SerializerOptions : byte
    {
        None = 0,
        SwapByteOrder = 0x01,
        IncludeHeader = 0x02,
    }
}