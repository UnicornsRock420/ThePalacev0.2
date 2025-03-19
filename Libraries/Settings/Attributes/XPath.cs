namespace Lib.Settings.Attributes;

public class XPathAttribute(string xPath) : Attribute
{
    public string XPath { get; } = xPath;
}