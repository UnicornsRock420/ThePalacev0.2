namespace ThePalace.Core.Factories
{
    public class Tree<T> : List<T>, IDisposable
    {
        protected bool IsDisposed { get; private set; } = false;

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
}