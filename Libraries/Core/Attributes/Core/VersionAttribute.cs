using ThePalace.Common.Constants;

namespace ThePalace.Core.Attributes.Core;

public class VersionAttribute : Attribute
{
    public VersionAttribute(
        ushort major = 0,
        ushort minor = 0,
        ushort revision = 0,
        ushort build = 0)
    {
        Version = string.Join('.', major, minor, revision, build);
    }

    public VersionAttribute(
        uint year = 0,
        uint month = 0,
        uint day = 0,
        uint hours = 0,
        uint minutes = 0,
        uint seconds = 0,
        uint milliseconds = 0)
    {
        Version = string.Join('.', year, month, day, hours, minutes, seconds, milliseconds);
    }

    public VersionAttribute(
        string? version)
    {
        Version = RegexConstants.REGEX_FILTER_NONNUMERIC.Replace(version, string.Empty);
    }

    public string Version { get; } = string.Empty;
}