namespace ThePalace.Common.Factories;

public class LockContext : IDisposable
{
    protected bool _hasLock;
    protected object? _lockObj;

    private LockContext()
    {
        _hasLock = false;
    }

    public LockContext(object? obj = null) : this()
    {
        _lockObj = obj ?? new object();
    }

    public bool HasLock => _hasLock;

    public void Dispose()
    {
        Unlock();

        _lockObj = null;

        GC.SuppressFinalize(this);
    }

    ~LockContext()
    {
        Dispose();
    }

    public static LockContext GetLock(object? obj = null, bool tryLock = false)
    {
        var result = (LockContext?)null;

        if (obj is LockContext _lockContext)
            result = _lockContext;
        else
            result = new LockContext(obj);

        if (tryLock)
            result.TryLock();
        else
            result.Lock();

        return result;
    }

    public bool Lock()
    {
        if (_lockObj == null ||
            _hasLock) return false;

        Monitor.Enter(_lockObj, ref _hasLock);

        return _hasLock;
    }

    public bool TryLock(int millisecondsTimeout = 0)
    {
        if (_lockObj == null ||
            _hasLock) return false;

        if (millisecondsTimeout > 0)
            Monitor.TryEnter(_lockObj, millisecondsTimeout, ref _hasLock);
        else
            Monitor.TryEnter(_lockObj, ref _hasLock);

        return _hasLock;
    }

    public bool Unlock()
    {
        if (_lockObj != null &&
            _hasLock)
        {
            Monitor.Exit(_lockObj);

            _hasLock = false;
        }

        return _hasLock;
    }
}