using ThePalace.Core.Attributes;

namespace ThePalace.Core.Enums
{
    [ByteSize(2)]
    public enum FeatureTypes : short
    {
        CORE,
        GUI,
        MEDIA,
        NETWORK,
    }

    [ByteSize(2)]
    public enum SubFeatureTypes : short
    {
        NONE,
        AUDIO,
        BUSINESS,
        CODEC,
        COMMUNICATION,
        LIBRARY,
        LOGGING,
        LOGIC,
        MANAGER,
        PROTOCOL,
        SCRIPTING,
        SYSTRAYICON,
        VIDEO,
    }

    [ByteSize(2)]
    public enum PurposeTypes : short
    {
        VOID,
        PROVIDER,
        CONSUMER,
    }

    [ByteSize(2)]
    public enum DeviceTypes : short
    {
        NONE,
        KEYBOARD,
        MOUSE,
        USB,
        BLUETOOTH,
        STORAGE,
        VIRTUAL,
    }
}