using System.Text.RegularExpressions;

namespace ThePalace.Core.Constants;

public static partial class RegexConstants
{
    public static readonly Regex REGEX_PALACEURL = _regex_palaceurl();

    // ThePalace:
    [GeneratedRegex(@"^palace[:][/]{2}([\w\d\.\-]+)[:]{0,1}([\d]*)[/]{0,1}([\d]*)",
        RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex _regex_palaceurl();
}