using System.Text.RegularExpressions;

namespace ThePalace.Core.Constants
{
    public static partial class RegexConstants
    {
        [GeneratedRegex(@"([\/:*?""<>|]+)", RegexOptions.Singleline | RegexOptions.Compiled)]
        private static partial Regex _regex_filesystemchars();
        public static readonly Regex REGEX_FILESYSTEMCHARS = _regex_filesystemchars();


        [GeneratedRegex(@"[\s]+", RegexOptions.Singleline | RegexOptions.Compiled)]
        private static partial Regex _regex_whitespace();
        public static readonly Regex REGEX_WHITESPACE = _regex_whitespace();


        [GeneratedRegex(@"([\da-f]{2})", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
        private static partial Regex _regex_hex_split();
        public static readonly Regex REGEX_HEX_SPLIT = _regex_hex_split();


        [GeneratedRegex(@"[^\da-f]+", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
        private static partial Regex _regex_hex_filter();
        public static readonly Regex REGEX_HEX_FILTER = _regex_hex_filter();


        [GeneratedRegex(@"^palace[:][/]{2}([\w\d\.\-]+)[:]{0,1}([\d]*)[/]{0,1}([\d]*)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
        private static partial Regex _regex_palaceurl();
        public static readonly Regex REGEX_PALACEURL = _regex_palaceurl();
    }
}