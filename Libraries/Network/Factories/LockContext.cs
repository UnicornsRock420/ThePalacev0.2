namespace ThePalace.Core.Factories
{
    public partial class LockContext : IDisposable
    {
        protected object? _LockObj = null;
        protected bool _HasLock = false;

        public bool HasLock => _HasLock;

        private LockContext() { }
        public LockContext(object? obj = null)
        {
            _LockObj = obj ?? new();
            _HasLock = false;
        }
        ~LockContext() =>
            this.Dispose();

        public static LockContext GetLock(object? obj = null)
        {
            var result = new LockContext(obj);
            result.Lock();
            return result;
        }

        public void Dispose()
        {
            Unlock();

            _LockObj = null;

            GC.SuppressFinalize(this);
        }

        public bool Lock()
        {
            if (_LockObj == null) return false;
            if (_HasLock) return true;

            Monitor.Enter(_LockObj, ref _HasLock);

            return _HasLock;
        }

        public bool TryLock(int millisecondsTimeout = 0)
        {
            if (_LockObj == null) return false;
            if (_HasLock) return true;

            if (millisecondsTimeout > 0)
            {
                Monitor.TryEnter(_LockObj, millisecondsTimeout, ref _HasLock);
            }
            else
            {
                Monitor.TryEnter(_LockObj, ref _HasLock);
            }

            return _HasLock;
        }

        public bool Unlock()
        {
            if (_LockObj != null &&
                _HasLock)
            {
                Monitor.Exit(_LockObj);

                _HasLock = false;
            }

            return _HasLock;
        }
    }
}