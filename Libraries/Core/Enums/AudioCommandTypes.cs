using ThePalace.Core.Attributes;

namespace ThePalace.Core.Enums
{
    [ByteSize(4)]
    public enum AudioCommandTypes : int
    {
        PAUSE,
        PLAY,
        ASTERISK,
        BEEP,
        EXCLAMATION,
        HAND,
        QUESTION,
    }
}