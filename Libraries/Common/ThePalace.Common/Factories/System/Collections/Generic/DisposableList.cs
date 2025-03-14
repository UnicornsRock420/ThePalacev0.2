﻿namespace System.Collections.Generic;

public class DisposableList<T> : List<T>, IDisposable
    where T : IDisposable
{
    // https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1063

    public DisposableList()
    {
    }

    public DisposableList(IEnumerable<T> list) : base(list)
    {
    }

    protected bool IsDisposed { get; private set; }
    protected List<IDisposable> _managedResources { get; private set; } = [];

    // Dispose() calls Dispose(true)
    public virtual void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    // NOTE: Leave out the finalizer altogether if this class doesn't
    // own unmanaged resources, but leave the other methods
    // exactly as they are.
    ~DisposableList()
    {
        // Finalizer calls Dispose(false)
        Dispose(false);
    }

    // The bulk of the clean-up code is implemented in Dispose(bool)
    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed) return;

        if (disposing)
        {
            // free managed resources
            if ((_managedResources?.Count ?? 0) > 0)
            {
                _managedResources
                    ?.ForEach(r =>
                    {
                        try
                        {
                            r?.Dispose();
                        }
                        catch
                        {
                        }
                    });
                _managedResources?.Clear();
            }

            _managedResources = null;
        }

        IsDisposed = true;
    }
}