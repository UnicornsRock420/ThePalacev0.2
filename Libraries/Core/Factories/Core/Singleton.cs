namespace ThePalace.Core.Factories
{
    public abstract partial class Singleton<T>
        where T : class, new()
    {
        protected Singleton() { }

        private static Lazy<T> _current = new Lazy<T>();

        public static T Current => _current.Value;
    }
}