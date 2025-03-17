namespace System.Collections.Concurrent;

public class DisposableQueue<TValue> : ConcurrentQueue<TValue>, IDisposable
    where TValue : IDisposable
{
    // https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1063

    protected bool IsDisposed { get; private set; }

    // Dispose() calls Dispose(true)
    public virtual void Dispose()
    {
        Dispose(true);

        Clear();

        GC.SuppressFinalize(this);
    }
    //protected List<IDisposable> _managedResources { get; private set; } = new();

    // NOTE: Leave out the finalizer altogether if this class doesn't
    // own unmanaged resources, but leave the other methods
    // exactly as they are.
    ~DisposableQueue()
    {
        // Finalizer calls Dispose(false)
        Dispose(false);
    }

    // The bulk of the clean-up code is implemented in Dispose(bool)
    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed) return;

        //if (disposing)
        //{
        //    // free managed resources
        //    if ((_managedResources?.Count ?? 0) > 0)
        //    {
        //        _managedResources
        //            ?.ForEach(r => { try { r?.Dispose(); } catch { } });
        //        _managedResources?.Clear();
        //    }
        //    _managedResources = null;
        //}

        IsDisposed = true;
    }

    public new virtual void Clear()
    {
        TValue item = default;
        while (Count > 0)
        {
            try
            {
                TryDequeue(out item);
            }
            catch
            {
            }

            try
            {
                item?.Dispose();
            }
            catch
            {
            }
        }

        base.Clear();
    }
}