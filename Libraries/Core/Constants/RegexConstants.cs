using System.Text.RegularExpressions;

namespace ThePalace.Core.Constants
{
    public static partial class RegexConstants
    {
        // General:
        [GeneratedRegex(@"([\/:*?""<>|]+)", RegexOptions.Singleline | RegexOptions.Compiled)]
        private static partial Regex _regex_filesystemchars();
        public static readonly Regex REGEX_FILESYSTEMCHARS = _regex_filesystemchars();


        [GeneratedRegex(@"[\s]+", RegexOptions.Singleline | RegexOptions.Compiled)]
        private static partial Regex _regex_whitespace_singleline();
        public static readonly Regex REGEX_WHITESPACE_SINGLELINE = _regex_whitespace_singleline();


        [GeneratedRegex(@"[\da-z]", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
        private static partial Regex _regex_alphanumeric_singlecharacter();
        public static readonly Regex REGEX_ALPHANUMERIC_SINGLECHARACTER = _regex_alphanumeric_singlecharacter();


        [GeneratedRegex(@"[\s]+", RegexOptions.Multiline | RegexOptions.Compiled)]
        private static partial Regex _regex_whitespace_multiline();
        public static readonly Regex REGEX_WHITESPACE_MULTILINE = _regex_whitespace_multiline();


        [GeneratedRegex(@"([\da-f]{2})", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
        private static partial Regex _regex_hex_split();
        public static readonly Regex REGEX_HEX_SPLIT = _regex_hex_split();


        [GeneratedRegex(@"[^\da-f]+", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
        private static partial Regex _regex_hex_filter();
        public static readonly Regex REGEX_HEX_FILTER = _regex_hex_filter();


        // ThePalace:
        [GeneratedRegex(@"^palace[:][/]{2}([\w\d\.\-]+)[:]{0,1}([\d]*)[/]{0,1}([\d]*)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
        private static partial Regex _regex_palaceurl();
        public static readonly Regex REGEX_PALACEURL = _regex_palaceurl();
    }
}