namespace ThePalace.Common.Desktop.Factories.System.Windows.Forms;

public abstract class SingletonDisposableApplicationContext<T> : DisposableApplicationContext
    where T : class, new()
{
    private static readonly Lazy<T> _current = new();

    public static T Current => _current.Value;
}