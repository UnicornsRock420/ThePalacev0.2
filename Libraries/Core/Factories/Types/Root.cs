namespace ThePalace.Core.Factories.Types
{
    public partial class Root<TKey, TValue> : IDisposable
        where TKey : notnull
    {
        ~Root() => Dispose(false);

        public void Dispose()
        {
            if (IsDisposed) return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public Dictionary<TKey, List<TKey>> Journal = [];
        public Tree<TValue> Children = [];

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected bool IsDisposed { get; private set; } = false;
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

                Journal?.Clear();
                Journal = null;

                Children?.Clear();
                Children?.Dispose();
                Children = null;
            }

            IsDisposed = true;
        }
    }
}