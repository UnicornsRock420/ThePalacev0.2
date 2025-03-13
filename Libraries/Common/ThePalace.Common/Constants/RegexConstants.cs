using System.Text.RegularExpressions;

namespace ThePalace.Common.Constants;

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


    [GeneratedRegex(@"^([\w\d\-]+)[:][/]{2}([\w\d\.\-]+)[:]{0,1}([\d]*)[/]{0,1}([^?]*)[?]{0,1}([^#]*)[#]{0,1}(.*)",
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
    public enum ParseUrlOptions : short
    {
        None = 0,
        IncludeProtocol = 0x0001,
        IncludeHostname = 0x0002,
        IncludePort = 0x0004,
        IncludePath = 0x0008,
        IncludeQueryString = 0x0010,
        IncludeHashtag = 0x0020,

        // Aliases:
        IncludeAddress = IncludeHostname,
        IncludeIPEndPoint = IncludeHostname | IncludePort,
    }

    public static Dictionary<string, string?> ParseUrl(this string url, ParseUrlOptions opts = ParseUrlOptions.IncludeIPEndPoint)
    {
        var result = new Dictionary<string, string?>();

        if (!REGEX_PARSE_URL.IsMatch(url)) return result;

        var match = REGEX_PARSE_URL.Match(url);

        if ((opts & ParseUrlOptions.IncludeProtocol) == ParseUrlOptions.IncludeProtocol)
        {
            result["Protocol"] = match.Groups.Count > 1 ? match.Groups[1].Value.Trim() : null;
        }

        if ((opts & ParseUrlOptions.IncludeHostname) == ParseUrlOptions.IncludeHostname)
        {
            result["Hostname"] = match.Groups.Count > 2 ? match.Groups[2].Value.Trim() : null;
        }

        if ((opts & ParseUrlOptions.IncludePort) == ParseUrlOptions.IncludePort)
        {
            result["Port"] = match.Groups.Count > 3 ? Convert.ToInt32(match.Groups[3].Value.Trim()).ToString() : null;
        }

        if ((opts & ParseUrlOptions.IncludePath) == ParseUrlOptions.IncludePath)
        {
            result["Path"] = match.Groups.Count > 4 ? match.Groups[4].Value.Trim() : null;
        }

        if ((opts & ParseUrlOptions.IncludeQueryString) == ParseUrlOptions.IncludeQueryString)
        {
            result["QueryString"] = match.Groups.Count > 5 ? match.Groups[5].Value.Trim() : null;
        }

        if ((opts & ParseUrlOptions.IncludeHashtag) == ParseUrlOptions.IncludeHashtag)
        {
            result["Hashtag"] = match.Groups.Count > 6 ? match.Groups[6].Value.Trim() : null;
        }

        return result;
    }
}