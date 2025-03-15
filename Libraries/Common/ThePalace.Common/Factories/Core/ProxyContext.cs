using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Reflection;
using ThePalace.Common.Entities.EventArgs;

namespace ThePalace.Common.Factories.Core;

public class ProxyContext : IDisposable
{
    protected class ProxyItem(Type type) : IDisposable
    {
        public Type Type { get; } = type;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    private ProxyContext()
    {
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    ~ProxyContext()
    {
        _proxies.TryRemove(GetType(), out _);

        Dispose();
    }

    protected static readonly ConcurrentDictionary<Type, ProxyItem> _proxies = new();

    public static event EventHandler PropertyChanged;
    public static event EventHandler ExceptionOccurred;

    public static object GetInstance(Type type)
    {
        var result = (object?)null;

        return result;
    }

    public static T GetInstance<T>()
    {
        return (T)GetInstance(typeof(T));
    }

    internal static void Do(object @ref, Action<object[]?> cb, params object[] args)
    {
        try
        {
            cb(args);

            PropertyChanged?.Invoke(@ref, new PropertyChangedEventArgs
            {
                Property = null,
                Name = null,
                Value = null,
            });
        }
        catch (TaskCanceledException ex)
        {
            return;
        }
        catch (Exception ex)
        {
            ExceptionOccurred?.Invoke(@ref, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });

            return;
        }
    }
}