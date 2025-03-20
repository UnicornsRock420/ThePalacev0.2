namespace Lib.Core.Attributes.Auth;

public class RestrictedAttribute(string? role = null) : Attribute
{
    public string? Role { get; set; } = role;
}