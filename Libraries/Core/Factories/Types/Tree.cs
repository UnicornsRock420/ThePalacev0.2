namespace ThePalace.Core.Factories.Types
{
    public class Tree<T> : List<T>, IDisposable
    {
        protected bool IsDisposed { get; private set; } = false;

        public Guid Id { get; internal set; } = Guid.NewGuid();

        public Tree<T> Children = [];

        ~Tree() => Dispose(false);

        public void Dispose()
        {
            if (IsDisposed) return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                if (typeof(T).GetInterfaces().Contains(typeof(IDisposable)))
                {
                    Children
                        ?.Cast<IDisposable>()
                        ?.ToList()
                        ?.ForEach(c => { try { c?.Dispose(); } catch { } });
                }

                Children?.Clear();
                Children?.Dispose(true);
                Children = null;
            }

            IsDisposed = true;
        }
    }

    public partial class Tree<TKey, TValue> : List<TValue>, IDisposable
    {
        public Tree()
        {
            Id = default;
        }
        public Tree(TKey id)
        {
            Id = id;
        }

        ~Tree() => Dispose(false);

        public void Dispose()
        {
            if (IsDisposed) return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected bool IsDisposed { get; private set; } = false;

        public TKey Id { get; internal set; }

        public Tree<TKey, TValue> Children = [];

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                if (typeof(TValue).GetInterfaces().Contains(typeof(IDisposable)))
                {
                    Children
                        ?.Where(c => c is IDisposable)
                        ?.Cast<IDisposable>()
                        ?.ToList()
                        ?.ForEach(c => { try { c?.Dispose(); } catch { } });
                }

                Children?.Clear();
                Children?.Dispose(true);
                Children = null;
            }

            IsDisposed = true;
        }
    }
}