namespace ThePalace.Core.Factories.Types
{
    public partial class Root<T> : IDisposable
    {
        ~Root() => Dispose(false);

        public void Dispose()
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
                        ?.Where(c => c is IDisposable)
                        ?.Cast<IDisposable>()
                        ?.ToList()
                        ?.ForEach(c => { try { c?.Dispose(); } catch { } });
                }

                Journal?.Clear();
                Journal = null;

                Children?.Clear();
                Children?.Dispose();
                Children = null;
            }

            IsDisposed = true;
        }

        public Dictionary<Guid, List<Guid>> Journal = [];
        public Tree<T> Children = [];
    }
}