namespace ThePalace.Core.Helpers
{
    public class Tree<T> : List<T>, IDisposable
    {
        protected bool IsDisposed { get; private set; } = false;

        public Tree<T> Children = [];

        ~Tree() => this.Dispose(false);

        public void Dispose()
        {
            if (this.IsDisposed) return;

            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed) return;

            if (disposing)
            {
                if (typeof(T).GetInterfaces().Contains(typeof(IDisposable)))
                {
                    this.Children
                        ?.Cast<IDisposable>()
                        ?.ToList()
                        ?.ForEach(c => { try { c?.Dispose(); } catch { } });
                }

                this.Children?.Clear();
                this.Children?.Dispose(true);
                this.Children = null;
            }

            this.IsDisposed = true;
        }
    }
}