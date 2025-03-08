using ThePalace.Common.Factories.System.Collections;

namespace System;

public abstract class SingletonDisposable<T> : Disposable
    where T : class, new()
{
    private static readonly Lazy<T> _current = new();

    public static T Current => _current.Value;
}