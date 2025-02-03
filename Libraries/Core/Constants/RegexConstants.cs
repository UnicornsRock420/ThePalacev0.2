using System.Text.RegularExpressions;

namespace ThePalace.Core.Constants
{
    public static partial class RegexConstants
    {
        [GeneratedRegex(@"([\/:*?""<>|]+)", RegexOptions.Singleline | RegexOptions.Compiled)]
        private static partial Regex _regex_filesystemchars();
        public static readonly Regex REGEX_FILESYSTEMCHARS = _regex_filesystemchars();


        [GeneratedRegex(@"([\da-f]{2})", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
        private static partial Regex _regex_hex_split();
        public static readonly Regex REGEX_HEX_SPLIT = _regex_hex_split();


        [GeneratedRegex(@"[^\da-f]+", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
        private static partial Regex _regex_hex_filter();
        public static readonly Regex REGEX_HEX_FILTER = _regex_hex_filter();
    }
}