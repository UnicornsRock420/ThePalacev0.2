namespace ThePalace.Common.Desktop.Entities.Ribbon;

public abstract class StandardItem : ItemBase
{
    public string Key { get; set; }
    public IconBase? Icon { get; set; }
}