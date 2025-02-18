﻿namespace System.Collections.Concurrent
{
    public class DisposableDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, IDisposable
        where TValue : IDisposable
    {
        // https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1063

        protected bool IsDisposed { get; private set; } = false;
        //protected List<IDisposable> _managedResources { get; private set; } = new();

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~DisposableDictionary()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // Dispose() calls Dispose(true)
        public virtual void Dispose()
        {
            Dispose(true);

            Clear();

            GC.SuppressFinalize(this);
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

        public virtual new void Clear()
        {
            foreach (var value in Values)
                try { value?.Dispose(); } catch { }

            base.Clear();
        }
    }
}