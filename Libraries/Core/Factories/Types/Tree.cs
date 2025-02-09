namespace ThePalace.Core.Factories.Types
{
    public class Tree<T> : List<T>, IDisposable
    {
        public Tree()
        {
            Parent = null;
            ParentId = null;
            Id = Guid.NewGuid();
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
                    _children
                        ?.Cast<IDisposable>()
                        ?.ToList()
                        ?.ForEach(c => { try { c?.Dispose(); } catch { } });
                }

                _children?.Clear();
                _children?.Dispose(true);
                _children = null;
            }

            IsDisposed = true;
        }

        public readonly Guid? ParentId;
        public readonly Guid Id;

        public Tree<T> Parent { get; protected set; }

        protected Tree<T> _children;
        public Tree<T> Children => _children ??= new(Id, this);

        public List<Guid> XPath
        {
            get
            {
                var result = new List<Guid>
                {
                    Id
                };

                var @ref = Parent;
                while (@ref != null)
                {
                    result.Add(@ref.Id);

                    @ref = @ref.Parent;
                }

                result.Reverse();
                return result;
            }
        }

        public List<T> GetAll(Tree<T>? node = null)
        {
            if ((this?.Children?.Count ?? 0) < 1) return [];

            return new List<T>(GetAll(this.Children));
        }
    }
}