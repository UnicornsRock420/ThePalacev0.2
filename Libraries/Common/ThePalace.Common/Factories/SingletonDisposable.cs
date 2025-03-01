using System.Collections;

namespace ThePalace.Common.Factories;

public abstract partial class SingletonDisposable<T> : Disposable
    where T : class, new()
{
    protected SingletonDisposable() { }

    private static Lazy<T> _current = new Lazy<T>();

    public static T Current => _current.Value;
}