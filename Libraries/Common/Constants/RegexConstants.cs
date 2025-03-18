using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Lib.Common.Constants;

public static partial class RegexConstants
{
    // General:
    [GeneratedRegex(@"[^.\d]+", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex _regex_nonnumeric_filter();

    public static readonly Regex REGEX_FILTER_NONNUMERIC = _regex_nonnumeric_filter();


    [GeneratedRegex(@"([\da-f]{2})", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex _regex_hex_split();

    public static readonly Regex REGEX_SPLIT_HEX = _regex_hex_split();


    [GeneratedRegex(@"[^\da-f]+", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex _regex_hex_filter();

    public static readonly Regex REGEX_FILTER_HEX = _regex_hex_filter();


    [GeneratedRegex(
        @"^([\w\d\-]*)[:]{0,1}[/]{0,2}([\w\d\-.]+[.][\w\d\-]+)[:]{0,1}([\d]*)([/]{0,1}[\w\d%&+-=_/]*)([?]{0,1}[\w\d%&+-=_/]*)([#]{0,1}[\w\d%&+-=_/]*)$",
        RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex _regex_parse_url();

    public static readonly Regex REGEX_PARSE_URL = _regex_parse_url();


    [GeneratedRegex(@"([\/:*?""<>|]+)", RegexOptions.Singleline | RegexOptions.Compiled)]
    private static partial Regex _regex_filesystemchars();

    public static readonly Regex REGEX_FILESYSTEMCHARS = _regex_filesystemchars();


    [GeneratedRegex(@"[\s]+", RegexOptions.Singleline | RegexOptions.Compiled)]
    private static partial Regex _regex_whitespace_singleline();

    public static readonly Regex REGEX_WHITESPACE_SINGLELINE = _regex_whitespace_singleline();


    [GeneratedRegex(@"[\s]+", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex _regex_whitespace_multiline();

    public static readonly Regex REGEX_WHITESPACE_MULTILINE = _regex_whitespace_multiline();


    [GeneratedRegex(@"[\da-z]", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex _regex_alphanumeric_singlecharacter();

    public static readonly Regex REGEX_ALPHANUMERIC_SINGLECHARACTER = _regex_alphanumeric_singlecharacter();

    [Flags]
    public enum ParseUrlOptions : int
    {
        None = 0,

        [Description("Protocol")] IncludeProtocol = 0x0001,
        [Description("Hostname")] IncludeHostname = 0x0002,
        [Description("Port")] IncludePort = 0x0004,
        [Description("Path")] IncludePath = 0x0008,
        [Description("Query")] IncludeQuery = 0x0010,
        [Description("Hashtag")] IncludeHashtag = 0x0020,

        [Description("Invariant")] ModifierInvariant = 0x010000,
        [Description("ToLower")] ModifierToLower = 0x020000,
        [Description("ToUpper")] ModifierToUpper = 0x040000,

        // Aliases:
        [Description("IPEndPoint")] IncludeIPEndPoint = IncludeHostname | IncludePort,
        [Description("BaseUrl")] IncludeBaseUrl = IncludeProtocol | IncludeHostname | IncludePort,

        [Description("ToLowerInvariant")] ModifierToLowerInvariant = ModifierToLower | ModifierInvariant,
        [Description("ToUpperInvariant")] ModifierToUpperInvariant = ModifierToUpper | ModifierInvariant,
    }

    private delegate void _parseUrl(int i);

    public static Dictionary<string, string?> ParseUrl(this string url, ParseUrlOptions opts = ParseUrlOptions.IncludeIPEndPoint)
    {
        if (!REGEX_PARSE_URL.IsMatch(url)) return [];

        var hasInv = opts.HasFlag(ParseUrlOptions.ModifierInvariant);
        var hasToLo = opts.HasFlag(ParseUrlOptions.ModifierToLower);
        var hasToUp = opts.HasFlag(ParseUrlOptions.ModifierToUpper);

        var match = REGEX_PARSE_URL.Match(url);
        var count = match.Groups.Count;

        var result = new Dictionary<string, string?>();
        for (var i = 1; i < 7; i++)
        {
            var b = (ParseUrlOptions)Math.Pow(2, i - 1);
            if (!opts.HasFlag(b)) continue;

            var k = b.GetDescription();
            if (string.IsNullOrWhiteSpace(k)) continue;

            var v = count > i ? match.Groups[i].Value.Trim() : null;
            if (string.IsNullOrWhiteSpace(v))
            {
                result[k] = null;

                continue;
            }

            v = v.Trim();

            if (i > 2)
            {
                result[k] = v;

                continue;
            }

            if (hasToLo)
            {
                v = hasInv ? v.ToLowerInvariant() : v.ToLower();
            }
            else if (hasToUp)
            {
                v = hasInv ? v.ToUpperInvariant() : v.ToUpper();
            }

            result[k] = v;
        }

        return result;
    }
}