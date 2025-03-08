namespace ThePalace.Common.Factories;

public abstract class Singleton<T>
    where T : class, new()
{
    private static readonly Lazy<T> _current = new();

    public static T Current => _current.Value;
}