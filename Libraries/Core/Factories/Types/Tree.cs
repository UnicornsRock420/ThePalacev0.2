namespace ThePalace.Core.Factories.Types
{
    public class Tree<T> : List<T>, IDisposable
    {
        public Tree()
        {
            Parent = null;
            ParentId = null;
            Id = Guid.NewGuid();

            Children = new(Id, this);
        }
        internal Tree(Guid id, Tree<T>? parentNode = null) : this()
        {
            Parent = parentNode;
            ParentId = id;
        }

        ~Tree() => Dispose(false);

        public virtual void Dispose()
        {
            if (IsDisposed) return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected bool IsDisposed { get; private set; } = false;
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

        public readonly Guid? ParentId;
        public readonly Guid Id;

        public Tree<T> Parent { get; protected set; }
        public Tree<T> Children { get; protected set; }
    }
}