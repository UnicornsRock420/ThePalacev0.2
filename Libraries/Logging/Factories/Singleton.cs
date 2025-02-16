namespace ThePalace.Logging.Factories
{
    public abstract partial class Singleton<T>
        where T : class, new()
    {
        protected Singleton() { }

        private static Lazy<T> _instance = new Lazy<T>();

        public static T Instance => _instance.Value;
    }
}