using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using ThePalace.Common.Entities.EventArgs;

namespace ThePalace.Common.Factories.Core;

public interface IORMapper
{
    void PopulateObject(object entity, object data);
}

public class ProxyContext : DynamicObject, IDisposable
{
    protected ProxyContext()
    {
    }

    protected class ProxyItem(
        Type sourceType,
        Type proxyType) : IDisposable
    {
        public Type SourceType { get; } = sourceType;
        public Type ProxyType { get; } = proxyType;

        private object _instance;
        public object Instance => _instance ??= Activator.CreateInstance(ProxyType);

        public void Dispose()
        {
            if (_instance is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _instance = null;

            GC.SuppressFinalize(this);
        }
    }

    ~ProxyContext()
    {
        Dispose();
    }

    public void Dispose()
    {
        _proxies.Values.ToList().ForEach(p => p.Dispose());
        _proxies.TryRemove(GetType(), out _);

        GC.SuppressFinalize(this);
    }

    protected static readonly ConcurrentDictionary<Type, ProxyItem> _proxies = new();

    public static event EventHandler MethodInvoked;
    public static event EventHandler PropertyAccessed;
    public static event EventHandler PropertyChanged;
    public static event EventHandler ExceptionOccurred;

    public static object GetInstance(Type sourceType, params object[] args)
    {
        var proxyType = (Type?)null;
        var @ref = (object?)null;

        try
        {
            var proxyItem = new ProxyItem(sourceType, proxyType);
            _proxies.TryAdd(sourceType, proxyItem);

            return @ref = proxyItem.Instance;
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            ExceptionOccurred?.Invoke(sourceType.Name, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }

        return null;
    }

    public static T GetInstance<T>(params object[] args)
    {
        return (T)GetInstance(typeof(T), args);
    }

    internal static object? Do(Func<object[], object?> cb, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(cb, nameof(cb));

        try
        {
            return cb(args);
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            ExceptionOccurred?.Invoke(null, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }

        return null;
    }

    internal static object? Invoke(MethodInfo nfo, object @ref, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(nfo, nameof(nfo));
        ArgumentNullException.ThrowIfNull(@ref, nameof(@ref));

        try
        {
            nfo.Invoke(@ref, args);
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            ExceptionOccurred?.Invoke(null, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }
        finally
        {
            MethodInvoked?.Invoke(null, new MethodInvokedEventArgs
            {
                Member = nfo,
                Args = args,
            });
        }

        return null;
    }

    internal static object? Get(MemberInfo nfo, object @ref, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(nfo, nameof(nfo));
        ArgumentNullException.ThrowIfNull(@ref, nameof(@ref));

        try
        {
            var value = (object?)null;

            switch (nfo)
            {
                case MethodInfo mi: return Invoke(mi, @ref, args);
                case PropertyInfo pi:
                    break;
                case FieldInfo fi:
                    break;
            }
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            ExceptionOccurred?.Invoke(@ref, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }
        finally
        {
            PropertyAccessed?.Invoke(@ref, new PropertyAccessedEventArgs
            {
                Member = nfo,
            });
        }

        return null;
    }

    internal static void Set(MemberInfo nfo, object @ref, object? value)
    {
        ArgumentNullException.ThrowIfNull(nfo, nameof(nfo));
        ArgumentNullException.ThrowIfNull(@ref, nameof(@ref));

        var oldValue = (object?)null;

        switch (nfo)
        {
            case MethodInfo mi: throw new ArgumentException(nameof(nfo));
            case PropertyInfo pi:
                oldValue = pi.GetValue(@ref);

                break;
            case FieldInfo fi:
                oldValue = fi.GetValue(@ref);

                break;
        }

        try
        {
            switch (nfo)
            {
                case PropertyInfo pi:
                    pi.SetValue(@ref, value, null);

                    break;
                case FieldInfo fi:
                    fi.SetValue(@ref, value);

                    break;
            }
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
        }
        finally
        {
            try
            {
                var newValue = nfo switch
                {
                    MethodInfo mi => throw new ArgumentException(nameof(nfo)),
                    PropertyInfo pi => pi.GetValue(@ref),
                    FieldInfo fi => fi.GetValue(@ref),
                    _ => null
                };

                if ((oldValue != null || newValue != null) &&
                    oldValue != newValue)
                    PropertyChanged?.Invoke(@ref, new PropertyChangedEventArgs
                    {
                        Member = nfo,
                        OldValue = oldValue,
                        NewValue = newValue,
                    });
            }
            catch (TaskCanceledException ex)
            {
            }
            catch (Exception ex)
            {
                ExceptionOccurred?.Invoke(@ref, new ExceptionEventArgs
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Exception = ex,
                });
            }
        }
    }

    private AssemblyName _asmName;
    private AssemblyBuilder _asmBuilder;
    private ModuleBuilder _modBuilder;

    private static void CreateAssembly(
        string? assemblyName,
        out AssemblyName asmName,
        out AssemblyBuilder asmBuilder)
    {
        if (string.IsNullOrWhiteSpace(assemblyName)) throw new ArgumentNullException(nameof(assemblyName));

        asmName = new AssemblyName(assemblyName);
        asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
    }

    private static void CreateModule(
        AssemblyName asmName,
        AssemblyBuilder asmBuilder,
        out ModuleBuilder modBuilder)
    {
        modBuilder = asmBuilder.DefineDynamicModule(asmName.Name);
    }

    private static TypeBuilder CreateType(
        ModuleBuilder modBuilder,
        Type sourceType,
        Type? parentType = null,
        TypeAttributes attribs =
            TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AnsiClass |
            TypeAttributes.BeforeFieldInit |
            TypeAttributes.AutoLayout,
        params Type[] interfaces)
    {
        return modBuilder.DefineType(
            sourceType.Name,
            attribs,
            parentType ??= ObjectExts.Types.Object,
            interfaces);
    }

    private static ConstructorBuilder CreateConstructor(
        TypeBuilder typeBuilder,
        MethodAttributes attribs =
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName)
    {
        return typeBuilder.DefineConstructor(
            attribs,
            CallingConventions.Standard,
            Type.EmptyTypes);
    }
}