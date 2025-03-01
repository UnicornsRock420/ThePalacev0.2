namespace ThePalace.Core.Enums.Palace;

[Flags]
public enum SerializerOptions : byte
{
    None = 0,
    SwapByteOrder = 0x01,
    IncludeHeader = 0x02,
    RefNumOnly = 0x04,
}