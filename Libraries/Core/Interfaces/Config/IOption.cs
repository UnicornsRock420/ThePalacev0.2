using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Interfaces.Config;

public interface IOption : ISettingBase
{
    string Text { get; }

    bool Enabled { get; set; }
}

public interface IOption<T> : IOption
{
    IReadOnlyDictionary<string, T> Values { get; }
    T Value { get; set; }
}