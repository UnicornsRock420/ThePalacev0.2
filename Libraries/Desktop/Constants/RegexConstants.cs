using System.Text.RegularExpressions;

namespace Lib.Common.Desktop.Constants;

public static partial class RegexConstants
{
    public static readonly Regex REGEX_HIDDEN = _regex_hidden();
    public static readonly Regex REGEX_BUBBLE_TYPE = _regex_bubble_type();
    public static readonly Regex REGEX_SOUND = _regex_sound();
    public static readonly Regex REGEX_COORDINATES = _regex_coordinates();

    [GeneratedRegex(@"^[;](.*)$", RegexOptions.Singleline | RegexOptions.Compiled)]
    private static partial Regex _regex_hidden();


    [GeneratedRegex(@"^[\s]*([!:^])[\s]*(.*)$", RegexOptions.Singleline | RegexOptions.Compiled)]
    private static partial Regex _regex_bubble_type();


    [GeneratedRegex(@"^[\s]*[\)]([\w\d\.]+)[\s]*(.*)$", RegexOptions.Singleline | RegexOptions.Compiled)]
    private static partial Regex _regex_sound();


    [GeneratedRegex(@"^[\s]*[@]([0-9]+)[\s]*[,][\s]*([0-9]+)[\s]*(.*)$",
        RegexOptions.Singleline | RegexOptions.Compiled)]
    private static partial Regex _regex_coordinates();
}