namespace ThePalace.Common.Factories
{
    public partial class LockContext : IDisposable
    {
        protected object? _lockObj;
        protected bool _hasLock;

        public bool HasLock => _hasLock;

        private LockContext()
        {
            _hasLock = false;
        }
        public LockContext(object? obj = null) : this()
        {
            _lockObj = obj ?? new();
        }

        ~LockContext() => this.Dispose();

        public void Dispose()
        {
            Unlock();

            _lockObj = null;

            GC.SuppressFinalize(this);
        }

        public static LockContext GetLock(object? obj = null, bool tryLock = false)
        {
            var result = (LockContext?)null;

            if (obj is LockContext _lockContext)
            {
                result = _lockContext;
            }
            else
            {
                result = new LockContext(obj);
            }

            if (tryLock)
            {
                result.TryLock();
            }
            else
            {
                result.Lock();
            }

            return result;
        }

        public bool Lock()
        {
            if (_lockObj == null) return false;
            if (_hasLock) return true;

            Monitor.Enter(_lockObj, ref _hasLock);

            return _hasLock;
        }

        public bool TryLock(int millisecondsTimeout = 0)
        {
            if (_lockObj == null) return false;
            if (_hasLock) return true;

            if (millisecondsTimeout > 0)
            {
                Monitor.TryEnter(_lockObj, millisecondsTimeout, ref _hasLock);
            }
            else
            {
                Monitor.TryEnter(_lockObj, ref _hasLock);
            }

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
}