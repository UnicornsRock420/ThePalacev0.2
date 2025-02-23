namespace System.Windows.Forms
{
    public abstract partial class SingletonApplicationContext<T> : ApplicationContext
        where T : class, new()
    {
        protected SingletonApplicationContext() { }

        private static Lazy<T> _current = new Lazy<T>();

        public static T Current => _current.Value;
    }
}