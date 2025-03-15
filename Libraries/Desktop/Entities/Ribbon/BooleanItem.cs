namespace ThePalace.Common.Desktop.Entities.Ribbon;

public abstract class BooleanItem : ItemBase
{
    public bool State { get; set; } = false;
    public override bool Checkable => true;

    public string OnKey { get; set; }
    public IconBase? OnIcon { get; set; }
    public string OffKey { get; set; }
    public IconBase? OffIcon { get; set; }
}