using Lib.Settings.Attributes;

namespace Lib.Settings.Entities.UserProfiles;

[XPath("$.UserProfiles[]:{}")]
public class UserProfile
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string RegCode { get; set; }
    public string PuidCode { get; set; }
}