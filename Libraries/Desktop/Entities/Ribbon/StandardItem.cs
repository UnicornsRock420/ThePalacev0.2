namespace Lib.Common.Desktop.Entities.Ribbon;

public abstract class StandardItem : ItemBase
{
    public virtual string Key { get; set; }
    public virtual IconBase? Icon { get; set; }
}