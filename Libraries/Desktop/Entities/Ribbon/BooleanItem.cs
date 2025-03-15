namespace ThePalace.Common.Desktop.Entities.Ribbon;

public abstract class BooleanItem : ItemBase
{
    public bool State { get; set; } = false;
    public override bool Checkable => true;

    public string OnHoverKey { get; set; }
    public IconBase? OnHoverIcon { get; set; }
    public string OffHoverKey { get; set; }
    public IconBase? OffHoverIcon { get; set; }
}