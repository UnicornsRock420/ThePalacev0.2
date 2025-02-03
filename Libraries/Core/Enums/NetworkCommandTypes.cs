using ThePalace.Core.Attributes;

namespace ThePalace.Core.Enums
{
    [ByteSize(4)]
    public enum NetworkCommandTypes : int
    {
        DISCONNECT,
        //LISTEN,
        CONNECT,
        RECEIVE,
        SEND,
    }
}